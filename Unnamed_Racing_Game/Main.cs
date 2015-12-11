using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Duality;
using SharpDX;
using SharpDX.Toolkit;
using SharpDX.Toolkit.Content;
using SharpDX.Toolkit.Graphics;
using SharpDX.Toolkit.Input;

namespace Kross_Kart
{
    public enum GameStates { MainMenu, Test };

    class Main : Game
    {
        private static GraphicsDeviceManager graphics;
        SpriteBatch spritebatch;

        private GameStates gameState;
        event EventHandler GameStateChanged;

        Menu menu;
        Level level;

        public GameStates GameState
        {
            get { return gameState; }
            set 
            { 
                gameState = value;
                OnGameStateChange();
            }
        }

        public static GraphicsDeviceManager Graphics
        {
            get { return graphics; }
        }

        public static ContentManager GameContent
        {
            get { return content; }
        }
        private static ContentManager content;

        public static MouseState CurrentMouse
        {
            get { return mouse; }
            set { mouse = value; }
        }
        private static MouseState mouse;

        public static KeyboardState CurrentKeyboard
        {
            get { return keyboard; }
            set { keyboard = value; }
        }
        private static KeyboardState keyboard;

        public static MouseManager Mouse
        {
            get { return mouseManager; }
        }
        private static MouseManager mouseManager;

        public static KeyboardManager Keyboard
        {
            get { return keyboardManager; }
        }
        private static KeyboardManager keyboardManager;

        public Main()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            mouseManager = new MouseManager(this);
            keyboardManager = new KeyboardManager(this);
            IsMouseVisible = true;
            graphics.PreferredBackBufferHeight = 450;
            graphics.PreferredBackBufferWidth = 800;
            WindowCreated += WindowShown;
            GameStateChanged += NewGameState;

            content = Content;
        }

        protected override void Initialize()
        {
            ErrorHandler.Initialize();

            base.Initialize();
        }

        private void WindowShown(object sender, EventArgs args)
        {
            Window.Title = "Kross Kart";
        }

        protected override void LoadContent()
        {
            spritebatch = new SpriteBatch(GraphicsDevice);

            GameState = GameStates.MainMenu;

            base.LoadContent();
        }

        protected override void Update(GameTime gameTime)
        {
            mouse = Mouse.GetState();
            keyboard = Keyboard.GetState();

            switch(gameState)
            {
                case GameStates.MainMenu:
                    menu.Update(gameTime);
                    break;
                case GameStates.Test:
                    level.Update(gameTime);
                    break;
                default:
                    break;
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            Window.AllowUserResizing = false;

            switch(gameState)
            {
                case GameStates.MainMenu:
                    spritebatch.Begin(SpriteSortMode.Deferred, graphics.GraphicsDevice.BlendStates.NonPremultiplied);
                    menu.Draw(spritebatch);
                    break;
                case GameStates.Test:
                    GraphicsDevice.SetRasterizerState(GraphicsDevice.RasterizerStates.CullFront);
                    level.Draw(GraphicsDevice);
                    spritebatch.Begin(SpriteSortMode.Deferred, graphics.GraphicsDevice.BlendStates.NonPremultiplied);
                    break;
                //case 
                default:
                    break;
            }

            spritebatch.End();

            base.Draw(gameTime);
        }

        private void NewGameState(object sender, EventArgs args)
        {
            switch(gameState)
            {
                case GameStates.MainMenu:
                    menu = new Menu(this);
                    menu.LoadContent();
                    break;
                case GameStates.Test:
                    level = new Level();
                    level.LoadContent();
                    break;
                default:
                    break;
            }
        }

        private void OnGameStateChange()
        {
            if (GameStateChanged != null)
            {
                GameStateChanged(this, EventArgs.Empty);
            }
        }
    }
}
