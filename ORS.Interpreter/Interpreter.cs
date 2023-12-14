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

        private readonly LibVLC _lib;
        private readonly MediaPlayer _videoPlayer;
        private readonly TasksTimer _timer = new TasksTimer();
        private readonly IAssetLoader _assetLoader;
        private readonly AudioPlayer _audioPlayer;
        private readonly Subtitles _subtitles;
        private readonly BlackBackground _blackBackground;
        private readonly Background _background;

        private SimpleLogger _logger = new();
        private bool _paused;
        private int _speed = DefaultSpeed;
        private const int DefaultSpeed = 1;
        private bool AllowPlay => _speed == DefaultSpeed;

        public Interpreter(LibVLC lib, MediaPlayer mediaPlayer, IAssetLoader assetLoader, Subtitles subtitles, BlackBackground blackBackground, Background background)
        {
            _lib = lib;
            _videoPlayer = mediaPlayer;
            _assetLoader = assetLoader;
            _audioPlayer = new AudioPlayer(_lib);
            _subtitles = subtitles;
            _blackBackground = blackBackground;
            _background = background;
        }

        public void Start()
        {
            _logger = new();
            _timer.Start();
        }

        public void Stop()
        {
            _timer.Stop();
            _videoPlayer.Stop();
            _audioPlayer.StopAll();
        }

        public void Reset()
        {
            _currentBackgroundPath = null;
            _blackBackground.Hide();
            _background.Hide();
            _timer.Reset();
            _audioPlayer.Reset();
        }

        public void SetSpeed(int speed)
        {
            _speed = speed;
            _timer.Speed = _speed;
            _videoPlayer.SetRate(speed);
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
            ParseTime(command, out TimeSpan start, out TimeSpan end);
            _timer.AddTask(start, () =>
            {
                _logger.Log(command.StartTime, "Black fading...");
                TimeSpan animationDuration = end - start;
                if(command.Type == "IN")
                    _blackBackground.FadeIn(animationDuration);
            });
            _timer.AddTask(end, () => _blackBackground.Hide());
        }

        private string? _currentBackgroundPath;

        public void Visit(CreateBackgroundCommand command)
        {
            _currentBackgroundPath = command.Path;
            var background = _assetLoader.LoadImage(command.Path + ".PNG");

            ParseTime(command, out TimeSpan start, out TimeSpan end);
            _timer.AddTask(start, () =>
            {
                _logger.Log(command.StartTime, "Create background...");
                _background.SetBackground(background);
            });
            
            _timer.AddTask(end, () =>
            {
                _logger.Log(command.EndTime, "Remove background...");
                _background.Hide();
            });
        }

        private void TryPlayAnimation(string character, TimeSpan start, TimeSpan end)
        {
            if (_currentBackgroundPath is null)
                return;

            var characterPath = _currentBackgroundPath + character;
            if (_assetLoader.TryLoadImage(characterPath + ".A.PNG", out Bitmap? frameA))
            {
                var frames = new Bitmap[3];
                frames[0] = frameA;
                frames[1] = _assetLoader.LoadImage(characterPath + ".B.PNG")!;
                frames[2] = _assetLoader.LoadImage(characterPath + ".C.PNG")!;

                _timer.AddTask(start, () =>
                {
                    Task.Run(() => _background.PlayAnimation(frames));
                });
                _timer.AddTask(end, () => _background.StopAnimation());
            }
            else
                Debug.WriteLine($"No such file: {characterPath + ".A.PNG"}");
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
                _logger.Log(command.StartTime, $"Playing movie {command.Path}...");
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
                _logger.Log(command.StartTime, $"Playing sound {command.Path}...");
                _audioPlayer.Play(player, media, start, AllowPlay);
            });
            _timer.AddTask(end, () =>
            {
                _logger.Log(command.EndTime, $"Stopping sound {command.Path}...");
                _audioPlayer.Stop(player);
            });
        }

        public void Visit(PlayVoiceCommand command)
        {
            int layer = int.Parse(command.IsMale);
            ParseTime(command, out TimeSpan start, out TimeSpan end);
            var media = _assetLoader.LoadMediaAsset(command.Path, ".OGG");

            TryPlayAnimation(command.Character, start, end);
            _timer.AddTask(start, () =>
            {
                _logger.Log(command.StartTime, $"Playing voice by {command.Character} from {command.Path}...");
                _audioPlayer.PlayVoice(media, start, AllowPlay);
                
            });
            _timer.AddTask(end, () =>
            {
                _logger.Log(command.EndTime, $"Stopping voice by {command.Character} from {command.Path}...");
                _audioPlayer.StopVoice();
            });
        }

        public void Visit(PrintTextCommand command)
        {
            ParseTime(command, out TimeSpan start, out TimeSpan end);
            _timer.AddTask(start, () =>
            {
                _logger.Log(command.StartTime, $"Print text by {command.Character}...");
                Dispatcher.UIThread.Invoke(() => _subtitles.Show(command.Text));
            });
            _timer.AddTask(end, () =>
            {
                _logger.Log(command.EndTime, $"Hide text by {command.Character}...");
                Dispatcher.UIThread.Invoke(() => _subtitles.Hide());
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
                _logger.Log(command.StartTime, $"Selecting {command.OptionA} or {command.OptionB} ...");
                SetPaused(true);
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
                _logger.Log(command.StartTime, "Playing background intro...");
                _audioPlayer.PlayBackground(mediaInt, start, AllowPlay);
            });
            _timer.AddTask(start + time, () =>
            {
                _logger.Log(command.StartTime, "Playing background loop...");
                _audioPlayer.PlayBackground(mediaLoop, start+time, AllowPlay);
            });
            _timer.AddTask(end, () =>
            {
                Console.WriteLine("Stopping background...");
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

        internal void Parse(ScriptParser parser)
        {
            parser.VisitCommands(this);
        }
    }
}
