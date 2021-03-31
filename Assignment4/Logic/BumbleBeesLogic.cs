using Assignment4.Enum;
using Assignment4.Model;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assignment4.Logic
{
    public class BumbleBeesLogic
    {
        private Texture2D pixel;
        List<Point> pnts;
        private EnemyShip ship;

        public BumbleBeesLogic(Dictionary<Sprites, Texture2D> sprites)
        {
            pixel = sprites[Sprites.Pixel];
            pnts = new List<Point>();
            CreateCurve();
            ship = new EnemyShip(sprites[Sprites.Pixel], pnts, new Vector2(800, 0));
        }
        public void Update(GameTime gameTime)
        {
            ship.Update(gameTime);
        }
        public void Draw(SpriteBatch _sp)
        {
            _sp.Begin();
            foreach(var p in pnts)
            {
                Rectangle srcRect = new Rectangle(p, new Point(3, 3));
                _sp.Draw(pixel, srcRect, Color.Orange);
            }
            ship.Draw(_sp);
            _sp.End();
        }
       
        private void CreateCurve()
        {
            List<Point> curve = new List<Point>();
            for (int a = 0; a < 12; a++)
            {
                float angle = -60 + (180 / 18) * a;
                pnts.Add(RotatePoint((int)(600 + 50), (int)(300 + 100), angle));
            }
           
        }
        private float degToRad(float angle)
        {
            return (float)(angle * (Math.PI / 180));
        }

        private Point RotatePoint(int nx, int ny, float angle)
        {
            float x = 600;
            float y = 300;
            float new_x = (float)(nx) - x;
            float new_y = (float)(ny) - y;

            var sin_ang = Math.Sin(degToRad(angle));
            var cos_ang = Math.Cos(degToRad(angle));
            var tx = (new_x * cos_ang) - (new_y * sin_ang);
            var ty = (new_x * sin_ang) + (new_y * cos_ang);

            Point p = new Point();
            p.X = (int)(tx + x);
            p.Y = (int)(ty + y);
            return p;
        }
    }
}
