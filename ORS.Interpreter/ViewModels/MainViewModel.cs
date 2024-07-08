using CommunityToolkit.Mvvm.ComponentModel;
using LibVLCSharp.Shared;
using ORS.Parser;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace ORS.Interpreter.ViewModels
{
    public partial class MainViewModel : ObservableObject
    {
        private readonly LibVLC _libVlc;

        public VideoPlayer VideoPlayer { get; }
        public Subtitles Subtitles { get; } = new Subtitles();
        public FadeBackground FadeBackground { get; } = new FadeBackground();
        public Background Background { get; } = new Background();
        public Selection Selection { get; } = new Selection();

        public IEnumerable<string> Scripts { get; }

        [ObservableProperty]
        private string? _selectedScript;

        private readonly FileAssetLoader _assetLoader;
        private readonly Interpreter _interpreter;
        private readonly ScriptParser _parser;

        private bool _paused;

        public MainViewModel()
        {
            _libVlc = new LibVLC();
            VideoPlayer = new VideoPlayer(_libVlc);
            var audioPlayer = new AudioPlayer(_libVlc);
            _assetLoader = new FileAssetLoader(_libVlc, "assets/SchoolDays/");
            _interpreter = new Interpreter(VideoPlayer, audioPlayer, _assetLoader, Subtitles, FadeBackground, Background, Selection);
            _parser = new ScriptParser();
            Scripts = GetScriptNames();
        }

        partial void OnSelectedScriptChanged(string? value)
        {
            if(value is not null)
                PlayScript(value);
        }

        public void Play()
        {
            
        }

        public void MoveNext()
        {
        }

        public void MoveBack() 
        {
        }

        public void PlayScript(string name)
        {
            _interpreter.Stop();
            LoadScript(name);
            _interpreter.Start();
        }

        private void LoadScript(string path)
        {
            string script = _assetLoader.LoadScript(path);
            var reader = new StringReader(script);
            _parser.Load(reader);
            _interpreter.Load(_parser);
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

        internal static IEnumerable<string> GetScriptNames()
        {
            string scriptsPath = "assets/SchoolDays/Script/";
            foreach (var item in Directory.GetFiles(scriptsPath, "*.ORS", SearchOption.AllDirectories))
            {
                yield return Path.GetRelativePath(scriptsPath, item);
            }
        }
    }
}
