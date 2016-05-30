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

namespace CameraViewer
{
    public abstract class BasicCamera
    {
        // Position and target
        public Vector3 position;
        public Vector3 target;
        protected Vector3 up;
        //Constructor
        public BasicCamera()
        {  
        }
        

        // Chase parameters
        public float desiredChaseDistance;
        public float minChaseDistance;
        public float maxChaseDistance;
        public float chaseSpeed;

        public void SetChaseParameters(float chaseSpeed,float desiredChaseDistance,
            float minChaseDistance, float maxChaseDistance)
        {
            this.chaseSpeed = chaseSpeed;
            this.desiredChaseDistance = desiredChaseDistance;
            this.minChaseDistance = minChaseDistance;
            this.maxChaseDistance = maxChaseDistance;
        }

        public void Synchronous(BasicCamera cam)
        {           
            //this.maxChaseDistance = cam.maxChaseDistance;
            //this.minChaseDistance = cam.minChaseDistance;
            //this.desiredChaseDistance = cam.desiredChaseDistance;
            
            this.viewMatrix = cam.viewMatrix;
            this.position = cam.position;
            this.target = cam.target;            
        }
        protected void SetView(Vector3 position, Vector3 target, Vector3 up)
        {
            this.position = position;
            this.target = target;
            this.up = up;
            SetLookAt(position, target, up);
        }


        // Set the camera perspective projection
        public void SetProjection(int windowWidth, int windowHeight)
        {
            // parameters are field of view, width/height, near clip, far clip
            projectionMatrix = Matrix.CreatePerspectiveFieldOfView(MathHelper.Pi / 4.0f,
                               (float)windowWidth / (float)windowHeight, 0.005f, 50000.0f);
        }



        protected bool needUpdateView;
        protected Matrix ViewMatrix;
        // Get the camera view matrix
        public Matrix viewMatrix
        {
            get
            {
                if (needUpdateView) UpdateView();
                return ViewMatrix;
            }
            set
            {
                ViewMatrix = value;
            }
        }

        public Matrix projectionMatrix;

        // Orientation vectors
        public Vector3 headingVec = new Vector3(0);
        public Vector3 strafeVec = new Vector3(0);
        //protected Vector3 upVec;

        protected float desiredCameraY = 35;
        protected void SetLookAt(Vector3 cameraPos, Vector3 cameraTarget, Vector3 cameraUp)
        {            
            this.target = cameraTarget;
            this.position = cameraPos;
            this.position.Y = target.Y;
            this.up = cameraUp;
            //Calculate the camera axes (heading, upVector, and strafeVector)
            headingVec = cameraTarget - cameraPos;
            headingVec.Normalize();

            strafeVec = Vector3.Cross(headingVec, up);
            strafeVec.Normalize();

            this.position.Y = desiredCameraY;            
            needUpdateView = true;
        }


        // Update the camera view
        protected virtual void UpdateView()
        {
            viewMatrix = Matrix.CreateLookAt(position, target , up);
            needUpdateView = false;
        }

        public void Update(GraphicsDeviceManager graphics, bool brakeEnabled)
        {           
        }

        

        protected float zoomSpeed = 2;
        public void ZoomOut()
        {
            if(desiredChaseDistance < 400)
            {
                this.desiredChaseDistance += zoomSpeed;
                this.minChaseDistance += zoomSpeed;
                this.maxChaseDistance += zoomSpeed;
            }
        }
        public void ZoomIn()
        {
            if (desiredChaseDistance > 10)
            {
                this.desiredChaseDistance -= zoomSpeed;
                this.minChaseDistance -= zoomSpeed;
                this.maxChaseDistance -= zoomSpeed;
            }
        }


    }
}