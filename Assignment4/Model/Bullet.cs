using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Assignment4.Model
{
    public class Bullet
    {
        private Vector2 location;
        private Texture2D pixel;
        public bool visible;
        
        public Bullet(Vector2 l, Texture2D p)
        {
            this.location = l;
            pixel = p;
            visible = true;

        }
        public void Update(GameTime gameTime)
        {
            visible = location.Y > 0;
            if(visible)
                location.Y -= 2;        
        }
        public void Draw(SpriteBatch _sp)
        {
            _sp.Draw(pixel, new Rectangle(location.ToPoint(), new Point(2, 8)), Color.OrangeRed);
        }
    }
}
