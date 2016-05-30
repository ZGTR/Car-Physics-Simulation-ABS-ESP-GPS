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
using System.Xml;
using System.Xml.Serialization;
using System.IO;
#endregion


namespace CarDynamics
{


    #region user definition
    public class UserGamePad : User
    {

        #region Fields

        public Device device;

        public Vector2 psLeftThumbStick;
        public Vector2 psRightThumbStick;
        public Vector2 LeftThumbStick;
        public Vector2 RightThumbStick;
        public bool HasLeft;
        public bool HasRight;
        const float center = 32767.5f;

        public bool psUp;
        public bool psRight;
        public bool psDown;
        public bool psLeft;
        public bool Up;
        public bool Right;
        public bool Down;
        public bool Left;

        public bool[] Buttons;
        public bool[] psButtons;

        public GamePadConfig conf;
        #endregion


        #region constructors


        /// <summary>
        /// contructor for local users
        /// </summary>
        /// <param name="controllerIndex">which playerindex this user controles</param>
        public UserGamePad(Guid gamepadInstanceGuid)
            : base(true)
        {
            device = new Device(gamepadInstanceGuid);
            device.SetDataFormat(DeviceDataFormat.Joystick);
            device.Acquire();
            

            Buttons = new bool[device.Caps.NumberButtons];
            psButtons = new bool[device.Caps.NumberButtons];
            HasLeft = false;
            HasRight = false;
            if (device.Caps.NumberAxes > 0)
                HasLeft = true;
            if (device.Caps.NumberAxes > 2)
                HasRight = true;


            #region load config
            try
            {
                XmlSerializer deserializer = new XmlSerializer(typeof(GamePadConfig));
                TextReader textReader = new StreamReader("Content\\" + 
                        device.DeviceInformation.InstanceGuid.ToString() + ".xml");
                Object o = deserializer.Deserialize(textReader);
                textReader.Close();
                conf = (GamePadConfig)o;
            }
            catch (FileNotFoundException)
            {
                conf = new GamePadConfig();
            }
            catch (DirectoryNotFoundException)
            {
                conf = new GamePadConfig();
            }
            catch (IOException)
            {
                conf = new GamePadConfig();
            }
            catch (InvalidOperationException)
            {
                conf = new GamePadConfig();
            }
            #endregion

        }

        #endregion






        #region public interface


        /// <summary>
        /// returns true if device has all the buttons need for the game
        /// </summary>
        /// <param name="dev">device to check</param>
        /// <param name="conf">config to fix</param>
        /// <returns></returns>
        public static bool CheckConfig(Device dev, GamePadConfig conf)
        {
            int nob = dev.Caps.NumberButtons;
            if (nob < 10) return false;
            if (conf.bX >= nob) conf.bX = nob - 1;
            if (conf.bY >= nob) conf.bY = nob - 1;
            if (conf.bA >= nob) conf.bA = nob - 1;
            if (conf.bB >= nob) conf.bB = nob - 1;
            if (conf.bBack >= nob) conf.bBack = nob - 1;
            if (conf.bStart >= nob) conf.bStart = nob - 1;
            if (conf.bShoulderLeft >= nob) conf.bShoulderLeft = nob - 1;
            if (conf.bShoulderRight >= nob) conf.bShoulderRight = nob - 1;
            if (conf.bTriggerLeft >= nob) conf.bTriggerLeft = nob - 1;
            if (conf.bTriggerRight >= nob) conf.bTriggerRight = nob - 1;

            if (dev.Caps.NumberPointOfViews < 1) return false;

            if (dev.Caps.NumberAxes < 4) return false;

            return true;
        }



        /// <summary>
        /// returns true if user pressed Start
        /// </summary>
        public override bool PressedY()
        {
            if (!active) return false;
            if (Buttons[conf.bY] && ! psButtons[conf.bY])
                return true;
            return false;
        }



        /// <summary>
        /// returns true if user pressed Start
        /// </summary>
        public override bool PressedStart()
        {
            if (!active) return false;
            if (Buttons[conf.bStart] && !psButtons[conf.bStart])
                return true;
            return false;
        }

        /// <summary>
        /// returns true if user pressed Start
        /// </summary>
        public override bool PressedBack()
        {
            if (!active) return false;
            if (Buttons[conf.bBack] && !psButtons[conf.bBack])
                return true;
            return false;
        }


        /// <summary>
        /// returns true if user pressed A
        /// </summary>
        public override bool PressedA()
        {
            if (!active) return false;
            if (Buttons[conf.bA] && !psButtons[conf.bA])
                return true;
            return false;
        }

        /// <summary>
        /// returns true if user pressed X
        /// </summary>
        public override bool PressedX()
        {
            if (!active) return false;
            if (Buttons[conf.bX] && !psButtons[conf.bX])
                return true;
            return false;
        }


        /// <summary>
        /// returns true if user pressed B
        /// </summary>
        public override bool PressedB()
        {
            if (!active) return false;
            if (Buttons[conf.bB] && !psButtons[conf.bB])
                return true;
            return false;
        }


        public override bool PressedMenuUp()
        {
            if (!active) return false;
            if (Up && !psUp)
                return true;
            if (LeftThumbStick.Y > 0.5f &&
                psLeftThumbStick.Y < 0.5f)
                return true;
            return false;
        }


        public override bool PressedMenuDown()
        {
            if (!active) return false;
            if (Down && !psDown)
                return true;
            if (LeftThumbStick.Y < -0.5f &&
                psLeftThumbStick.Y > -0.5f)
                return true;
            return false;
        }

        public override bool PressedMenuRight()
        {
            if (!active) return false;
            if (Right && !psRight)
                return true;
            if (LeftThumbStick.X > 0.5f &&
                psLeftThumbStick.X < 0.5f)
                return true;
            return false;
        }


        public override bool PressedMenuLeft()
        {
            if (!active) return false;
            if (Left && !psLeft)
                return true;
            if (LeftThumbStick.X < -0.5f &&
                psLeftThumbStick.X > -0.5f)
                return true;
            return false;
        }

        public override bool PressedLeftBumper()
        {
            if (!active) return false;
            if (Buttons[conf.bShoulderLeft] && !psButtons[conf.bShoulderLeft])
                return true;
            return false;
        }

        public override bool PressedRightBumper()
        {
            if (!active) return false;
            if (Buttons[conf.bShoulderRight] && !psButtons[conf.bShoulderRight])
                return true;
            return false;
        }



        public override bool PressedLeftTrigger()
        {
            if (!active) return false;
            if (Buttons[conf.bTriggerLeft] && !psButtons[conf.bTriggerLeft])
                return true;
            return false;
        }

        public override bool PressedRightTrigger()
        {
            if (!active) return false;
            if (Buttons[conf.bTriggerRight] && !psButtons[conf.bTriggerRight])
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
            if (Down && !psDown)
                return true;
            return false;
        }
        public override bool PressedDigitalUp()
        {
            if (!active) return false;
            if (Up && !psUp)
                return true;
            return false;
        }
        public override bool PressedDigitalLeft()
        {
            if (!active) return false;
            if (Left && !psLeft)
                return true;
            return false;
        }
        public override bool PressedDigitalRight()
        {
            if (!active) return false;
            if (Right && !psRight)
                return true;
            return false;
        }




        public override Vector2 GetLeftStick()
        {
            if (!active) return Vector2.Zero;
            return LeftThumbStick;
        }

        public override Vector2 GetRightStick()
        {
            if (!active) return Vector2.Zero;
            return RightThumbStick;
        }


        /// <summary>
        /// update game pad states
        /// </summary>
        /// 

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

                JoystickState deviceState = device.CurrentJoystickState;

                #region thumb sticks
                // save previous state
                psLeftThumbStick = LeftThumbStick;
                psRightThumbStick = RightThumbStick;

                // empty current state
                LeftThumbStick = Vector2.Zero;
                RightThumbStick = Vector2.Zero;

                if (HasLeft)
                {
                    // get raw data
                    LeftThumbStick = new Vector2((deviceState.X - center) / center, (deviceState.Y - center) / center);

                    // switxh X and Y, some controlers are messed up..
                    if (conf.rotateLeftThumbStick)
                        LeftThumbStick = new Vector2((deviceState.Y - center) / center, (deviceState.X - center) / center);
                    // invert y axis
                    if (conf.invertLeftThumbStick)
                        LeftThumbStick.Y = LeftThumbStick.Y * (-1f);
                    // ajust sensitivity
                    // needs improvement -> we should recalculate percentage of overall allowed movement..
                    if (Math.Abs(LeftThumbStick.Y) < conf.sensitivityLeftThumbStick)
                        LeftThumbStick.Y = 0;
                    if (Math.Abs(LeftThumbStick.X) < conf.sensitivityLeftThumbStick)
                        LeftThumbStick.X = 0;
                }

                if (HasRight)
                {
                    RightThumbStick = new Vector2((deviceState.Z - center) / center, (deviceState.Rz - center) / center);
                    if (conf.rotateRightThumbStick)
                        RightThumbStick = new Vector2((deviceState.Rz - center) / center, (deviceState.Z - center) / center);
                    if (conf.invertRightThumbStick)
                        RightThumbStick.Y = RightThumbStick.Y * (-1f);
                    if (Math.Abs(RightThumbStick.Y) < conf.sensitivityRightThumbStick)
                        RightThumbStick.Y = 0;
                    if (Math.Abs(RightThumbStick.X) < conf.sensitivityRightThumbStick)
                        RightThumbStick.X = 0;
                }
                #endregion



                #region point of view / digital pad
                // get raw data
                int direction = deviceState.GetPointOfView()[conf.pointOfView];

                // copy states to previous states
                psUp = Up;
                psDown = Down;
                psLeft = Left;
                psRight = Right;

                // clear up current states
                Up = false;
                Right = false;
                Down = false;
                Left = false;

                // set currrent states
                if (direction != -1)
                {
                    if (direction > 27000 || direction < 9000)
                        Up = true;

                    if (0 < direction && direction < 18000)
                        Right = true;

                    if (9000 < direction && direction < 27000)
                        Down = true;

                    if (18000 < direction)
                        Left = true;
                }
                #endregion



                #region buttons
                // get raw data
                byte[] bs = deviceState.GetButtons();

                // copy current state to previous state;
                // and calculate new data
                for (int i = 0; i < Buttons.Length; i++)
                {
                    psButtons[i] = Buttons[i];
                    Buttons[i] = bs[i] != 0;
                }
                //if (bs[i] == 0) Buttons[i] = false;
                //else Buttons[i] = true;
                #endregion
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
