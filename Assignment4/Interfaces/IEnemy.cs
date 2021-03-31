using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Assignment4.Interfaces
{
    public interface IEnemy
    {
        void Update(GameTime gameTime);
        void Draw(SpriteBatch _sp);
    }
}
