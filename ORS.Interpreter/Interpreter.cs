using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Threading;
using LibVLCSharp.Shared;
using ORS.Parser;
using ORS.Parser.Commands;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ORS.Interpreter
{
    internal class Interpreter : ICommandVisitor, IDisposable
    {
        public event EventHandler? Completed;

        private readonly VideoPlayer _videoPlayer;
        private readonly TasksTimer _timer = new TasksTimer();
        private readonly IAssetLoader _assetLoader;
        private readonly AudioPlayer _audioPlayer;
        private readonly Subtitles _subtitles;
        private readonly FadeBackground _blackBackground;
        private readonly Background _background;
        private readonly Selection _selection;
        private SimpleLogger _logger = new();
        private bool _paused;
        private int _speed = DefaultSpeed;
        private const int DefaultSpeed = 1;
        private bool AllowPlay => _speed == DefaultSpeed;

        public Interpreter(VideoPlayer videoPlayer, AudioPlayer audioPlayer, IAssetLoader assetLoader, Subtitles subtitles, FadeBackground blackBackground, Background background, Selection selection)
        {
            _videoPlayer = videoPlayer;
            _assetLoader = assetLoader;
            _audioPlayer = audioPlayer;
            _subtitles = subtitles;
            _blackBackground = blackBackground;
            _background = background;
            _selection = selection;
        }

        public void Start()
        {
            _timer.Start();
            _logger.Setup();
        }

        public void Stop()
        {
            _videoPlayer.Stop();
            _audioPlayer.Stop();

            _timer.Stop();
            _timer.Reset();

            _blackBackground.Hide();
            _background.Hide();
            _currentBackgroundPath = null;
        }

        public void SetSpeed(int speed)
        {
            _speed = speed;
            _timer.Speed = _speed;
            _videoPlayer.SetSpeed(speed);
            _audioPlayer.SetSpeed(_timer.CurrentTime, speed);
        }

        public void SetPaused(bool value)
        {
            _paused = value;
            _timer.SetEnabled(!value);
            _videoPlayer.SetPause(value);
            _audioPlayer.SetPause(value);
        }

        public void Visit(BlackFadeCommand command)
        {
            Fade(command, Colors.White);
        }

        private string? _currentBackgroundPath;
        private TimeSpan _currentBackgroundEndTime;

        public void Visit(CreateBackgroundCommand command)
        {
            _currentBackgroundPath = command.Path;
            var background = _assetLoader.LoadImage(command.Path + ".PNG");

            ParseTime(command, out TimeSpan start, out TimeSpan end);
            _currentBackgroundEndTime = end;

            _timer.AddTask(start, () =>
            {
                _logger.Log(start, "Create background...");
                _background.SetBackground(background);
            });
            _timer.AddTask(end, () =>
            {
                _logger.Log(end, "Remove background...");
                _background.Hide();
            });
        }

        private void SetupAnimation(string character, TimeSpan start, TimeSpan end)
        {
            if (_currentBackgroundPath is null || start > _currentBackgroundEndTime)
                return;

            var characterPath = _currentBackgroundPath + character;
            if (!_assetLoader.TryLoadImage(characterPath + ".A.PNG", out Bitmap? spriteA))
            {
                Debug.WriteLine($"No such file: {characterPath + ".A.PNG"}");
                return;
            }
            var sprites = new Bitmap[3];
            sprites[0] = spriteA;
            sprites[1] = _assetLoader.LoadImage(characterPath + ".B.PNG");
            sprites[2] = _assetLoader.LoadImage(characterPath + ".C.PNG");

            _timer.AddTask(start, () =>
            {
                _logger.Log(start, $"Playing {character} animation");
                Task.Run(() => _background.PlayAnimation(sprites));
            });
            _timer.AddTask(end, () =>
            {
                _logger.Log(start, $"Stopping {character} animation");
                _background.StopAnimation();
            });
        }

        public void Visit(NextCommand command)
        {
            var time = ParseTime(command.Time);
            _timer.AddTask(time, () =>
            {
                Console.WriteLine("Next...");
                Completed?.Invoke(this, EventArgs.Empty);
            });
        }

        public void Visit(PlayMovieCommand command)
        {
            _currentBackgroundPath = null;
            ParseTime(command, out TimeSpan start, out TimeSpan end);

            var media = _assetLoader.LoadMediaAsset(command.Path, ".WMV");
            media.AddOption(":no-audio");

            _timer.AddTask(start, () =>
            {
                _logger.Log(end, $"Playing movie {command.Path}...");
                _videoPlayer.Play(media);
            });
        }

        public void Visit(PlaySoundCommand command)
        {
            var channel = int.Parse(command.Layer);
            ParseTime(command, out TimeSpan start, out TimeSpan end);

            var media = _assetLoader.LoadMediaAsset(command.Path, ".OGG");
            var player = _audioPlayer.GetSoundPlayer(channel);

            _timer.AddTask(start, () =>
            {
                _logger.Log(end, $"Playing sound {command.Path}...");
                _audioPlayer.Play(player, media, start, AllowPlay);
            });
            _timer.AddTask(end, () =>
            {
                _logger.Log(end, $"Stopping sound {command.Path}...");
                _audioPlayer.StopPlayer(player);
            });
        }

        public void Visit(PlayVoiceCommand command)
        {
            int layer = int.Parse(command.IsMale);
            ParseTime(command, out TimeSpan start, out TimeSpan end);
            var media = _assetLoader.LoadMediaAsset(command.Path, ".OGG");

            SetupAnimation(command.Character, start, end);
            _timer.AddTask(start, () =>
            {
                _logger.Log(start, $"Playing voice by {command.Character} from {command.Path}...");
                _audioPlayer.PlayVoice(media, start, AllowPlay);

            });
            _timer.AddTask(end, () =>
            {
                _logger.Log(end, $"Stopping voice by {command.Character} from {command.Path}...");
                _audioPlayer.StopVoice();
            });
        }

        public void Visit(PrintTextCommand command)
        {
            ParseTime(command, out TimeSpan start, out TimeSpan end);
            _timer.AddTask(start, () =>
            {
                _logger.Log(start, $"Print text by {command.Character}...");
                _subtitles.Show(command.Text);
            });
            _timer.AddTask(end, () =>
            {
                _logger.Log(end, $"Hide text by {command.Character}...");
                _subtitles.Hide();
            });

        }

        public void Visit(SkipFrameCommand command)
        {
            Console.WriteLine("Started execution...");
        }

        public void Visit(SelectCommand command)
        {
            ParseTime(command, out TimeSpan start, out TimeSpan end);
            _timer.AddTask(start, () =>
            {
                if(command.OptionB.ToUpper() == "NULL" || string.IsNullOrEmpty(command.OptionB))
                    _selection.ShowOptions(command.OptionA, null);
                else
                    _selection.ShowOptions(command.OptionA, command.OptionB);
            });
            _timer.AddTask(end, () =>
            {
                _selection.Hide();
            });
        }

        public void Visit(PlayBackgroundCommand command)
        {
            ParseTime(command, out TimeSpan start, out TimeSpan end);
            var mediaInt = _assetLoader.LoadMediaAsset(command.Path+"_INT", ".OGG");
            var mediaLoop = _assetLoader.LoadMediaAsset(command.Path+"_LOOP", ".OGG");
            var time = TimeSpan.FromMilliseconds(mediaInt.Duration);
            _timer.AddTask(start, () =>
            {
                _logger.Log(start, "Playing background intro...");
                _audioPlayer.PlayBackground(mediaInt, start, AllowPlay);
            });
            _timer.AddTask(start + time, () =>
            {
                _logger.Log(start+time, "Playing background loop...");
                _audioPlayer.PlayBackground(mediaLoop, start+time, AllowPlay);
            });
            _timer.AddTask(end, () =>
            {
                _logger.Log(end, "Stoping background...");
                _audioPlayer.StopBackground();
            });
        }
        public void Dispose()
        {
            _audioPlayer.Dispose();
        }

        private TimeSpan ParseTime(string value)
        {
            string[] values = value.Split(':');
            int minutes = int.Parse(values[0]);
            int seconds = int.Parse(values[1]);
            int miliseconds = int.Parse(values[2]) * 10;

            var time = new TimeSpan(0, 0, minutes, seconds, miliseconds);
            return time;
        }

        private void ParseTime(BaseCommand command, out TimeSpan start, out TimeSpan end)
        {
            start = ParseTime(command.StartTime);
            end = ParseTime(command.EndTime);
        }

        internal void Load(ScriptParser parser)
        {
            parser.VisitCommands(this);
        }

        public void Visit(WhiteFadeCommand command)
        {
            Fade(command, Colors.White);
        }

        private void Fade(FadeCommand command, Color color)
        {
            ParseTime(command, out TimeSpan start, out TimeSpan end);
            _timer.AddTask(start, () =>
            {
                _logger.Log(start, "White fading...");
                TimeSpan animationDuration = end - start;
                if (command.Type == "IN")
                    _blackBackground.FadeIn(animationDuration);
                _blackBackground.SetColor(color);
            });
            _timer.AddTask(end, () =>
            {
                _blackBackground.Hide();
            });
        }
    }
}
