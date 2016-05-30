using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using CameraViewer;
using GUI;
using Sounds;
namespace CarDynamics
{
	public class Car
    {
        //wpublic static EngineTo

        #region Car-related Variables
        // Jaeger - Car Variables
        // The body has 4 tires and 1 engine
        public Tire[] tires;
        public Engine engine;
        public Gear gear;
        public CarSystems carSystems;
        public const double DragCoefficient = 4.8;			// In Porsche
        public bool CarbrakeEnabled = false;
        public GraphicsDeviceManager graphicsDeviceManager; 

        bool brakeEnabled;


        // The model variable
        Model model;
        Matrix[] carTransforms;
        Matrix translation = Matrix.Identity;
        public Matrix Translation
        {
            set
            {
                translation = value;
            }
            get
            {
                return translation;
            }
        }

        private ForceVector Acceleration;
        public ForceVector acceleration
        {
            get
            {
                return Acceleration;
            }
        }

        private double AirDensity;

        private double Width = 1.78d;
        public double width
        {
            get
            {
                return Width;
            }
        }

        private double Height = 1.28d;
        public double height
        {
            get
            {
                return Height;
            }
        }

        public double FrontArea;

        private double RollingCoefficient = 0.03;
        public double rollingCoefficient
        {
            get
            {
                return RollingCoefficient;
            }
        }

        private ForceVector Velocity;
        public ForceVector velocity
        {
            get
            {
                return Velocity;
            }
        }

        // NOUR        
        private Vector3 Position = new Vector3();
        public Vector3 position
        {
            get
            {
                UpdatePosition();
                return Position;
            }
        }

        private void UpdatePosition()
        {
            Position.X = translation.Translation.X;
            Position.Y = translation.Translation.Y;
            Position.Z = translation.Translation.Z;
        }


        private float Angle = 0;
        public float GetAngle()
        {
            return Angle;
        }

        private bool steeredThisFrame;

        public void Steer(bool direction)
        {
            // edit by zaher
            if (!tires[2].WheelLocked && !tires[3].WheelLocked)
            {
                this.tires[2].Steer(direction);
                this.tires[3].SetAngle(this.tires[2].angle);
                steeredThisFrame = true;
            }
        }

        private ForceVector OldGeneral;
        private int GeneralCounter = 0;
        
        #endregion

        # region ESP-related Variables
        // ZGTR - ESP variables
        public CarDataFrom MyForm;
        private float GRAVITY = 9.80665f;
        private readonly float L;
        private readonly float L_f;
        private readonly float L_r;
        public float MASS ;
        public readonly float CARWEIGHT;
        private readonly float heightOfCenterOfGravity = 20 * 0.0254f; // 20 in --> * 0.508 m  
        private float F_xf;
        private float F_xr;
        private float K_us;         // (rad)
        private float Radius;
        public float Yaw_Rate;      // (rad / sec)
        public float alpha_f;
        public float alpha_r; 
        private float c_af; // ( kn / rad )
        private float c_ar; // ( kn / rad )
        // private float c = 725.189f;
        // private float I = 215.9f;
        private float delta_f;
        public float m_f;
        public float m_r;
        private float tireAngle;
        private float lateralAcceleration;
        private float beta;
        private float Average_alpha; 
        private float v_x ;
        private float v_z ;
        public float Duration;
        // zaher12
        public bool ESPAtivated = false;


        #endregion

        #region Brake-related Variables
        // Zaher 
        float torque;
        public float[] brakeTorque;
        public float[] brakeForce;
        public bool ABSEnabled = false;
        #endregion

        #region Constructor
        public void InitMainComponent(ref InitializationForm initForm)
        {
            //Nour
            this.tires = new Tire[4];
            for (int i = 0; i < 4; i++)
                this.tires[i] = new Tire(i + 1);
            this.Width = float.Parse(initForm.VehicleWidth.Text);
            this.Height = float.Parse(initForm.VehicleHeigh.Text);
            this.MASS = float.Parse(initForm.VehicelWieght.Text);
            for (int i = 0; i < 4; i++)
                this.tires[0].radius = float.Parse(initForm.WheelRadius.Text);
            this.DragCoeff = float.Parse(initForm.DragCoeff.Text);
            this.RollingCoefficient = float.Parse(initForm.RollingCoeff.Text);
            this.GRAVITY = float.Parse(initForm.GravityConst.Text);
            this.differentialRatio = float.Parse(initForm.textBox1.Text);
            this.FrontArea = 0.85 * Width * Height;
            this.AirDensity = 1.29;// float.Parse(initForm.airDensity.Text);
        }

        float differentialRatio;
        float DragCoeff;
        float airDensity;

        public Car(Game1 game1 , Road road)
        {
            // ZGTR - ESP System
            this.carSystems = new CarSystems(this, true, true, game1 , road);

            // ZGTR
            this.MASS = 1500;
            this.L_f = 1.234f;  // L_f;
            this.L_r = 1.022f;  // L_r;
            this.L = L_f + L_r;
            this.CARWEIGHT = MASS * GRAVITY; 
            CalculateNative_m_f();
            CalculateNative_m_r();
            Calculate_c_af();
            Calculate_c_ar();
            MyForm = new CarDataFrom();
            //MyForm.Show();

            // Jaeger
            this.Acceleration = new ForceVector();
            this.Angle = 0;
            this.Velocity = new ForceVector();
    

            //One Engine :D
            this.engine = new Engine();

            //One Gear, too :D
            this.gear = new Gear();

            //zaher
            this.brakeTorque = new float[4];
            this.brakeForce = new float[4];
            for (int i = 0; i < 4; i++)
            {
                this.brakeTorque[i] = carSystems.Brakes.MaxTorque;
                this.brakeForce[i] = 0;

            }
        }

        public Car(Game1 game1, GPSRoad gpsroad)
        {
            // ZGTR - ESP System
            this.carSystems = new CarSystems(this, true, true, game1, gpsroad);

            // ZGTR
            this.MASS = 1500;
            this.L_f = 1.234f;  // L_f;
            this.L_r = 1.022f;  // L_r;
            this.L = L_f + L_r;
            this.CARWEIGHT = MASS * GRAVITY;
            CalculateNative_m_f();
            CalculateNative_m_r();
            Calculate_c_af();
            Calculate_c_ar();
            MyForm = new CarDataFrom();
            //MyForm.Show();

            // Jaeger
            this.Acceleration = new ForceVector();
            this.Angle = 0;
            this.Velocity = new ForceVector();


            //One Engine :D
            this.engine = new Engine();

            //One Gear, too :D
            this.gear = new Gear();


            //zaher
            this.brakeTorque = new float[4];
            this.brakeForce = new float[4];
            for (int i = 0; i < 4; i++)
            {
                this.brakeTorque[i] = carSystems.Brakes.MaxTorque;
                this.brakeForce[i] = 0;

            }

        }


        #endregion

        # region ESP-related Functions

        private void Calculate_K_us()
        {
            K_us = (m_f/c_af) - (m_r/c_ar); // rad
        }

        private void Calculate_c_af()
        {
            c_af = 117.44f ; // kn / rad
        }

        private void Calculate_c_ar()
        {
            c_ar = 144.93f ; // kn / rad
        }

        private void CalculateNative_m_f()
        {
            // Weight on single front-tyre (kN)
            m_f = (MASS * L_r) / (2 * (L)); // kg
            KgTokNConverter(ref m_f); // kN
        }

        private void CalculateNative_m_r()
        {
            // Weight on single rear-tyre (kN)
            m_r = (MASS * L_f) / (2 * (L)); // kg
            KgTokNConverter(ref m_r); // kN
        }

        private void Calculate_m_f()
        {
            // Weight on single front-tyre (kN)
            if (AccelerationBoolean)
                m_f = (float)(L_r / L)
                    * CARWEIGHT
                    - (float)(heightOfCenterOfGravity / L)
                    * MASS
                    * (float)Math.Abs(acceleration.Value); // Newton 

            else
                m_f = (float)(L_r / L)
                    * CARWEIGHT
                    + (float)(heightOfCenterOfGravity / L)
                    * MASS
                    * (float)Math.Abs(acceleration.Value); // Newton 
            m_f /= 2; // Divid the weight equally on both front tyres 
            NtokNConverter(ref m_f);
            //KgTokNConverter(ref m_f); // kN
        }

        private void Calculate_m_r()
        {
            // Weight on single rear-tyre (kN)
            if (AccelerationBoolean)
                m_r = (float)(L_f / L)
                    * CARWEIGHT
                    + (float)(heightOfCenterOfGravity / L)
                    * MASS
                    * (float)Math.Abs(acceleration.Value); // Newton
            else
                m_r = (float)(L_f / L)
                    * CARWEIGHT
                    - (float)(heightOfCenterOfGravity / L)
                    * MASS
                    * (float)Math.Abs(acceleration.Value); // Newton
            m_r /= 2; // Divid the weight equally on both front tyres 
            NtokNConverter(ref m_r);
            //KgTokNConverter(ref m_r); // kN
        }

        private void Calculate_TireAngle()
        {
            tireAngle = MathHelper.ToRadians(this.tires[2].angle) ;   // rad 
        }

        private void Calculate_Radius()
        {
            Calculate_TireAngle();
            this.Radius = (float)Math.Abs(((L_f + L_r) / Math.Sin(tireAngle)));
        }

        private void Calculate_delta_f()
        {
            this.delta_f = MathHelper.ToRadians(this.tires[2].angle);
        }

        private void Calculate_LateralAcceleration()
        {
            lateralAcceleration = (float)Math.Pow(Velocity.Value,2) / (GRAVITY * Radius) ;
            lateralAcceleration /= Duration;  
        }

        public void Calculate_Yaw_Rate()
        {
            Yaw_Rate =
                    (((float)Velocity.Value
                        / (L + (K_us * (float)(Math.Pow(Velocity.Value, 2) / GRAVITY)))
                    )
                    * (float)Math.Abs(delta_f));            
                  //(float)Velocity.Value / Radius ; 

            Yaw_Rate /= Duration;
        }

        private void Calculate_F_xf()
        {
            F_xf = (2*m_f)*((float) Math.Pow(Velocity.Value, 2)/(Radius*GRAVITY)); // kN
        }

        private void Calculate_F_xr()
        {
            F_xr = (2*m_r)*((float) Math.Pow(Velocity.Value, 2)/(Radius*GRAVITY)); // kN
        }

        private void Calculate_alpha_f()
        {
            Calculate_F_xf();
            alpha_f = F_xf/(2*c_af); // rad
        }

        private void Calculate_alpha_r()
        {
            Calculate_F_xr();
            alpha_r = F_xr/(2*c_ar); // rad
        }

        public void Calculate_AlphaAngles()
        {
            Calculate_alpha_f();
            Calculate_alpha_r();
        }

        public bool RightTurn()
        {
            return delta_f < 0;
        }

        public bool LeftTurn()
        {
            return delta_f > 0;
        }

        private void KgTokNConverter(ref float paramToConvert)
        {
            paramToConvert = (float) (paramToConvert
                                      *GRAVITY      // Converting to N
                                      /1000);       // Converting to kN
        }

        private float KgTokNConverter(float paramToConvert)
        {
            return (float)(paramToConvert
                                      * GRAVITY      // Converting to N
                                      / 1000);       // Converting to kN
        }

        private void NtokNConverter(ref float paramToConvert)
        {
            paramToConvert = (float)(paramToConvert
                                      / 1000);       // Converting to kN
        }

        private float NtokNConverter(float paramToConvert)
        {
            return  (float)(paramToConvert
                                      / 1000);       // Converting to kN
        }

        private void Calculate_beta()
        {
            if (alpha_f > alpha_r)
            {
                beta = delta_f + alpha_f - alpha_r ;
            }
            else if (alpha_f < alpha_r)
            {
                beta = delta_f + alpha_r - alpha_f;
            }
            else
            {
                beta = delta_f ;
            }
        }

        #endregion

        #region Car-related Functions

		public void IncreaseRPM(GameTime gameTime)								   // Pressing 'increase speed' button
		{
			if (gear.gearOn == 0)
				this.gear.ShiftUp();
			bool ShiftUp = false;
			this.engine.IncreaseRPM(ref ShiftUp, this.gear.gearOn);
			if (ShiftUp)
			{
				if (this.gear.ShiftUp())
				{
					this.engine.UpdateRPM(this.gear.GetRatio(this.gear.gearOn - 1)
                        , this.gear.GetRatio(this.gear.gearOn));
					//Sound.SetCue(this.gear.gearOn - 1, this.gear.gearOn);
				}
			}
		}

		public void DecreaseRPM(GameTime gameTime)								   
		{
			bool ShiftDown = false;
			this.engine.DecreaseRPM(ref ShiftDown);
			if (ShiftDown)
			{
				if (this.gear.ShiftDown())
				{
					this.engine.UpdateRPM(this.gear.GetRatio(this.gear.gearOn + 1)
                        , this.gear.GetRatio(this.gear.gearOn));
					//Sound.SetCue(this.gear.gearOn + 1, this.gear.gearOn);
				}
			}
		}
		public int GetGearOn()
		{
			return this.gear.gearOn;
        }
        # endregion       

        #region Brake-related Functions

        // Zaher 
        public void setBrakeTorque(float scale )
        {
            for (int i = 0; i < 4; i++)
                brakeTorque[i] = carSystems.Brakes.MaxTorque * scale ;
        }
        public void resetBrakeValues()
        {
            for (int i = 0; i < 4; i++)
            {
                if (!tires[i].ESPActive)
                {
                    tires[i].WheelLocked = false;
                    brakeTorque[i] = 0;
                    brakeForce[i] = 0;
                }
            }
        }
        public void updateSingleBrakeForce(int i,float braketorque, GameTime gameTime)
        {
            brakeTorque[i] = carSystems.Brakes.BrakesUpdate(i, braketorque, this.ABSEnabled, gameTime);
            brakeForce[i] = carSystems.Brakes.Tire2GroundForce(i, brakeTorque[i]);
        }
        public void updateBrakeForce(GameTime gameTime)
        {
            for (int i = 0; i < 4; i++)
            {
                    brakeTorque[i] = carSystems.Brakes.BrakesUpdate(i, brakeTorque[i], this.ABSEnabled, gameTime);
                brakeForce[i] = carSystems.Brakes.Tire2GroundForce(i, brakeTorque[i]);
            }
        }

        public void UpdateAngularVelocity(GameTime gameTime, bool brakeEnabled)
        {
            for (int i = 0; i < 4; i++)
            {
                tires[i].updateAngularVelocity(this, (float)this.velocity.Value, torque, brakeTorque[i], gameTime, i
                    , brakeEnabled);
            }
        }
        #endregion

        # region Updates

        // ZGTR - WRONG !!!!!!!
        private void ManipulateAngle()
        {
            // WRONG !!!!!!!
            if (this.tires[2].angle > 0)
            {
                this.Angle = this.Angle 
                    + (int)MathHelper.ToDegrees(alpha_f) 
                    - (int)MathHelper.ToDegrees(alpha_r) ;
            }
            else
            {
                if (this.tires[2].angle < 0)
                {
                    this.Angle = this.Angle
                        - (int)MathHelper.ToDegrees(alpha_f)
                        + (int)MathHelper.ToDegrees(alpha_r);
                }
                //else
                //{
                //    this.Angle += (int)MathHelper.ToDegrees(alpha_f) * Duration;
                //}
            }
        }   // ZGTR - WRONG !!!!!!!

        private void UpdateAngle()
        {
            double Value = 0;
            if ((Value = this.velocity.Value) != 0)
            {
                double Sin = Math.Abs(this.velocity.componentX / Value);

                this.Angle = Math.Abs(MathHelper.ToDegrees((float)Math.Abs(Math.Asin(Sin)))) % 180;

                if (velocity.componentX < 0 && velocity.componentZ < 0)
                    ;
                else if (velocity.componentX > 0 && velocity.componentZ < 0)
                    this.Angle *= -1;
                else if (velocity.componentX < 0 && velocity.componentZ > 0)
                    this.Angle = 180 - this.Angle;
                else if (velocity.componentX > 0 && velocity.componentZ > 0)
                    this.Angle = -180 + this.Angle;
                // ManipulateAngle(); // ZGTR - WRONG !!!!!!!
            }
        }

        bool AccelerationBoolean
        {
            get
            {
                double Value = this.acceleration.Value;
                float Angle = 0;
                if (Value != 0)
                {
                    double Sin = Math.Abs(this.acceleration.componentX / Value);

                    Angle = Math.Abs(MathHelper.ToDegrees((float)Math.Abs(Math.Asin(Sin)))) % 180;

                    if (this.acceleration.componentX < 0 && this.acceleration.componentZ < 0)
                        ;
                    else if (this.acceleration.componentX > 0 && this.acceleration.componentZ < 0)
                        Angle *= -1;
                    else if (this.acceleration.componentX < 0 && this.acceleration.componentZ > 0)
                        Angle = 180 - Angle;
                    else if (this.acceleration.componentX > 0 && this.acceleration.componentZ > 0)
                        Angle = -180 + Angle;
                }
                if (Angle == 0 && Value == 0)
                {
                }
                else if ( Math.Abs(Angle - this.Angle) > 15 && brakeEnabled)
                    return false;
                return true;
            }
        }

        private void UpdateAngle(float componentX, float componentZ)
        {
            if (componentX != 0 || componentZ != 0)
            {
                float r = (float)Math.Sqrt(Math.Pow(componentX, 2) + Math.Pow(componentZ, 2));
                float Sin = componentZ / r;

                this.Angle = MathHelper.ToDegrees((float)Math.Abs(Math.Asin(Sin)));
                if (componentX < 0)
                    this.Angle += 2 * (90 - this.Angle);

                if (componentX > 0 && componentZ > 0)
                    this.Angle *= -1;
                else if (componentX > 0 && componentZ < 0)
                    ;
                else if (componentX < 0 && componentZ > 0)
                    this.Angle *= -1;
                else
                    ;
            }
        }

        private bool BrakeThisFrame= true;
        private float OldSpeed = 0;
        private ForceVector UpdatePullingForce(bool brakeEnabled,GameTime gameTime)
        {
            ForceVector[] wheelForce = new ForceVector[4];
            ForceVector[] rollingForce = new ForceVector[4];
            this.CarbrakeEnabled = brakeEnabled;

            float Torque = (float) (engine.torque * gear.GetRatio() * gear.differentialRatio);
            //Differential Ration isn't true here (3.14) :(
            // zaher
            this.torque = Torque;
			for (int i = 0; i < 4; i++)
                //Four-wheels poll :D
			{
                float brakeValue = 0;
                // zaher12
                if (brakeEnabled)
                {
                    brakeValue = brakeForce[i];
                }
                //edit by zaher12
                if (tires[i].ESPActive)
                    brakeValue = brakeForce[i];
				tires[i].torque =( i < 2 ? 0 : (float)Torque);
                // zaher12
                if (ESPAtivated)
                    tires[i].torque *= (this.Yaw_Rate/6.2f); 
				wheelForce[i] = new ForceVector(
                    (this.Angle + (i < 2 ? 0 : this.tires[2].angle)),   //tires[2].angle gives us steering angle
                    (float)(- brakeValue + tires[i].torque / tires[i].radius / 2.0f ),
                    true);

				float rollingValue = 0;
                if (wheelForce[i].componentX != 0 || wheelForce[i].componentZ != 0)
                {
                    rollingValue = (float)(rollingCoefficient * MASS * 9.8f);
                }
				rollingForce[i] = new ForceVector((180 + this.Angle + (i < 2 ? 0 : tires[2].angle)), rollingValue / 4, true);	   //9.8 is gravity power

                this.OldSpeed = (float)this.velocity.Value;
			}

			ForceVector[] wheels = new ForceVector[4] { ForceVector.Sum(wheelForce[0], rollingForce[0]), ForceVector.Sum(wheelForce[1], rollingForce[1]), ForceVector.Sum(wheelForce[2], rollingForce[2]), ForceVector.Sum(wheelForce[3], rollingForce[3]) };
            return ForceVector.Sum(ForceVector.Sum(wheels[1], wheels[0]), ForceVector.Sum(wheels[2], wheels[3]));
        }

        private ForceVector UpdateDragForce(bool brakeEnabled)
        {
            float TotalTiresForce = 0;
            //if (brakeEnabled)
            //    for (int i = 0;i <4 ;i++)
            //        TotalTiresForce += carSystems.Brakes.Tire2GroundForce(i);

            double velocity = this.velocity.Value;
            double dragValue = (0.5 * DragCoeff * AirDensity * FrontArea * Math.Pow(velocity, 2))+TotalTiresForce;		//0.4257
            return new ForceVector((180 + this.Angle), (float)dragValue, true);
        }

        private void UpdateCarPosition(GameTime gameTime)
        {
            if (engine.rpm > 0)
            {
                this.Acceleration.Set
                    ((float)(OldGeneral.componentX / MASS)
                    , (float)(OldGeneral.componentZ / MASS));
                double duration = (double)(gameTime.ElapsedGameTime.Milliseconds) / 1000.0d;
                this.velocity.Set(
                    (float)(this.acceleration.componentX * duration + this.velocity.componentX),
                    (float)(this.acceleration.componentZ * duration + this.velocity.componentZ)
                    );

                this.Translation = Matrix.CreateTranslation
                    (
                        (float)((this.Translation.Translation.X) + this.velocity.componentX * duration * 35.0f)
                        , this.Translation.Translation.Y
                        , (float)((this.Translation.Translation.Z + this.velocity.componentZ * duration * 35.0f))
                    );
            }
        }

        private void UpdateHandleSteering()
        {
            if (!steeredThisFrame)
            {
                if (tires[2].angle != 0)
                {
                    if (tires[2].angle > 0)
                    {
                        tires[2].Steer(true);
                        tires[3].SetAngle(tires[2].angle);
                    }
                    else
                    {
                        tires[2].Steer(false);
                        tires[3].SetAngle(tires[2].angle);
                    }
                }
            }
            else
            {
                steeredThisFrame = false;
            }
        }

        private void UpdateTireLongAcc(GameTime gameTime)
        {
            for (int i = 0; i < 4; i++)
                this.tires[i].UpdateLongAcceleration(gameTime, KgTokNConverter(MASS) , m_f, m_r, this.Velocity.Value, i > 1);
        }           

        private void ManipulateCarYawAngle()
        {
            Yaw_Rate /= Duration;
            if (Yaw_Rate > 0)
            {
                if (delta_f > 0)
                {
                    this.Angle += MathHelper.ToDegrees(Yaw_Rate);
                    Velocity.ResetAngle(this.Angle);
                }
                else
                {
                    this.Angle -= MathHelper.ToDegrees(Yaw_Rate);
                    Velocity.ResetAngle(this.Angle);
                }
            }
        }

        float[] OldAngle = new float[4];

        private void UpdateWheels(GameTime gameTime)
        {
            for (int i = 0; i < 4; i++)
            {
                float Angle = (float)(tires[i].angularVelocity) * (float)(gameTime.ElapsedGameTime.TotalMilliseconds / 50) + OldAngle[i];
                tires[i].WheelRotation = MathHelper.ToRadians( Angle );
                OldAngle[i] = Angle;
            }


            if ((Keyboard.GetState().IsKeyDown(Keys.A)|| leftJoyStick) && Tire.SteerRotation < 0.8)
                Tire.SteerRotation += 0.1f;
            if ((Keyboard.GetState().IsKeyDown(Keys.D) || rightJoyStick ) && Tire.SteerRotation > -0.8)
                Tire.SteerRotation -= 0.1f;
        }
        public static bool rightJoyStick = false;
        public static bool leftJoyStick = false;
        private void UpdateRPMandSpeed(int engineRPM, double velocity)
        {
            double wheelRPM = engineRPM * this.gear.GetRPMRatio() * differentialRatio;	// 0.7 is 

            double wheelRPS = wheelRPM / 60.0;

            double velocityShouldBe = wheelRPS * tires[0].surface;

            if (velocityShouldBe < velocity)
            {
                this.Velocity.Set(this.Angle, (float)(velocityShouldBe + velocity) / 2.0f, true);

                wheelRPS = this.Velocity.Value / tires[0].surface;
                wheelRPM = 60.00 * wheelRPS;
                int engineRPMShouldBe = (int)(wheelRPM / this.gear.GetRPMRatio() / 0.7);

                //if (this.velocity.Value < velocity)
                this.engine.RPM = engineRPMShouldBe;
            }
        }

        public void Update(GameTime gameTime, bool brakeEnabled)
        {
            this.brakeEnabled = brakeEnabled;
            // Update ESP-related Variables 
            Duration = gameTime.ElapsedGameTime.Milliseconds;
            Calculate_m_f();
            Calculate_m_r();
            Calculate_Radius();
            Calculate_delta_f();
            // zaher12
            if (brakeEnabled)
                updateBrakeForce(gameTime);
            // Update Car-related Variables 
            if (!(brakeEnabled && this.velocity.Value < 0.3))
                OldGeneral = ForceVector.Sum(UpdatePullingForce(brakeEnabled, gameTime), UpdateDragForce(brakeEnabled));
            else OldGeneral = new ForceVector();
            UpdateCarPosition(gameTime);
            Calculate_AlphaAngles();    // ESP-related Variables 
            UpdateAngle();              // Car-related Variables 
            //Calculate_beta();           // ESP-related Variables
            UpdateHandleSteering();
            UpdateTireLongAcc(gameTime);

            // Update ESP-related Variables             
            Calculate_Yaw_Rate();
            Calculate_LateralAcceleration();
            Calculate_K_us();
            
            // Manipulate Car 
            ManipulateCarYawAngle();
            
            // ESP-System Update
            carSystems.ESP.UpdateESPSystem(gameTime);


            if (Game1.GPSRoadChosen == true)
            {
                // GPS-System Update
                carSystems.GPS.UpdateGPSSystem(gameTime);
            }

            // Fill MyForm With Car Data           

            // Update Sounds
            // Sound.Update();
            UpdateWheels(gameTime);


            //zaher
            UpdateAngularVelocity(gameTime, brakeEnabled);
            //for (int i = 0; i < 4; i++)
            //    tires[i].UpdateAngularSpeed(this.velocity.Value);

			UpdateRPMandSpeed(this.engine.rpm, this.velocity.Value);
        }

        # endregion
		



        # region Car-data Output
        public void UpdateGUI(ref ESPForm MyForm)
        {
            // Filling From with ESP data
            //MyForm.TopMost = true;
            //MyForm.Focus();
            if (delta_f == 0)
            {
                MyForm.F_xfT.Text = FormatingNumber(0, " kN");
                MyForm.F_xrT.Text = FormatingNumber(0, " kN");
                // Angles in Radians
                MyForm.delta_fT.Text = "0.0";
                MyForm.alpha_fT.Text = "0.0";
                MyForm.alpha_rT.Text = "0.0";
                MyForm.TireAngleT.Text = "0.0";
                MyForm.betaT.Text = "0.0";
                MyForm.YawRateT.Text = "0.0";
                // Angles in Degrees
                MyForm.delta_fTT.Text = "0.0";
                MyForm.alpha_fTT.Text = "0.0";
                MyForm.alpha_rTT.Text = "0.0";
                MyForm.TireAngleTT.Text = "0.0";
                MyForm.betaTT.Text = "0.0";
                MyForm.YawRateTT.Text = "0.0";
            }
            else
            {
                MyForm.F_xfT.Text = FormatingNumber(F_xf, " kN");
                MyForm.F_xrT.Text = FormatingNumber(F_xr, " kN");
                // Angles in Radians
                MyForm.delta_fT.Text = FormatingNumber(delta_f, " rad");
                MyForm.alpha_fT.Text = FormatingNumber(alpha_f, " rad");
                MyForm.alpha_rT.Text = FormatingNumber(alpha_r, " rad");
                MyForm.TireAngleT.Text = FormatingNumber(tireAngle, " rad");
                MyForm.betaT.Text = FormatingNumber(beta, " rad");
                MyForm.YawRateT.Text = FormatingNumber(Yaw_Rate, " rad");
                // Angles in Degrees
                MyForm.delta_fTT.Text = FormatingNumber((float)MathHelper.ToDegrees(delta_f), " deg");
                MyForm.alpha_fTT.Text = FormatingNumber((float)MathHelper.ToDegrees(alpha_f), " deg");
                MyForm.alpha_rTT.Text = FormatingNumber((float)MathHelper.ToDegrees(alpha_r), " deg");
                MyForm.TireAngleTT.Text = FormatingNumber((float)MathHelper.ToDegrees(tireAngle), " deg");
                MyForm.betaTT.Text = FormatingNumber((float)MathHelper.ToDegrees(beta), " deg");
                MyForm.YawRateTT.Text = FormatingNumber((float)MathHelper.ToDegrees(Yaw_Rate), " deg");
            }
            // Velocity
            String.Format("{0:0.00}", 123.4567);
            MyForm.VelocityT.Text = FormatingNumber((float)Velocity.Value, " m/s");
            MyForm.VelocityTT.Text = FormatingNumber((float)Velocity.Value * 3.6f, " Km/h");
            MyForm.VelocityXT.Text = FormatingNumber((float)Math.Abs(Velocity.componentX), " m/s");
            MyForm.VelocityXTT.Text = FormatingNumber((float)Math.Abs(Velocity.componentX * 3.6f), " Km/h");
            MyForm.VelocityZT.Text = FormatingNumber((float)Math.Abs(Velocity.componentZ), " m/s");
            MyForm.VelocityZTT.Text = FormatingNumber((float)Math.Abs(Velocity.componentZ * 3.6f), " Km/h");
            // Remaining data
            MyForm.K_usT.Text = FormatingNumber(K_us, " rad");
            MyForm.K_usTT.Text = FormatingNumber(MathHelper.ToDegrees(K_us), " deg");
            MyForm.ESPStatusT.Text = carSystems.ESP.ESPStatus == true ? "ON" : "OFF";
            MyForm.CarStatusT.Text = carSystems.ESP.CarStatus.ToString();
            MyForm.TurnSideT.Text = carSystems.ESP.TurnSide.ToString();
            MyForm.RadiusT.Text = double.IsInfinity(Radius) ? "No Turn" : FormatingNumber(Radius, " m");
            MyForm.L_fT.Text = L_f.ToString() + " m";
            MyForm.L_rT.Text = L_r.ToString() + " m";
            MyForm.m_fT.Text = m_f.ToString() + " kN";
            MyForm.m_rT.Text = m_r.ToString() + " kN";
            MyForm.BrakingTiresT.Text = carSystems.ESP.DefineBrakingTire();
            MyForm.c_afT.Text = c_af.ToString() + " kN/rad";
            MyForm.c_arT.Text = c_ar.ToString() + " kN/rad";
            MyForm.LateralAccT.Text = FormatingNumber(lateralAcceleration, " m/s.s");
            MyForm.AccT.Text = (AccelerationBoolean ? "+" : "-" ) + " " + FormatingNumber((float)acceleration.Value, " m/s.s");
            
            //MyForm.PositionT.Text = this.Position.ToString();
            //MyForm.GPSPointT.Text = carSystems.GPS.GPSPoint.ToString();
            //MyForm.CLosestTrackPointT.Text = carSystems.GPS.ClosestTrackPoint.ToString();
            //MyForm.OffRoadModeT.Text = carSystems.GPS.GPSPointApproached.ToString();
        }
        private String FormatingNumber(float numberToFormat, String unit)
        {
            return String.Format("{0:0.0000}", numberToFormat) + unit;
        }
        #endregion
        
        #region Load
        public void Load(ContentManager content)
        {
            // Load the tank model from the ContentManager.
            model = content.Load<Model>("Models\\CarModel\\MyBMWBody1");

            // Look up shortcut references to the bones we are going to animate.
            leftFrontWheelBone = model.Bones["Group01"];
            leftBackWheelBone = model.Bones["Group02"];
            rightFrontWheelBone = model.Bones["Group03"];
            rightBackWheelBone = model.Bones["Group04"];

            leashBone = model.Bones["Tube03"];
            // Store the original transform matrix for each animating bone.
            leftBackWheelTransform = leftBackWheelBone.Transform;
            rightBackWheelTransform = rightBackWheelBone.Transform;
            leftFrontWheelTransform = leftFrontWheelBone.Transform;
            rightFrontWheelTransform = rightFrontWheelBone.Transform;

            leashTransform = leashBone.Transform;
            // Allocate the transform matrix array.
            carTransforms = new Matrix[this.model.Bones.Count];
        }
        #endregion

        #region Fields
        ModelBone leftBackWheelBone;
        ModelBone rightBackWheelBone;
        ModelBone leftFrontWheelBone;
        ModelBone rightFrontWheelBone;
        ModelBone leashBone;




        // Store the original transform matrix for each animating bone.
        Matrix leftBackWheelTransform;
        Matrix rightBackWheelTransform;
        Matrix leftFrontWheelTransform;
        Matrix rightFrontWheelTransform;
        Matrix leashTransform;



        // Array holding all the bone transform matrices for the entire model.
        // We could just allocate this locally inside the Draw method, but it
        // is more efficient to reuse a single array, as this avoids creating
        // unnecessary garbage.
        Matrix[] boneTransforms;




        #endregion

        #region Draw functions

        public void DrawCar(GraphicsDeviceManager graphics, BasicCamera camera)
        {
            // Calculate matrices based on the current animation position.
            Matrix leftBackWheelRotation = Matrix.CreateRotationX(-tires[1].WheelRotation);
            Matrix rightBackWheelRotation = Matrix.CreateRotationX(tires[3].WheelRotation);
            Matrix leftFrontWheelRotation = Matrix.CreateRotationX(-tires[0].WheelRotation)
                * Matrix.CreateRotationZ(-Tire.SteerRotation);
            Matrix rightFrontWheelRotation = Matrix.CreateRotationX(tires[2].WheelRotation)
                * Matrix.CreateRotationZ(Tire.SteerRotation);
            Matrix leashRotation = Matrix.CreateRotationZ(-Tire.SteerRotation * 2);

            // Apply matrices to the relevant bones.
            leftBackWheelBone.Transform = leftBackWheelRotation * leftBackWheelTransform;
            rightBackWheelBone.Transform = rightBackWheelRotation * rightBackWheelTransform;
            leftFrontWheelBone.Transform = leftFrontWheelRotation * leftFrontWheelTransform;
            rightFrontWheelBone.Transform = rightFrontWheelRotation * rightFrontWheelTransform;

            leashBone.Transform = leashRotation * leashTransform;
            this.model.CopyAbsoluteBoneTransformsTo(carTransforms);
            Matrix World = Matrix.CreateScale(0.05f)
                * Matrix.CreateRotationY(MathHelper.ToRadians(this.Angle))
                * translation;

            foreach (ModelMesh mesh in this.model.Meshes)
            {
                if (mesh.Name.Equals("Line04") || mesh.Name.Equals("Line13") || mesh.Name.Equals("Line03"))
                {
                    graphics.GraphicsDevice.RenderState.AlphaBlendEnable = true;
                    graphics.GraphicsDevice.RenderState.SourceBlend = Blend.SourceAlpha;
                    graphics.GraphicsDevice.RenderState.DestinationBlend = Blend.InverseSourceAlpha;
                }
                else
                {
                    graphics.GraphicsDevice.RenderState.AlphaBlendEnable = false;
                }
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.EnableDefaultLighting();
                    effect.World = carTransforms[mesh.ParentBone.Index] * World;
                    effect.View = camera.viewMatrix;
                    effect.Projection = camera.projectionMatrix;
                }
                mesh.Draw();
            }
            graphics.GraphicsDevice.RenderState.AlphaBlendEnable = false;      
        }

        public void Draw(GraphicsDeviceManager graphics, BasicCamera camera)
        {
            DrawCar(graphics,camera);            
        }
        # endregion

        #region Sound
        //float speed;
        //float speedChange;
        //float moveFactor;
        float oldSpeed = 0;
        //////////////Sound Handle
        void UpdateSound(GameTime gameTime)
        {
            float moveFactor = (float)gameTime.ElapsedRealTime.TotalMilliseconds / 1000;
            float speed = (float)this.Velocity.Value / 5;

            float speedChange = speed - oldSpeed;



            if (speed > oldSpeed + 100 * moveFactor)
                speed = (oldSpeed + 100 * moveFactor);
            if (speed < oldSpeed - 100 * moveFactor)
                speed = (oldSpeed - 100 * moveFactor);

            if (speed > 0.5f && speed < 7.5f && speedChange > 5.5f * moveFactor ||
                    speed > 0.75f && speedChange < 10 * moveFactor)
            {
                Sound.Sounds brakeType = Sound.GetBreakSoundType(speed, speedChange, 0);
                 //And play sound for braking
                Sound.PlayBrakeSound(brakeType);
            }
            oldSpeed = speed;
        }

        #endregion
    }
}