using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
    public class BrakeSystem
    {
        private float Force_BrakePedal =1800f;
        private float Pressuer_MasterCylinder;
        private float Area_MasterCylinder ;
        private float Area_Caliper;
        private float Force_Caliper, Force_Calmp, Force_PadFriction;
        public float Force_TireGround;
        private float Frictioncoefficient_BrakePad=0.45f, Frictioncoefficient_Road = 0.8f;
        private float Torque_Rotor, Torque_Wheel;
        public float MaxTorque;
        public float Torque_Tire;
        private float Radius_eff = 0.053f, Radius_Tire;
        private float BrakeForce;
        private Car MyCar;
        public  ABSSystem [] myABSSystem;
        public float ButtonScale;
         
        /// <summary>
        //    pedal_force_in= 300 , ratio 6:1
        //    pedal_force_out = 2130
        //    MasterCylinder = 0.01905 m
        //    piston size = 0.038 m
        //    roto_outer_diameter = 0.332
        //    roto_inner_diameter =   226.0
        /// </summary>
        /// <param name="_Mycar"></param>
        public BrakeSystem(Car _Mycar)
        {
            MyCar = _Mycar;
            MaxTorque = CalcT_R();
            //MaxTorque = 1000;
            myABSSystem =  new ABSSystem[4];
            for (int i = 0; i < 4; i++)
                myABSSystem[i] = new ABSSystem(MyCar);
            this.ButtonScale = 1;
        }
        //float CalcFBP()
        //{
        //    //
        //}
        float calcA_MC()
        {
            return (Area_MasterCylinder = (float)Math.PI * 0.001905f * 0.01905f);
        }
        float CalcP_MC()
        {
            return (Pressuer_MasterCylinder = Force_BrakePedal/calcA_MC());
        }
        float CalcA_C()
        {
            return (Area_Caliper = (float)Math.PI * 0.038f * 0.038f);
        }
        float CalcF_C()
        {
            Force_Caliper = CalcP_MC() * CalcA_C();
            return (Force_Calmp = 4 * Force_Caliper );
        }
        float CalcF_PF()
        {
            return (Force_PadFriction = CalcF_C() * Frictioncoefficient_BrakePad);
        }
        float CalcT_R()
        {
            return (Torque_Rotor = Torque_Tire = Torque_Wheel = CalcF_PF() * Radius_eff);
            //MyCar.carSystems.ABS.updateABS(i);
            //return MyCar.carSystems.ABS.BrakeTorqueValue;
        }


        public float Tire2GroundForce(int i,float brakeTorque)
        {
            return CalcF_TG(i);
        }
        float CalcF_TG(int i)
        {
            Radius_Tire = (float)MyCar.tires[0].radius;
            Force_TireGround = (i <2 ? MyCar.m_r : MyCar.m_f) * myABSSystem[i].Mu * 1000;
            //if (i < 2)
            //    MyCar.MyForm.reattorqueT.Text = Force_TireGround.ToString()+" || "+(myABSSystem[i].Mu).ToString ();
            //else
            //    MyCar.MyForm.frontorquT.Text = Force_TireGround.ToString() + " || " + (myABSSystem[i].Mu).ToString();
            return Force_TireGround;
        }

        float CalcBrakeTorque(int i, float brakeTorque, bool ABSenabled, GameTime gameTime)
        {
            return myABSSystem[i].updateABS(i, brakeTorque, ABSenabled, gameTime);
           
            //  return 500f;
        }
        public float BrakesUpdate(int i, float brakeTorque,bool ABSenabled, GameTime gameTime)
        {
            return CalcBrakeTorque(i, brakeTorque, ABSenabled, gameTime);
 
        }
    }
}
