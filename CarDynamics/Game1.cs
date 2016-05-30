using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Timers; 
using CameraViewer;
using CarDynamics.Terrian;
using TerrainNamespace;
using System;
using System.Collections.Generic;
using System.Linq;
using Sounds;
using FuchsGUI;
using GUI;


namespace CarDynamics
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        public GraphicsDeviceManager graphicsDeviceManager;
        SpriteBatch spriteBatch;
        SpriteFont font;
        Object3D[] myModelArray;
        public static bool GPSRoadChosen = true ; 

        // FPS
        int brakeTimer = 0;

        CameraManagement cameraManager = new CameraManagement();
        FormsManager formsManager = new FormsManager();

        public BasicCamera GetCurrentCamera
        {
            get
            {
                return cameraManager.currentCamera;
            }
        }
        
        Car myCar;

        // NOUR 
        private Road road = new Road();
        private GPSRoad gpsRoad = new GPSRoad();
        private SkyBox skyBox;
        private SpeedoMeter speedoMeter;

        // Brake
        bool brakeEnabled = false;

        // Terrain
        Terrain terrain;

		//Joystick
		bool waiting_for_start = true;
		public User[] localUsers;
        public IUserInterface users;
        User u1;


        public Game1()
        {
            graphicsDeviceManager = new GraphicsDeviceManager(this);            
            Content.RootDirectory = "Content";            
            gpsRoad = new GPSRoad();
            new UserControl(this);
            graphicsDeviceManager.PreparingDeviceSettings += new EventHandler<PreparingDeviceSettingsEventArgs>(graphics_PreparingDeviceSettings);
        }

        protected override void Initialize()
        {
            # region NOUR
            //Apply FullScreen Mode
            this.IsMouseVisible = false;
            //graphicsDeviceManager.IsFullScreen = true;
            graphicsDeviceManager.ApplyChanges();
            graphicsDeviceManager.GraphicsDevice.RenderState.CullMode = CullMode.None;
            skyBox = new SkyBox(Content, graphicsDeviceManager.GraphicsDevice, Window);
            formsManager.Initialize();
            cameraManager.Initialize(Window);            
            Sound.Initialize();
            #endregion           
            
            
            // Intitalize road - Nour		
            List<List<Vector3>> dummyBigList = new List<List<Vector3>>();
            if (GPSRoadChosen == true)
            {
                dummyBigList.Add(RoadsImporter.Road3());
                dummyBigList.Add(RoadsImporter.Road4());
                gpsRoad.Initialize(dummyBigList);
                myCar = new Car(this, gpsRoad);
            }
            else 
            {
                dummyBigList.Add(XMLImporter.Import("RoadsDataBase\\Road1",this.Content));
                //dummyBigList.Add(RoadsImporter.Road2());
                road.Initialize(dummyBigList);
                myCar = new Car(this, road);
            }

            
            

            // Intitalize speedometer - Zaher
            this.speedoMeter = new SpeedoMeter(myCar, Content, graphicsDeviceManager.GraphicsDevice, Window);
            speedoMeter.InitializeSpeedoMeter();

            // Initialize Environment Objects - ZGTR
            this.myModelArray = new Object3D[2];
            myModelArray[0] = new Object3D
                (this.graphicsDeviceManager.GraphicsDevice, @"Models\Environment\Building1"
                , this.Content, this.GetCurrentCamera , 70 , new Vector3(0,0,-2000));

            base.Initialize();
        }

        private Texture2D pointerTex;
        protected override void LoadContent()
        {
            #region NOUR
            font = Content.Load<SpriteFont>("MyFont");
            pointerTex = Content.Load<Texture2D>(@"Images\cross");
            formsManager.Load(Content);
            cameraManager.SetGUI(ref formsManager.camerasControl);
            skyBox.LoadContent();
            Sound.StartGearSound();
            #endregion


            //Load texture's road//Nour
            if (GPSRoadChosen == true)
            {
                gpsRoad.LoadContent(graphicsDeviceManager.GraphicsDevice, Content);
            }
            else
            {
                road.LoadContent(graphicsDeviceManager.GraphicsDevice, Content);
            }

            myCar.Load(Content);
            speedoMeter.LoadContent();
            users = (IUserInterface)Services.GetService(typeof(IUserInterface));
            terrain = new Terrain(Content, graphicsDeviceManager);
            myCar.InitMainComponent(ref formsManager.initializationForm);
            spriteBatch = new SpriteBatch(GraphicsDevice);
            //formsManager.GPSform.defineDestination.onClick += new FuchsGUI.EHandler(DefineDestinationbool);
            formsManager.initializationForm.Submit.onClick += new FuchsGUI.EHandler(Submit);


        }

        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if ((cameraManager.currentCamera.GetType()                                                                  ) != typeof(FreeCamera))
            {
                if (enableJoystick)
                    HandleJoyStick(gameTime);
                else
                    if (enableKeyboard)
                        HandleKeyboardInput(gameTime);
            }
           myCar.Update(gameTime, brakeEnabled);
           speedoMeter.update(gameTime);

           if (brakeEnabled)
               brakeTimer += gameTime.ElapsedRealTime.Milliseconds;
           else brakeTimer = 0;

           myCar.carSystems.Brakes.myABSSystem[3].UpdateABSValues(ref formsManager.ABS_Dataform);
           myCar.carSystems.Brakes.updateBrakeValues(ref formsManager.brakeDataForm);
           #region NOUR
           myCar.UpdateGUI(ref formsManager.ESPform);
           cameraManager.Update(gameTime, myCar.position, (float)myCar.velocity.Value, Window);
           MouseState mouseState = Mouse.GetState();
            
           formsManager.Update(this);
           Sound.UpdateGearSound((float)Math.Round(myCar.velocity.Value * 3.6 * 0.621371) / 5, (float)myCar.acceleration.Value);//, myCar.gear.gearOn);
           Sound.Update(gameTime);
           #endregion
           base.Update(gameTime);
        }

        //edit by zaher       
        private void HandleJoyStick(GameTime gameTime)
        {
            if (waiting_for_start)
            {
                if (users.FindUserOne())
                {
                    waiting_for_start = false;
                    u1 = users.GetUser(0);
                }
            }
            else
            {
                // Left Stick 
                Vector2 l = u1.GetLeftStick();
                l.Y = l.Y * -1;
                if (l.X > 0)
                {
                    myCar.tires[2].SteerPerc = l.X;
                    myCar.Steer(true);
                    Car.rightJoyStick = true;

                }
                else if (l.X < 0)
                {
                    myCar.tires[2].SteerPerc = -1 * l.X;
                    myCar.Steer(false);
                    Car.leftJoyStick = true;
                    //  myCar.MyForm.textBox1.Text = (-1*l.X).ToString();
                    //  myCar.MyForm.textBox2.Text = Tire.floatAngleToSteer.ToString();
                }
                else 
                {
                    Car.rightJoyStick = false;
                    Car.leftJoyStick = false;
                }

                // Right Stick 
                Vector2 r = u1.GetRightStick();
                r.Y = r.Y * -1;
                if (r.X > 0)
                {
                    //zaher
                    myCar.carSystems.Brakes.ButtonScale = r.X;
                    myCar.setBrakeTorque(r.X);
                    brakeEnabled = true;
                    myCar.DecreaseRPM(gameTime);
                    

                }
                else if (r.X < 0)
                {
                    myCar.IncreaseRPM(gameTime);
                    brakeEnabled = false;
                    //zaher
                    myCar.resetBrakeValues();

                }
                else
                {
                    myCar.resetBrakeValues();
                    myCar.DecreaseRPM(gameTime);
                }
                // A Pressed
                if (u1.PressedA())
                    myCar.IncreaseRPM(gameTime);

                // Start Pressed De/Activate ABS 
                if (u1.PressedStart())
                {
                    myCar.ABSEnabled = !myCar.ABSEnabled;
                    if (myCar.ABSEnabled)
                        DirectInputWrapper.SendForce(DirectInputWrapper.ForceType.LowRumble, true);
                }
                // Back Pressed De/Activate ESP 
                if (u1.PressedBack())
                {
                    myCar.carSystems.ESP.ESPenabled = !myCar.carSystems.ESP.ESPenabled;
                    if (myCar.carSystems.ESP.ESPenabled)
                        DirectInputWrapper.SendForce(DirectInputWrapper.ForceType.LowRumble, true);
                }

                if (u1.PressedMenuUp())
                {
                    enableJoystick = false;
                    enableKeyboard = true;
                }
            }
        }
        bool enableJoystick = true;
        bool enableKeyboard = false;

        private void HandleKeyboardInput(GameTime gameTime)
        {
            KeyboardState kS = Keyboard.GetState();
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || kS.IsKeyDown(Keys.Escape))
            {
                this.Exit();
            }

            //Engine's RPM
            if (kS.IsKeyDown(Keys.W))
                myCar.IncreaseRPM(gameTime);
            else 
                myCar.DecreaseRPM(gameTime);
            if (kS.IsKeyDown(Keys.S))
                myCar.DecreaseRPM(gameTime);
           
            //Steering
            if (kS.IsKeyDown(Keys.A))
            {
                myCar.Steer(false);		 //1 <=> Left
                //myCar.UpdateAngle();
            }
            if (kS.IsKeyDown(Keys.D))
            { 
                myCar.Steer(true);		 //0 <=> Right
            }


            //Braking
            if (kS.IsKeyDown(Keys.Space))
            {
                myCar.setBrakeTorque(1);
                brakeEnabled = true;
            }
            else
            {
                myCar.resetBrakeValues();
                brakeEnabled = false;
            }

            // Avtivate ABS 
            if (kS.IsKeyDown(Keys.Q) && prevKeyboardState.IsKeyUp(Keys.Q))
            {
                myCar.ABSEnabled = !myCar.ABSEnabled;
                if (myCar.ABSEnabled)
                    DirectInputWrapper.SendForce(DirectInputWrapper.ForceType.BriefJolt, true);
                //  myCar.MyForm.reattorqueT.Text = ABSenabled? "true" : "fasle"; 
            }
            if (kS.IsKeyDown(Keys.E) && prevKeyboardState.IsKeyUp(Keys.E))
            {
                myCar.carSystems.ESP.ESPenabled = !myCar.carSystems.ESP.ESPenabled;
                if (myCar.carSystems.ESP.ESPenabled)
                    DirectInputWrapper.SendForce(DirectInputWrapper.ForceType.BriefJolt, true);
                //  myCar.MyForm.reattorqueT.Text = ABSenabled? "true" : "fasle"; 
            }
            if (kS.IsKeyDown(Keys.G) && prevKeyboardState.IsKeyUp(Keys.G))
            {
                enableJoystick = true;
                enableKeyboard = false;
            }

            prevKeyboardState = kS;
            //if (kS.IsKeyDown(Keys.S))
                //Sound.SetCue(1, 1);
		}
        KeyboardState prevKeyboardState;
        

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            
            //this five lines Are the solution of using SpriteBatch in 3D World
            GraphicsDevice.RenderState.DepthBufferEnable = true;
            GraphicsDevice.RenderState.AlphaBlendEnable = false;
            GraphicsDevice.RenderState.AlphaTestEnable = false;
            GraphicsDevice.SamplerStates[0].AddressU = TextureAddressMode.Wrap;
            GraphicsDevice.SamplerStates[0].AddressV = TextureAddressMode.Wrap;


            terrain.Draw(cameraManager.currentCamera.viewMatrix, cameraManager.currentCamera.projectionMatrix);

            myCar.Draw(graphicsDeviceManager, cameraManager.currentCamera);
            skyBox.Draw(cameraManager.currentCamera);
            if (GPSRoadChosen == true)
            {
                gpsRoad.Draw(cameraManager.currentCamera, graphicsDeviceManager.GraphicsDevice);
            }
            else
            {
                road.Draw(cameraManager.currentCamera, graphicsDeviceManager.GraphicsDevice);
            }
            //myModelArray[0].Draw(gameTime);
            //
            speedoMeter.DrawSpeedoMeter();
            if (Game1.GPSRoadChosen == true)
            {
                foreach (Object2D o in GPSSystem.Object2DList)
                {
                    o.Draw(Vector3.Zero, graphicsDeviceManager.GraphicsDevice, myCar.carSystems.GPS.GetWorldMatrix(1, myCar.carSystems.GPS.GetGPSPointAngle(), myCar.position));
                }
                foreach (Object3D o in GPSSystem.Object3DList)
                {
                    o.Draw(gameTime);
                }
            }
            formsManager.Draw(spriteBatch);
            spriteBatch.Begin(SpriteBlendMode.AlphaBlend, SpriteSortMode.Deferred, SaveStateMode.SaveState);
            spriteBatch.Draw(pointerTex, new Vector2(Mouse.GetState().X, Mouse.GetState().Y), new Rectangle(100, 50, 400, 400), Color.White, 0, new Vector2(7, 7), 0.06f, SpriteEffects.None, 0);
            spriteBatch.End();
            base.Draw(gameTime);
        }
        
        public Vector3 getMousePosition
        {
            get
            {
                //Get Mouse State  
                MouseState mouseState = Mouse.GetState();

                //Set variables  
                int mouseX = mouseState.X;
                int mouseY = mouseState.Y;

                //Set near and far Vector positions  
                Vector3 nearsource = new Vector3((float)mouseX, (float)mouseY, 0f);
                Vector3 farsource = new Vector3((float)mouseX, (float)mouseY, 1f);

                //Set the world Matrix (Matrix.Identity works fine)  
                Matrix world = Matrix.CreateTranslation(0, 0, 0);

                //Calculate the actual near and far using camera properties  
                Vector3 nearPoint = GraphicsDevice.Viewport.Unproject(nearsource,
                    cameraManager.currentCamera.projectionMatrix, cameraManager.currentCamera.viewMatrix, world);

                Vector3 farPoint = GraphicsDevice.Viewport.Unproject(farsource,
                    cameraManager.currentCamera.projectionMatrix, cameraManager.currentCamera.viewMatrix, world);

                // Create a ray from the near clip plane to the far clip plane.  
                Vector3 direction = farPoint - nearPoint;
                direction.Normalize();
                Ray pickRay = new Ray(nearPoint, direction);

                //Check if the ray is pointing down towards the ground  
                //(aka will it intersect the plane)  
                if (pickRay.Direction.Y < 0)
                {
                    float xPos = 0f;
                    float zPos = 0f;

                    //Move the ray lower along its direction vector  
                    while (pickRay.Position.Y > 0)
                    {
                        pickRay.Position += pickRay.Direction;
                        xPos = pickRay.Position.X;
                        zPos = pickRay.Position.Z;
                    }

                    //Once it has move pass y=0, stop and record the X  
                    // and Y position of the ray, return new Vector3  
                    return new Vector3(xPos, 0, zPos);
                }
                else
                    return new Vector3(0);
            }
        }
           
        void graphics_PreparingDeviceSettings(object sender, PreparingDeviceSettingsEventArgs e)
        {
            DisplayMode displayMode = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode;
            e.GraphicsDeviceInformation.PresentationParameters.BackBufferFormat = displayMode.Format;
            e.GraphicsDeviceInformation.PresentationParameters.BackBufferWidth = displayMode.Width;
            e.GraphicsDeviceInformation.PresentationParameters.BackBufferHeight = displayMode.Height;
        }
        private bool mouseDes = false;
        void DefineDestination(Control sender)
        {
            MouseState mouseState = Mouse.GetState();
            if (mouseState.LeftButton == ButtonState.Pressed)
            {
                mouseDes = true;
                myCar.carSystems.GPS.DestinationPoint = getMousePosition;
            }            
        }
        void Submit(Control sender)
        {
            float.TryParse(formsManager.initializationForm.textBox27.Text, out myCar.carSystems.GPS.DestinationPoint.X);
            float.TryParse(formsManager.initializationForm.textBox15.Text, out myCar.carSystems.GPS.DestinationPoint.Z);
            Game1.GPSRoadChosen = true; 
        }
    }
}
