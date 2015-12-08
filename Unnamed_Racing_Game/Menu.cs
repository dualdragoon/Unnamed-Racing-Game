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
        Button test, test2, test3;

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

            test = new Button(new Vector2(500, 150), 170, 90, 1, Main.CurrentMouse, unPressed, pressed, Main.Graphics.PreferredBackBufferWidth, Main.Graphics.PreferredBackBufferHeight);
            test2 = new Button(new Vector2(200, 300), 2, Main.CurrentMouse, eUnPressed, ePressed, Main.Graphics.PreferredBackBufferWidth, Main.Graphics.PreferredBackBufferHeight);
            test3 = new Button(new Vector2(100, 100), 100, 3, Main.CurrentMouse, cUnPressed, cPressed, Main.Graphics.PreferredBackBufferWidth, Main.Graphics.PreferredBackBufferHeight);

            test.ButtonPressed += StartGame;
        }

        public void Update(GameTime gameTime)
        {
            test.Update(Main.CurrentMouse);
            test2.Update(Main.CurrentMouse);
            test3.Update(Main.CurrentMouse);
        }

        public void Dispose()
        {
            test.Dispose();
            test2.Dispose();
            test3.Dispose();

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
            test.ButtonPressed -= StartGame;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            try
            {
                spriteBatch.Draw(test.Texture, test.Position, Color.White);
                spriteBatch.Draw(test2.Texture, test2.Position, Color.White);
                spriteBatch.Draw(test3.Texture, test3.Position, Color.White);
            }
            catch { }
        }
    }
}