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
        enum MenuType { Main, Credits };

        Main main;
        MenuType type = MenuType.Main;
        Texture2D unPressed, pressed, eUnPressed, ePressed, cUnPressed, cPressed;
        Button Rectangle, Ellipse, Circle;

        public Menu(Main main)
        {
            this.main = main;
        }

        public void LoadContent()
        {
            switch (type)
            {
                case MenuType.Main:
                    unPressed = Main.GameContent.Load<Texture2D>("Test/Button Base Un-Pressed");
                    pressed = Main.GameContent.Load<Texture2D>("Test/Button Base Pressed");
                    eUnPressed = Main.GameContent.Load<Texture2D>("Test/Ellipse Test 1");
                    ePressed = Main.GameContent.Load<Texture2D>("Test/Ellipse Test 2");
                    cUnPressed = Main.GameContent.Load<Texture2D>("Test/Circular Button Base Un-Pressed");
                    cPressed = Main.GameContent.Load<Texture2D>("Test/Circular Button Base Pressed");

                    Rectangle = new Button(new Vector2(500, 150), 170, 90, 1, Main.CurrentMouse, unPressed, pressed, Main.Graphics.PreferredBackBufferWidth, Main.Graphics.PreferredBackBufferHeight);
                    Ellipse = new Button(new Vector2(200, 300), 2, Main.CurrentMouse, eUnPressed, ePressed, Main.Graphics.PreferredBackBufferWidth, Main.Graphics.PreferredBackBufferHeight);
                    Circle = new Button(new Vector2(100, 100), 100, 3, Main.CurrentMouse, cUnPressed, cPressed, Main.Graphics.PreferredBackBufferWidth, Main.Graphics.PreferredBackBufferHeight);

                    Rectangle.ButtonPressed += StartGame;
                    Circle.ButtonPressed += Credits;
                    break;

                case MenuType.Credits:
                    eUnPressed = Main.GameContent.Load<Texture2D>("Test/Ellipse Test 1");
                    ePressed = Main.GameContent.Load<Texture2D>("Test/Ellipse Test 2");

                    Ellipse = new Button(new Vector2(200, 300), 2, Main.CurrentMouse, eUnPressed, ePressed, Main.Graphics.PreferredBackBufferWidth, Main.Graphics.PreferredBackBufferHeight);

                    Ellipse.ButtonPressed += MainMenu;
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
                    Rectangle.Update(Main.CurrentMouse);
                    Ellipse.Update(Main.CurrentMouse);
                    Circle.Update(Main.CurrentMouse);
                    break;

                case MenuType.Credits:
                    Ellipse.Update(Main.CurrentMouse);
                    break;

                default:
                    break;
            }
        }

        public void Dispose()
        {
            Rectangle.Dispose();
            Ellipse.Dispose();
            Circle.Dispose();

            unPressed.Dispose();
            pressed.Dispose();
            eUnPressed.Dispose();
            ePressed.Dispose();
            cUnPressed.Dispose();
            cPressed.Dispose();
        }

        public void StartGame(object sender, EventArgs args)
        {
            main.GameState = GameStates.Test;
            Rectangle.ButtonPressed -= StartGame;
        }
        public void Credits(object sender, EventArgs args)
        {
            type = MenuType.Credits;
            LoadContent();
            Circle.ButtonPressed -= Credits;
        }
        public void MainMenu(object sender, EventArgs args)
        {
            type = MenuType.Main;
            LoadContent();
            Ellipse.ButtonPressed -= MainMenu;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            try
            {
                switch (type)
                {
                    case MenuType.Main:
                        spriteBatch.Draw(Rectangle.Texture, Rectangle.Position, Color.White);
                        spriteBatch.Draw(Ellipse.Texture, Ellipse.Position, Color.White);
                        spriteBatch.Draw(Circle.Texture, Circle.Position, Color.White);
                        break;

                    case MenuType.Credits:
                        spriteBatch.Draw(Ellipse.Texture, Ellipse.Position, Color.White);
                        break;

                    default:
                        break;
                }
            }
            catch { }
        }
    }
}