using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ORS.Interpreter;
using ORS.Interpreter.MonoGame;
using ORS.Parser;
using ORS.Parser.Commands;
using ORS.Player.Commands;
using System;
using System.Collections.Generic;

namespace ORS.Player
{
    public class CommandsLoader : ICommandVisitor
    {
        public IReadOnlyList<IRuntimeCommand> Commands => _commands;

        private readonly List<IRuntimeCommand> _commands = new();
        private readonly IAssetLoader _assetLoader;
        private readonly VideoPlayer _videoPlayer;
        private readonly Subtitles _subtitles;
        private readonly Background _background;
        private readonly FadeScreen _fadeScreen;
        private readonly SoundManager _soundManager;
        private readonly LipSyncAnimator _lipSync;

        private CreateBackgroundCommand _lastBackgroundCommand;
        private PlayVoiceCommand _lastVoiceCommand;
        private TimeSpan _lastVoiceEndTime;

        public CommandsLoader(IAssetLoader assetLoader, VideoPlayer videoPlayer,
            Subtitles subtitles, Background background, FadeScreen fadeScreen,
            SoundManager soundManager, LipSyncAnimator lipSync)
        {
            _assetLoader = assetLoader;
            _videoPlayer = videoPlayer;
            _subtitles = subtitles;
            _background = background;
            _fadeScreen = fadeScreen;
            _soundManager = soundManager;
            _lipSync = lipSync;
        }

        public void Clear() => _commands.Clear();

        public void Visit(BlackFadeCommand command)
        {
            command.ParseTime(out TimeSpan begin, out TimeSpan end);
            FadeEffect effectType = ParseFadeEffect(command.Type);
            _commands.Add(new FadeRuntimeCommand(begin, end, effectType, Color.Black, _fadeScreen));
        }

        public void Visit(CreateBackgroundCommand command)
        {
            command.ParseTime(out TimeSpan begin, out TimeSpan end);
            Texture2D image = _assetLoader.LoadImage(command.Path);
            _commands.Add(new CreateBackgroundRuntimeCommand(begin, end, image, _background));

            _lastBackgroundCommand = command;
            if (_lastVoiceCommand is not null)
            {
                if (begin < _lastVoiceEndTime)
                    CreateLipSyncCommand(begin, end, _lastVoiceCommand.Character);
            }
        }

        public void Visit(NextCommand command)
        {
        }

        public void Visit(PlayMovieCommand command)
        {
            command.ParseTime(out TimeSpan begin, out TimeSpan end);
            var video = _assetLoader.LoadVideo(command.Path);
            if (video is not null)
            {
                var runtimeCommand = new PlayMovieRuntimeCommand(begin, end, _videoPlayer, video);
                _commands.Add(runtimeCommand);
            }
        }

        public void Visit(PlaySoundCommand command)
        {
            command.ParseTime(out TimeSpan begin, out TimeSpan end);
            var sound = _assetLoader.LoadAudio(command.Path);
            if (sound is not null)
            {
                var runtimeCommand = new PlaySoundRuntimeCommand(begin, end, sound, _soundManager);
                _commands.Add(runtimeCommand);
            }
        }

        public void Visit(PlayVoiceCommand command)
        {
            command.ParseTime(out TimeSpan begin, out TimeSpan end);
            var sound = _assetLoader.LoadAudio(command.Path);
            if (sound is not null)
            {
                var runtimeCommand = new PlayVoiceRuntimeCommand(begin, end, sound, _soundManager);
                _commands.Add(runtimeCommand);

                _lastVoiceCommand = command;
                _lastVoiceEndTime = end;
                if (_lastBackgroundCommand is not null)
                    CreateLipSyncCommand(begin, end, command.Character);
            }
        }

        public void Visit(PrintTextCommand command)
        {
            command.ParseTime(out TimeSpan begin, out TimeSpan end);
            var runtimeCommand = new PrintTextRuntimeCommand(begin, end, command.Text, _subtitles);
            _commands.Add(runtimeCommand);
        }

        public void Visit(SkipFrameCommand command)
        {
        }

        public void Visit(SelectCommand command)
        {
        }

        public void Visit(PlayBackgroundCommand command)
        {
            command.ParseTime(out TimeSpan begin, out TimeSpan end);
            var intro = _assetLoader.LoadAudio(command.Path + "_INT");
            var loop = _assetLoader.LoadAudio(command.Path + "_LOOP");
            if (intro is not null && loop is not null)
            {
                var runtimeCommand = new PlayBackgroundRuntimeCommand(begin, end, intro, loop, _soundManager);
                _commands.Add(runtimeCommand);
            }
        }

        public void Visit(WhiteFadeCommand command)
        {
            command.ParseTime(out TimeSpan begin, out TimeSpan end);
            FadeEffect effectType = ParseFadeEffect(command.Type);
            _commands.Add(new FadeRuntimeCommand(begin, end, effectType, Color.White, _fadeScreen));
        }

        public void Visit(EndBackgroundCommand command)
        {
            command.ParseTime(out TimeSpan begin, out TimeSpan end);
            var music = _assetLoader.LoadAudio(command.Path);
            if (music is not null)
            {
                var runtimeCommand = new PlaySoundRuntimeCommand(begin, end, music, _soundManager);
                _commands.Add(runtimeCommand);
            }
        }

        public void Visit(EndRollCommand command)
        {
            command.ParseTime(out TimeSpan begin, out TimeSpan end);
            var video = _assetLoader.LoadVideo(command.Path);
            if (video is not null)
            {
                var runtimeCommand = new PlayMovieRuntimeCommand(begin, end, _videoPlayer, video);
                _commands.Add(runtimeCommand);
            }
        }

        private void CreateLipSyncCommand(TimeSpan begin, TimeSpan end, string character)
        {
            var baseFilename = _lastBackgroundCommand.Path + character.ToUpper();
            var frame1 = _assetLoader.LoadImage(baseFilename + ".A");
            if (frame1 is not null)
            {
                var frames = new List<Texture2D>
                    {
                        frame1,
                        _assetLoader.LoadImage(baseFilename + ".B"),
                        _assetLoader.LoadImage(baseFilename + ".C")
                    };
                var lipSyncCommand = new LipSyncCommand(begin, end, frames, _lipSync);
                _commands.Add(lipSyncCommand);
            }
        }

        private static FadeEffect ParseFadeEffect(string effect)
        {
            return effect switch
            {
                "IN" => FadeEffect.In,
                "OUT" => FadeEffect.Out,
                _ => throw new NotSupportedException()
            };
        }
    }
}
