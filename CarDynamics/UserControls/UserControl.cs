#region using
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;
using Microsoft.DirectX.DirectInput;
#endregion

namespace CarDynamics
{


    #region interface definition

    public interface IUserInterface
    {
         /// <summary>
        /// splash screen helper. will find which user pressed A or start
        /// </summary>
        /// <returns></returns>
        bool FindUserOne();


        /// <summary>
        /// return user object
        /// </summary>
        /// <param name="UserIndex">user to fetch</param>
        User GetUser(int UserIndex);

        /// <summary>
        /// return user status
        /// </summary>
        /// <param name="UserIndex">user to fetch</param>
        bool GetUserActiveStatus(int UserIndex);


        /// <summary>
        /// return number of active local users
        /// </summary>
        int GetNoActiveLocalUsers();

        /// <summary>
        /// returns the status of user connection change
        /// </summary>
        bool GetUserChange();



        
        /// <summary>
        /// some times we might not want to check users
        /// </summary>
        /// <param name="IsUpdating"></param>
        void ChangeIsUpdateing(bool IsUpdating);
    }


    #endregion



    /// <summary>
    /// this class manages users
    /// it extends drawable, because we need regular updates
    /// </summary>
    public class UserControl : Microsoft.Xna.Framework.DrawableGameComponent, IUserInterface
    {



        #region Fields

        List<User> users;
        const int remoteUserCount = 0;
        int localUserCount = 4;
        bool userConnectionChange;
        bool isUpdating;
        #endregion




        #region construct and initialize

        /// <summary>
        /// constructor, adds this component to the components and services lists
        /// </summary>
        public UserControl(Game game)
            : base(game)
        {
            game.Services.AddService(typeof(IUserInterface), this);
            game.Components.Add(this);
            userConnectionChange = false;
            isUpdating = true;
        }



        /// <summary>
        /// create all objects needed for game play
        /// </summary>
        public override void Initialize()
        {
            users = new List<User>();

            // create local users
            users.Add(new UserXbox(PlayerIndex.One));
            users.Add(new UserXbox(PlayerIndex.Two));
            users.Add(new UserXbox(PlayerIndex.Three));
            users.Add(new UserXbox(PlayerIndex.Four));
#if !XBOX
            users.Add(new UserKeyboard());
            localUserCount++;


            // get list of active game pads
            DeviceList dl = Manager.GetDevices(
                Microsoft.DirectX.DirectInput.DeviceClass.GameControl,
                EnumDevicesFlags.AttachedOnly);

            foreach (DeviceInstance deviceInstance in dl)
            {
                // ignore xbox controllers
                if (deviceInstance.ProductName.IndexOf("Xbox 360") < 0)
                {
                    UserGamePad gp = new UserGamePad(deviceInstance.InstanceGuid);
                    if (UserGamePad.CheckConfig(gp.device, gp.conf))
                    {
                        users.Add(gp);
                        localUserCount++;
                    }
                }
            }


#endif
            // crate remote users
            //for (int i = 0; i < remoteUserCount; i++)
            //    users.Add(new UserXbox(true));

            base.Initialize();

        }


        /// <summary>
        /// load content
        /// </summary>
        protected override void LoadContent()
        {


            base.LoadContent();

        }


        #endregion




        #region update

        /// <summary>
        /// do any updating here
        /// </summary>
        public override void Update(GameTime gameTime)
        {

            if (isUpdating)
            {
                // reset flag each cycle
                userConnectionChange = false;


                // check connections every cycle
                for (int i = 0; i < localUserCount; i++)
                {
                    if (users[i].Update()) userConnectionChange = true;
                }

                // order user list. make sure connected users are always shown first
                if (userConnectionChange)
                {
                    for (int i = 0; i < localUserCount - 1; i++)
                        if (!users[i].active)
                            for (int j = i + 1; j < localUserCount; j++)
                            {
                                if (users[j].active)
                                {
                                    User a = users[j];
                                    users[j] = users[i];
                                    users[i] = a;
                                    break;
                                }
                            }
                }
            }
            base.Update(gameTime);

        }



        #endregion



        #region public interface


        /// <summary>
        /// some times we might not want to check users
        /// </summary>
        /// <param name="IsUpdating"></param>
        public void ChangeIsUpdateing(bool IsUpdating)
        {
            isUpdating = IsUpdating;
        }


        /// <summary>
        /// return user object
        /// </summary>
        /// <param name="UserIndex">user to fetch</param>
        public User GetUser(int UserIndex)
        {
            if (UserIndex >= 0 && UserIndex < users.Count)
                return users[UserIndex];
            return null;
        }


        /// <summary>
        /// return user status
        /// </summary>
        /// <param name="UserIndex">user to fetch</param>
        public bool GetUserActiveStatus(int UserIndex)
        {
            if (UserIndex >= 0 && UserIndex < users.Count)
                return users[UserIndex].active && users[UserIndex].playing;
            return false;
        }


        /// <summary>
        /// returns the status of user connection change
        /// </summary>
        public bool GetUserChange()
        {
            return userConnectionChange;
        }


        /// <summary>
        /// returns number of active local users
        /// </summary>
        public int GetNoActiveLocalUsers()
        {
            int total = 0;
            for (int i = 0; i < localUserCount; i++)
                if (users[i].active && users[i].playing) total++;
            return total;
        }


        /// <summary>
        /// splash screen helper. will find which user pressed A or start
        /// </summary>
        /// <returns></returns>
        public bool FindUserOne()
        {
            for (int i = 0; i < localUserCount; i++)
                if (users[i].active)
                {
                    if (users[i].PressedA() || users[i].PressedStart())
                    {
                        // switch users if needed
                        if (i != 0)
                        {
                            User a = users[i];
                            users[i] = users[0];
                            users[0] = a;
                        }
                        return true;
                    }
                }


            return false;
        }







        #endregion




    }







}



/* 
 Copyright (c) 2010, Damjan Stulic of Wicked Smiles Studios
 http://www.wickedsmilesstudios.com/
 
 Contact information:
 damjan[at]wickedsmilesstudios.com
 http://twitter.com/dstulic

Permission is hereby granted, free of charge, to any person
obtaining a copy of this software and associated documentation
files (the "Software"), to deal in the Software without
restriction, including without limitation the rights to use,
copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the
Software is furnished to do so, subject to the following
conditions:

The above copyright notice and this permission notice shall be
included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES
OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT
HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
OTHER DEALINGS IN THE SOFTWARE.
 */
