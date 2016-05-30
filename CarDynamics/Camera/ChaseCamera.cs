using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace CameraViewer
{
    public class ChaseCamera : BasicCamera
    {
        

        //Vector3 ChasePosition;
        //public Vector3 chasePosition
        //{
        //    get { return ChasePosition; }
        //    set { ChasePosition = value;}
        //}

        Vector3 ChaseDirection;
        public Vector3 chaseDirection
        {
            get { return ChaseDirection; }
            set { ChaseDirection = value; }
        }

       public ChaseCamera()
       {
            Vector3 position = new Vector3(0.0f, 10.0f, 50.0f);
            Vector3 target = new Vector3(0.0f, 0.0f, 0.0f);
            Vector3 up = new Vector3(0.0f, 1.0f, 0.0f);
            base.SetView(position, target, up);
            drivingOutSideTheCar = true;        
       }



       

        private void UpdateFollowPosition(float elapsedTimeSeconds, bool interpolate)
        {
            Vector3 targetPosition = target + new Vector3(0.0f, 20, 0.0f);
            Vector3 desiredCameraPosition = target - ChaseDirection * desiredChaseDistance;

            if (interpolate)
            {
                float interpolatedSpeed = MathHelper.Clamp(chaseSpeed * elapsedTimeSeconds, 0.0f, 1.0f);
                desiredCameraPosition = Vector3.Lerp(position, desiredCameraPosition, 1);
                // Clamp the min and max follow distances
                Vector3 targetVector = desiredCameraPosition - targetPosition;
                float targetLength = targetVector.Length();
                targetVector /= targetLength;
                if (targetLength < minChaseDistance)
                {
                    desiredCameraPosition = targetPosition + targetVector * minChaseDistance;
                }
                else if (targetLength > maxChaseDistance)
                {
                    desiredCameraPosition = targetPosition + targetVector * maxChaseDistance;
                }

            }            
            SetLookAt(desiredCameraPosition , targetPosition , up);
        }



        bool isFirstTimeChase = true;

        bool drivingInsideTheCar = false;
        bool drivingOutSideTheCar = false;

        public void Update(GameTime time, Vector3 chasedObjectPosition)
        {
            chaseDirection = Vector3.Normalize(chasedObjectPosition - position);
            target = chasedObjectPosition;
            float elapsedTimeSeconds = (float)time.ElapsedGameTime.TotalSeconds;
            // Update the follow position
            UpdateFollowPosition(elapsedTimeSeconds, !isFirstTimeChase);

            isFirstTimeChase = false;
            if (Keyboard.GetState().IsKeyDown(Keys.PageUp))
                ZoomIn();
            if (Keyboard.GetState().IsKeyDown(Keys.PageDown))
                ZoomOut();


            //if (Keyboard.GetState().IsKeyDown(Keys.I))
            //{
            //    SetDrivingInsideTheCar();
            //}

            //if (Keyboard.GetState().IsKeyDown(Keys.O))
            //{
            //    SetDrivingOutsideTheCar();
            //}
        }

        public void SetDrivingInsideTheCar()
        {
            desiredCameraY = 20.0f;
            SetChaseParameters(20.0f, 10, 9, 12);
            drivingInsideTheCar = true;
            drivingOutSideTheCar = false;
        }

        public void SetDrivingOutsideTheCar()
        {
            desiredCameraY = 35.0f;
            SetChaseParameters(5.0f, 130, 100, 150);
            drivingInsideTheCar = false;
            drivingOutSideTheCar = true;
        }
    }
}
