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
    class Main : Game
    {
        private static GraphicsDeviceManager graphics;
        SpriteBatch spritebatch;

        //Menu menu;
        Level level;

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

            content = Content;

            //menu = new Menu();
            level = new Level();
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
            //menu.LoadContent();
            level.LoadContent();

            base.LoadContent();
        }

        protected override void Update(GameTime gameTime)
        {
            mouse = Mouse.GetState();
            keyboard = Keyboard.GetState();
            //menu.Update(gameTime);
            level.Update(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            GraphicsDevice.SetRasterizerState(GraphicsDevice.RasterizerStates.CullFront);

            level.Draw(GraphicsDevice);

            spritebatch.Begin(SpriteSortMode.Deferred, graphics.GraphicsDevice.BlendStates.NonPremultiplied);

            Window.AllowUserResizing = false;

            //menu.Draw(spritebatch);

            spritebatch.End();

            base.Draw(gameTime);
        }
    }
}
