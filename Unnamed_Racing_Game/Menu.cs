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
    class Menu
    {
        enum MenuType { Main, Options, HighScores };

        bool muted;
        Main main;
        MenuType type = MenuType.Main;
        string soundOn;
        Texture2D startUnPressed, startPressed, highScoresUnPressed, highScoresPressed, optionsUnPressed, optionsPressed, title, menuPressed, menuUnPressed, Worthless, soundUnPressed, soundPressed;
        SpriteFont font;
        Button start, highScores, options, menu, sound;

        public Menu(Main main)
        {
            this.main = main;
        }

        public void LoadContent()
        {
            font = Main.GameContent.Load<SpriteFont>("Font/Font1");
            switch (type)
            {
                case MenuType.Main:
                    title = Main.GameContent.Load<Texture2D>("Menus/Logo");
                    Worthless = Main.GameContent.Load<Texture2D>("Menus/Worthless");
                    startUnPressed = Main.GameContent.Load<Texture2D>("Menus/Start");
                    startPressed = Main.GameContent.Load<Texture2D>("Menus/Start Pressed");
                    highScoresUnPressed = Main.GameContent.Load<Texture2D>("Menus/High Scores");
                    highScoresPressed = Main.GameContent.Load<Texture2D>("Menus/High Scores Pressed");
                    optionsUnPressed = Main.GameContent.Load<Texture2D>("Menus/Options");
                    optionsPressed = Main.GameContent.Load<Texture2D>("Menus/Options Pressed");
                    menuUnPressed = Main.GameContent.Load<Texture2D>("Menus/Menu");
                    menuPressed = Main.GameContent.Load<Texture2D>("Menus/Menu Pressed");
                    

                    start = new Button(new Vector2(500, 500), 173, 53, 1, Main.CurrentMouse, startUnPressed, startPressed, Main.Graphics.PreferredBackBufferWidth, Main.Graphics.PreferredBackBufferHeight);
                    highScores = new Button(new Vector2(200, 300), 324, 55, 2, Main.CurrentMouse, highScoresUnPressed, highScoresPressed, Main.Graphics.PreferredBackBufferWidth, Main.Graphics.PreferredBackBufferHeight);
                    options = new Button(new Vector2(100, 500), 233, 42, 3, Main.CurrentMouse, optionsUnPressed, optionsPressed, Main.Graphics.PreferredBackBufferWidth, Main.Graphics.PreferredBackBufferHeight);
                   
                    start.ButtonPressed += StartGame;
                    options.ButtonPressed += Options;
                    highScores.ButtonPressed += HighScores;
                    break;

                case MenuType.Options:
                    highScoresUnPressed = Main.GameContent.Load<Texture2D>("Menus/High Scores");
                    highScoresPressed = Main.GameContent.Load<Texture2D>("Menus/High Scores Pressed");
                    menuUnPressed = Main.GameContent.Load<Texture2D>("Menus/Menu");
                    menuPressed = Main.GameContent.Load<Texture2D>("Menus/Menu Pressed");
                    soundUnPressed = Main.GameContent.Load<Texture2D>("Menus/Sound");
                    soundPressed = Main.GameContent.Load<Texture2D>("Menus/SoundPressed");


                    highScores = new Button(new Vector2(470, 540), 324, 55, 2, Main.CurrentMouse, highScoresUnPressed, highScoresPressed, Main.Graphics.PreferredBackBufferWidth, Main.Graphics.PreferredBackBufferHeight);
                    menu = new Button(new Vector2(100, 300), 168, 54, 4, Main.CurrentMouse, menuUnPressed, menuPressed, Main.Graphics.PreferredBackBufferWidth, Main.Graphics.PreferredBackBufferHeight);
                    sound = new Button(new Vector2(100, 100), 164, 53, 4, Main.CurrentMouse, soundUnPressed, soundPressed, Main.Graphics.PreferredBackBufferWidth, Main.Graphics.PreferredBackBufferHeight);

                    menu.ButtonPressed += MainMenu;
                    highScores.ButtonPressed += HighScores;
                    sound.ButtonPressed += Muted;
                    break;

                case MenuType.HighScores:
                    menuUnPressed = Main.GameContent.Load<Texture2D>("Menus/Menu");
                    menuPressed = Main.GameContent.Load<Texture2D>("Menus/Menu Pressed");
                    optionsUnPressed = Main.GameContent.Load<Texture2D>("Menus/Options");
                    optionsPressed = Main.GameContent.Load<Texture2D>("Menus/Options Pressed");

                    options = new Button(new Vector2(100, 500), 233, 42, 3, Main.CurrentMouse, optionsUnPressed, optionsPressed, Main.Graphics.PreferredBackBufferWidth, Main.Graphics.PreferredBackBufferHeight);
                    menu = new Button(new Vector2(100, 300), 168, 54, 4, Main.CurrentMouse, menuUnPressed, menuPressed, Main.Graphics.PreferredBackBufferWidth, Main.Graphics.PreferredBackBufferHeight);

                    menu.ButtonPressed += MainMenu;
                    highScores.ButtonPressed += HighScores;
                    break;


                default:
                    break;
            }
        }

        public void Update(GameTime gameTime)
        {
            switch (type)
            {
                case MenuType.Main:
                    start.Update(Main.CurrentMouse);
                    highScores.Update(Main.CurrentMouse);
                    options.Update(Main.CurrentMouse);
                    break;

                case MenuType.Options:
                    highScores.Update(Main.CurrentMouse);
                    menu.Update(Main.CurrentMouse);
                    sound.Update(Main.CurrentMouse);
                    soundOn = (muted) ? "Off" : "On";
                    break;

                case MenuType.HighScores:
                    menu.Update(Main.CurrentMouse);

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

        public void StartGame(object sender, EventArgs args)
        {
            main.GameState = GameStates.Test;
            //Dispose();
            start.ButtonPressed -= StartGame;
        }
        public void Options(object sender, EventArgs args)
        {
            type = MenuType.Options;
            //Dispose();
            LoadContent();
            options.ButtonPressed -= Options;
        }
        public void MainMenu(object sender, EventArgs args)
        {
            type = MenuType.Main;
            //Dispose();
            LoadContent();
            menu.ButtonPressed -= MainMenu;
        }
        public void HighScores(object sender, EventArgs args)
        {
            type = MenuType.HighScores;
            //Dispose();
            LoadContent();
            highScores.ButtonPressed -= HighScores;
        }

        public void Muted(object sender, EventArgs args)
        {
            muted = !muted;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            try
            {
                switch (type)
                {
                    case MenuType.Main:
                        spriteBatch.Draw(title, new Vector2(200, 0), Color.White);
                        spriteBatch.Draw(start.Texture, start.Position, Color.White);
                        spriteBatch.Draw(highScores.Texture, highScores.Position, Color.White);
                        spriteBatch.Draw(options.Texture, options.Position, Color.White);
                        break;

                    case MenuType.Options:
                        spriteBatch.Draw(highScores.Texture, highScores.Position, Color.White);
                        spriteBatch.Draw(menu.Texture, menu.Position, Color.White);
                        spriteBatch.Draw(sound.Texture, sound.Position, Color.White);
                        spriteBatch.DrawString(font, soundOn,new Vector2(400,105), Color.Black);
                        break;

                    case MenuType.HighScores:
                        spriteBatch.Draw(menu.Texture, menu.Position, Color.White);
                        spriteBatch.Draw(Worthless, new Vector2(100, 0), Color.White);
                        break;

                    default:
                        break;
                }
            }
            catch { }
        }
    }
}