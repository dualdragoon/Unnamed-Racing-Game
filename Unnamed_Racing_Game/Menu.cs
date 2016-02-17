using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Duality.Interaction;
using SharpDX;
using SharpDX.Toolkit;
using SharpDX.Toolkit.Content;
using SharpDX.Toolkit.Graphics;
using SharpDX.Toolkit.Input;

namespace Kross_Kart
{
    enum MenuType { MainMenu, Options, HighScores, Pause, Credits, KartSelect };

    class Menu
    {
        bool muted, screen;

        Main main;
        public MenuType type = MenuType.MainMenu;
        string SoundOn, ScreenOn;
        Texture2D background, startUnPressed, startPressed, kartSelectUnPressed, kartSelectPressed, highScoresUnPressed, highScoresPressed, optionsUnPressed, optionsPressed, title, menuPressed, menuUnPressed, Worthless, soundUnPressed, soundPressed, fullscreenUnPressed, fullscreenPressed, ExitUnHighLighted, ExitHighLighted, ResumeUnHighlighted, ResumeHighlighted, InGameMenu, creditsUnPressed, creditsPressed;
        SpriteFont font;
        Button start, highScores, options, menu, sound, fullscreen, credits, kartSelect;

        public Menu(Main main)
        {
            this.main = main;
        }

        public void LoadContent()
        {
            background = Main.GameContent.Load<Texture2D>("Menus/Background 2");
            font = Main.GameContent.Load<SpriteFont>("Font/Font1");
            switch (type)
            {
                case MenuType.MainMenu:
                    title = Main.GameContent.Load<Texture2D>("Menus/Logo");
                    Worthless = Main.GameContent.Load<Texture2D>("Menus/Worthless");
                    kartSelectUnPressed = Main.GameContent.Load<Texture2D>("Menus/Start");
                    kartSelectPressed = Main.GameContent.Load<Texture2D>("Menus/Start Pressed");
                    highScoresUnPressed = Main.GameContent.Load<Texture2D>("Menus/High Scores");
                    highScoresPressed = Main.GameContent.Load<Texture2D>("Menus/High Scores Pressed");
                    optionsUnPressed = Main.GameContent.Load<Texture2D>("Menus/Options");
                    optionsPressed = Main.GameContent.Load<Texture2D>("Menus/Options Pressed");
                    menuUnPressed = Main.GameContent.Load<Texture2D>("Menus/Menu");
                    menuPressed = Main.GameContent.Load<Texture2D>("Menus/Menu Pressed");
                    creditsUnPressed = Main.GameContent.Load<Texture2D>("Menus/CreditsUnPressed");
                    creditsPressed = Main.GameContent.Load<Texture2D>("Menus/CreditsPressed");


                    kartSelect = new Button(new Vector2(500, 500), 173, 53, 1, Main.CurrentMouse, kartSelectUnPressed, kartSelectPressed, Main.Graphics.PreferredBackBufferWidth, Main.Graphics.PreferredBackBufferHeight);
                    highScores = new Button(new Vector2(200, 300), 324, 55, 2, Main.CurrentMouse, highScoresUnPressed, highScoresPressed, Main.Graphics.PreferredBackBufferWidth, Main.Graphics.PreferredBackBufferHeight);
                    options = new Button(new Vector2(100, 500), 233, 42, 3, Main.CurrentMouse, optionsUnPressed, optionsPressed, Main.Graphics.PreferredBackBufferWidth, Main.Graphics.PreferredBackBufferHeight);
                    credits = new Button(new Vector2(200, 400), 191, 47, 7, Main.CurrentMouse, creditsUnPressed, creditsPressed, Main.Graphics.PreferredBackBufferWidth, Main.Graphics.PreferredBackBufferHeight);

                    credits.ButtonPressed += ButtonPressed;
                    kartSelect.ButtonPressed += ButtonPressed;
                    options.ButtonPressed += ButtonPressed;
                    highScores.ButtonPressed += ButtonPressed;
                    break;

                case MenuType.Options:
                    highScoresUnPressed = Main.GameContent.Load<Texture2D>("Menus/High Scores");
                    highScoresPressed = Main.GameContent.Load<Texture2D>("Menus/High Scores Pressed");
                    menuUnPressed = Main.GameContent.Load<Texture2D>("Menus/Menu");
                    menuPressed = Main.GameContent.Load<Texture2D>("Menus/Menu Pressed");
                    soundUnPressed = Main.GameContent.Load<Texture2D>("Menus/Sound");
                    soundPressed = Main.GameContent.Load<Texture2D>("Menus/SoundPressed");
                    fullscreenUnPressed = Main.GameContent.Load<Texture2D>("Menus/FullScreen");
                    fullscreenPressed = Main.GameContent.Load<Texture2D>("Menus/FullScreenPressed");

                    highScores = new Button(new Vector2(470, 540), 324, 55, 2, Main.CurrentMouse, highScoresUnPressed, highScoresPressed, Main.Graphics.PreferredBackBufferWidth, Main.Graphics.PreferredBackBufferHeight);
                    menu = new Button(new Vector2(100, 300), 168, 54, 4, Main.CurrentMouse, menuUnPressed, menuPressed, Main.Graphics.PreferredBackBufferWidth, Main.Graphics.PreferredBackBufferHeight);
                    sound = new Button(new Vector2(100, 100), 164, 53, 5, Main.CurrentMouse, soundUnPressed, soundPressed, Main.Graphics.PreferredBackBufferWidth, Main.Graphics.PreferredBackBufferHeight);
                    fullscreen = new Button(new Vector2(100, 200), 248, 45, 6, Main.CurrentMouse, fullscreenUnPressed, fullscreenPressed, Main.Graphics.PreferredBackBufferWidth, Main.Graphics.PreferredBackBufferHeight);

                    menu.ButtonPressed += ButtonPressed;
                    highScores.ButtonPressed += ButtonPressed;
                    sound.ButtonPressed += ButtonPressed;
                    fullscreen.ButtonPressed += ButtonPressed;
                    break;

                case MenuType.HighScores:
                    menuUnPressed = Main.GameContent.Load<Texture2D>("Menus/Menu");
                    menuPressed = Main.GameContent.Load<Texture2D>("Menus/Menu Pressed");
                    optionsUnPressed = Main.GameContent.Load<Texture2D>("Menus/Options");
                    optionsPressed = Main.GameContent.Load<Texture2D>("Menus/Options Pressed");

                    options = new Button(new Vector2(100, 500), 233, 42, 3, Main.CurrentMouse, optionsUnPressed, optionsPressed, Main.Graphics.PreferredBackBufferWidth, Main.Graphics.PreferredBackBufferHeight);
                    menu = new Button(new Vector2(100, 300), 168, 54, 4, Main.CurrentMouse, menuUnPressed, menuPressed, Main.Graphics.PreferredBackBufferWidth, Main.Graphics.PreferredBackBufferHeight);

                    menu.ButtonPressed += ButtonPressed;
                    highScores.ButtonPressed += ButtonPressed;
                    break;

                case MenuType.Pause:
                    InGameMenu = Main.GameContent.Load<Texture2D>("Menus/InGameMenu");
                    ExitUnHighLighted = Main.GameContent.Load<Texture2D>("Menus/Exit");
                    ExitHighLighted = Main.GameContent.Load<Texture2D>("Menus/ExitHighlight");
                    ResumeHighlighted = Main.GameContent.Load<Texture2D>("Menus/ResumeHighlight");
                    ResumeUnHighlighted = Main.GameContent.Load<Texture2D>("Menus/Resume");

                    start = new Button(new Vector2(325, 200), 123, 38, 8, Main.CurrentMouse, ResumeUnHighlighted, ResumeHighlighted, Main.Graphics.PreferredBackBufferWidth, Main.Graphics.PreferredBackBufferHeight);
                    menu = new Button(new Vector2(350, 300), 79, 29, 4, Main.CurrentMouse, ExitUnHighLighted, ExitHighLighted, Main.Graphics.PreferredBackBufferWidth, Main.Graphics.PreferredBackBufferHeight);

                    start.ButtonPressed += ButtonPressed;
                    menu.ButtonPressed += ButtonPressed;
                    break;

                case MenuType.KartSelect:
                    startUnPressed = Main.GameContent.Load<Texture2D>("Menus/Start");
                    startPressed = Main.GameContent.Load<Texture2D>("Menus/Start Pressed");

                    start = new Button(new Vector2(500, 500), 173, 53, 8, Main.CurrentMouse, startUnPressed, startPressed, Main.Graphics.PreferredBackBufferWidth, Main.Graphics.PreferredBackBufferHeight);

                    start.ButtonPressed += ButtonPressed;
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
                    break;

                case MenuType.Options:
                    highScores.Update(Main.CurrentMouse);
                    menu.Update(Main.CurrentMouse);
                    sound.Update(Main.CurrentMouse);
                    fullscreen.Update(Main.CurrentMouse);
                    ScreenOn = (screen) ? "On" : "Off";
                    SoundOn = (muted) ? "Off" : "On";
                    break;

                case MenuType.HighScores:
                    menu.Update(Main.CurrentMouse);
                    break;

                case MenuType.Pause:
                    menu.Update(Main.CurrentMouse);
                    start.Update(Main.CurrentMouse);
                    break;

                case MenuType.Credits:
                    break;

                case MenuType.KartSelect:
                    start.Update(Main.CurrentMouse);
                    break;
                default:
                    break;
            }
        }

        public void Dispose()
        {
            start.Dispose();
            highScores.Dispose();
            options.Dispose();
            menu.Dispose();

            startUnPressed.Dispose();
            startPressed.Dispose();
            highScoresUnPressed.Dispose();
            highScoresPressed.Dispose();
            optionsUnPressed.Dispose();
            optionsPressed.Dispose();
            menuUnPressed.Dispose();
            menuPressed.Dispose();

        }

        #region Event Handler

        public void ButtonPressed(object sender, EventArgs args)
        {
            switch (((Button)sender).ButtonNum)
            {
                case 1:
                    type = MenuType.KartSelect;
                    LoadContent();
                    break;

                case 2:
                    type = MenuType.HighScores;
                    //Dispose();
                    LoadContent();
                    break;

                case 3:
                    type = MenuType.Options;
                    //Dispose();
                    LoadContent();
                    break;

                case 4:
                    type = MenuType.MainMenu;
                    if (main.GameState == GameStates.Pause) main.GameState = GameStates.MainMenu;
                    //Dispose();
                    LoadContent();
                    break;

                case 5:
                    muted = !muted;
                    break;

                case 6:
                    screen = !screen;
                    Main.Graphics.IsFullScreen = !Main.Graphics.IsFullScreen;
                    Main.Graphics.ApplyChanges();
                    break;

                case 7:
                    type = MenuType.Credits;
                    break;

                case 8:
                    main.GameState = GameStates.Play;
                    //Dispose();
                    break;

                default:
                    break;
            }
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
                        break;

                    case MenuType.Options:
                        spriteBatch.Draw(highScores.Texture, highScores.Position, Color.White);
                        spriteBatch.Draw(menu.Texture, menu.Position, Color.White);
                        spriteBatch.Draw(sound.Texture, sound.Position, Color.White);
                        spriteBatch.Draw(fullscreen.Texture, fullscreen.Position, Color.White);
                        spriteBatch.DrawString(font, SoundOn, new Vector2(500, 106), Color.Black);
                        spriteBatch.DrawString(font, ScreenOn, new Vector2(500, 206), Color.Black);
                        break;

                    case MenuType.HighScores:
                        spriteBatch.Draw(menu.Texture, menu.Position, Color.White);
                        spriteBatch.Draw(Worthless, new Vector2(100, 0), Color.White);
                        break;

                    case MenuType.Pause:
                        spriteBatch.Draw(InGameMenu, new Vector2(250, 150), Color.White);
                        spriteBatch.Draw(menu.Texture, menu.Position, Color.White);
                        spriteBatch.Draw(start.Texture, start.Position, Color.White);
                        break;

                    case MenuType.KartSelect:
                        spriteBatch.Draw(start.Texture, start.Position, Color.White);
                        break;

                    default:
                        break;
                }
            }
            catch { }
        }
    }
}