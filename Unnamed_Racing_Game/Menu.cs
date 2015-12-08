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
        Main main;
        Texture2D unPressed, pressed, eUnPressed, ePressed, cUnPressed, cPressed;
        Button Rectangle, Ellipse, Circle;

        public Menu(Main main)
        {
            this.main = main;
        }

        public void LoadContent()
        {
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
        }

        public void Update(GameTime gameTime)
        {
            Rectangle.Update(Main.CurrentMouse);
            Ellipse.Update(Main.CurrentMouse);
            Circle.Update(Main.CurrentMouse);
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

        public void Draw(SpriteBatch spriteBatch)
        {
            try
            {
                spriteBatch.Draw(Rectangle.Texture, Rectangle.Position, Color.White);
                spriteBatch.Draw(Ellipse.Texture, Ellipse.Position, Color.White);
                spriteBatch.Draw(Circle.Texture, Circle.Position, Color.White);
            }
            catch { }
        }
    }
}