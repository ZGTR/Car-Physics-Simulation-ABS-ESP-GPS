using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using FuchsGUI;
using GUI;

namespace CameraViewer
{
    class CameraManagement
    {
        // camera 
        public BasicCamera currentCamera;
        public ChaseCamera camera1 = new ChaseCamera();
        public ExtraCamera camera2 = new ExtraCamera();
        public FreeCamera  camera3  = new FreeCamera();


        public void Initialize(GameWindow Window)
        {
            camera1.SetProjection(Window.ClientBounds.Width, Window.ClientBounds.Height);
            camera2.SetProjection(Window.ClientBounds.Width, Window.ClientBounds.Height);
            camera3.SetProjection(Window.ClientBounds.Width, Window.ClientBounds.Height);
            
            camera1.SetChaseParameters(5.0f, 130, 100, 150);
            camera2.SetChaseParameters(30.0f, 100, 80, 130);
            currentCamera = camera1;
        }

        public void Update(GameTime gameTime,Vector3 carPositon,float carVelocity,GameWindow Window)
        {
            KeyboardState kS = Keyboard.GetState();

            // change camera
            //if (kS.IsKeyDown(Keys.Down))
            //{
            //    currentCamera = camera1;
            //}
            //if (kS.IsKeyDown(Keys.F))
            //{
                            
            //}
            camera2.ChoosingCameraType(ref currentCamera);

            camera1.Update(gameTime, carPositon);
            camera2.Update(gameTime, camera1, (float)carVelocity);
            if ((currentCamera).GetType() == (camera3).GetType())
                camera3.Update(gameTime);
        }



        public void SetGUI(ref CamerasControl CamControl)
        {
            CamControl.DrivingBehindTheCar.onClick += new EHandler(SetDrivingOutsideTheCar);
            CamControl.DrivingInsideTheCar.onClick += new EHandler(SetDrivingInsideTheCar);
            CamControl.DrivingInfrontOfTheCar.onClick += new EHandler(DrivingInFrontOfTheCar);
            CamControl.FreeCamera.onClick += new EHandler(SetFreeCamera);
            CamControl.ShowLeftWheel.onClick += new EHandler(ShowLeftWheel);
            CamControl.ShowRightWheel.onClick += new EHandler(ShowRightWheel);
            CamControl.ZoomIn.onClick += new EHandler(ZoomIn);
            CamControl.ZoomOut.onClick += new EHandler(ZoomOut);            
        }

        void SetDrivingInsideTheCar(Control sender)
        {
            camera1.SetDrivingInsideTheCar();
            currentCamera = camera1;            
        }
        void SetDrivingOutsideTheCar(Control sender)
        {
            camera1.SetDrivingOutsideTheCar();
            currentCamera = camera1;
        }
        void SetFreeCamera(Control sender)
        {
            camera3.Synchronous(currentCamera);
            currentCamera = camera3;    
        }
        void ShowLeftWheel(Control sender)
        {
            camera1.SetDrivingOutsideTheCar();
            camera2.Synchronous(currentCamera);
            currentCamera = camera2;
            camera2.type = 1;
        }
        void ShowRightWheel(Control sender)
        {
            camera1.SetDrivingOutsideTheCar();
            camera2.Synchronous(currentCamera);
            currentCamera = camera2;
            camera2.type = 2;
        }
        void DrivingInFrontOfTheCar(Control sender)
        {
            camera1.SetDrivingOutsideTheCar();
            camera2.Synchronous(currentCamera);
            currentCamera = camera2;
            camera2.type = 3;
        }
        void SetChaseDistance(Control sender)
        {
            DomainLeftRight dlr = (DomainLeftRight)sender;
            object obj = dlr.SelectedObject;
            if (obj != null)// && obj is int)
            {
                //int value = (int)obj;
                currentCamera.ZoomIn();
            }
        }
        void ZoomIn(Control sender)
        {
            currentCamera.ZoomIn();
            
        }
        void ZoomOut(Control sender)
        {
            currentCamera.ZoomOut();
        }
    }
}
