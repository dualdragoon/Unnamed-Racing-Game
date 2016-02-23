using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Xml;
using Duality.Interaction;
using Duality.Records;
using SharpDX;
using SharpDX.Toolkit;
using SharpDX.Toolkit.Audio;
using SharpDX.Toolkit.Graphics;
using SharpDX.Toolkit.Input;

namespace Kross_Kart
{
    enum MenuType { MainMenu, Options, HighScores, Pause, Credits, KartSelect, Publisher };

    class Menu
    {
        bool muted, fullScreen;
        Button start, highScores, options, menu, sound,
            fullscreen, credits, kartSelect, tieFighter,
            testKart, shoppingCart, guminKart, exit;

        Main main;
        public MenuType type = MenuType.Publisher;
        SoundEffect pressed;
        SoundEffectInstance pressedSound;
        SpriteFont font;
        private string soundOn, screenOn, Aaron, Josh, Colin;
        public string selectedKart = "";
        public ScoreBoard scores;
        Texture2D background, chosenKart, startUnPressed, startPressed,
            highScoresUnPressed, highScoresPressed, optionsUnPressed,
            optionsPressed, title, menuPressed, menuUnPressed,
            soundUnPressed, soundPressed, fullscreenUnPressed,
            fullscreenPressed, exitUnHighLighted, exitHighLighted,
            resumeUnHighlighted, resumeHighlighted, inGameMenu,
            creditsUnPressed, creditsPressed, testKartUnPressed,
            testKartPressed, tifighterUnPressed, tifighterPressed,
            shoppingCartUnPressed, shoppingCartPressed,
            guminKartUnPressed, guminKartPressed, Untitled;

        Timer timer;
        XmlDocument write;

        /// <summary>
        /// Multi Menu encompassing class.
        /// </summary>
        /// <param name="main">Main game class.</param>
        public Menu(Main main)
        {
            this.main = main;
            muted = main.muted;
            fullScreen = main.fullscreen;
        }

        public void LoadContent()
        {
            #region Load Rescources
            background = Main.GameContent.Load<Texture2D>("Menus/Background 2");
            font = Main.GameContent.Load<SpriteFont>("Font/Font1");
            title = Main.GameContent.Load<Texture2D>("Menus/Logo");
            highScoresUnPressed = Main.GameContent.Load<Texture2D>("Menus/High Scores");
            highScoresPressed = Main.GameContent.Load<Texture2D>("Menus/High Scores Pressed");
            optionsUnPressed = Main.GameContent.Load<Texture2D>("Menus/Options");
            optionsPressed = Main.GameContent.Load<Texture2D>("Menus/Options Pressed");
            menuUnPressed = Main.GameContent.Load<Texture2D>("Menus/Menu");
            menuPressed = Main.GameContent.Load<Texture2D>("Menus/Menu Pressed");
            creditsUnPressed = Main.GameContent.Load<Texture2D>("Menus/CreditsUnPressed");
            creditsPressed = Main.GameContent.Load<Texture2D>("Menus/CreditsPressed");
            soundUnPressed = Main.GameContent.Load<Texture2D>("Menus/Sound");
            soundPressed = Main.GameContent.Load<Texture2D>("Menus/SoundPressed");
            fullscreenUnPressed = Main.GameContent.Load<Texture2D>("Menus/FullScreen");
            fullscreenPressed = Main.GameContent.Load<Texture2D>("Menus/FullScreenPressed");
            inGameMenu = Main.GameContent.Load<Texture2D>("Menus/InGameMenu");
            exitUnHighLighted = Main.GameContent.Load<Texture2D>("Menus/Exit");
            exitHighLighted = Main.GameContent.Load<Texture2D>("Menus/ExitHighlight");
            resumeHighlighted = Main.GameContent.Load<Texture2D>("Menus/ResumeHighlight");
            resumeUnHighlighted = Main.GameContent.Load<Texture2D>("Menus/Resume");
            startUnPressed = Main.GameContent.Load<Texture2D>("Menus/Start");
            startPressed = Main.GameContent.Load<Texture2D>("Menus/Start Pressed");
            testKartUnPressed = Main.GameContent.Load<Texture2D>("Menus/TestKartUnPressed");
            testKartPressed = Main.GameContent.Load<Texture2D>("Menus/TestKartPressed");
            tifighterUnPressed = Main.GameContent.Load<Texture2D>("Menus/TiFighter");
            tifighterPressed = Main.GameContent.Load<Texture2D>("Menus/TiFighterPressed");
            shoppingCartUnPressed = Main.GameContent.Load<Texture2D>("Menus/ShoppingCart");
            shoppingCartPressed = Main.GameContent.Load<Texture2D>("Menus/ShoppingCartPressed");
            guminKartUnPressed = Main.GameContent.Load<Texture2D>("Menus/GuminKart");
            guminKartPressed = Main.GameContent.Load<Texture2D>("Menus/GuminKartPressed");
            Untitled = Main.GameContent.Load<Texture2D>("Menus/Untitled");
            pressed = Main.GameContent.Load<SoundEffect>("Music and Sounds/Button Press");
            pressedSound = pressed.Create();
            pressedSound.Volume = .25f;
            pressedSound.IsLooped = false;

            scores = new ScoreBoard("Scores", "Scores", 10);
            scores.retrieveScores();
            #endregion

            switch (type)
            {
                case MenuType.MainMenu:
                    kartSelect = new Button(new Vector2(310, 380), 173, 53, 1, Main.CurrentMouse, startUnPressed, startPressed, Main.Graphics.PreferredBackBufferWidth, Main.Graphics.PreferredBackBufferHeight);
                    highScores = new Button(new Vector2(9, 536), 324, 55, 2, Main.CurrentMouse, highScoresUnPressed, highScoresPressed, Main.Graphics.PreferredBackBufferWidth, Main.Graphics.PreferredBackBufferHeight);
                    options = new Button(new Vector2(280, 469), 233, 42, 3, Main.CurrentMouse, optionsUnPressed, optionsPressed, Main.Graphics.PreferredBackBufferWidth, Main.Graphics.PreferredBackBufferHeight);
                    credits = new Button(new Vector2(600, 545), 191, 47, 7, Main.CurrentMouse, creditsUnPressed, creditsPressed, Main.Graphics.PreferredBackBufferWidth, Main.Graphics.PreferredBackBufferHeight);
                    exit = new Button(new Vector2(720, 9), 71, 29, 9, Main.CurrentMouse, exitUnHighLighted, exitHighLighted, Main.Graphics.PreferredBackBufferWidth, Main.Graphics.PreferredBackBufferHeight);

                    credits.ButtonPressed += ButtonPressed;
                    kartSelect.ButtonPressed += ButtonPressed;
                    options.ButtonPressed += ButtonPressed;
                    highScores.ButtonPressed += ButtonPressed;
                    exit.ButtonPressed += ButtonPressed;
                    break;

                case MenuType.Options:
                    highScores = new Button(new Vector2(467, 536), 324, 55, 2, Main.CurrentMouse, highScoresUnPressed, highScoresPressed, Main.Graphics.PreferredBackBufferWidth, Main.Graphics.PreferredBackBufferHeight);
                    menu = new Button(new Vector2(9, 537), 168, 54, 4, Main.CurrentMouse, menuUnPressed, menuPressed, Main.Graphics.PreferredBackBufferWidth, Main.Graphics.PreferredBackBufferHeight);
                    sound = new Button(new Vector2(100, 100), 164, 53, 5, Main.CurrentMouse, soundUnPressed, soundPressed, Main.Graphics.PreferredBackBufferWidth, Main.Graphics.PreferredBackBufferHeight);
                    fullscreen = new Button(new Vector2(100, 200), 248, 45, 6, Main.CurrentMouse, fullscreenUnPressed, fullscreenPressed, Main.Graphics.PreferredBackBufferWidth, Main.Graphics.PreferredBackBufferHeight);

                    menu.ButtonPressed += ButtonPressed;
                    highScores.ButtonPressed += ButtonPressed;
                    sound.ButtonPressed += ButtonPressed;
                    fullscreen.ButtonPressed += ButtonPressed;
                    break;

                case MenuType.HighScores:
                    menu = new Button(new Vector2(9, 537), 168, 54, 4, Main.CurrentMouse, menuUnPressed, menuPressed, Main.Graphics.PreferredBackBufferWidth, Main.Graphics.PreferredBackBufferHeight);

                    menu.ButtonPressed += ButtonPressed;
                    break;

                case MenuType.Pause:
                    start = new Button(new Vector2(325, 200), 123, 38, 8, Main.CurrentMouse, resumeUnHighlighted, resumeHighlighted, Main.Graphics.PreferredBackBufferWidth, Main.Graphics.PreferredBackBufferHeight);
                    menu = new Button(new Vector2(350, 300), 79, 29, 4, Main.CurrentMouse, exitUnHighLighted, exitHighLighted, Main.Graphics.PreferredBackBufferWidth, Main.Graphics.PreferredBackBufferHeight);

                    start.ButtonPressed += ButtonPressed;
                    menu.ButtonPressed += ButtonPressed;
                    break;

                case MenuType.KartSelect:
                    start = new Button(new Vector2(500, 500), 173, 53, 8, Main.CurrentMouse, startUnPressed, startPressed, Main.Graphics.PreferredBackBufferWidth, Main.Graphics.PreferredBackBufferHeight);
                    testKart = new Button(new Vector2(100, 100), 165, 23, 10, Main.CurrentMouse, testKartUnPressed, testKartPressed, Main.Graphics.PreferredBackBufferWidth, Main.Graphics.PreferredBackBufferHeight);
                    tieFighter = new Button(new Vector2(100, 200), 173, 26, 11, Main.CurrentMouse, tifighterUnPressed, tifighterPressed, Main.Graphics.PreferredBackBufferWidth, Main.Graphics.PreferredBackBufferHeight);
                    shoppingCart = new Button(new Vector2(100, 300), 255, 32, 12, Main.CurrentMouse, shoppingCartUnPressed, shoppingCartPressed, Main.Graphics.PreferredBackBufferWidth, Main.Graphics.PreferredBackBufferHeight);
                    guminKart = new Button(new Vector2(100, 400), 205, 25, 13, Main.CurrentMouse, guminKartUnPressed, guminKartPressed, Main.Graphics.PreferredBackBufferWidth, Main.Graphics.PreferredBackBufferHeight);
                    menu = new Button(new Vector2(100, 500), 168, 54, 4, Main.CurrentMouse, menuUnPressed, menuPressed, Main.Graphics.PreferredBackBufferWidth, Main.Graphics.PreferredBackBufferHeight);
                    
                    menu.ButtonPressed += ButtonPressed;
                    start.ButtonPressed += ButtonPressed;
                    testKart.ButtonPressed += ButtonPressed;
                    tieFighter.ButtonPressed += ButtonPressed;
                    shoppingCart.ButtonPressed += ButtonPressed;
                    guminKart.ButtonPressed += ButtonPressed;
                    break;

                case MenuType.Credits:
                    menu = new Button(new Vector2(9, 537), 168, 54, 4, Main.CurrentMouse, menuUnPressed, menuPressed, Main.Graphics.PreferredBackBufferWidth, Main.Graphics.PreferredBackBufferHeight);

                    menu.ButtonPressed += ButtonPressed;
                    break;

                case MenuType.Publisher:
                    timer = new Timer(4000);
                    timer.Elapsed += TimeElapsed;
                    timer.Start();
                    break;

                default:
                    break;
            }
        }

        public void Update(GameTime gameTime)
        {
            switch (type)
            {
                case MenuType.MainMenu:
                    kartSelect.Update(Main.CurrentMouse);
                    highScores.Update(Main.CurrentMouse);
                    options.Update(Main.CurrentMouse);
                    credits.Update(Main.CurrentMouse);
                    exit.Update(Main.CurrentMouse);
                    break;

                case MenuType.Options:
                    highScores.Update(Main.CurrentMouse);
                    menu.Update(Main.CurrentMouse);
                    sound.Update(Main.CurrentMouse);
                    fullscreen.Update(Main.CurrentMouse);
                    screenOn = (fullScreen) ? "On" : "Off";
                    soundOn = (muted) ? "Off" : "On";
                    break;

                case MenuType.HighScores:
                    menu.Update(Main.CurrentMouse);
                    break;

                case MenuType.Pause:
                    menu.Update(Main.CurrentMouse);
                    start.Update(Main.CurrentMouse);
                    break;

                case MenuType.Credits:
                    menu.Update(Main.CurrentMouse);

                    break;

                case MenuType.KartSelect:
                    start.Update(Main.CurrentMouse);
                    testKart.Update(Main.CurrentMouse);
                    tieFighter.Update(Main.CurrentMouse);
                    shoppingCart.Update(Main.CurrentMouse);
                    guminKart.Update(Main.CurrentMouse);
                    menu.Update(Main.CurrentMouse);
                    break;

                default:
                    break;
            }
        }

        private void TimeElapsed(object sender, EventArgs args)
        {
            type = MenuType.Credits;
            timer.Stop();
            LoadContent();
        }

        /// <summary>
        /// Event Handler for every button in every menu.
        /// </summary>
        /// <param name="sender">Button clicked.</param>
        /// <param name="args">Event Arguments</param>
        #region Event Handler

        public void ButtonPressed(object sender, EventArgs args)
        {
            pressedSound.Play();
            switch (((Button)sender).ButtonNum)
            {
                case 1:
                    type = MenuType.KartSelect;
                    LoadContent();
                    break;

                case 2:
                    type = MenuType.HighScores;
                    LoadContent();
                    break;

                case 3:
                    type = MenuType.Options;
                    
                    LoadContent();
                    break;

                case 4:
                    if (type == MenuType.Options)
                    {
                        XmlDocument settings = new XmlDocument();
                        XmlNode rootNode = settings.CreateElement("Settings");
                        settings.AppendChild(rootNode);
                        XmlNode userNode = settings.CreateElement("Muted");
                        userNode.InnerText = muted.ToString();
                        rootNode.AppendChild(userNode);
                        userNode = settings.CreateElement("Fullscreen");
                        userNode.InnerText = fullScreen.ToString();
                        rootNode.AppendChild(userNode);
                        settings.Save("Settings.xml");
                    }
                    else if (type == MenuType.Credits)
                    {
                        main.currentSound.Stop();
                        main.currentSound = main.menuSound;
                        main.currentSound.Play();
                    }
                    else if (type == MenuType.HighScores)
                    {
                        main.GameState = GameStates.MainMenu;
                    }
                    type = MenuType.MainMenu;
                    if (main.GameState == GameStates.Pause) main.GameState = GameStates.MainMenu;
                    LoadContent();
                    break;

                case 5:
                    muted = !muted;
                    main.vol = (muted) ? 0 : .025f;
                    break;

                case 6:
                    fullScreen = !fullScreen;
                    Main.Graphics.IsFullScreen = !Main.Graphics.IsFullScreen;
                    Main.Graphics.ApplyChanges();
                    break;

                case 7:
                    type = MenuType.Credits;
                    LoadContent();
                    break;

                case 8:
                    if (selectedKart != "") main.GameState = GameStates.Play;
                    break;

                case 9:
                    main.Exit();
                    break;

                case 10:
                    selectedKart = "Test Kart";
                    break;

                case 11:
                    selectedKart = "TIE";
                    break;

                case 12:
                    selectedKart = "Shopping Cart";
                    break;

                case 13:
                    selectedKart = "Gumin";
                    break;

                default:
                    break;
            }
            try { chosenKart = Main.GameContent.Load<Texture2D>(string.Format("Menus/{0} Preview", selectedKart)); }
            catch { }
        }

        #endregion

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(background, Vector2.Zero, Color.White);
            try
            {
                switch (type)
                {
                    case MenuType.MainMenu:
                        spriteBatch.Draw(title, new Vector2(200, 0), Color.White);
                        spriteBatch.Draw(kartSelect.Texture, kartSelect.Position, Color.White);
                        spriteBatch.Draw(highScores.Texture, highScores.Position, Color.White);
                        spriteBatch.Draw(options.Texture, options.Position, Color.White);
                        spriteBatch.Draw(credits.Texture, credits.Position, Color.White);
                        spriteBatch.Draw(exit.Texture, exit.Position, Color.White);
                        break;

                    case MenuType.Options:
                        spriteBatch.Draw(highScores.Texture, highScores.Position, Color.White);
                        spriteBatch.Draw(menu.Texture, menu.Position, Color.White);
                        spriteBatch.Draw(sound.Texture, sound.Position, Color.White);
                        spriteBatch.Draw(fullscreen.Texture, fullscreen.Position, Color.White);
                        spriteBatch.DrawString(font, soundOn, new Vector2(500, 106), Color.White);
                        spriteBatch.DrawString(font, screenOn, new Vector2(500, 206), Color.White);
                        break;

                    case MenuType.HighScores:
                        spriteBatch.Draw(menu.Texture, menu.Position, Color.White);
                        for (int i = 0; i < scores.HighScores.Length; i++)
                        {
                            spriteBatch.DrawString(font, scores.HighScores[i], new Vector2(250, 50 + (50 * i)), Color.White);
                        }
                        break;

                    case MenuType.Pause:
                        spriteBatch.Draw(inGameMenu, new Vector2(250, 150), Color.White);
                        spriteBatch.Draw(menu.Texture, menu.Position, Color.White);
                        spriteBatch.Draw(start.Texture, start.Position, Color.White);
                        break;

                    case MenuType.KartSelect:
                        spriteBatch.Draw(start.Texture, start.Position, Color.White);
                        spriteBatch.Draw(testKart.Texture, testKart.Position, Color.White);
                        spriteBatch.Draw(tieFighter.Texture, tieFighter.Position, Color.White);
                        spriteBatch.Draw(shoppingCart.Texture, shoppingCart.Position, Color.White);
                        spriteBatch.Draw(guminKart.Texture, guminKart.Position, Color.White);
                        spriteBatch.Draw(menu.Texture, menu.Position, Color.White);
                        spriteBatch.Draw(chosenKart, new Vector2(400, 125), Color.White);
                        break;

                    case MenuType.Credits:
                        spriteBatch.Draw(menu.Texture, menu.Position, Color.White);
                        spriteBatch.DrawString(font, "Aaron Hosler", new Vector2(200, 220), Color.White);
                        spriteBatch.DrawString(font, "Josh Glover", new Vector2(300, 270), Color.White);
                        spriteBatch.DrawString(font, "Colin Holmes", new Vector2(400, 320), Color.White);
                        spriteBatch.Draw(Untitled, Vector2.Zero, Color.White);
                        break;

                    case MenuType.Publisher:
                        spriteBatch.Draw(title, new Vector2(200, 0), Color.White);
                        spriteBatch.DrawString(font, "Published by Cognitive Thought Media", new Vector2(54, 500), Color.White);
                        break;

                    default:
                        break;
                }
            }
            catch { }
        }
    }
}