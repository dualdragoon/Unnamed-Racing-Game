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
        Texture2D startUnPressed, startPressed, optionsUnPressed, optionsPressed, cUnPressed, cPressed;
        Button start, options, circle;

        public Menu(Main main)
        {
            this.main = main;
        }

        public void LoadContent()
        {
            switch (type)
            {
                case MenuType.Main:
                    startUnPressed = Main.GameContent.Load<Texture2D>("Test/Start");
                    startPressed = Main.GameContent.Load<Texture2D>("Test/Start Pressed");
                    optionsUnPressed = Main.GameContent.Load<Texture2D>("Test/Options");
                    optionsPressed = Main.GameContent.Load<Texture2D>("Test/Options Pressed");
                    cUnPressed = Main.GameContent.Load<Texture2D>("Test/Circular Button Base Un-Pressed");
                    cPressed = Main.GameContent.Load<Texture2D>("Test/Circular Button Base Pressed");

                    start = new Button(new Vector2(500, 150), 173, 53, 1, Main.CurrentMouse, startUnPressed, startPressed, Main.Graphics.PreferredBackBufferWidth, Main.Graphics.PreferredBackBufferHeight);
                    options = new Button(new Vector2(200, 300), 233, 42, 2, Main.CurrentMouse, optionsUnPressed, optionsPressed, Main.Graphics.PreferredBackBufferWidth, Main.Graphics.PreferredBackBufferHeight);
                    circle = new Button(new Vector2(100, 100), 100, 3, Main.CurrentMouse, cUnPressed, cPressed, Main.Graphics.PreferredBackBufferWidth, Main.Graphics.PreferredBackBufferHeight);

                    start.ButtonPressed += StartGame;
                    circle.ButtonPressed += Credits;
                    break;

                case MenuType.Options:
                    optionsUnPressed = Main.GameContent.Load<Texture2D>("Test/Options");
                    optionsPressed = Main.GameContent.Load<Texture2D>("Test/Options Pressed");

                    options = new Button(new Vector2(200, 300), 233, 42, 2, Main.CurrentMouse, optionsUnPressed, optionsPressed, Main.Graphics.PreferredBackBufferWidth, Main.Graphics.PreferredBackBufferHeight);

                    options.ButtonPressed += MainMenu;
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
                    options.Update(Main.CurrentMouse);
                    circle.Update(Main.CurrentMouse);
                    break;

                case MenuType.Options:
                    options.Update(Main.CurrentMouse);
                    break;

                default:
                    break;
            }
        }

        public void Dispose()
        {
            start.Dispose();
            options.Dispose();
            circle.Dispose();

            startUnPressed.Dispose();
            startPressed.Dispose();
            optionsUnPressed.Dispose();
            optionsPressed.Dispose();
            cUnPressed.Dispose();
            cPressed.Dispose();
        }

        public void StartGame(object sender, EventArgs args)
        {
            main.GameState = GameStates.Test;
            start.ButtonPressed -= StartGame;
        }
        public void Credits(object sender, EventArgs args)
        {
            type = MenuType.Options;
            LoadContent();
            circle.ButtonPressed -= Credits;
        }
        public void MainMenu(object sender, EventArgs args)
        {
            type = MenuType.Main;
            LoadContent();
            options.ButtonPressed -= MainMenu;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            try
            {
                switch (type)
                {
                    case MenuType.Main:
                        spriteBatch.Draw(start.Texture, start.Position, Color.White);
                        spriteBatch.Draw(options.Texture, options.Position, Color.White);
                        spriteBatch.Draw(circle.Texture, circle.Position, Color.White);
                        break;

                    case MenuType.Options:
                        spriteBatch.Draw(options.Texture, options.Position, Color.White);
                        break;

                    default:
                        break;
                }
            }
            catch { }
        }
    }
}