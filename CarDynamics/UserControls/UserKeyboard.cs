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
    public class UserKeyboard : User
    {

        #region Fields
        public KeyboardState preks;
        public KeyboardState ks;
        Keys y;
        Keys x;
        Keys a;
        Keys b;
        Keys lt;
        Keys lb;
        Keys rt;
        Keys rb;
        Keys start;
        Keys back;
        Keys ddown;
        Keys dup;
        Keys dleft;
        Keys dright;
        Keys ldown;
        Keys lup;
        Keys lleft;
        Keys lright;
        Keys rdown;
        Keys rup;
        Keys rleft;
        Keys rright;

        #endregion


        #region constructors

        /// <summary>
        /// constructor for remote users
        /// </summary>
        /// <param name="remote">true if user is remote user</param>
        public UserKeyboard()
            : base(true)
        {
            lleft = Keys.A;
            lright = Keys.D;
            lup = Keys.W;
            ldown = Keys.S;
            rleft = Keys.J;
            rright = Keys.L;
            rup = Keys.I;
            rdown = Keys.K;
            dleft = Keys.F;
            dright = Keys.H;
            dup = Keys.T;
            ddown = Keys.G;
            lb = Keys.U;
            lt = Keys.Y;
            rb = Keys.O;
            rt = Keys.P;
            back = Keys.Z;
            start = Keys.X;
            a = Keys.C;
            b = Keys.V;
            x = Keys.B;
            y = Keys.N;
        }




        #endregion

        #region public interface





        /// <summary>
        /// returns true if user pressed Start
        /// </summary>
        public override bool PressedY()
        {
            if (!active) return false;
            if (ks.IsKeyDown(y) && preks.IsKeyUp(y))
                return true;
            return false;
        }



        /// <summary>
        /// returns true if user pressed Start
        /// </summary>
        public override bool PressedStart()
        {
            if (!active) return false;
            if (ks.IsKeyDown(start) && preks.IsKeyUp(start))
                return true;
            return false;
        }


        /// <summary>
        /// returns true if user pressed Start
        /// </summary>
        public override bool PressedBack()
        {
            if (!active) return false;
            if (ks.IsKeyDown(back) && preks.IsKeyUp(back))
                return true;
            return false;
        }


        /// <summary>
        /// returns true if user pressed A
        /// </summary>
        public override bool PressedA()
        {
            if (!active) return false;
            if (ks.IsKeyDown(a) && preks.IsKeyUp(a))
                return true;
            return false;
        }

        /// <summary>
        /// returns true if user pressed X
        /// </summary>
        public override bool PressedX()
        {
            if (!active) return false;
            if (ks.IsKeyDown(x) && preks.IsKeyUp(x))
                return true;
            return false;
        }


        /// <summary>
        /// returns true if user pressed B
        /// </summary>
        public override bool PressedB()
        {
            if (!active) return false;
            if (ks.IsKeyDown(b) && preks.IsKeyUp(b))
                return true;
            return false;
        }


        public override bool PressedMenuUp()
        {
            if (!active) return false;
            if (ks.IsKeyDown(rup) && preks.IsKeyUp(rup))
                return true;
            if (ks.IsKeyDown(lup) && preks.IsKeyUp(lup))
                return true;
            return false;
        }


        public override bool PressedMenuDown()
        {
            if (!active) return false;
            if (ks.IsKeyDown(rdown) && preks.IsKeyUp(rdown))
                return true;
            if (ks.IsKeyDown(ldown) && preks.IsKeyUp(ldown))
                return true;
            return false;
        }

        public override bool PressedMenuRight()
        {
            if (!active) return false;
            if (ks.IsKeyDown(rright) && preks.IsKeyUp(rright))
                return true;
            if (ks.IsKeyDown(lright) && preks.IsKeyUp(lright))
                return true;
            return false;
        }


        public override bool PressedMenuLeft()
        {
            if (!active) return false;
            if (ks.IsKeyDown(rleft) && preks.IsKeyUp(rleft))
                return true;
            if (ks.IsKeyDown(lleft) && preks.IsKeyUp(lleft))
                return true;
            return false;
        }

        public override bool PressedLeftBumper()
        {
            if (!active) return false;
            if (ks.IsKeyDown(lb) && preks.IsKeyUp(lb))
                return true;
            return false;
        }

        public override bool PressedRightBumper()
        {
            if (!active) return false;
            if (ks.IsKeyDown(rb) && preks.IsKeyUp(rb))
                return true;
            return false;
        }

        public override bool PressedLeftTrigger()
        {
            if (!active) return false;
            if (ks.IsKeyDown(lt) && preks.IsKeyUp(lt))
                return true;
            return false;
        }

        public override bool PressedRightTrigger()
        {
            if (!active) return false;
            if (ks.IsKeyDown(rt) && preks.IsKeyUp(rt))
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
            if (ks.IsKeyDown(ddown) && preks.IsKeyUp(ddown))
                return true;
            return false;
        }
        public override bool PressedDigitalUp()
        {
            if (!active) return false;
            if (ks.IsKeyDown(dup) && preks.IsKeyUp(dup))
                return true;
            return false;
        }
        public override bool PressedDigitalLeft()
        {
            if (!active) return false;
            if (ks.IsKeyDown(dleft) && preks.IsKeyUp(dleft))
                return true;
            return false;
        }
        public override bool PressedDigitalRight()
        {
            if (!active) return false;
            if (ks.IsKeyDown(dright) && preks.IsKeyUp(dright))
                return true;
            return false;
        }


        public override Vector2 GetLeftStick()
        {
            if (!active) return Vector2.Zero;
            Vector2 ret = Vector2.Zero;
            if (ks.IsKeyDown(lleft)) ret.X -= 1f;
            if (ks.IsKeyDown(lright)) ret.X += 1f;
            if (ks.IsKeyDown(ldown)) ret.Y -= 1f;
            if (ks.IsKeyDown(lup)) ret.Y += 1f;
            if (ret != Vector2.Zero) ret.Normalize();
            return ret;
        }

        public override Vector2 GetRightStick()
        {
            if (!active) return Vector2.Zero;
            Vector2 ret = Vector2.Zero;
            if (ks.IsKeyDown(rleft)) ret.X -= 1f;
            if (ks.IsKeyDown(rright)) ret.X += 1f;
            if (ks.IsKeyDown(rdown)) ret.Y -= 1f;
            if (ks.IsKeyDown(rup)) ret.Y += 1f;
            if (ret != Vector2.Zero) ret.Normalize();
            return ret;
        }


        /// <summary>
        /// update game pad states
        /// </summary>
        public override bool Update()
        {
            bool userConnectionChange = false;
            
            if (!active)
            {
                // do something cuz there was connection change
                active = true;
                userConnectionChange = true;
            }

            if (active)
            {
                preks = ks;
                ks = Keyboard.GetState();
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
