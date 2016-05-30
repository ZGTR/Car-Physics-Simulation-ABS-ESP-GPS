using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace CarDynamics
{
	class XMLImporter
	{
		public static List<Vector3> Import(String path, ContentManager Content)
		{
			List<Vector3> result = new List<Vector3>();
			result = Content.Load<List<Vector3>>(path);

			return result;
		}
	}
}
