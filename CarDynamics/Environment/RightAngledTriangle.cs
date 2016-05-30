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
    public class RightAngledTriangle
    {
        private Vector3 TargetPoint;
        public Vector3 targetPoint 
        {
            set
            {
            }
            get
            {
                return TargetPoint;
            }
        }

        private float Opposite;
        public float opposite
        {
            set
            {
                Opposite = Math.Abs(TargetPoint.X - 0);
            }
            get
            {
                return Opposite;
            }
        }

        private float AdjacentSide;
        public float adjacentSide
        {
            set
            {
                AdjacentSide = Math.Abs(TargetPoint.Y - 0);
            }
            get
            {
                return AdjacentSide;
            }
        }

        private float Hypotenuse;
        public float hypotenuse
        {
            set
            {
                Hypotenuse = (float)Math.Sqrt((float)Math.Pow(Opposite,2) + (float)Math.Pow(AdjacentSide,2));
            }
            get
            {
                return Hypotenuse;
            }
        }

        private int AngleToReturn;
        public int angleToReturn
        {
            set 
            {
                AngleToReturn  = (int)(Opposite / Hypotenuse);
            }
            get
            {
                return AngleToReturn ;
            }
        }

        public RightAngledTriangle(Vector3 StartPoint , Vector3 TargetPoint)
        {
            this.targetPoint = TargetPoint;       
        }        
    }
}
