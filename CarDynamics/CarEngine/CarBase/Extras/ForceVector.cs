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

namespace CarDynamics
{
	public class ForceVector
	{
		float ComponentX;   //Has the direction
		float ComponentZ;   //Of the force

		public float componentX
		{
			get
			{
				return ComponentX;
			}
		}
		public float componentZ
		{
			get
			{
				return ComponentZ;
			}
		}


		public ForceVector(float ComponentX, float ComponentZ)
		{
			this.ComponentX = ComponentX;
			this.ComponentZ = ComponentZ;
		}

		public ForceVector(float AngleInDegrees, float Value, bool Angle)
		{
			Set(AngleInDegrees, Value, Angle);
		}


		public ForceVector()
		{
			this.ComponentX = 0;
			this.ComponentZ = 0;
		}

		public void Set(float AngleInDegrees, float Value, bool Angle)
		{
			float AngleInRadians = MathHelper.ToRadians(AngleInDegrees);

			if (AngleInDegrees == 90)
				this.ComponentX = -1 * Value;
			else if (AngleInDegrees == 180)
				this.ComponentX = 0;
			else if (AngleInDegrees == 270)
				this.ComponentX = Value;
			else this.ComponentX = -1 * Value * (float)Math.Sin(AngleInRadians);

			if (AngleInDegrees == 90)
				this.ComponentZ = 0;
			else if (AngleInDegrees == 180)
				this.ComponentZ = Value;
			else if (AngleInDegrees == 270)
				this.ComponentZ = 0;
			else this.ComponentZ = -1 * Value * (float)Math.Cos(AngleInRadians);
		}
		public void Set(float X, float Z)
		{
			this.ComponentX = X;
			this.ComponentZ = Z;
		}

		public void ResetAngle(float AngleInDegrees)
		{
            float Value = (float)this.Value;
            float AngleInRadians = MathHelper.ToRadians(AngleInDegrees);

            if (AngleInDegrees == 90)
                this.ComponentX = -1 * Value;
            else if (AngleInDegrees == 180)
                this.ComponentX = 0;
            else if (AngleInDegrees == 270)
                this.ComponentX = Value;
            else this.ComponentX = -1 * Value * (float)Math.Sin(AngleInRadians);

            if (AngleInDegrees == 90)
                this.ComponentZ = 0;
            else if (AngleInDegrees == 180)
                this.ComponentZ = Value;
            else if (AngleInDegrees == 270)
                this.ComponentZ = 0;
            else this.ComponentZ = -1 * Value * (float)Math.Cos(AngleInRadians);
        }
		public void AutoDecreament()
		{
			this.ComponentX /= 10;
			this.ComponentZ /= 10;
		}

		//The result's starting position is as same as the start position of force1
		public static ForceVector Sum(ForceVector force1, ForceVector force2)
		{
			return new ForceVector
				(
					force1.componentX + force2.componentX,
					force1.componentZ + force2.componentZ
				);
		}
		public static ForceVector Av(ForceVector force1, ForceVector force2)	//Average
		{
			return new ForceVector
				(
					(force1.componentX + force2.componentX / 10.0f),
					(force1.componentZ + force2.componentZ / 10.0f)
				);
		}

		public static void ResetComponents(ref double componentX, ref double componentZ, int angle)
		{
			float Value = (float)Math.Sqrt( Math.Pow(componentX, 2) + Math.Pow(componentZ, 2) );

			componentX = Math.Abs( Value * (float)Math.Cos(MathHelper.ToRadians(angle)) );
			componentZ = Math.Abs( Value * (float)Math.Sin(MathHelper.ToRadians(angle)) );

			int a = angle;
			while (a < 0)
			{
				a += 360;
			}
			a %= 360;

			if (a >= 90 && a < 180)
				componentZ *= -1;
			else if (a >= 180 && a < 270)
			{
				componentZ *= -1;
				componentX *= -1;
			}
			else if (a >= 270 && a < 360)
				componentX *= -1;

			//if (angle < 0)
			//    componentZ *= -1;
		}
		public double Value
		{
			get
			{
				return Math.Sqrt(Math.Pow(this.componentX, 2) + Math.Pow(this.componentZ, 2));
			}
		}
	}
}