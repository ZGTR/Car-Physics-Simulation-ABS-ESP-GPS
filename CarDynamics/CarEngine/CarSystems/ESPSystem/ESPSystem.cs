using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Timers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;

namespace CarDynamics
{
    public class ESPSystem
    {
        public String CarStatus = "";
        public String TurnSide = "";
        public bool ESPStatus;
        private Car ESPCar;
        private float alpha_f;
        private float alpha_r; 
        private bool brakeFrontRight = false;
        private bool brakeFrontLeft = false;
        private bool brakeRearRight = false;
        private bool brakeRearLeft = false;
        public bool ESPenabled = false;

        public ESPSystem(Car myCar)
        {
            this.ESPCar = myCar;
        }

        private void Calculating_alphas() 
        {
            alpha_f = ESPCar.alpha_f;
            alpha_r = ESPCar.alpha_r;
        }

        private bool Understeering()
        {
            return alpha_f > alpha_r;
        }

        private bool Oversteering()
        {
            return alpha_f < alpha_r;
        }

        private void GetESPDecision(
            ref bool brakeFrontRight
            , ref bool brakeFrontLeft
            , ref bool brakeRearRight
            , ref bool brakeRearLeft
            , GameTime gametime)
        {
            if (ESPCar.RightTurn())
            {
                if (Oversteering())
                {
                    brakeFrontLeft = true;
                    ESPStatus = true;
                    CarStatus = "Oversteering";
                    TurnSide = "RightTurn";
                }
                else
                {
                    if (Understeering())
                    {
                        brakeRearRight = true;
                        ESPStatus = true;
                        CarStatus = "Understeering";
                        TurnSide = "RightTurn";
                    }
                }
            }
            else
            {
                if (ESPCar.LeftTurn())
                {
                    if (Oversteering())
                    {
                        brakeFrontRight = true;
                        ESPStatus = true;
                        CarStatus = "Oversteering";
                        TurnSide = "LeftTurn";
                    }
                    else
                    {
                        if (Understeering())
                        {
                            brakeRearLeft = true;
                            ESPStatus = true;
                            TurnSide = "LeftTurn";
                            CarStatus = "Understeering";
                        }
                    }
                }
                else
                {
                    ESPStatus = false;
                    CarStatus = "Normal";
                    TurnSide = "No Turn";
                }
            }
        }

        // Determine which Tire Should be breaked now 
        public string DefineBrakingTire()
        {
            if (brakeFrontLeft == true)
            {
                return "brakeFrontLeft";
            }
            else
            {
                if (brakeFrontRight== true)
                {
                    return "brakeFrontRight";
                }
                else 
                {
                    if (brakeRearLeft == true)
                    {
                        return "brakeRearLeft";
                    }
                    else
                    {
                        if (brakeRearRight == true)
                        {
                            return "brakeRearRight";
                        }
                        else
                        {
                            return "None";
                        }                        
                    }
                }                        
            }
        }

        private void NormalizeBrakes()
        {
            brakeFrontRight = false;
            brakeFrontLeft = false;
            brakeRearRight = false;
            brakeRearLeft = false;
            for (int j = 0; j < 4; j++)
            {
                    ESPCar.tires[j].ESPActive = false;
            }
            ESPCar.ESPAtivated = false;
        }

        // Manipulating ESP System
        public void UpdateESPSystem(GameTime gameTime)
        {
            
            // Setting Brakes back to normal (off) 
            
                NormalizeBrakes();
                if (ESPenabled)
                {
                // Updating alpha angles 
                Calculating_alphas();

                // Getting decision about deploying ESP System
                GetESPDecision(ref brakeFrontRight, ref brakeFrontLeft, ref brakeRearRight, ref brakeRearLeft, gameTime);

                // Send Braking message to ABS System
                // edit by zaher

                int i = -1;
                if (brakeRearLeft)
                    i = 0;
                else if (brakeRearRight)
                    i = 1;
                else if (brakeFrontLeft)
                    i = 2;
                else
                    if (brakeFrontRight)
                        i = 3;

                if (ESPCar.velocity.Value > 15)
                {
                    if (i > 0)
                    {
                        ESPCar.ESPAtivated = true;
                        float scale = ESPCar.Yaw_Rate / 6.2f; // converting to radian
                        ESPCar.updateSingleBrakeForce(i, ESPCar.carSystems.Brakes.MaxTorque * scale, gameTime);
                        ESPCar.tires[i].ESPActive = true;
                    }
                }

            }       
        }
    }
}