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
    public class UserXbox: User
    {

        #region Fields
        public PlayerIndex controller;
        public GamePadState pregps;
        public GamePadState gps;

        public const float TriggerTolerance = 0.5f;
        #endregion


        #region constructors


        /// <summary>
        /// contructor for local users
        /// </summary>
        /// <param name="controllerIndex">which playerindex this user controles</param>
        public UserXbox(PlayerIndex controllerIndex): base(true)
        {
            controller = controllerIndex;
        }

        #endregion

        #region public interface





        /// <summary>
        /// returns true if user pressed Start
        /// </summary>
        public override bool PressedY()
        {
            if (!active) return false;
            if (gps.Buttons.Y == ButtonState.Pressed &&
                pregps.Buttons.Y == ButtonState.Released)
                return true;
            return false;
        }



        /// <summary>
        /// returns true if user pressed Start
        /// </summary>
        public override bool PressedStart()
        {
            if (!active) return false;
            if (gps.Buttons.Start == ButtonState.Pressed &&
                pregps.Buttons.Start == ButtonState.Released)
                return true;
            return false;
        }


        /// <summary>
        /// returns true if user pressed Start
        /// </summary>
        public override bool PressedBack()
        {
            if (!active) return false;
            if (gps.Buttons.Back == ButtonState.Pressed &&
                pregps.Buttons.Back == ButtonState.Released)
                return true;
            return false;
        }

        /// <summary>
        /// returns true if user pressed A
        /// </summary>
        public override bool PressedA()
        {
            if (!active) return false;
            if (gps.Buttons.A == ButtonState.Pressed &&
                pregps.Buttons.A == ButtonState.Released)
                return true;
            return false;
        }

        /// <summary>
        /// returns true if user pressed X
        /// </summary>
        public override bool PressedX()
        {
            if (!active) return false;
            if (gps.Buttons.X == ButtonState.Pressed &&
                pregps.Buttons.X == ButtonState.Released)
                return true;
            return false;
        }


        /// <summary>
        /// returns true if user pressed B
        /// </summary>
        public override bool PressedB()
        {
            if (!active) return false;
            if (gps.Buttons.B == ButtonState.Pressed &&
                pregps.Buttons.B == ButtonState.Released)
                return true;
            return false;
        }


        public override bool PressedMenuUp()
        {
            if (!active) return false;
            if (gps.DPad.Up == ButtonState.Pressed &&
                pregps.DPad.Up == ButtonState.Released)
                return true;
            if (gps.ThumbSticks.Left.Y > 0.5f &&
                pregps.ThumbSticks.Left.Y < 0.5f)
                return true;
            return false;
        }


        public override bool PressedMenuDown()
        {
            if (!active) return false;
            if (gps.DPad.Down == ButtonState.Pressed &&
                pregps.DPad.Down == ButtonState.Released)
                return true;
            if (gps.ThumbSticks.Left.Y < -0.5f &&
                pregps.ThumbSticks.Left.Y > -0.5f)
                return true;
            return false;
        }

        public override bool PressedMenuRight()
        {
            if (!active) return false;
            if (gps.DPad.Right == ButtonState.Pressed &&
                pregps.DPad.Right == ButtonState.Released)
                return true;
            if (gps.ThumbSticks.Left.X > 0.5f &&
                pregps.ThumbSticks.Left.X < 0.5f)
                return true;
            return false;
        }


        public override bool PressedMenuLeft()
        {
            if (!active) return false;
            if (gps.DPad.Left == ButtonState.Pressed &&
                pregps.DPad.Left == ButtonState.Released)
                return true;
            if (gps.ThumbSticks.Left.X < -0.5f &&
                pregps.ThumbSticks.Left.X > -0.5f)
                return true;
            return false;
        }

        public override bool PressedLeftBumper()
        {
            if (!active) return false;
            if (gps.Buttons.LeftShoulder == ButtonState.Pressed &&
                pregps.Buttons.LeftShoulder == ButtonState.Released)
                return true;
            return false;
        }

        public override bool PressedRightBumper()
        {
            if (!active) return false;
            if (gps.Buttons.RightShoulder == ButtonState.Pressed &&
                pregps.Buttons.RightShoulder == ButtonState.Released)
                return true;
            return false;
        }



        public override bool PressedLeftTrigger()
        {
            if (!active) return false;
            if (gps.Triggers.Left > TriggerTolerance &&
                pregps.Triggers.Left < TriggerTolerance)
                return true;
            return false;
        }

        public override bool PressedRightTrigger()
        {
            if (!active) return false;
            if (gps.Triggers.Right > TriggerTolerance &&
                pregps.Triggers.Right < TriggerTolerance)
                return true;
            return false;
        }

        /// <summary>
        /// section of code for checking digital pad inputs
        /// </summary>
        /// <returns></returns>
        public override bool PressedDigitalDown()
        {
            if (!active) return false;
            if (gps.DPad.Down == ButtonState.Pressed &&
                pregps.DPad.Down == ButtonState.Released)
                return true;
            return false;
        }
        public override bool PressedDigitalUp()
        {
            if (!active) return false;
            if (gps.DPad.Up == ButtonState.Pressed &&
                pregps.DPad.Up == ButtonState.Released)
                return true;
            return false;
        }
        public override bool PressedDigitalLeft()
        {
            if (!active) return false;
            if (gps.DPad.Left == ButtonState.Pressed &&
                pregps.DPad.Left == ButtonState.Released)
                return true;
            return false;
        }
        public override bool PressedDigitalRight()
        {
            if (!active) return false;
            if (gps.DPad.Right == ButtonState.Pressed &&
                pregps.DPad.Right == ButtonState.Released)
                return true;
            return false;
        }




        public override Vector2 GetLeftStick()
        {
            if (!active) return Vector2.Zero;
            return gps.ThumbSticks.Left;
        }

        public override Vector2 GetRightStick()
        {
            if (!active) return Vector2.Zero;
            return gps.ThumbSticks.Right;
        }


        /// <summary>
        /// update game pad states
        /// </summary>
        public override bool Update()
        {
            bool userConnectionChange = false;
            GamePadState tgps = GamePad.GetState(controller);
            if (tgps.IsConnected != active)
            {
                // do something cuz there was connection change
                active = tgps.IsConnected;
                userConnectionChange = true;
            }

            if (active)
            {
                pregps = gps;
                gps = tgps;
            }
            return userConnectionChange;
        }

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
