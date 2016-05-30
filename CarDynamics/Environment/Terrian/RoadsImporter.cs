using System;
using System.Collections.Generic;
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
using CameraViewer;

namespace CarDynamics.Terrian
{
    class RoadsImporter
    {
        public static List<Vector3> Road1()
        {
            List<Vector3> basePoints = new List<Vector3>();

            basePoints.Add(new Vector3(-100, 0, -80));
            basePoints.Add(new Vector3(-50, 0, -80));
            basePoints.Add(new Vector3(-40, 0, -50));


            basePoints.Add(new Vector3(40, 0, -50));
            basePoints.Add(new Vector3(50, 0, -80));
            basePoints.Add(new Vector3(100, 0, -80));


            basePoints.Add(new Vector3(100, 0, 80));
            basePoints.Add(new Vector3(50, 0, 80));
            basePoints.Add(new Vector3(40, 0, 50));


            basePoints.Add(new Vector3(-40, 0, 50));
            basePoints.Add(new Vector3(-50, 0, 80));
            basePoints.Add(new Vector3(-100, 0, 80));

            return basePoints;
        }

        public static List<Vector3> Road2()
        {
            List<Vector3> basePoints = new List<Vector3>();

            basePoints.Add(new Vector3(-40, 0, -50));
            basePoints.Add(new Vector3(40, 0, -50));

            basePoints.Add(new Vector3(40, 0, 50));
            basePoints.Add(new Vector3(-40, 0, 50));

            return basePoints;
        }



        public static List<Vector3> Road3()
        {
            List<Vector3> basePoints = new List<Vector3>();

            // 1st-2nd Point
            basePoints.Add((new Vector3(0, 0, 0)));
            basePoints.Add((new Vector3(0, 0, -10)));
            basePoints.Add((new Vector3(0, 0, -20)));
            basePoints.Add((new Vector3(0, 0, -30)));
            basePoints.Add((new Vector3(0, 0, -40)));
            basePoints.Add((new Vector3(0, 0, -50)));
            basePoints.Add((new Vector3(0, 0, -60)));

            // 3rd Point
            basePoints.Add((new Vector3(-1, 0, -60)));
            basePoints.Add((new Vector3(-20, 0, -60)));
            basePoints.Add((new Vector3(-30, 0, -60)));
            basePoints.Add((new Vector3(-40, 0, -60)));
            basePoints.Add((new Vector3(-50, 0, -60)));

            // 4th Point
            basePoints.Add((new Vector3(-50, 0, -59)));
            basePoints.Add((new Vector3(-50, 0, -40)));
            basePoints.Add((new Vector3(-50, 0, -30)));
            basePoints.Add((new Vector3(-50, 0, -20)));
            basePoints.Add((new Vector3(-50, 0, -10)));
            basePoints.Add((new Vector3(-50, 0, 0)));

            // Back to 1st Node
            basePoints.Add((new Vector3(-49, 0, 0)));
            basePoints.Add((new Vector3(-40, 0, 0)));
            basePoints.Add((new Vector3(-30, 0, 0)));
            basePoints.Add((new Vector3(-20, 0, 0)));
            basePoints.Add((new Vector3(-10, 0, 0)));

            return basePoints;
        }

        public static List<Vector3> Road4()
        {

            List<Vector3> basePoints = new List<Vector3>();

            // 1st - 2nd Point
            basePoints.Add((new Vector3(-50, 0, 0)));
            basePoints.Add((new Vector3(-50, 0, -10)));
            basePoints.Add((new Vector3(-50, 0, -20)));
            basePoints.Add((new Vector3(-50, 0, -30)));
            basePoints.Add((new Vector3(-50, 0, -40)));
            basePoints.Add((new Vector3(-50, 0, -50)));
            basePoints.Add((new Vector3(-50, 0, -60)));



            // 3rd Point         
            basePoints.Add((new Vector3(-51, 0, -60)));
            basePoints.Add((new Vector3(-60, 0, -60)));
            basePoints.Add((new Vector3(-65, 0, -60)));
            basePoints.Add((new Vector3(-70, 0, -60)));
            basePoints.Add((new Vector3(-75, 0, -60)));
            basePoints.Add((new Vector3(-80, 0, -60)));

            // 4th Point
            basePoints.Add((new Vector3(-80, 0, -59)));
            basePoints.Add((new Vector3(-80, 0, -50)));
            basePoints.Add((new Vector3(-80, 0, -40)));
            basePoints.Add((new Vector3(-80, 0, -30)));
            basePoints.Add((new Vector3(-80, 0, -20)));
            basePoints.Add((new Vector3(-80, 0, -10)));
            basePoints.Add((new Vector3(-80, 0, 0)));

            // Back to 1st Node
            basePoints.Add((new Vector3(-79, 0, 0)));
            basePoints.Add((new Vector3(-70, 0, 0)));
            basePoints.Add((new Vector3(-60, 0, 0)));
            basePoints.Add((new Vector3(-50, 0, 0)));
            basePoints.Add((new Vector3(-40, 0, 0)));
            basePoints.Add((new Vector3(-30, 0, 0)));
            basePoints.Add((new Vector3(-20, 0, 0)));
            basePoints.Add((new Vector3(-10, 0, 0)));
            basePoints.Add((new Vector3(-1, 0, 0)));

            return basePoints;
        }
    }
}
