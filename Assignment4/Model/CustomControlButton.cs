using Assignment4.Enum;
using Assignment4.Logic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assignment4.Model
{
    public class CustomControlButton 
    {
        private Rectangle rectangle { get; set; }
        private Texture2D texture2D { get; set; }
        private SpriteFont font { get; set; }
        private MouseState previousMouseState;
        private string text { get; set; }
        private string titleText { get; set; }
        public bool canUpdate { get; set; }
        private UserControl userControl { get; set; }
        public Keys newKey { get; private set; }
        public CustomControlButton(Texture2D texture, Rectangle rect, SpriteFont f, Keys key, UserControl cntrl)
        {
            texture2D = texture;
            rectangle = rect;
            font = f;
            canUpdate = false;
            userControl = cntrl;
            this.text = key.ToString();
            this.titleText = cntrl == UserControl.Left ? "Rotate Left: " : (cntrl == UserControl.Right ? "Rotate Right: ": "Shoot");
        }

        public UserControl Update(UserControl focusedControl, UserControlKeys userControlKeys )
        {
            MouseState currentMouseState = Mouse.GetState();
            this.text = userControlKeys.key.ToString();
            //If Left mouse button clicked
            if ((previousMouseState != null
                    && new UserControl[] { UserControl.None, userControl}.Contains(focusedControl)
                    && isClickedButton()
                    && currentMouseState.LeftButton == ButtonState.Released
                    && previousMouseState.LeftButton == ButtonState.Pressed) 
                || canUpdate)
                canUpdate = true;
            else
                canUpdate = false;

            if (canUpdate)
            {
                text = "Enter New Key";
                KeyboardState state = Keyboard.GetState();
                if(state.GetPressedKeys().Length > 0)
                {
                    newKey = state.GetPressedKeys()[0];
                    this.text = newKey.ToString();
                    canUpdate = false;
                    previousMouseState = currentMouseState;
                    return UserControl.None;
                }

            }

            previousMouseState = currentMouseState;
            return canUpdate ? this.userControl : focusedControl;
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            
            var x = (rectangle.X + (rectangle.Width / 2)) - (font.MeasureString(text).X / 2);
            var y = (rectangle.Y + (rectangle.Height / 2)) - (font.MeasureString(text).Y / 2);

            if(canUpdate)
            {
                spriteBatch.DrawString(font, titleText, new Vector2(rectangle.X - 200, rectangle.Y), Color.Red);
                spriteBatch.Draw(texture2D, rectangle, Color.Red);
                spriteBatch.DrawString(font, this.text, new Vector2(x, y), Color.Blue);
            }
            else
            {
                spriteBatch.DrawString(font, titleText, new Vector2(rectangle.X - 200, rectangle.Y), Color.White);
                spriteBatch.Draw(texture2D, rectangle, Color.Blue);
                spriteBatch.DrawString(font, this.text, new Vector2(x, y), Color.White);
            }
            
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
