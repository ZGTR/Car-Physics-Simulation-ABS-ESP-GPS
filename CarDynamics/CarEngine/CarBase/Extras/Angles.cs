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
	public class Angles
	{
		public static void ResetAngleToDefaultForm(double angle, out double outAngle)
		{
			if (angle >= 0 && angle <= MathHelper.Pi)
				;
			else if (angle <= 0 && angle <= -MathHelper.Pi)
				;
			else
			{
				angle %= (float)MathHelper.Pi;
			}
			outAngle = angle;
		}
	}
}