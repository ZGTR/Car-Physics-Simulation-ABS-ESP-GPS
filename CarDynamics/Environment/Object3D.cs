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
    // ZGTR Made
    public class Object3D
    {
        private Model myModel;
        private Matrix[] boneTransformations;
        private GraphicsDevice graphicsDevice;
        private BasicCamera chaseCamera ; 
        private BasicEffect basicEffect;
        private float scaleVal = 1;
        private Vector3 position = Vector3.Zero; 

        public Object3D(GraphicsDevice graphicDevice, String objectPath,
            ContentManager content, BasicCamera chaseCamera, float scaleVal , Vector3 position) 
        {
            this.position = position;
            this.scaleVal = scaleVal;

            // Initialize Model 
            this.myModel = content.Load<Model>(objectPath);
            this.boneTransformations = new Matrix[myModel.Bones.Count];
            this.myModel.CopyAbsoluteBoneTransformsTo(boneTransformations);

            // Initialize private variables 
            this.chaseCamera = chaseCamera ; 
            this.graphicsDevice = graphicDevice;
            basicEffect = new BasicEffect(graphicDevice, null);
            basicEffect.LightingEnabled = true;
        }

        public void Draw(GameTime gameTime)
        {
            graphicsDevice.RenderState.CullMode = CullMode.CullClockwiseFace; 
            foreach (ModelMesh mesh in myModel.Meshes)
            {
                // 1: declare matrices
                Matrix world, scale, rotationZ, translation;
                // 2: initialize matrices
                scale = Matrix.CreateScale(scaleVal, scaleVal, scaleVal);
                translation = Matrix.CreateTranslation(position);
                rotationZ = Matrix.CreateRotationZ(0.0f);
                // 3: build cumulative world matrix using I.S.R.O.T. sequence
                // identity, scale, rotate, orbit(translate&rotate), translate
                world = scale * rotationZ * translation;
                // 4: set shader parameters
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.World = boneTransformations[mesh.ParentBone.Index] * world;
                    effect.View = chaseCamera.viewMatrix;
                    effect.Projection = chaseCamera.projectionMatrix;
                    effect.EnableDefaultLighting();
                }
                // 5: draw object
                mesh.Draw();
            }
            // stop culling
            graphicsDevice.RenderState.CullMode = CullMode.None;
        }

        // Overloaded Draw Method - Position Taken
        public void Draw(GameTime gameTime , Vector3 position)
        {
            this.position = position;
            graphicsDevice.RenderState.CullMode = CullMode.CullClockwiseFace;
            foreach (ModelMesh mesh in myModel.Meshes)
            {
                // 1: declare matrices
                Matrix world, scale, rotationZ, translation;
                // 2: initialize matrices
                scale = Matrix.CreateScale(scaleVal, scaleVal, scaleVal);
                translation = Matrix.CreateTranslation(position);
                rotationZ = Matrix.CreateRotationZ(0.0f);
                // 3: build cumulative world matrix using I.S.R.O.T. sequence
                // identity, scale, rotate, orbit(translate&rotate), translate
                world = scale * rotationZ * translation;
                // 4: set shader parameters
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.World = boneTransformations[mesh.ParentBone.Index] * world;
                    effect.View = chaseCamera.viewMatrix;
                    effect.Projection = chaseCamera.projectionMatrix;
                    effect.EnableDefaultLighting();
                }
                // 5: draw object
                mesh.Draw();
            }
            // stop culling
            graphicsDevice.RenderState.CullMode = CullMode.None;
        }
    }
}
