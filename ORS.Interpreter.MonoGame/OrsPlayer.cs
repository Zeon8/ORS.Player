using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ORS.Parser.Commands;
using ORS.Player.Assets;
using ORS.Player.Commands;
using ORS.Player.Components;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Cache;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;

namespace ORS.Player
{
    public class OrsPlayer
    {
        public TimeSpan Time => _time;

        public bool IsPaused 
        {
            get => _isPaused;
            set
            {
                _isPaused = value;
                _soundManager.SetPaused(value);
            }
        }


        public float Speed 
        {
            get => _speed;
            set
            {
                _speed = value;
                _soundManager.SetSpeed(value);
            }
        }

        internal IEnumerable<IRuntimeCommand> LoadedCommands { get; private set; }

        private TimeSpan _time;
        private bool _isPaused;
        private float _speed = 1f;

        private readonly List<IRuntimeCommand> _runningCommands = new();
        private readonly CommandsLoader _commandsLoader;
        private readonly VideoPlayer _videoPlayer;
        private readonly Subtitles _subtitles;
        private readonly Background _background;
        private readonly FadeScreen _fadeScreen;
        private readonly SoundManager _soundManager = new();
        private readonly LipSyncAnimator _lipSync;
        private readonly Stopwatch _stopwatch = new();

        private double _videoElapsedTime;
        private const double TargetVideoUpdateTime = 1.0 / 24;

        public OrsPlayer(IAssetLoader assetLoader, Rectangle screenRect, SpriteBatch spriteBatch, 
            SpriteFont font, Game game)
        {
            _videoPlayer = new VideoPlayer(game.GraphicsDevice, spriteBatch, screenRect);
            _subtitles = new Subtitles(spriteBatch, font, screenRect);
            _background = new Background(spriteBatch, screenRect);
            _fadeScreen = new FadeScreen(spriteBatch, game.GraphicsDevice);
            _lipSync = new LipSyncAnimator(screenRect, spriteBatch);
            _commandsLoader = new CommandsLoader(assetLoader, _videoPlayer, _subtitles, 
            _background, _fadeScreen, _soundManager, _lipSync);
        }

        public void Play(IEnumerable<ICommand> commands)
        {
            Reset();
            LoadedCommands = _commandsLoader.Load(commands);
            _stopwatch.Restart();
        }

        public void Update(GameTime gameTime)
        {
            if (IsPaused)
                return;

            UpdateCommands();

            var deltaTime = gameTime.ElapsedGameTime * Speed;
            _time += deltaTime;

            _videoElapsedTime += deltaTime.TotalSeconds;
            if (_videoElapsedTime >= TargetVideoUpdateTime)
            {
                _videoPlayer.Update();
                _videoElapsedTime -= TargetVideoUpdateTime;
            }

            _lipSync.Update(deltaTime);
            _fadeScreen.Update(deltaTime);
        }

        private void UpdateCommands()
        {
            for (int i = _runningCommands.Count - 1; i >= 0; i--)
            {
                IRuntimeCommand command = _runningCommands[i];
                command.Update();
                if (_time >= command.EndTime)
                {
                    command.Stop();
                    command.IsRunning = false;
                    _runningCommands.Remove(command);
                    command.RealEndTime = _stopwatch.Elapsed;
                }
            }

            foreach(IRuntimeCommand command in LoadedCommands)
            {
                if (_time >= command.StartTime 
                    && _time < command.EndTime
                    && !command.IsRunning)
                {
                    command.Start();
                    command.IsRunning = true;
                    _runningCommands.Add(command);
                    command.RealStartTime = _stopwatch.Elapsed;
                }
            }
        }

        public void Draw()
        {
            _background.Draw();
            _videoPlayer.Draw();
            _lipSync.Draw();
            _subtitles.Draw();
            _fadeScreen.Draw();
        }

        public void Reset()
        {
            _time = TimeSpan.Zero;
            foreach (var command in CollectionsMarshal.AsSpan(_runningCommands))
            {
                command.Stop();
                command.IsRunning = false;
            }
            _runningCommands.Clear();
            IsPaused = false;
        }
    }
}
