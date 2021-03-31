using Assignment4.Enum;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace Assignment4.Model
{
    public class Player
    {
        private Vector2 location;
        private Point spriteSize;
        private Texture2D sprite;
        private Texture2D pixel;
        private int lives;
        private List<Bullet> bullets;
        private TimeSpan lastShoot;
        KeyboardState previousState;
        public Player(Dictionary<Sprites, Texture2D> sprites)
        {

            location = new Vector2(600, 800);
            sprite = sprites[Sprites.player];
            pixel = sprites[Sprites.Pixel];
            pixel.SetData<Color>(new Color[] { Color.OrangeRed });
            lives = 3;
            int h = (80 * sprite.Height)/sprite.Width;
            spriteSize = new Point(80, h);
            bullets = new List<Bullet>();
            lastShoot = new TimeSpan(0, 0, 0, 0, 0);
        }

        public void Update(GameTime gameTime)
        {
            KeyboardState state = Keyboard.GetState();
            lastShoot -= gameTime.ElapsedGameTime;
            if (state.IsKeyDown(Keys.Left))
            {
                location.X -= 2;
            }
            if (state.IsKeyDown(Keys.Right))
            {
                location.X += 2;
            }
            if (state.IsKeyDown(Keys.Space))
            {
                if(lastShoot.TotalMilliseconds < 0 || !previousState.IsKeyDown(Keys.Space))
                {
                    bullets.Add(new Bullet(new Vector2(location.X + (spriteSize.X / 2), location.Y), pixel));
                    lastShoot = new TimeSpan(0, 0, 0, 0, 500);
                }
               
            }
            bullets.ForEach(b => b.Update(gameTime));
            bullets.RemoveAll(b => !b.visible);
            previousState = state;
        }
        public void Draw(SpriteBatch _sp)
        {
            _sp.Begin();
                _sp.Draw(sprite, new Rectangle(location.ToPoint(), spriteSize), Color.White);
                bullets.ForEach(b => b.Draw(_sp));
            _sp.End();
        }
    }
}
