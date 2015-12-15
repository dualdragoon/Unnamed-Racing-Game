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
        enum MenuType { Main, Options };

        Main main;
        MenuType type = MenuType.Main;
        Texture2D startUnPressed, startPressed, highScoresUnPressed, highScoresPressed, optionsUnPressed, optionsPressed, title, menuPressed, menuUnPressed;
        Button start, highScore, options, menu;

        public Menu(Main main)
        {
            this.main = main;
        }

        public void LoadContent()
        {
            switch (type)
            {
                case MenuType.Main:
                    title = Main.GameContent.Load<Texture2D>("Menus/Logo");
                    startUnPressed = Main.GameContent.Load<Texture2D>("Menus/Start");
                    startPressed = Main.GameContent.Load<Texture2D>("Menus/Start Pressed");
                    highScoresUnPressed = Main.GameContent.Load<Texture2D>("Menus/High Scores");
                    highScoresPressed = Main.GameContent.Load<Texture2D>("Menus/High Scores Pressed");
                    optionsUnPressed = Main.GameContent.Load<Texture2D>("Menus/Options");
                    optionsPressed = Main.GameContent.Load<Texture2D>("Menus/Options Pressed");
                    menuUnPressed = Main.GameContent.Load<Texture2D>("Menus/Menu");
                    menuPressed = Main.GameContent.Load<Texture2D>("Menus/Menu Pressed");

                    start = new Button(new Vector2(500, 500), 173, 53, 1, Main.CurrentMouse, startUnPressed, startPressed, Main.Graphics.PreferredBackBufferWidth, Main.Graphics.PreferredBackBufferHeight);
                    highScore = new Button(new Vector2(200, 300), 324, 55, 2, Main.CurrentMouse, highScoresUnPressed, highScoresPressed, Main.Graphics.PreferredBackBufferWidth, Main.Graphics.PreferredBackBufferHeight);
                    options = new Button(new Vector2(100, 500), 233, 42, 3, Main.CurrentMouse, optionsUnPressed, optionsPressed, Main.Graphics.PreferredBackBufferWidth, Main.Graphics.PreferredBackBufferHeight);
                   
                    start.ButtonPressed += StartGame;
                    options.ButtonPressed += Options;
                    break;

                case MenuType.Options:
                    highScoresUnPressed = Main.GameContent.Load<Texture2D>("Menus/High Scores");
                    highScoresPressed = Main.GameContent.Load<Texture2D>("Menus/High Scores Pressed");

                    highScore = new Button(new Vector2(470, 540), 324, 55, 2, Main.CurrentMouse, highScoresUnPressed, highScoresPressed, Main.Graphics.PreferredBackBufferWidth, Main.Graphics.PreferredBackBufferHeight);
                    menu = new Button(new Vector2(100, 300), 168, 54, 4, Main.CurrentMouse, menuUnPressed, menuPressed, Main.Graphics.PreferredBackBufferWidth, Main.Graphics.PreferredBackBufferHeight);

                    menu.ButtonPressed += MainMenu;
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
                    highScore.Update(Main.CurrentMouse);
                    options.Update(Main.CurrentMouse);
                    break;

                case MenuType.Options:
                    highScore.Update(Main.CurrentMouse);
                    menu.Update(Main.CurrentMouse);
                    break;

                default:
                    break;
            }
        }

        public void Dispose()
        {
            start.Dispose();
            highScore.Dispose();
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
            start.ButtonPressed -= StartGame;
        }
        public void Options(object sender, EventArgs args)
        {
            type = MenuType.Options;
            LoadContent();
            options.ButtonPressed -= Options;
        }
        public void MainMenu(object sender, EventArgs args)
        {
            type = MenuType.Main;
            LoadContent();
            menu.ButtonPressed -= MainMenu;
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
                        spriteBatch.Draw(highScore.Texture, highScore.Position, Color.White);
                        spriteBatch.Draw(options.Texture, options.Position, Color.White);
                        break;

                    case MenuType.Options:
                        spriteBatch.Draw(highScore.Texture, highScore.Position, Color.White);
                        spriteBatch.Draw(menu.Texture, menu.Position, Color.White);
                        break;

                    default:
                        break;
                }
            }
            catch { }
        }
    }
}