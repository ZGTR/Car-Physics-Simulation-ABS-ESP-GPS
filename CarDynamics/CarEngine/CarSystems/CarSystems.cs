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
	public class CarSystems
	{
        public ESPSystem ESP;
        public ABSSystem ABS;
        public GPSSystem GPS; 
        public BrakeSystem Brakes;

        public CarSystems(Car myCar , bool ESP_ON , bool ABS_ON , Game1 game1 , Road road) 
        { 
            Brakes = new BrakeSystem(myCar);
            if (ESP_ON == true)
            {
                ESP = new ESPSystem(myCar);
            }
            if (ABS_ON == true ) 
            {
                ABS = new ABSSystem(myCar); 
            }
        }

        public CarSystems(Car myCar, bool ESP_ON, bool ABS_ON, Game1 game1, GPSRoad gpsRoad)
        {
            GPS = new GPSSystem(game1, myCar, gpsRoad);
            Brakes = new BrakeSystem(myCar);
            if (ESP_ON == true)
            {
                ESP = new ESPSystem(myCar);
            }
            if (ABS_ON == true)
            {
                ABS = new ABSSystem(myCar);
            }
        }
	}
}
