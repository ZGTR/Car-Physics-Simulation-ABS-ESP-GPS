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
#endregion


namespace CarDynamics
{



    #region user definition
    public abstract class User
    {

        #region Fields
        public bool localOrRemote;
        public bool active;
        public bool playing;

        // stats can be stored here:
        // signed in data can be stored here:
        #endregion


        #region constructors
        
        /// <summary>
        /// constructor for remote users
        /// </summary>
        /// <param name="remote">true if user is remote user</param>
        public User(bool remote)
        {
            localOrRemote = remote;
            active = false;
            
        }




        #endregion


        #region public interface


  





        /// <summary>
        /// these need to be imlpemented
        /// </summary>
        public abstract bool PressedStart();
        public abstract bool PressedBack();
        public abstract bool PressedA();
        public abstract bool PressedB();
        public abstract bool PressedX();
        public abstract bool PressedY();
        public abstract bool PressedMenuUp();
        public abstract bool PressedMenuDown();
        public abstract bool PressedMenuRight();
        public abstract bool PressedMenuLeft();
        public abstract bool PressedLeftBumper();
        public abstract bool PressedRightBumper();
        public abstract bool PressedRightTrigger();
        public abstract bool PressedLeftTrigger();
        public abstract bool PressedDigitalDown();
        public abstract bool PressedDigitalUp();
        public abstract bool PressedDigitalLeft();
        public abstract bool PressedDigitalRight();
        public abstract Vector2 GetLeftStick();
        public abstract Vector2 GetRightStick();

        public abstract bool Update();
        #endregion

    }


    #endregion

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
