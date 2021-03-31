using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace Assignment4.Model
{
    public class EnemyShip
    {
        private Vector2 location;
        private Vector2 trajectory;
        private int hit;
        private Texture2D sprite;
        private Point spriteSize;
        private List<Point> points;
        private List<Point> CurrentMovemnet;
        private int pointIndex;
        private Vector2 speed;
        private bool stop;
        
        public EnemyShip(Texture2D s, List<Point> p, Vector2 startPoint)
        {
            sprite = s;
            points = p;
            location = startPoint;
            CurrentMovemnet = points;
            pointIndex = 0;
            hit = 0;
            RecalculateTrajectory();
            speed = new Vector2(.1f, .1f);
            spriteSize = new Point(8, 8);
            stop = false;
        }
        public void Update(GameTime gameTime)
        {
            if (!stop)
            {
                Point target = CurrentMovemnet[pointIndex];
                double angle = getAngle(target);
                double deg = angle * 180.0 / Math.PI;
                //location += (trajectory * speed * new Vector2((float)gameTime.ElapsedGameTime.TotalSeconds, (float)gameTime.ElapsedGameTime.TotalSeconds));
                double thrustX = Math.Sin(angle);
                double thrustY = Math.Cos(angle);

                location.X -= (float)((thrustX * 20* gameTime.ElapsedGameTime.TotalSeconds));
                location.Y += (float)(thrustY *20 * gameTime.ElapsedGameTime.TotalSeconds);

                if ((int)target.X == (int)location.X && (int)target.Y == (int)location.Y)
                {
                    if (pointIndex == CurrentMovemnet.Count - 1)
                        stop = true;
                    else
                    {
                        ++pointIndex;
                    }
                    
                } 
            }
        }
        public void Draw(SpriteBatch _sp)
        {
            _sp.Draw(sprite, new Rectangle(location.ToPoint(), spriteSize), Color.White);
           
        }
        //http://devmag.org.za/2011/04/05/bzier-curves-a-tutorial/
        Vector2 CalculateBezierPoint(float t, Vector2 p0, Vector2 p1, Vector2 p2, Vector2 p3)
        {
            float u = 1 - t;
            float tt = t * t;
            float uu = u * u;
            float uuu = uu * u;
            float ttt = tt * t;

            Vector2 p = uuu * p0; //first term
            p += 3 * uu * t * p1; //second term
            p += 3 * u * tt * p2; //third term
            p += ttt * p3; //fourth term

            return p;
        }

    }
}
