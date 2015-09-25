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

namespace Unnamed_Racing_Game
{
    class Main : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spritebatch;

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
        }

        protected override void Initialize()
        {
            ErrorHandler.Initialize();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spritebatch = new SpriteBatch(GraphicsDevice);

            base.LoadContent();
        }

        protected override void Update(GameTime gameTime)
        {
            
            
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spritebatch.Begin(SpriteSortMode.Deferred, graphics.GraphicsDevice.BlendStates.NonPremultiplied);

            Window.AllowUserResizing = false;

            base.Draw(gameTime);
        }
    }
}
