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

namespace Unnamed_Racing_Game
{
    class Menu
    {
        Texture2D unPressed, pressed, eUnPressed, ePressed, cUnPressed, cPressed;
        Button test, test2, test3;

        public void LoadContent()
        {
            unPressed = Main.GameContent.Load<Texture2D>("Test/Button Base Un-Pressed");
            pressed = Main.GameContent.Load<Texture2D>("Test/Button Base Pressed");
            eUnPressed = Main.GameContent.Load<Texture2D>("Test/Ellipse Test 1");
            ePressed = Main.GameContent.Load<Texture2D>("Test/Ellipse Test 2");
            cUnPressed = Main.GameContent.Load<Texture2D>("Test/Circular Button Base Un-Pressed");
            cPressed = Main.GameContent.Load<Texture2D>("Test/Circular Button Base Pressed");

            test = new Button(new Vector2(300, 150), 170, 90, 1, Main.CurrentMouse, unPressed, pressed, Main.Graphics.PreferredBackBufferWidth, Main.Graphics.PreferredBackBufferHeight);
            test2 = new Button(new Vector2(200, 300), 2, Main.CurrentMouse, eUnPressed, ePressed, Main.Graphics.PreferredBackBufferWidth, Main.Graphics.PreferredBackBufferHeight);
            test3 = new Button(new Vector2(100, 100), 100, 3, Main.CurrentMouse, cUnPressed, cPressed, Main.Graphics.PreferredBackBufferWidth, Main.Graphics.PreferredBackBufferHeight);
        }

        public void Update(GameTime gameTime)
        {
            test.Update(Main.CurrentMouse);
            test2.Update(Main.CurrentMouse);
            test3.Update(Main.CurrentMouse);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(test.Texture, test.Position, Color.White);
            spriteBatch.Draw(test2.Texture, test2.Position, Color.White);
            spriteBatch.Draw(test3.Texture, test3.Position, Color.White);
        }
    }
}
