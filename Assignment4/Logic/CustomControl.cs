using Assignment4.Enum;
using Assignment4.Model;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Assignment4.Logic
{
    public class UserControlKeys
    {
        public UserControl control { get; set; }
        public Keys key { get; set; }
        public UserControlKeys() { }
        public UserControlKeys(UserControl cntrl, Keys k)
        {
            this.control = cntrl;
            this.key = k;
        }
    }
    public class CustomControl
    {
        private List<UserControlKeys> userControls { get;  set; }
        private List<UserControlKeys> loaded_controls = null;
        private Dictionary<FontStyle, SpriteFont> fonts;
        private Button menuButton { get; set; }
        private CustomControlButton Left { get; set; }
        private CustomControlButton Right { get; set; }
        private CustomControlButton Shoot { get; set; }
        private UserControl controlEditing { get; set; }
        private bool open { get; set; }
        public CustomControl(Dictionary<FontStyle, SpriteFont> gameFonts, Texture2D p, Button mb)
        {
            this.open = false;
            this.fonts = gameFonts;
            //this.pixel = p;
            this.menuButton = mb;
            this.controlEditing = UserControl.None;

            loadControls();
            
            Left = new CustomControlButton(p, new Rectangle(250, 200, 200, 50), fonts[FontStyle.Button], Keys.Left, UserControl.Left);
            Right = new CustomControlButton(p, new Rectangle(250, 275, 200, 50), fonts[FontStyle.Button], Keys.Right, UserControl.Right);
            Shoot = new CustomControlButton(p, new Rectangle(250, 350, 200, 50), fonts[FontStyle.Button], Keys.Up, UserControl.Shoot);

        }
        
        public List<UserControlKeys> getUserControls()
        {
            if (loaded_controls == null)
            {
                userControls = new List<UserControlKeys>() {
                               new UserControlKeys(UserControl.Left, Keys.Left),
                               new UserControlKeys(UserControl.Right, Keys.Right ),
                               new UserControlKeys(UserControl.Shoot,  Keys.Space),

                            };
            }
            else
            {
                userControls = loaded_controls;
            }
            return userControls;
        }

        public GameMode Update()
        {
            this.menuButton.Update(); // Was menu button clicked
            if(loaded_controls != null && userControls == null)
            {
                getUserControls();
            }
            //Update user control menu buttons
            UserControl updatedControl = this.controlEditing;
            updatedControl = Left.Update(updatedControl, userControls.SingleOrDefault(x=> x.control == UserControl.Left));
            updatedControl = Right.Update(updatedControl, userControls.SingleOrDefault(x => x.control == UserControl.Right));
            updatedControl = Shoot.Update(updatedControl, userControls.SingleOrDefault(x => x.control == UserControl.Shoot));

            //check for updated user controls
            if (controlEditing != UserControl.None && updatedControl != controlEditing)
            {
                var cntrl = userControls.Single(x => x.control == controlEditing);
             
                // if user control was just unselected, update the key
                if (controlEditing == UserControl.Left)
                {
                    userControls.Single(x => x.control == controlEditing).key = this.Left.newKey;
                }
                else if(controlEditing == UserControl.Right)
                {
                    userControls.Single(x => x.control == controlEditing).key = this.Right.newKey;
                }
                else if (controlEditing == UserControl.Shoot)
                {
                    userControls.Single(x => x.control == controlEditing).key = this.Shoot.newKey;
                }
                saveControls();
            }
            controlEditing = updatedControl;

            return this.menuButton.isClicked ? GameMode.Menu : GameMode.CustomizeControls;
        }
        public void Draw(SpriteBatch _spritebatch)
        {
            menuButton.Draw(_spritebatch);//Draw Menu
            
            _spritebatch.DrawString(fonts[FontStyle.Heading1], "Customize Controls",new Vector2(100, 50), Color.Black); //Draw Title

            //Draw butttons
            Left.Draw(_spritebatch);
            Right.Draw(_spritebatch);
            Shoot.Draw(_spritebatch);
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
                    finalizeSaveAsync(this.userControls);
                }
            }
        }

        private async void finalizeSaveAsync(List<UserControlKeys> state)
        {
            await Task.Run(() =>
            {
                using (IsolatedStorageFile storage = IsolatedStorageFile.GetUserStoreForApplication())
                {
                    try
                    {
                        using (IsolatedStorageFileStream fs = storage.OpenFile("Assignment4UserControlKeys.xml", FileMode.Create))
                        {
                            if (fs != null)
                            {
                                XmlSerializer mySerializer = new XmlSerializer(typeof(List<UserControlKeys>));
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
                        if (storage.FileExists("UserControlKeys.xml"))
                        {
                            using (IsolatedStorageFileStream fs = storage.OpenFile("UserControlKeys.xml", FileMode.Open ))
                            {
                                if (fs != null)
                                {
                                    XmlSerializer mySerializer = new XmlSerializer(typeof(List<UserControlKeys>));
                                    loaded_controls = (List<UserControlKeys>)mySerializer.Deserialize(fs);
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
