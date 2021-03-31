using Assignment4.Enum;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace Assignment4.Model
{
    public class Button
    {
        private Rectangle rectangle { get; set; }
        private Texture2D texture2D { get; set; }
        private SpriteFont font { get; set; }
        private MouseState previousMouseState;
        private string text { get; set; }
        public bool isClicked { get; set; }
        public GameMode gameMode { get; private set; }

        public Button() { }
        public Button(Texture2D texture,  Rectangle rect, SpriteFont f, string t, GameMode gm)
        {
            texture2D = texture;
            rectangle = rect;
            text = t;
            font = f;
            gameMode = gm;
        }
        public void Update()
        {
            MouseState currentMouseState = Mouse.GetState();

            //If Left mouse button clicked
            if (isClickedButton() && previousMouseState != null && currentMouseState.LeftButton == ButtonState.Released && previousMouseState.LeftButton == ButtonState.Pressed)
                isClicked = true;
            else
                isClicked = false;

            previousMouseState = currentMouseState;
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            var x = (rectangle.X + (rectangle.Width / 2)) - (font.MeasureString(text).X / 2);
            var y = (rectangle.Y + (rectangle.Height / 2)) - (font.MeasureString(text).Y / 2);

            spriteBatch.Draw(texture2D, rectangle, Color.White);
            spriteBatch.DrawString(font, this.text, new Vector2(x, y), Color.Black);
        }
        private bool isClickedButton()
        {
            return (previousMouseState.X < rectangle.X + rectangle.Width &&
                    previousMouseState.X > rectangle.X &&
                    previousMouseState.Y < rectangle.Y + rectangle.Height &&
                    previousMouseState.Y > rectangle.Y);
        }
    }
}
