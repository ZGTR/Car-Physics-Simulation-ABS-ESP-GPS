using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace CarDynamics
{
	public class Tire
	{
		Matrix translation;
		public Matrix Translation
		{
			get
			{
				return translation;
			}
		}

        private float Acceleration;
        public float acceleration
        {
            get
            {
                return Acceleration;
            }
        }

        private float LinearVelocity;
        public float linearVelocity
        {
            get
            {
                return LinearVelocity;
            }
        }

		const double RoolingCoefficient = 0.015;		 //Coefficient of Rolling friction

		private double Surface;
		public double surface
		{
			get
			{
				return Surface;
			}
		}

        //public void UpdateAngularSpeed(double velocity)
        //{
        //    this.AngularVelocity = velocity / this.radius;
        //}

		private double AngularVelocity;
		public double angularVelocity
		{
			get
			{
				return AngularVelocity;
			}
		}

		private double Torque;
		public double torque
		{
			set
			{
				Torque = value;
			}
			get
			{
				return Torque;
			}
		}

		private int Angle = 0;	 // For front tires
		public int angle
		{
			get
			{ 
				return Angle;
			}
		}
        public void SetAngle(int Value)
        {
            this.Angle = Value;
        }

		private double Radius;
		public double radius
		{
			get
			{
				return Radius;
			}
            set
            {
                Radius = value;
            }
		}


        private int tireID;
        public Tire(int tireId)
        {
            //this.translation = translation;
            this.AngularVelocity = 0;
            this.Radius = 0.3186;
            this.Surface = 2 * Math.PI * Radius;
            this.Torque = 0;
            this.tireID = tireId;
        }

        public void UpdateLongAcceleration(GameTime gameTime, float mass, float frontMass, float backMass, double velocity, bool front)
        {
            float oldVelocity = this.linearVelocity;

            if (front)
            {
                if (frontMass > backMass)
                {
                    float per = backMass / mass;
                    this.LinearVelocity = (float)(velocity + velocity * per);
                }
                else
                {
                    float per = frontMass / mass;
                    this.LinearVelocity = (float)(velocity - velocity * per);
                }
            }
            else
            {
                if (frontMass > backMass)
                {
                    float per = backMass / mass;
                    this.LinearVelocity = (float)(velocity - velocity * per);
                }
                else
                {
                    float per = frontMass / mass;
                    this.LinearVelocity = (float)(velocity + velocity * per);
                }
            }
            this.Acceleration = (this.linearVelocity - oldVelocity)
                / gameTime.ElapsedGameTime.Milliseconds / 1000;
        }

        static float floatAngleToSteer = 0 ;
        public void Steer(bool direction)   //1 <=> Left	0 <=> Right
		{
            KeyboardState keyboardState = Keyboard.GetState();
            if (floatAngleToSteer > 1 || floatAngleToSteer < -1)
            {
                floatAngleToSteer = 0; 
            }
            floatAngleToSteer += 0.6f;
            floatAngleToSteer *= SteerPerc;
			if (direction && this.Angle > -60)
			{
                if (!(keyboardState.IsKeyDown(Keys.A) && (keyboardState.IsKeyDown(Keys.D))))
                {
                    this.Angle -= (int)floatAngleToSteer;
                }
                else
                {
                    this.Angle -= 1;
                }
			}
            else if (!direction && this.Angle < 60)
			{
                if (!(keyboardState.IsKeyDown(Keys.A) && (keyboardState.IsKeyDown(Keys.D))))
                {
                    this.Angle += (int)floatAngleToSteer;
                }
                else
                {
                    this.Angle += 1;
                }
			}
			//if (Math.Abs(Angle) < 15)
			//    Angle = 0;
		}


        // Current animation positions.
        float wheelRotationValue = 0.0f;
        static float steerRotationValue = 0;
        #region Properties


        /// <summary>
        /// Gets or sets the wheel rotation amount.
        /// </summary>
        public float WheelRotation
        {
            get { return wheelRotationValue; }
            set { wheelRotationValue = value; }
        }


        /// <summary>
        /// Gets or sets the steering rotation amount.
        /// </summary>
        public static float SteerRotation
        {
            get
            {
                if (steerRotationValue < 0 && !Keyboard.GetState().IsKeyDown(Keys.D))
                    steerRotationValue += 0.005f;
                else
                    if (steerRotationValue > 0 && !Keyboard.GetState().IsKeyDown(Keys.A))
                        steerRotationValue -= 0.005f;

                return steerRotationValue;
            }
            set { steerRotationValue = value; }
        }

        #endregion

        public void Update(GameTime gameTime)
        {
            float time = (float)gameTime.TotalGameTime.TotalSeconds;
            wheelRotationValue = time * 2;
        }

        // auther : zaher , wheel angular velocity , and wheel liner velocity
        public float SteerPerc = 1;
        public bool ESPActive = false;
        public bool WheelLocked = false;
        public float TireMass = 10f;
        int TimeSinceLastFrame = 0;
        int TimePerFrame = 66;
        public float AngularAcceleration = 0;
        public void updateAngularVelocity(Car myCar, float velocity, float _torque, float _braketorqe
            , GameTime gametime, int index, bool brakeEnabled)
        {
            TimeSinceLastFrame += gametime.ElapsedGameTime.Milliseconds;
            //if (TimeSinceLastFrame > TimePerFrame)
            //{
            if (index == 3)
                TimeSinceLastFrame = 0;
            float temp = TimePerFrame;


            float totalTorque;
            if (!brakeEnabled && !ESPActive)
            {
                totalTorque = _torque;
                this.AngularVelocity = (float)velocity / (float)radius;
            }
            else
            {
                //if (velocity > 0.05f)
                //{
                //totalTorque = _torque - _braketorqe - ((index < 2 ? myCar.m_f : myCar.m_r) * (float)this.radius * 1000);
                totalTorque = ((float)this.radius * myCar.carSystems.Brakes.myABSSystem[index].Mu * (index < 2 ? myCar.m_r : myCar.m_f))
                        - _braketorqe;
                float inertia = 0.5f * (float)TireMass * (float)TireMass * (float)radius;
                float Wdot = totalTorque / inertia;
                AngularAcceleration = Wdot;
                this.AngularVelocity = this.AngularVelocity + Wdot * (temp / 1000);
                double temp2 = this.AngularVelocity;
                //}
            }
            if (AngularVelocity <= 0)
                AngularVelocity = 0;
            //}
        }
	}
}