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
using GUI;

namespace CarDynamics
{
    public class BrakeSystem
    {
        private float Force_BrakePedal = 1800f, Driver_Force,ratio;
        private float Pressuer_MasterCylinder;
        private float Area_MasterCylinder ;
        private float Area_Caliper;
        private float Force_Caliper, Force_Calmp, Force_PadFriction;
        public float Force_TireGround;
        private float Frictioncoefficient_BrakePad=0.45f, Frictioncoefficient_Road = 0.8f;
        private float Torque_Rotor, Torque_Wheel;
        public float MaxTorque,RealMaxTorque;
        public float Torque_Tire;
        private float Radius_eff = 0.053f, Radius_Tire, MCdiameter;
        private float BrakeForce;

        private Car MyCar;
        public  ABSSystem [] myABSSystem;
        public float ButtonScale;
        private int pistonsNumber = 4;
         
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
            RealMaxTorque = MaxTorque = CalcT_R();
            //MaxTorque = 1000;
            Driver_Force = 300;
            ratio = 6;
            myABSSystem =  new ABSSystem[4];
            for (int i = 0; i < 4; i++)
                myABSSystem[i] = new ABSSystem(MyCar);
            this.ButtonScale = 1;
            MCdiameter = 0.01905f;
            Mu_1 = 0.8f;
            Mu_P = 0.9f;
            S_P = 0.2f;

        }
        float CalcFBP()
        {
         return Force_BrakePedal = Driver_Force * ratio;
        }
        float calcA_MC()
        {
            return (Area_MasterCylinder = (float)Math.PI * 0.01905f * 0.01905f);
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
            return (Force_Calmp =2* pistonsNumber * Force_Caliper );
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
        #region Mu and Slip functions 
        
        public float Mu, LongitudinalSlipRatio, S_P;
        private float Mu_1, Mu_P;
        public float A_PacejkaForm, B_PacejkaForm, C_PacejkaForm;
        void CalcLongitudinalSlipRatio(int i)
        {
             LongitudinalSlipRatio = (float)((MyCar.velocity.Value / MyCar.tires[i].radius - MyCar.tires[i].angularVelocity)
               / (MyCar.velocity.Value / MyCar.tires[i].radius));
            if (LongitudinalSlipRatio < 0.0000001)
                LongitudinalSlipRatio = 0;            
            if (LongitudinalSlipRatio > 0.99998)
                MyCar.tires[i].WheelLocked = true;
            else
                MyCar.tires[i].WheelLocked = false;

        }
        void A_Pacejka()
        {
            A_PacejkaForm = ((Mu_P * Mu_1) / (Mu_P - Mu_1)) * (float)Math.Pow(1 - S_P, 2);
        }
        void B_Pacejka()
        {
            B_PacejkaForm = ((float)Math.Pow(S_P, 2));
        }
        void C_Pacejka()
        {
            C_PacejkaForm = ((Mu_1 * (1 + (float)Math.Pow(S_P, 2))) - (2 * Mu_P * S_P)) / (Mu_P - Mu_1);
        }
        void CalcMuFromPacejka()
        {
            Mu = A_PacejkaForm * LongitudinalSlipRatio / (B_PacejkaForm + (C_PacejkaForm * LongitudinalSlipRatio) + (float)Math.Pow(LongitudinalSlipRatio, 2));
        }
        #endregion

        // zaher12

        public float Tire2GroundForce(int i,float brakeTorque)
        {
            Force_TireGround = (i <2 ? MyCar.m_r : MyCar.m_f) * myABSSystem[i].Mu * 1000;
            return Force_TireGround;
        }
        public float BrakesUpdate(int i, float brakeTorque,bool ABSenabled, GameTime gameTime)
        {
            CalcLongitudinalSlipRatio(i);
            A_Pacejka();
            B_Pacejka();
            C_Pacejka();
            CalcMuFromPacejka();
            this.myABSSystem[i].LongitudinalSlipRatio = this.LongitudinalSlipRatio;
            this.myABSSystem[i].A_PacejkaForm = this.A_PacejkaForm;
            this.myABSSystem[i].B_PacejkaForm = this.B_PacejkaForm;
            this.myABSSystem[i].C_PacejkaForm = this.C_PacejkaForm;
            this.myABSSystem[i].Mu = this.Mu;
           // MaxTorque = RealMaxTorque;
            if (ABSenabled && MyCar.velocity.Value > 10)
               return myABSSystem[i].updateABS(i, brakeTorque, ABSenabled, gameTime);
            return brakeTorque; 
        }

        // update Brake form
        private String FormatingNumber(float numberToFormat, String unit)
        {
            return String.Format("{0:0.0000}", numberToFormat) + unit;
        }
        public void updateBrakeValues(ref BrakeDataForm brakeform)
        {
            brakeform.Caliper_Area_T.Text = FormatingNumber(this.Area_Caliper, "m^2");
            brakeform.Caliper_Force_T.Text = FormatingNumber(this.Force_Caliper, "N");
            brakeform.Driver_Force_T.Text = FormatingNumber(this.Driver_Force, "N");
            brakeform.input_Torque_T.Text = FormatingNumber(this.MaxTorque, "N.m");
            brakeform.Max_Torque_T.Text = FormatingNumber(this.RealMaxTorque, "N.m");
            brakeform.MC_Area_T.Text = FormatingNumber(this.Area_MasterCylinder, "m^2");
            brakeform.MC_presssuer_T.Text = FormatingNumber(this.Pressuer_MasterCylinder, "N/m^2");
            brakeform.MCdiameter_T.Text = FormatingNumber(this.MCdiameter, "m");
            brakeform.Pedal_ratio_T.Text = FormatingNumber(this.ratio, "");
            brakeform.PF_Coef_T.Text = FormatingNumber(this.Frictioncoefficient_BrakePad, "");
            brakeform.PF_Force_T.Text = FormatingNumber(this.Force_PadFriction, "N");
            brakeform.Pistons_Number_T.Text = FormatingNumber(this.pistonsNumber, "");
            brakeform.Rotor_Brake_torque_T.Text = FormatingNumber(this.Torque_Rotor, "N.m");
            brakeform.Tire_Gorund_Force_T.Text = FormatingNumber(this.Force_TireGround, "");
        }
        public void updateInputBrakeValues(ref BrakeDataForm brakeform)
        {
            this.MCdiameter = float.Parse(brakeform.MCdiameter_T.Text);
            this.ratio = float.Parse(brakeform.Pedal_ratio_T.Text);
            this.Frictioncoefficient_BrakePad = float.Parse(brakeform.PF_Coef_T.Text);
            this.pistonsNumber = int.Parse(brakeform.Pistons_Number_T.Text);
        }
    }
}
