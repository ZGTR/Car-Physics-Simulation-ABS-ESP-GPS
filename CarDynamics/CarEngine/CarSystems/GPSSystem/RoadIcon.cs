using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Data;
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


namespace CarDynamics
{
    public class RoadIcon
    {
        public Vector3 position = Vector3.Zero;
        public int NodeNumber = -1;
        public List<RoadIcon> LinkedNodes ;

        public RoadIcon(Vector3 position)
        {
            this.position = position;
            LinkedNodes = new List<RoadIcon>();
        }

        public RoadIcon(RoadIcon Icon)
        {
            this.position =  Icon.position ;
            this.LinkedNodes = Icon.LinkedNodes ;
            this.NodeNumber = Icon.NodeNumber; 
        }
    }
}
