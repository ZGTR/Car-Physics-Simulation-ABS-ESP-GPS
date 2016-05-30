using System;


namespace CarDynamics
{
	public class Gear
	{
		private double[] GearRatio;

		private int GearOn;
		public int gearOn
		{
			get
			{
				return GearOn;
			}
		}
        private readonly float DifferentialRatio = 3.14f; //Almost
        public float differentialRatio
        {
            get
            {
                return DifferentialRatio;
            }
        }

		public bool ShiftUp()
		{
			if (GearOn < 6)
			{
				GearOn++;
				return true;
			}
			return false;
		}
		public bool ShiftDown()
		{
			if (GearOn > 1)
			{
				GearOn--;
				return true;
			}
			return false;
		}

		public double GetRatio()
		{
			return this.GetRatio(GearOn);
		}
		public double GetRatio(int GearNumber)
		{
			if (GearNumber >= 0 && GearNumber <= 6)
				return GearRatio[GearNumber];
			else return -1;		//Error value
		}
		public double GetRPMRatio()
		{
			return this.GetRatio(6 - this.gearOn + 1);
		}

		public Gear()
		{   //Defaule Values
			GearRatio = new double[7];
			GearRatio[0] = 0.00d;   // Nuteral
			GearRatio[1] = 3.82d;
			GearRatio[2] = 2.20d;
			GearRatio[3] = 1.52d;
			GearRatio[4] = 1.22d;
			GearRatio[5] = 1.02d;
			GearRatio[6] = 0.84d;

			GearOn = 1;							 //Should be ZERO !!!!
		}
		public Gear(double G1, double G2, double G3, double G4, double G5, double G6)
		{
			GearRatio = new double[7];
			GearRatio[0] = 0.0d;   // Nuteral
			GearRatio[1] = G1;
			GearRatio[2] = G2;
			GearRatio[3] = G3;
			GearRatio[4] = G4;
			GearRatio[5] = G5;
			GearRatio[6] = G6;

			GearOn = 0;
		}
	}
}