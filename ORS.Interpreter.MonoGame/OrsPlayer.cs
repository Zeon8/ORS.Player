using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ORS.Interpreter;
using ORS.Interpreter.MonoGame;
using ORS.Parser.Commands;
using ORS.Player.Commands;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

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

        public float Speed { get; set; } = 1f;

        internal IEnumerable<IRuntimeCommand> LoadedCommands => _commandsLoader.Commands;

        private bool _isPaused;
        private List<IRuntimeCommand> _runningCommands = new();
        private TimeSpan _time;
        private int _currentCommandIndex;

        private readonly CommandsLoader _commandsLoader;
        private readonly VideoPlayer _videoPlayer;
        private readonly Subtitles _subtitles;
        private readonly Background _background;
        private readonly FadeScreen _fadeScreen;
        private readonly SoundManager _soundManager = new();
        private readonly LipSyncAnimator _lipSync;


        public OrsPlayer(IAssetLoader assetLoader, Rectangle screenRect, SpriteBatch spriteBatch, 
            SpriteFont font, Game game)
        {
            _videoPlayer = new VideoPlayer(game.GraphicsDevice, spriteBatch, screenRect);
            _subtitles = new Subtitles(spriteBatch, font, game.Window);
            _background = new Background(spriteBatch, screenRect);
            _fadeScreen = new FadeScreen(spriteBatch, game.GraphicsDevice);
            _lipSync = new LipSyncAnimator(screenRect, spriteBatch);
            _commandsLoader = new CommandsLoader(assetLoader, _videoPlayer, _subtitles, 
                _background, _fadeScreen, _soundManager, _lipSync);
        }

        public void Play(IEnumerable<ICommand> commands)
        {
            Reset();
            _commandsLoader.Clear();
            foreach (ICommand command in commands)
                command.Accept(_commandsLoader);
        }

        public void Update(GameTime gameTime)
        {
            if (IsPaused)
                return;

            for (int i = _runningCommands.Count - 1; i >= 0; i--)
            {
                IRuntimeCommand command = _runningCommands[i];
                command.Update();
                if (_time >= command.EndTime)
                {
                    command.Stop();
                    _runningCommands.Remove(command);
                }
            }

            var commands = _commandsLoader.Commands;
            for (int i = _currentCommandIndex; i < commands.Count; i++)
            {
                IRuntimeCommand command = commands[i];
                if (_time >= command.StartTime)
                {
                    command.Start();
                    _runningCommands.Add(command);
                    _currentCommandIndex++;
                }
            }

            _time += gameTime.ElapsedGameTime * Speed;

            var deltaTime = (float)(gameTime.ElapsedGameTime.TotalSeconds) * Speed;
            _videoPlayer.Update();
            _lipSync.Update(deltaTime);
            _fadeScreen.Update(deltaTime);
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
                command.Stop();
            _runningCommands.Clear();
            _currentCommandIndex = 0;
            IsPaused = false;
        }
    }
}
