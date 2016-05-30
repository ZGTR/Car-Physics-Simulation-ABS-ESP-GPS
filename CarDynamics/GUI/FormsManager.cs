using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FuchsGUI;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using CarDynamics;
using Sounds;

namespace GUI
{
    class FormsManager
    {
        public MainForm mainForm;
        public CamerasControl camerasControl;
        public ESPForm ESPform;
        public BrakeDataForm brakeDataForm;
        public ABS_DataForm ABS_Dataform;
        public GPSForm GPSform;
        public InitializationForm initializationForm;

        private const int formsNumber = 5;

        private int cameraIndex = 0;
        private int absIndex = 1;
        private int brakeIndex = 2;
        private int esbIndex = 3;
        private int gpsIndex = 4;
        //private const int 

        public void Initialize()
        {
        }
        public void Load(ContentManager Content)
        {
            Texture2D tex = Content.Load<Texture2D>(@"GUI\texForm");
            SpriteFont font = Content.Load<SpriteFont>(@"GUI\Arial");
            mainForm = new MainForm(null, font, Color.Black, Content);

            mainForm.ESP.onMouseEnter += new EHandler(PlayScreenBack);
            mainForm.ESP.onClick += new EHandler(PlayScreenClick);

            mainForm.ABS.onMouseEnter += new EHandler(PlayScreenBack);
            mainForm.ABS.onClick += new EHandler(PlayScreenClick);

            mainForm.CamerasControl.onMouseEnter += new EHandler(PlayScreenBack);
            mainForm.CamerasControl.onClick += new EHandler(PlayScreenClick);

            mainForm.Brake_Data_Form.onMouseEnter += new EHandler(PlayScreenBack);
            mainForm.Brake_Data_Form.onClick += new EHandler(PlayScreenClick);


            mainForm.ABS.onMouseEnter += new EHandler(PlayScreenBack);
            mainForm.ABS.onClick += new EHandler(PlayScreenClick);

            mainForm.GPS.onMouseEnter += new EHandler(PlayScreenBack);
            mainForm.GPS.onClick += new EHandler(PlayScreenClick);

            mainForm.Exit.onMouseEnter += new EHandler(PlayScreenBack);
            mainForm.Exit.onClick += new EHandler(PlayScreenClick);

            mainForm.Enabled = mainForm.Visible = false;

            camerasControl = new CamerasControl(tex, font, Color.Black, Content);
            camerasControl.Visible = false;
            camerasControl.Enabled = false;
            HideForm(camerasControl, cameraIndex);
            camerasControl.CamerasControlGroupHide.onMouseEnter += new EHandler(PlayScreenBack);
            camerasControl.CamerasControlGroupHide.onClick += new EHandler(PlayScreenClick);


            ESPform = new ESPForm(tex, font, Color.White, Content);
            ESPform.Visible = false;
            ESPform.Enabled = false;            
            HideForm(ESPform, esbIndex);
            ESPform.Hide.onMouseEnter += new EHandler(PlayScreenBack);
            ESPform.Hide.onClick += new EHandler(PlayScreenClick);

            ABS_Dataform = new ABS_DataForm(tex, font, Color.White, Content);
            ABS_Dataform.Visible = ABS_Dataform.Enabled = false;
            HideForm(ABS_Dataform, absIndex);
            ABS_Dataform.Hide.onMouseEnter += new EHandler(PlayScreenBack);
            ABS_Dataform.Hide.onClick += new EHandler(PlayScreenClick);


            brakeDataForm = new BrakeDataForm(tex, font, Color.White, Content);
            brakeDataForm.Visible = brakeDataForm.Enabled = false;
            HideForm(brakeDataForm, brakeIndex);
            brakeDataForm.Hide.onMouseEnter += new EHandler(PlayScreenBack);
            brakeDataForm.Hide.onClick += new EHandler(PlayScreenClick);

            GPSform = new GPSForm(tex, font, Color.White, Content);
            GPSform.Visible = GPSform.Enabled = false;
            HideForm(GPSform, gpsIndex);
            GPSform.Hide.onMouseEnter += new EHandler(PlayScreenBack);
            GPSform.Hide.onClick += new EHandler(PlayScreenClick);

            initializationForm = new InitializationForm(tex, font, Color.White, Content);
            initializationForm.Submit.onMouseEnter += new EHandler(PlayScreenBack);
            initializationForm.Submit.onClick += new EHandler(PlayScreenClick);
            initializationForm.Submit.onClick += new EHandler(CloseMainMenu);
            initializationForm.Enabled = initializationForm.Visible = true;

            Sound.Play(Sound.Sounds.MenuMusic);
        }


        public void Update(Game1 game)
        {

            MouseState mouseState = Mouse.GetState();
            KeyboardState keyState = Keyboard.GetState();
            if(mainForm.Visible)
                mainForm.Update(mouseState, keyState);
            if(camerasControl.Visible)
                camerasControl.Update(mouseState, keyState);
            if(ESPform.Visible)
                ESPform.Update(mouseState, keyState);
            if(ABS_Dataform.Visible)
                ABS_Dataform.Update(mouseState, keyState);
            if(brakeDataForm.Visible)
                brakeDataForm.Update(mouseState, keyState);
            if(GPSform.Visible)
                GPSform.Update(mouseState, keyState);


            //Camera Button
            if (camerasControl.CamerasControlGroupHide.Clicked || hideForm[cameraIndex])
                HideForm(camerasControl, cameraIndex);
            else
                if (mainForm.CamerasControl.Clicked || showForm[cameraIndex])
                    ShowForm(camerasControl, cameraIndex);


            //ABS Button
            if (ABS_Dataform.Hide.Clicked || hideForm[absIndex])
                HideForm(ABS_Dataform, absIndex);
            else
                if (mainForm.ABS.Clicked || showForm[absIndex])
                    ShowForm(ABS_Dataform, absIndex);


            //ESP Button
            if (ESPform.Hide.Clicked || hideForm[esbIndex])
                HideForm(ESPform, esbIndex);
            else
                if (mainForm.ESP.Clicked || showForm[esbIndex])
                    ShowForm(ESPform, esbIndex);
            //brake
            if (brakeDataForm.Hide.Clicked || hideForm[brakeIndex])
                HideForm(brakeDataForm, brakeIndex);
            else
                if (mainForm.Brake_Data_Form.Clicked || showForm[brakeIndex])
                    ShowForm(brakeDataForm, brakeIndex);

            //GPS
            if (GPSform.Hide.Clicked || hideForm[gpsIndex])
                HideForm(GPSform, gpsIndex);
            else
                if (mainForm.GPS.Clicked || showForm[gpsIndex])
                    ShowForm(GPSform, gpsIndex);

            if (initializationForm.Visible)
                initializationForm.Update(mouseState, keyState);
            //camerasControl.label1.Text += "   " + game.cameraManager.currentCamera.desiredChaseDistance;
            if (mainForm.Exit.Clicked)
            {
                game.Exit();
            }
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            if (mainForm.Visible)
                mainForm.Draw(spriteBatch);
            if (camerasControl.Visible)
                camerasControl.Draw(spriteBatch);
            if (ESPform.Visible)
                ESPform.Draw(spriteBatch);
            if (ABS_Dataform.Visible) 
                ABS_Dataform.Draw(spriteBatch);
            if (brakeDataForm.Visible) 
                brakeDataForm.Draw(spriteBatch);
            if (GPSform.Visible) 
                GPSform.Draw(spriteBatch);
            if (initializationForm.Visible)
                initializationForm.Draw(spriteBatch);
            spriteBatch.End();
        }

        bool[] hideForm = new bool[formsNumber];
        void HideForm(Form form, int index)
        {
            if (form.Position.X + form.Width > 0)
            {
                form.Position -= new Vector2(6, 0);
                hideForm[index] = true;
            }
            else
            {
                hideForm[index] = false;
                form.Enabled = false;
                mainForm.Visible = true;
                mainForm.Enabled = true;
            }
        }

        bool[] showForm = new bool[formsNumber];
        void ShowForm(Form form, int index)
        {
            if (form.Position.X < 0)
            {
                form.Position += new Vector2(6, 0);
                showForm[index] = true;
                form.Visible = true;
                mainForm.Visible = false;
                mainForm.Enabled = false;
            }
            else
            {
                showForm[index] = false;
                form.Enabled = true;                
            }
        }

        void PlayScreenClick(Control sender)
        {
            Sound.Play(Sound.Sounds.ScreenClick);
        }
        void PlayScreenBack(Control sender)
        {
            Sound.Play(Sound.Sounds.ScreenBack);
        }
        void CloseMainMenu(Control sender)
        {
            initializationForm.Visible = initializationForm.Enabled = false;
            mainForm.Visible = mainForm.Enabled = true;
            Sound.StopMusic();
        }
    }
}
