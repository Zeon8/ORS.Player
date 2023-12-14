using CommunityToolkit.Mvvm.ComponentModel;
using LibVLCSharp.Shared;
using ORS.Parser;
using System;
using System.Collections;
using System.IO;

namespace ORS.Interpreter.ViewModels
{
    public partial class MainViewModel : ObservableObject
    {
        public MediaPlayer Player { get; }
        public Subtitles Subtitles { get; } = new Subtitles();
        public BlackBackground BlackBackground { get; } = new BlackBackground();
        public Background Background { get; } = new Background();

        private readonly FileAssetLoader _assetLoader;
        private readonly Interpreter _interpreter;
        private readonly IEnumerator _enumerator;
        private readonly ScriptParser _parser;

        private readonly string[] _scripts = [
            "01/01-00-A01.ORS",
            "01/01-00-A02.ORS",
            "01/01-00-A03.ORS",
            "01/01-00-A04.ORS",
        ];
        private int _currentScript = 0;

        private bool _paused;

        public MainViewModel()
        {
            LibVLC _libVlc = new();
            Player = new MediaPlayer(_libVlc);
            _assetLoader = new FileAssetLoader(_libVlc, "assets/CrossDays/");
            _interpreter = new Interpreter(_libVlc, Player, _assetLoader, Subtitles, BlackBackground, Background);
            _enumerator = _scripts.GetEnumerator();
            _parser = new ScriptParser();
        }

        public void Play()
        {
            LoadScript(_parser, "01/01-00-A00.ORS");

            _interpreter.Start();
            _interpreter.Completed += (_, _) => MoveNext();
        }

        public void MoveNext()
        {
            PlayScript();
            if (_currentScript < _scripts.Length - 1)
                _currentScript++;
        }

        public void MoveBack() 
        {
            if (_currentScript > 0)
                _currentScript--;

            PlayScript();
        }

        private void PlayScript()
        {
            _interpreter.Stop();
            _interpreter.Reset();
            LoadScript(_parser, _scripts[_currentScript]);
            _interpreter.Start();
        }

        private void LoadScript(ScriptParser parser, string path)
        {
            string script = _assetLoader.LoadScript(path);
            var reader = new StringReader(script);
            parser.Load(reader);
            _interpreter.Reset();
            _interpreter.Parse(parser);
        }

        public void TogglePause()
        {
            _paused = !_paused;
            _interpreter.SetPaused(_paused);
        }

        public void SetSpeed(int speed)
        {
            _interpreter.SetSpeed(speed);
        }

    }
}
