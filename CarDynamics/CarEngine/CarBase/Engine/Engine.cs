using System;


namespace CarDynamics
{
	public class Engine
	{
		public const int RPM_MAX = 3200;
		private int RPM_MIN = 0;
		private const int RPM_IncreaseValue = 3;
		private const int RPM_DecreaseValue = 3;
		private double Torque;									  // Engine Torque
		public int RPM;											// Engine's Rounds Per Minute
		private double Power;									   // Engine Power
		private double AngularVelocity;							 // Angular Velocity of the engine

		public void UpdateRPM(double OldGearRatio, double GearOnRatio)
		{
			this.RPM = (int)(this.RPM * GearOnRatio / OldGearRatio);
		}

		public double torque
		{
			get
			{
				return Torque;
			}
		}

		public int rpm
		{
			get
			{
				return RPM;
			}
		}

		public void IncreaseRPM(ref bool ShiftUp, int GearOn)								   // Pressing 'increase speed' button
		{
			RPM += 6 / GearOn;
			if (RPM > RPM_MAX)
			{
				RPM = RPM_MAX;
				ShiftUp = true;
			}
			UpdateTorque();

			if (this.RPM > 50)
				this.RPM_MIN = 50;
		}

		public void DecreaseRPM(ref bool ShiftDown)								   // Pressing 'decrease speed' button
		{
			RPM -= RPM_DecreaseValue;
			if (RPM < RPM_MIN)
			{
				RPM = RPM_MIN;
				ShiftDown = true;
			}
			UpdateTorque();
		}
		private void UpdateTorque()
		{
			if (RPM > 0)
			{
				if (RPM < 500)
					Torque = 100;
				else if (Torque < 700)
					Torque = 150;
				else if (RPM < 1000)
					Torque = 200;
				else if (RPM < 4600)
					Torque = 0.025 * RPM + 125;
				else Torque = -0.032 * RPM + 457.2;
			}
			else Torque = 0;
			//if (RPM > 0)
			//{
			//    if (RPM < 1000)
			//        Torque = 200;
			//    else if (RPM < 4600)
			//        Torque = 0.025 * RPM + 125;
			//    else Torque = -0.032 * RPM + 457.2;
			//}
			//else Torque = 0;
		}

		public double power
		{
			get
			{
				return Power;
			}
		}


		public double angularVelocity
		{
			get
			{
				return AngularVelocity;
			}
		}

		public Engine()
		{
			this.AngularVelocity = 0;
			this.Power = 0;
			this.RPM = 0;
			this.Torque = 0;
		}

	}
}
