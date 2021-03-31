using Assignment4.Enum;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Assignment4.Model
{
    public class HighScores
    {
        public List<int> scores { get; set; }
        private List<int> loaded_scores { get; set; }
        private SpriteFont header { get; set; }
        private SpriteFont scoreText { get; set; }
        private Button MenuButton { get; set; }
        private bool open { get; set; }
        public HighScores(SpriteFont HeaderText, SpriteFont ScoreText, Button mB)
        {
            this.scores = new List<int>();
            this.loaded_scores = new List<int>();
            this.header = HeaderText;
            this.scoreText = ScoreText;
            this.MenuButton = mB;
            loadControls();
        }
        public GameMode Update()
        {
            MenuButton.Update();
            if (MenuButton.isClicked)
            {
                return GameMode.Menu;
            }
            if(loaded_scores.Count > 0)
            {
                scores = loaded_scores;
                loaded_scores = new List<int>(); 
            }

            return GameMode.HighScores;
            
        }
        public bool UpdateScores(int newScore)
        {
            if (this.scores.Count < 5 || this.scores.Any(s => s < newScore))
            {
                scores.Add(newScore);
                scores = scores.OrderByDescending(s => s).ToList();
                if(scores.Count > 5)
                {
                    scores.RemoveAt(scores.Count - 1);
                }
                saveControls();
                loadControls();
                return true;
            }
            return false;
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            MenuButton.Draw(spriteBatch);
            spriteBatch.DrawString(header, "High Scores", new Microsoft.Xna.Framework.Vector2(100, 50), Color.White);

            int i = 0;
            while(i < 5 && i< scores.Count)
            {
                string txt = String.Format("Score {0} - {1}", i + 1, scores[i]);
                spriteBatch.DrawString(scoreText, txt, new Microsoft.Xna.Framework.Vector2(100, 150 + ( 50*i)), Color.White);
                ++i;
            }
            spriteBatch.End();


        }
        #region Persistence

        /// <summary>
        /// Demonstrates how serialize an object to storage
        /// </summary>
        private void saveControls()
        {
            lock (this)
            {
                if (!this.open)
                {
                    this.open = true;
                    //
                    // Create something to save
                    finalizeSaveAsync(this.scores);
                }
            }
        }

        private async void finalizeSaveAsync(List<int> state)
        {
            await Task.Run(() =>
            {
                using (IsolatedStorageFile storage = IsolatedStorageFile.GetUserStoreForApplication())
                {
                    try
                    {
                        using (IsolatedStorageFileStream fs = storage.OpenFile("Assignment4HighScores.xml", FileMode.Create))
                        {
                            if (fs != null)
                            {
                                XmlSerializer mySerializer = new XmlSerializer(typeof(List<int>));
                                mySerializer.Serialize(fs, state);
                            }
                        }
                    }
                    catch (IsolatedStorageException exe)
                    {

                    }
                }

                this.open = false;
            });
        }

        /// <summary>
        /// Demonstrates how to deserialize an object from storage device
        /// </summary>
        private void loadControls()
        {
            lock (this)
            {
                if (!this.open)
                {
                    this.open = true;
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
                    finalizeLoadAsync();

#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
                }
            }
        }


        private async Task finalizeLoadAsync()
        {
            await Task.Run(() =>
            {
                using (IsolatedStorageFile storage = IsolatedStorageFile.GetUserStoreForApplication())
                {
                    try
                    {
                        if (storage.FileExists("HighScores.xml"))
                        {
                            using (IsolatedStorageFileStream fs = storage.OpenFile("HighScores.xml", FileMode.Open))
                            {
                                if (fs != null)
                                {
                                    XmlSerializer mySerializer = new XmlSerializer(typeof(List<int>));
                                    loaded_scores = (List<int>)mySerializer.Deserialize(fs);
                                }
                            }
                        }
                    }
                    catch (IsolatedStorageException exe)
                    {
                        // Ideally show something to the user, but this is demo code :)
                    }
                }
                this.open = false;
            });
        }
        #endregion
    }
}
