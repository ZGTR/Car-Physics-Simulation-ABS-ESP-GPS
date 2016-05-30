using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace CameraViewer
{
    class FreeCamera : BasicCamera
    {

        float leftrightRot;
        float updownRot;

        MouseState lastMouseState;

        void Initialize()
        {
            leftrightRot = 0.0f;
            updownRot = 0.0f;
            position = new Vector3(0.0f);
            UpdateViewMatrix();
            lastMouseState = Mouse.GetState();            
        }

        public void Update(GameTime gameTime)
        {
            float rotationSpeed = 0.005f;
            
            MouseState currentMouseState = Mouse.GetState();
            if (currentMouseState.MiddleButton == ButtonState.Pressed)
            {
                if (currentMouseState != lastMouseState)
                {
                    float xDifference = currentMouseState.X - lastMouseState.X;
                    float yDifference = currentMouseState.Y - lastMouseState.Y;
                    leftrightRot -= rotationSpeed * xDifference;
                    updownRot -= rotationSpeed * yDifference;
                    //Mouse.SetPosition(Window.ClientBounds.Width / 2, Window.ClientBounds.Height / 2);
                    //originalMouseState = Mouse.GetState();
                    UpdateViewMatrix();
                }
            }

            
            KeyboardState keyState = Keyboard.GetState();
            //if (currentMouseState.ScrollWheelValue > lastMouseState.ScrollWheelValue)//(keyState.IsKeyDown(Keys.W))
            //    AddToCameraPosition(new Vector3(0, 0, -30));
            //if (currentMouseState.ScrollWheelValue < lastMouseState.ScrollWheelValue)//(keyState.IsKeyDown(Keys.S))
            //    AddToCameraPosition(new Vector3(0, 0, 30));
            if (keyState.IsKeyDown(Keys.D))
                AddToCameraPosition(new Vector3(20, 0, 0));
            if (keyState.IsKeyDown(Keys.A))
                AddToCameraPosition(new Vector3(-20, 0, 0));
            if (keyState.IsKeyDown(Keys.W))
                AddToCameraPosition(new Vector3(0, 0, -20));
            if (keyState.IsKeyDown(Keys.S))
                AddToCameraPosition(new Vector3(0, 0, 20));

            lastMouseState = currentMouseState;
        }

        private void AddToCameraPosition(Vector3 vectorToAdd)
        {
            float moveSpeed = 0.5f;
            Matrix cameraRotation = Matrix.CreateRotationX(updownRot) * Matrix.CreateRotationY(leftrightRot);
            Vector3 rotatedVector = Vector3.Transform(vectorToAdd, cameraRotation);
            position += moveSpeed * rotatedVector;
            
            
            UpdateViewMatrix();    
        }

        private void UpdateViewMatrix()
        {

            if (position.Y < 10)
                position.Y = 10;
            if (position.Y > 250)
                position.Y = 250;
            Matrix cameraRotation = Matrix.CreateRotationX(updownRot) * Matrix.CreateRotationY(leftrightRot);

            Vector3 cameraOriginalTarget = new Vector3(0, 0, -1);
            Vector3 cameraOriginalUpVector = new Vector3(0, 1, 0);

            Vector3 cameraRotatedTarget = Vector3.Transform(cameraOriginalTarget, cameraRotation);
            Vector3 cameraFinalTarget = position + cameraRotatedTarget;

            Vector3 cameraRotatedUpVector = Vector3.Transform(cameraOriginalUpVector, cameraRotation);
            Vector3 cameraFinalUpVector = position + cameraRotatedUpVector;

            //if (position.Y > 50)
           
            target = cameraFinalTarget;
            up = cameraRotatedUpVector;
            needUpdateView = true;
            //viewMatrix = Matrix.CreateLookAt(position, cameraFinalTarget, cameraRotatedUpVector);
        }
    }
}
