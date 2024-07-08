using MonoGame.Extended.Framework.Media;
using NAudio.Wave;
using ORS.Interpreter.MonoGame.Commands;
using ORS.Parser;
using ORS.Parser.Commands;
using System;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace ORS.Interpreter.MonoGame
{
    public class CommandsLoader : ICommandVisitor
    {
        public IEnumerable<IRuntimeCommand> Commands => _commands;

        private List<IRuntimeCommand> _commands = new();
        
        private readonly IAssetLoader _assetLoader;

        private readonly VideoPlayer _videoPlayer;

        private readonly AudioPlayer _audioPlayer;

        private readonly Subtitles _subtitles;

        public CommandsLoader(IAssetLoader assetLoader, VideoPlayer videoPlayer, AudioPlayer audioPlayer, Subtitles subtitles)
        {
            _assetLoader = assetLoader;
            _videoPlayer = videoPlayer;
            _audioPlayer = audioPlayer;
            _subtitles = subtitles;
        }


        public void Visit(BlackFadeCommand command)
        {
        }

        public void Visit(CreateBackgroundCommand command)
        {
        }

        public void Visit(NextCommand command)
        {
        }

        public void Visit(PlayMovieCommand command)
        {
            command.ParseTime(out TimeSpan begin, out TimeSpan end);
            Video video = _assetLoader.LoadVideo(command.Path, ".WMV");
            var runtimeCommand = new PlayMovieRuntimeCommand(begin, end, _videoPlayer, video, command.Path);
            _commands.Add(runtimeCommand);
        }

        public void Visit(PlaySoundCommand command)
        {
            if (!int.TryParse(command.Layer, out int layer))
                return;

            command.ParseTime(out TimeSpan begin, out TimeSpan end);
            WaveStream sound = _assetLoader.LoadSound(command.Path);
            var runtimeCommand = new PlaySoundRuntimeCommand(begin, end, layer, sound);
            _commands.Add(runtimeCommand);
        }

        public void Visit(PlayVoiceCommand command)
        {
            command.ParseTime(out TimeSpan begin, out TimeSpan end);
            WaveStream sound = _assetLoader.LoadSound(command.Path);
            var runtimeCommand = new PlayVoiceRuntimeCommand(begin, end, sound, _audioPlayer);
            _commands.Add(runtimeCommand);
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
            WaveStream intro = _assetLoader.LoadSound(command.Path+"_INT");
            WaveStream loop = _assetLoader.LoadSound(command.Path+"_LOOP");
            var runtimeCommand = new PlayBackgroundRuntimeCommand(begin, end, intro, loop);
            _commands.Add(runtimeCommand);
        }

        public void Visit(WhiteFadeCommand command)
        {
        }

        
    }
}
