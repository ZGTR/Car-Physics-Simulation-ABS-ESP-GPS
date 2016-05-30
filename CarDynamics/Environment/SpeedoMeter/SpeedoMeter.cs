using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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



namespace CarDynamics
{
    class SpeedoMeter
    {
        private SpriteBatch spriteBatch;
        private SpriteFont font;
        private Texture2D SpeedoMeterTexture;
        private Car MyCar;
        float SpeedoMeterscale;
        float rotation = 2.4f;
        float scale = 0.6f;
        GameWindow Window;
        GraphicsDevice graphics;
        ContentManager Content;

        //texture contents
        Vector2 SpeedoMeterPos, SpeedoMeterArrowPos, MphPos, RotatingPointPos;
        Rectangle TachoGfxRect, TachoArrowGfxRect, TachoMphGfxRect, TachoGearGfxRect;
        public SpeedoMeter(Car _myCar, ContentManager _Content, GraphicsDevice _graphic, GameWindow _window)
        {
            MyCar = _myCar;
            this.graphics = _graphic;
            this.Content = _Content;
            this.Window = _window;
            InitializeSpeedoMeter();

        }
        public void LoadContent()
        {
            spriteBatch = new SpriteBatch(graphics);
            font = Content.Load<SpriteFont>("Myfont");
            SpeedoMeterTexture = Content.Load<Texture2D>("Images\\ingame");
        }

        public void InitializeSpeedoMeter()
        {
            SpeedoMeterPos = new Vector2((Window.ClientBounds.Width - (344 * SpeedoMeterscale)),
                    (Window.ClientBounds.Height - (344 * SpeedoMeterscale)));
            SpeedoMeterArrowPos = new Vector2(Window.ClientBounds.Width - ((344 / 2.5f + 27) * SpeedoMeterscale),
                     Window.ClientBounds.Height - ((344 / 2.5f + 190) * SpeedoMeterscale));
            MphPos = new Vector2(Window.ClientBounds.Width - (136 * SpeedoMeterscale),
                     Window.ClientBounds.Height - (84 * SpeedoMeterscale));
            RotatingPointPos = new Vector2(TachoArrowGfxRect.Width / 2, TachoArrowGfxRect.Height - 10);
            TachoGfxRect = new Rectangle(0, 0, 344, 344);
            TachoArrowGfxRect = new Rectangle(348, 2, 27, 182);
            TachoMphGfxRect = new Rectangle(184, 256, 148, 72);
            TachoGearGfxRect = new Rectangle(286, 149, 52, 72);

            max = scale + 0.004f;
            min = scale - 0.004f;
            SpeedoMeterscale = max;
            maxbool = false;
            minbool = true;

        }
        float step = 0.000165f, max, min;
        int TimeSinceLastFrame = 0;
        int TimePerFrame = 1;
        bool maxbool, minbool;
        public void update(GameTime gameTime)
        {
            rotation = ((2.4f * (float)MyCar.engine.rpm) / 7200f) - 2.4f;
            //edit
            TimeSinceLastFrame += gameTime.ElapsedGameTime.Milliseconds;

            if (TimeSinceLastFrame > TimePerFrame)
            {
                if ((SpeedoMeterscale < max) && maxbool)
                {
                    SpeedoMeterscale += step;
                    if (SpeedoMeterscale >= max)
                    {
                        maxbool = false;
                        minbool = true;
                    }
                }
                else
                    if ((SpeedoMeterscale > min) && minbool)
                    {
                        SpeedoMeterscale -= step;
                        if (SpeedoMeterscale <= min)
                        {
                            maxbool = true;
                            minbool = false;
                        }
                    }
                TimeSinceLastFrame = 0;
            }
        }
        // Drawing speedometer


        public void DrawSpeedoMeter()
        {
            spriteBatch.Begin();
            spriteBatch.Draw(SpeedoMeterTexture, SpeedoMeterPos, TachoGfxRect, Color.White, 0, Vector2.Zero, SpeedoMeterscale, SpriteEffects.None, 0.5f);
            spriteBatch.Draw(SpeedoMeterTexture, new Vector2(SpeedoMeterArrowPos.X + 13 * SpeedoMeterscale, SpeedoMeterArrowPos.Y + 176.5f * SpeedoMeterscale),
                            TachoArrowGfxRect, Color.White, rotation, RotatingPointPos,
                            SpeedoMeterscale, SpriteEffects.None, 0.6f);
            MphPos = new Vector2(Window.ClientBounds.Width - (136 * SpeedoMeterscale),
                           Window.ClientBounds.Height - (84 * SpeedoMeterscale));
            spriteBatch.DrawString(font, (Math.Round(MyCar.velocity.Value * 3.6 * 0.621371)).ToString(), MphPos, Color.White);
            spriteBatch.End();
        }
    }
}
