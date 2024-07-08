using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Framework.Media;
using NAudio.Wave;
using ORS.Parser;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ORS.Interpreter.MonoGame
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private VideoPlayer _videoPlayer;
        private CommandsLoader _commandsCreator;
        private Stopwatch _stopwatch = new();
        private SpriteFont _font;
        private Subtitles _subtitles;
        private IRuntimeCommand[] _commands;
        private Texture2D _texture;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            _graphics.PreferredBackBufferHeight = 510;
            _graphics.ApplyChanges();
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _videoPlayer = new VideoPlayer(GraphicsDevice);

            _font = Content.Load<SpriteFont>("Font");
            _subtitles = new(_spriteBatch, _font, Window);

            var audioPlayer = new AudioPlayer();
            var fileLoader = new FileAssetLoader("assets/SchoolDays/");
            //var fileLoader = new FileAssetLoader("assets/CrossDays/");
            _commandsCreator = new CommandsLoader(fileLoader, _videoPlayer, audioPlayer, _subtitles);

            string script = fileLoader.LoadScript("ENGLISH/00/00-00-A00.ENG");
            //string script = fileLoader.LoadScript("01/01-00-A00");
            var parser = new ScriptParser(new StringReader(script));
            parser.VisitCommands(_commandsCreator);

            _stopwatch.Start();
            new Thread(_ => UpdateCommands(_commandsCreator.Commands.ToArray()))
            {
                IsBackground = true,
            }.Start();
            UpdateCommands(_commandsCreator.Commands.ToArray());
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            base.Update(gameTime);
        }

        private void UpdateCommands(IRuntimeCommand[] commands)
        {
            while (true)
            {
                for (int i = 0; i < commands.length; i++)
                {
                    var command = commands[i];
                    if (command.isrunning)
                    {
                        if (_stopwatch.elapsed >= command.endtime)
                        {
                            debug.writeline($"{command}, {_stopwatch.elapsed}, {command.endtime}");
                            command.end();
                            command.isrunning = false;
                        }
                    }
                    else if (_stopwatch.elapsed >= command.begintime && _stopwatch.elapsed < command.endtime)
                    {
                        debug.writeline($"{command}, {_stopwatch.elapsed}, {command.begintime}");
                        command.begin();
                        command.isrunning = true;
                    }
                }
            }
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            _spriteBatch.Begin();
            DrawVideo();
            _subtitles.Draw();

            //_spriteBatch.DrawString(_font, $"Time: {_stopwatch.Elapsed}", new Vector2(10, 10), Color.White);

            _spriteBatch.End();
            base.Draw(gameTime);
        }

        private void DrawVideo()
        {

            if (_videoPlayer.Video is not null)
                _texture = _videoPlayer.GetTexture() ?? _texture;
            if (_texture is not null)
            {
                var destRect = new Rectangle(Window.ClientBounds.Width/2 - 400, 0, 800, 452);
                _spriteBatch.Draw(_texture, destRect, Color.White);
            }
        }
    }
}
