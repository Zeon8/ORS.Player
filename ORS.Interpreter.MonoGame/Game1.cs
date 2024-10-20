using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.ImGuiNet;
using ORS.Parser;
using System;
using System.IO;

namespace ORS.Player
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private ImGuiRenderer _guiRenderer;
        private OrsPlayer _player;
        private DebugScreen _debugScreen;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            _graphics.PreferredBackBufferHeight = 510;
            _graphics.SynchronizeWithVerticalRetrace = false;
            _graphics.ApplyChanges();

            IsFixedTimeStep = true;
            TargetElapsedTime = TimeSpan.FromMilliseconds(1000 / 24);
            //TargetElapsedTime = TimeSpan.FromMilliseconds(10);

            _guiRenderer = new ImGuiRenderer(this);
            
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            var font = Content.Load<SpriteFont>("Font");
            _guiRenderer.RebuildFontAtlas();

            var fileLoader = new FileAssetLoader(Directory.GetCurrentDirectory(), GraphicsDevice);
            string script = fileLoader.LoadScript("Script/ENGLISH/00/00-00-A03.ENG");
            ScriptParser parser = ScriptParser.FromString(script);
            var screenRect = new Rectangle(Window.ClientBounds.Width / 2 - 400, 0, 800, 452);
            _player = new(fileLoader, screenRect, _spriteBatch, font, this);
            _player.Play(parser.ParseCommands());
            _debugScreen = new DebugScreen(_player, _guiRenderer);
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            _player.Update(gameTime);
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            _spriteBatch.Begin();
            _player.Draw();
            _spriteBatch.End();
            base.Draw(gameTime);
            _debugScreen.Draw(gameTime);
        }
    }
}
