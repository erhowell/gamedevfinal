using Assignment4.Enum;
using Assignment4.Model;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Assignment4.Logic
{
    public class GameMenu
    {
        private Dictionary<FontStyle, SpriteFont> fonts { get; set; }
        private Button buttonNewGame { get; set; }
        private Button buttonHighScore { get; set; }
        private Button buttonCustContrl { get; set; }
        private Button buttonCredit { get; set; }
        public GameMenu(Dictionary<FontStyle, SpriteFont> gameFonts, Game game)
        {
            Texture2D pixel = new Texture2D(game.GraphicsDevice, 1, 1);
            pixel.SetData<Color>(new Color[] { Color.Olive });
            this.fonts = gameFonts;
            buttonNewGame = new Button(pixel, new Rectangle(50, 50, 200, 50), gameFonts[FontStyle.Button], "New Game", GameMode.Game);
            buttonHighScore = new Button(pixel, new Rectangle(50, 125, 200, 50), gameFonts[FontStyle.Button], "High Score",GameMode.HighScores);
            buttonCustContrl = new Button(pixel, new Rectangle(50, 200, 200, 50), gameFonts[FontStyle.Button], "Customize \nControls",GameMode.CustomizeControls);
            buttonCredit = new Button(pixel, new Rectangle(50, 275, 200, 50), gameFonts[FontStyle.Button], "View Credits", GameMode.ViewCredits);
        }
        public GameMode Update()
        {
            buttonNewGame.Update();
            buttonHighScore.Update();
            buttonCustContrl.Update();
            buttonCredit.Update();

            if (buttonNewGame.isClicked)
                return GameMode.Game;
            else if (buttonHighScore.isClicked)
                return GameMode.HighScores;
            else if (buttonCustContrl.isClicked)
                return GameMode.CustomizeControls;
            else if (buttonCredit.isClicked)
                return GameMode.ViewCredits;
            
            return GameMode.Menu;
        }
        public void Draw(SpriteBatch _spriteBatch)
        {
            _spriteBatch.Begin();
            buttonNewGame.Draw(_spriteBatch);
            buttonHighScore.Draw(_spriteBatch);
            buttonCustContrl.Draw(_spriteBatch);
            buttonCredit.Draw(_spriteBatch);
            _spriteBatch.End();
        }
    }
}
