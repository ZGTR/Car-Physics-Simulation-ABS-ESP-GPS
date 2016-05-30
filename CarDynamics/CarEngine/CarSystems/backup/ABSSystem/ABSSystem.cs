using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
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
	public class ABSSystem
	{
        private float VehicleSpeed, TyreRimSpeed, AngularSpeed;
        private float Mu_1, Mu_P,  MuDerived, Mu_bar;
        public float Mu;
        public float LongitudinalSlipRatio, S_P;
        private const float S_P_bar = 0.17f;
        const float GRAVITY = 9.8f;
        public float A_PacejkaForm, B_PacejkaForm, C_PacejkaForm;
        private float MomentOfInertia, MomentOfInertiaValue;
        public  float BrakeTorqueValue,BrakingTorque;
        private float WheelWngularAcceleration, WheelAcceleration;
        private float  BrkaingTorque_Max, BrkaingTorque_Min, BrakingTorque_bar;
        private Car MyCar;
        private float FrictionForce;
        private float TireMass;
        private int CurrWheel;


        public ABSSystem( Car _MyCar)
        {
            MyCar = _MyCar;
            TireMass = 10f;
            Mu_1 = 0.8f;
            Mu_P = 0.9f;
            S_P = 0.2f;
        }

        void CalcvehicleSpeed()
        {
            VehicleSpeed = (float)MyCar.velocity.Value;
        }
        //void CalcAngularSpeed()
        //{
        //    AngularSpeed = VehicleSpeed / (float)MyCar.tires[CurrWheel].radius;
        //}
        void CalcTyreRimSpeed()
        {
            //TyreRimSpeed = (float)MyCar.tires[CurrWheel].angularVelocity * (float)MyCar.tires[CurrWheel].radius;
           // TyreRimSpeed;
        }

        void CalcLongitudinalSlipRatio()
        {
             LongitudinalSlipRatio = (float)((MyCar.velocity.Value / MyCar.tires[CurrWheel].radius - MyCar.tires[CurrWheel].angularVelocity) 
                /(MyCar.velocity.Value / MyCar.tires[CurrWheel].radius));
             if (LongitudinalSlipRatio < 0.0000001)
                 LongitudinalSlipRatio = 0;
             // Math.Max(MyCar.velocity.Value / MyCar.tires[CurrWheel].radius, MyCar.tires[CurrWheel].angularVelocity));/
             //if ((MyCar.velocity.Value / MyCar.tires[CurrWheel].radius) < (MyCar.tires[CurrWheel].angularVelocity))
             //    MyCar.MyForm.frontorquT.Text = " True";
             //else
             //    MyCar.MyForm.frontorquT.Text = " False";
             //if (CurrWheel < 2)
             //    MyCar.MyForm.SlipT.Text = (FormatingNumber(LongitudinalSlipRatio, "")).ToString();
             //else
             //{
             //    MyCar.MyForm.slipTF.Text = (FormatingNumber(LongitudinalSlipRatio, "")).ToString();
                // MyCar.MyForm.SlipValusT.Text += (FormatingNumber(LongitudinalSlipRatio, "")).ToString() + "\t";
             //}
        }
        private String FormatingNumber(float numberToFormat, String unit)
        {
            return String.Format("{0:0.0000}", numberToFormat) + unit;
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
            C_PacejkaForm = ((Mu_1 * (1+(float)Math.Pow( S_P, 2))) - (2 * Mu_P * S_P)) / (Mu_P - Mu_1);
        }
        void CalcMuFromPacejka()
        {
             Mu = A_PacejkaForm * LongitudinalSlipRatio / (B_PacejkaForm + (C_PacejkaForm * LongitudinalSlipRatio) + (float)Math.Pow(LongitudinalSlipRatio, 2));
           //  MyCar.MyForm.MuT.Text = Mu.ToString();
        }


        void CalcMomentOfInertia()
        {
            float inertia = 0.5f * (float)MyCar.tires[CurrWheel].TireMass * (float)MyCar.tires[CurrWheel].TireMass * (float)MyCar.tires[CurrWheel].radius;
            MomentOfInertia = inertia;
        }
        void CalcMomentOfInertiaValue()
        {
            MomentOfInertiaValue = ((float)MyCar.MASS  
                * (float)MyCar.tires[CurrWheel].radius 
                * (float)MyCar.tires[CurrWheel].radius) 
                / MomentOfInertia;
        }
        void CalcMuDerived()
        {
        //    float temp1 = A_PacejkaForm * B_PacejkaForm 
        //        + A_PacejkaForm * C_PacejkaForm * LongitudinalSlipRatio 
        //        + A_PacejkaForm * LongitudinalSlipRatio;
        //    float temp2 = (C_PacejkaForm + 2 * LongitudinalSlipRatio) * A_PacejkaForm * LongitudinalSlipRatio;
            float temp4 = (A_PacejkaForm * B_PacejkaForm ) - (A_PacejkaForm * LongitudinalSlipRatio * LongitudinalSlipRatio);
            float temp3 = (float)Math.Pow((B_PacejkaForm + C_PacejkaForm * LongitudinalSlipRatio + LongitudinalSlipRatio * LongitudinalSlipRatio), 2);
              MuDerived = temp4 / temp3;
        }
        void CalcWheelWngularAcceleration()
        {
            WheelWngularAcceleration = ((MomentOfInertiaValue * GRAVITY) / VehicleSpeed) * MuDerived * (BrakeTorqueValue - (MomentOfInertiaValue + 1 - LongitudinalSlipRatio) * Mu);
        }
        void CalcWheelAcceleration()
        {
            WheelAcceleration = MyCar.tires[CurrWheel].acceleration;
        }
        void CalcBrakingTorque(float brakeTorque)
        {

            //float deceleraton = 2 * Mu * GRAVITY;
            //float Brakeforce = deceleraton * (MyCar.MASS / 4.0f);
            //BrakingTorque = Brakeforce * (float)MyCar.tires[CurrWheel].radius;
            BrakingTorque = brakeTorque;
        }
        void CalcBrakingTorqueValue()
        {
            BrakeTorqueValue = (BrakingTorque * (float)MyCar.tires[CurrWheel].radius) / (MomentOfInertia * GRAVITY);
        }
        void CalcBrakingTorque_MaxMin()
        {
            Mu_bar = (BrakeTorqueValue + (WheelAcceleration / GRAVITY)) / MomentOfInertiaValue;
            BrakingTorque_bar = (MomentOfInertiaValue + 1 - S_P_bar) * Mu_bar;
            BrkaingTorque_Max = BrakingTorque_bar + 1;
            BrkaingTorque_Min = BrakingTorque_bar - 1;
        }

        int TimeSinceLastFrame = 64,TimePerSecond;
        int TimePerFrame = 66;
        int countPerSecond = 0; 
        void GetABSDecision(GameTime gameTime)
        {
            //if (WheelWngularAcceleration < 0)
            //{
            TimeSinceLastFrame += gameTime.ElapsedGameTime.Milliseconds;
            TimePerSecond += gameTime.ElapsedGameTime.Milliseconds;
            //if (TimeSinceLastFrame > TimePerFrame)
            //{
                
                if (TimePerSecond > 1000)
                {

                   // MyCar.MyForm.ABSstatusT.Text += countPerSecond.ToString() + " "; ;
                    countPerSecond = 0;
                    TimePerSecond = 0;
                }
                else
                {
                    countPerSecond++;
                }
                if (MuDerived <0)
                { 

                    BrakingTorque = 0;
                    //MyCar.MyForm.ABSstatusT.Text += CurrWheel.ToString() +"off\t"; 
                    //BrakingTorque = 0;
                }
                else if (LongitudinalSlipRatio < S_P) //(Mu_bar < 0.1f) //&& )
                //BrakeTorqueValue = BrkaingTorque_Max;
                //BrakingTorque = 400;
                {
                   // MyCar.MyForm.ABSstatusT.Text += CurrWheel.ToString()+"on\t"; 
                    BrakingTorque = MyCar.carSystems.Brakes.MaxTorque; 
                }
                 
                 

                 
                //}
               
                ////if (LongitudinalSlipRatio > 0.3f)
                ////    // BrakeTorqueValue = BrkaingTorque_Min;
                ////    BrakingTorque = 0;
                ////else if (LongitudinalSlipRatio < 0.1f) //&& (LongitudinalSlipRatio < S_P))
                ////    //BrakeTorqueValue = BrkaingTorque_Max;
                ////    BrakingTorque = 400;
                ////// BrakingTorque =( BrakeTorqueValue * MomentOfInertia*GRAVITY)/(float)MyCar.tires[0].radius;
                //////}
                TimeSinceLastFrame = 0;
            //}
            
        }

        void ResetAll()
        {
             VehicleSpeed=TyreRimSpeed=AngularSpeed= 0;
             Mu=MuDerived=Mu_bar =LongitudinalSlipRatio =0;
             A_PacejkaForm=B_PacejkaForm=C_PacejkaForm = 0;
             MomentOfInertia = MomentOfInertiaValue = BrakeTorqueValue = 0;
             WheelWngularAcceleration=WheelAcceleration=0;
             BrakingTorque = BrkaingTorque_Max = BrkaingTorque_Min = BrakingTorque_bar = 0;
             FrictionForce = 0;
        }
        public float updateABS(int i, float brakeTorque, bool ABSenabled, GameTime gameTime)
        {
            CurrWheel = i;
            ResetAll();
            CalcvehicleSpeed(); 
            //CalcAngularSpeed();
            //CalcTyreRimSpeed();
            CalcLongitudinalSlipRatio();
            A_Pacejka();
            B_Pacejka();
            C_Pacejka();
            CalcMuFromPacejka();
            if (ABSenabled && (VehicleSpeed > 10))
            {
                CalcMomentOfInertia();
                CalcMomentOfInertiaValue();
                CalcBrakingTorque(brakeTorque);
                CalcBrakingTorqueValue();
                CalcWheelAcceleration();
                CalcMuDerived();
                CalcWheelWngularAcceleration();
                CalcBrakingTorque_MaxMin();
                GetABSDecision(gameTime);
                CalcFrictionForce();
                return BrakingTorque;
            }
           return MyCar.carSystems.Brakes.MaxTorque;
            
        }

        void  CalcFrictionForce()
        {
              FrictionForce = BrakeTorqueValue / (float)MyCar.tires[CurrWheel].radius;
         }
        public float frictionforce 
        {
            get { return FrictionForce; }
        }
      




	}
}
