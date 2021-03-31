using Assignment4.Enum;
using Assignment4.Logic;
using Assignment4.Model;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace Assignment4
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private GameMenu gameMenu;
        public Dictionary<FontStyle, SpriteFont> gameFonts;
        private GameMode gameMode;
        private HighScores highscores;
        private CustomControl userControls;
        private KeyboardState previousState;
        private GameLogic gameLogic;
        private Texture2D pixel;
        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            _graphics.PreferredBackBufferWidth = 1200;
            _graphics.PreferredBackBufferHeight = 900;
            _graphics.ApplyChanges();
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            gameFonts = new Dictionary<FontStyle, SpriteFont>();
            gameFonts.Add(FontStyle.Button, Content.Load<SpriteFont>(".\\Font\\ButtonFont"));
            gameFonts.Add(FontStyle.Heading1, Content.Load<SpriteFont>(".\\Font\\Heading1"));
            gameFonts.Add(FontStyle.SubHeading, Content.Load<SpriteFont>(".\\Font\\Subheading"));
            gameFonts.Add(FontStyle.Paragraph, Content.Load<SpriteFont>(".\\Font\\Paragraph"));
            pixel = new Texture2D(_graphics.GraphicsDevice, 1, 1);
            pixel.SetData<Color>(new Color[] { Color.Olive });

            Dictionary<Sprites, Texture2D> sprites = new Dictionary<Sprites, Texture2D>();
            sprites.Add(Sprites.player, Content.Load<Texture2D>(".\\Sprites\\ship3"));
            sprites.Add(Sprites.bee, Content.Load<Texture2D>(".\\Sprites\\ship2"));
            sprites.Add(Sprites.Pixel, pixel);

            gameLogic = new GameLogic(sprites);
            gameMenu = new GameMenu(gameFonts, this);
            Button MenuButton = new Button(pixel, new Rectangle(1, 1, 125, 50), gameFonts[FontStyle.Button], "Menu", GameMode.Menu);
            highscores = new HighScores(gameFonts[FontStyle.Heading1], gameFonts[FontStyle.SubHeading], MenuButton);
            userControls = new CustomControl(gameFonts, pixel, MenuButton);
            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            KeyboardState state = Keyboard.GetState();
            GameMode updatedGameMode = gameMode;
            if (updatedGameMode == GameMode.Menu)
            {
                updatedGameMode = gameMenu.Update();
            }
            if (updatedGameMode == GameMode.HighScores)
            {
                updatedGameMode = highscores.Update();
            }
            if (updatedGameMode == GameMode.CustomizeControls)
            {
                updatedGameMode = userControls.Update();
                //gameLogic.UpdateGameControls(userControls.getUserControls());
            }
            if (updatedGameMode == GameMode.Game)
            {
                if(gameMode != updatedGameMode)
                {
                    gameLogic.startNewGame();
                }
                gameLogic.Update(gameTime);
            }

            gameMode = updatedGameMode;
            previousState = state;

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
           

            // TODO: Add your drawing code here
            if (gameMode == GameMode.Menu)
            {
                GraphicsDevice.Clear(Color.White);
                gameMenu.Draw(_spriteBatch);
            }
            else if (gameMode == GameMode.HighScores)
            {
                GraphicsDevice.Clear(Color.White);
                highscores.Draw(_spriteBatch);
            }
            else if (gameMode == GameMode.CustomizeControls)
            {
                GraphicsDevice.Clear(Color.White);
                _spriteBatch.Begin();
                userControls.Draw(_spriteBatch);
                _spriteBatch.End();
            }
            else if (gameMode == GameMode.Game)
            {
                GraphicsDevice.Clear(Color.Black);
                //gameLogic.Draw(_spriteBatch);
                gameLogic.Draw(_spriteBatch);

            }

            base.Draw(gameTime);
        }

        //private float degToRad(float angle)
        //{
           
        //    return (float)(angle * (Math.PI / 180));
        //}

        //private Point RotatePoint(int nx, int ny, float angle)
        //{
        //    float x = 600;
        //    float y = 300;
        //    float new_x = (float)(nx) - x;
        //    float new_y = (float)(ny) - y;

        //    var sin_ang = Math.Sin(degToRad(angle));
        //    var cos_ang = Math.Cos(degToRad(angle));
        //    var tx = (new_x * cos_ang) - (new_y * sin_ang);
        //    var ty = (new_x * sin_ang) + (new_y * cos_ang);

        //    Point p = new Point();
        //    p.X = (int)(tx + x);
        //    p.Y = (int)(ty + y);
        //    return p;
        //}
    }
}
