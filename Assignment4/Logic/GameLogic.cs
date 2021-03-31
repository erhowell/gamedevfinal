using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Assignment4.Model;
using System;
using System.Collections.Generic;
using System.Text;
using Assignment4.Enum;

namespace Assignment4.Logic
{
    public class GameLogic
    {
        private GameSetting settings;
        Dictionary<Sprites, Texture2D> sprites;
        private Player player;
        private BumbleBeesLogic bbLogic;
        public GameLogic(Dictionary<Sprites, Texture2D> s)
        {
            sprites = s;
            
        }
        public void startNewGame()
        {
            //settings = s;
            player = new Player(sprites);
            bbLogic = new BumbleBeesLogic(sprites);
        }
        public void Update(GameTime gameTime)
        {
            bbLogic.Update(gameTime);
            player.Update(gameTime);

        }
        public void Draw(SpriteBatch _spriteBatch)
        {
            player.Draw(_spriteBatch);
            bbLogic.Draw(_spriteBatch);
        }
    }
}
