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
using GUI;

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
            S_P = 0.2f;

        }
        void CalcvehicleSpeed()
        {
            VehicleSpeed = (float)MyCar.velocity.Value;
        }

        private String FormatingNumber(float numberToFormat, String unit)
        {
            return String.Format("{0:0.0000}", numberToFormat) + unit;
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

        void GetABSDecision(GameTime gameTime)
        {
                if (MuDerived <0)
                { 
                    BrakingTorque = 0;
                    DirectInputWrapper.SendForce(DirectInputWrapper.ForceType.HardRumble, true);
                }
                else if (LongitudinalSlipRatio < S_P)
                {
                     BrakingTorque = MyCar.carSystems.Brakes.MaxTorque; 
                }
        }

        void ResetAll()
        {
             //VehicleSpeed=TyreRimSpeed=AngularSpeed= 0;
             MuDerived=Mu_bar =0;
             //LongitudinalSlipRatio =Mu= A_PacejkaForm=B_PacejkaForm=C_PacejkaForm = 0;
             MomentOfInertia = MomentOfInertiaValue = BrakeTorqueValue = 0;
             WheelWngularAcceleration=WheelAcceleration=0;
             BrakingTorque = BrkaingTorque_Max = BrkaingTorque_Min = BrakingTorque_bar = 0;
             FrictionForce = 0;
        }
        public float updateABS(int i, float brakeTorque, bool ABSenabled, GameTime gameTime)
        {
            CurrWheel = i;
            ResetAll();
            //CalcvehicleSpeed(); 
            //CalcAngularSpeed();
            //CalcTyreRimSpeed();
            //CalcLongitudinalSlipRatio();
            //A_Pacejka();
            //B_Pacejka();
            ////C_Pacejka();
            //CalcMuFromPacejka();
            CalcMomentOfInertia();
            CalcMomentOfInertiaValue();
            CalcBrakingTorque(brakeTorque);
            CalcBrakingTorqueValue();
            CalcWheelAcceleration();
            CalcMuDerived();
            CalcWheelWngularAcceleration();
            CalcBrakingTorque_MaxMin();
            GetABSDecision(gameTime);
            return BrakingTorque;

        }

        // update ABS Form
        private void getSurface(String type)
        {
            if (type.Equals("Dry Asphalt"))
            {
                Mu_P = 0.95f; Mu_1 = 0.85f;
            }
            else if (type.Equals("Wet Asphalt / little Water"))
            {
                Mu_P = 0.8f; Mu_1 = 0.6f;
            }
            else if (type.Equals("Wet Asphalt / dense water"))
            {
                Mu_P = 0.75f; Mu_1 = 0.5f;
            }
            else if (type.Equals("New snow"))
            {
                Mu_P = 0.32f; Mu_1 = 0.42f;
            }
            else if (type.Equals("Compressed snow"))
            {
                Mu_P = 0.3f; Mu_1 = 0.2f;
            }
            else if (type.Equals("Ice"))
            {
                Mu_P = 0.11f; Mu_1 = 0.112f;
            }

        }
        public void UpdateABSValues(ref ABS_DataForm ABSform)
        {
            object obj = ABSform.Surface_Type.SelectedObject;
            if (obj != null)
            {

                String Surface = (String)(ABSform.Surface_Type.SelectedObject);
                if (Surface.Equals("Other"))
                {
                    if (ABSform.Mu_Peak.Text != "")
                        Mu_P = float.Parse(ABSform.Mu_Peak.Text);
                    if (ABSform.Mu_low.Text != "")
                        Mu_1 = float.Parse(ABSform.Mu_low.Text);
                }
                else
                {
                    getSurface(Surface);
                    ABSform.Mu_low.Text = (FormatingNumber(Mu_1, "")).ToString();
                    ABSform.Mu_Peak.Text = (FormatingNumber(Mu_P, "")).ToString();
                }
            }
            ABSform.A_Pacejka_Formula_T.Text = FormatingNumber(A_PacejkaForm, "");
            ABSform.ABS_Status_T.Text = MyCar.ABSEnabled ? " ON " : " OFF ";
            ABSform.B_Pacejka_Formula_T.Text = FormatingNumber(B_PacejkaForm, "");
            ABSform.C_Pacejka_Formula_T.Text = FormatingNumber(C_PacejkaForm, "");
            ABSform.LongitudinalSlipRatio_T.Text = FormatingNumber(LongitudinalSlipRatio, "");
            ABSform.Moment_Of_Inertia_T.Text = FormatingNumber(MomentOfInertia, "Kg * m^2 ");
            ABSform.Mu_T.Text = FormatingNumber(Mu, "");
            ABSform.Mu_Dervied_T.Text = FormatingNumber(MuDerived, "");
        }

      




	}
}
