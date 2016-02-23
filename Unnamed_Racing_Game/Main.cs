using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Duality;
using SharpDX;
using SharpDX.Toolkit;
using SharpDX.Toolkit.Audio;
using SharpDX.Toolkit.Content;
using SharpDX.Toolkit.Graphics;
using SharpDX.Toolkit.Input;

namespace Kross_Kart
{
    public enum GameStates { MainMenu, Play, Pause, EndGame };

    class Main : Game
    {
        private static GraphicsDeviceManager graphics;
        SpriteBatch spritebatch;

        public bool muted, fullscreen;
        event EventHandler GameStateChanged;
        public float vol;
        private GameStates gameState;
        AudioManager audioManager;
        SoundEffect menuSoundEffect, creditsSoundEffect;
        public SoundEffectInstance currentSound, menuSound, creditsSound;

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
            audioManager = new AudioManager(this);
            mouseManager = new MouseManager(this);
            keyboardManager = new KeyboardManager(this);
            IsMouseVisible = true;
            graphics.PreferredBackBufferHeight = 600;
            graphics.PreferredBackBufferWidth = 800;
            WindowCreated += WindowShown;
            GameStateChanged += NewGameState;

            content = Content;
            try
            {
                XmlDocument read = new XmlDocument();
                read.Load("Settings.xml");
                try
                {
                    muted = bool.Parse(read.SelectSingleNode("/Settings/Muted").InnerText);
                    vol = (muted) ? 0 : .025f;
                }
                catch
                {
                    Console.WriteLine("Muted value is invalid; setting to false.");
                    read.SelectSingleNode("/Settings/Muted").InnerText = "false";
                    vol = .25f;
                }
                try
                {
                    fullscreen = bool.Parse(read.SelectSingleNode("/Settings/Fullscreen").InnerText);
                    graphics.IsFullScreen = fullscreen;
                }
                catch
                {
                    Console.WriteLine("Fullscreen value is invalid; setting to false.");
                    read.SelectSingleNode("/Settings/Fullscreen").InnerText = "false";
                    graphics.IsFullScreen = false;
                }
                read.Save("Settings.xml");
            }
            catch
            {
                Console.WriteLine("Could not load settings file; most likely the file doesn't exist.");
                Console.WriteLine("Generating settings file and setting all options to default.");
                XmlDocument settings = new XmlDocument();
                XmlNode rootNode = settings.CreateElement("Settings");
                settings.AppendChild(rootNode);
                XmlNode userNode = settings.CreateElement("Muted");
                userNode.InnerText = "false";
                rootNode.AppendChild(userNode);
                userNode = settings.CreateElement("Fullscreen");
                userNode.InnerText = "false";
                rootNode.AppendChild(userNode);
                settings.Save("Settings.xml");
            }
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

            menuSoundEffect = GameContent.Load<SoundEffect>("Music and Sounds/Menus");
            creditsSoundEffect = GameContent.Load<SoundEffect>("Music and Sounds/Credits");
            menuSound = menuSoundEffect.Create();
            creditsSound = creditsSoundEffect.Create();
            creditsSound.IsLooped = true;
            creditsSound.Volume = vol;
            menuSound.IsLooped = true;
            menuSound.Volume = vol;
            currentSound = creditsSound;
            currentSound.Play();
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
                case GameStates.Play:
                    level.Update(gameTime);
                    break;
                case GameStates.Pause:
                    menu.Update(gameTime);
                    break;
                case GameStates.EndGame:
                    menu.Update(gameTime);
                    break;
                default:
                    break;
            }

            if (CurrentKeyboard.IsKeyDown(Keys.Alt) && CurrentKeyboard.IsKeyDown(Keys.F4)) Exit();

            currentSound.Volume = vol;

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            Window.AllowUserResizing = false;

            switch(gameState)
            {
                case GameStates.MainMenu:
                    GraphicsDevice.SetRasterizerState(GraphicsDevice.RasterizerStates.CullFront);
                    spritebatch.Begin(SpriteSortMode.Deferred, graphics.GraphicsDevice.BlendStates.NonPremultiplied);
                    menu.Draw(spritebatch);
                    break;
                case GameStates.Play:
                    GraphicsDevice.Clear(Color.CornflowerBlue);
                    GraphicsDevice.SetRasterizerState(GraphicsDevice.RasterizerStates.CullFront);
                    spritebatch.Begin(SpriteSortMode.Deferred, graphics.GraphicsDevice.BlendStates.NonPremultiplied);
                    level.Draw(GraphicsDevice, spritebatch);
                    break;
                case GameStates.Pause:
                    GraphicsDevice.Clear(Color.Black);
                    GraphicsDevice.SetRasterizerState(GraphicsDevice.RasterizerStates.CullFront);
                    spritebatch.Begin(SpriteSortMode.Deferred, graphics.GraphicsDevice.BlendStates.NonPremultiplied);
                    level.Draw(GraphicsDevice, spritebatch);
                    menu.Draw(spritebatch);
                    break;
                case GameStates.EndGame:
                    GraphicsDevice.Clear(Color.Black);
                    spritebatch.Begin(SpriteSortMode.Deferred, graphics.GraphicsDevice.BlendStates.NonPremultiplied);
                    menu.Draw(spritebatch);
                    break;
                default:
                    break;
            }

            spritebatch.End();

            base.Draw(gameTime);
        }

        /// <summary>
        /// Event Handler for changing GameState.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void NewGameState(object sender, EventArgs args)
        {
            switch(gameState)
            {
                case GameStates.MainMenu:
                    if (menu == null)menu = new Menu(this);
                    menu.LoadContent();
                    level = null;
                    break;
                case GameStates.Play:
                    if (level == null)
                    {
                        level = new Level(this, menu.selectedKart);
                        level.LoadContent();
                    }
                    break;
                case GameStates.Pause:
                    menu.type = MenuType.Pause;
                    menu.LoadContent();
                    break;
                case GameStates.EndGame:
                    menu.type = MenuType.HighScores;
                    menu.scores.recordScore(level.Player.Score, "");
                    menu.LoadContent();
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
