//using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;


namespace CameraViewer
{
    public class ExtraCamera : BasicCamera
    {
        public int type = 0;

        public ExtraCamera()
        {
            Vector3 position = new Vector3(100.0f, 10.0f, 0.0f);
            Vector3 target = new Vector3(0.0f, 0.0f, 0.0f);
            Vector3 up = new Vector3(0.0f, 1.0f, 0.0f);
            base.SetView(position, target, up);
        }

        private Vector3 GetdesiredCameraPosition(Vector3 shiftpositoin)
        {
            Vector3 desiredCameraPosition = new Vector3();
           
            switch (type)
            {
                case 1:
                    {
                        target += headingVec * 30 + shiftpositoin;
                        if (desiredChaseDistance > 40)
                        {
                            ZoomIn();
                        }
                        desiredCameraPosition = target - strafeVec * desiredChaseDistance;
                        break;
                    }

                case 2:
                    {
                        target += headingVec * 30 + shiftpositoin;
                        if (desiredChaseDistance > 40)
                            ZoomIn();
                        desiredCameraPosition = target + strafeVec * desiredChaseDistance;
                        break;
                    }
                case 3:
                    {
                        if (desiredChaseDistance < 100)
                        {
                            ZoomOut();
                        }
                        else
                            if (desiredChaseDistance > 100)
                            {
                                ZoomIn();
                            }
                        desiredCameraPosition = target + headingVec * desiredChaseDistance + shiftpositoin;
                        position.Y = 50;
                        break;
                    }
                case 4:
                    {
                        desiredCameraPosition = target + up * desiredChaseDistance;
                        break;
                    }
            }
            return desiredCameraPosition;
        }

        public void Update(GameTime gameTime, ChaseCamera cam ,float carVelocity)
        {
            this.target = cam.target;
            strafeVec = cam.strafeVec;
            headingVec = cam.headingVec;

            var shiftpositoin = carVelocity / 3 * headingVec;
            Vector3 desiredCameraPosition = GetdesiredCameraPosition(shiftpositoin);
            
            float interpolatedSpeed = MathHelper.Clamp(chaseSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds, 0.0f, 1.0f);
            desiredCameraPosition = Vector3.Lerp(position, desiredCameraPosition, interpolatedSpeed);
            
            Vector3 targetVector = desiredCameraPosition - target;
            float targetLength = targetVector.Length();
            targetVector /= targetLength;

            if (targetLength < minChaseDistance)
            {
                desiredCameraPosition = target + targetVector * minChaseDistance;
            }
            else if (targetLength > maxChaseDistance)
            {
                desiredCameraPosition = target + targetVector * maxChaseDistance;
            }

            position = desiredCameraPosition;
            desiredCameraY = 35;
            needUpdateView = true;
        }

        public void ChoosingCameraType(ref BasicCamera cam)
        {
            KeyboardState kS = Keyboard.GetState();
            if (kS.IsKeyDown(Keys.Left))
            {
                this.Synchronous(cam);
                cam = this;
                this.type = 1;
            }
            if (kS.IsKeyDown(Keys.Right))
            {
                this.Synchronous(cam);
                cam = this;
                this.type = 2;
            }
            if (kS.IsKeyDown(Keys.Up))
            {
                this.Synchronous(cam);
                cam = this;
                this.type = 3;
            }
            if (kS.IsKeyDown(Keys.U))
            {
                cam = this;
                this.type = 4;
            }

        }

    }
}
