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
    public class Object2D
    {
        private Texture2D texture ;
        private GraphicsDevice graphicsDevice;
        private VertexPositionTexture[] vertextPositionTextureArr;
        private BasicCamera chaseCamera ; 
        private BasicEffect basicEffect;
        private Vector2 size = Vector2.One ;
        private Vector3 position = Vector3.Zero;
        private String orientation = "Ground";

        public Object2D(GraphicsDevice graphicDevice, String texturePath,
            ContentManager content, BasicCamera chaseCamera, Vector2 size, String orientation , Vector3 position)  
        {
            // Initialize private variables
            this.position = position;
            this.orientation = orientation;
            this.size = size; 
            this.chaseCamera = chaseCamera ; 
            this.graphicsDevice = graphicDevice;
            vertextPositionTextureArr = new VertexPositionTexture[4];

            // Load Content
            texture = content.Load<Texture2D>(texturePath);           
        }

        protected virtual void SetUpVertices(Vector3 position, Vector2 size)
        {
            if (orientation == "Ground")
            {
                vertextPositionTextureArr[0] = new VertexPositionTexture(
                    new Vector3(0, 0, 0) + position, new Vector2(0, 0));
                vertextPositionTextureArr[1] = new VertexPositionTexture(
                    new Vector3(size.X, 0, 0) + position, new Vector2(1, 0));
                vertextPositionTextureArr[2] = new VertexPositionTexture(
                    new Vector3(0, 0, size.Y) + position, new Vector2(0, 1));
                vertextPositionTextureArr[3] = new VertexPositionTexture(
                    new Vector3(size.X, 0, size.Y) + position, new Vector2(1, 1));
            }
            else if (orientation == "Face")
            {
                vertextPositionTextureArr[0] = new VertexPositionTexture(
                    new Vector3(0, 0, 0) + position, new Vector2(0, 0));
                vertextPositionTextureArr[1] = new VertexPositionTexture(
                    new Vector3(size.X, 0, 0) + position, new Vector2(1, 0));
                vertextPositionTextureArr[2] = new VertexPositionTexture(
                    new Vector3(0, -size.Y, 0) + position, new Vector2(0, 1));
                vertextPositionTextureArr[3] = new VertexPositionTexture(
                    new Vector3(size.X, -size.Y, 0) + position, new Vector2(1, 1));
            }
            else if (orientation == "Side")
            {
                vertextPositionTextureArr[0] = new VertexPositionTexture(
                    new Vector3(0, 0, 0) + position, new Vector2(0, 0));
                vertextPositionTextureArr[1] = new VertexPositionTexture(
                    new Vector3(0, 0, -size.X) + position, new Vector2(1, 0));
                vertextPositionTextureArr[2] = new VertexPositionTexture(
                    new Vector3(0, -size.Y, 0) + position, new Vector2(0, 1));
                vertextPositionTextureArr[3] = new VertexPositionTexture(
                    new Vector3(0, -size.Y, -size.X) + position, new Vector2(1, 1));
            }
        }

        //private Vector3 GetReferncePoint()
        //{
        //    Vector3 ReferencePoint = Vector3.Zero;
        //    ReferencePoint.X = Math.Abs(vertextPositionTextureArr[0].Position.X - vertextPositionTextureArr[1].Position.X);
        //    ReferencePoint.Y = Math.Abs(vertextPositionTextureArr[0].Position.Y - vertextPositionTextureArr[2].Position.Y);
        //    return ReferencePoint;         
        //}

        //private void RotateVertex (Vector3 ReferencePoint ,ref Vector3 PointToRotate)
        //{
        //    RightAngledTriangle rightAngledTriangle = new RightAngledTriangle(ReferencePoint,PointToRotate);
        //    int OrigionalAngle = rightAngledTriangle.angleToReturn;
        //    // STOPPED HERE
    
        //}

        private void RotateVector(Matrix WorldMatrix)
        {
            for (int i = 0; i < 4; i++)
            {
                Vector3 rotatedVector = vertextPositionTextureArr[i].Position;
                Matrix tempMatrix = Matrix.Identity;
                tempMatrix = Matrix.CreateTranslation(rotatedVector);
                tempMatrix *= WorldMatrix;
                rotatedVector.X = tempMatrix.Translation.X;
                rotatedVector.Z = tempMatrix.Translation.Z;
                rotatedVector.Y = tempMatrix.Translation.Y;            
                vertextPositionTextureArr[i].Position = rotatedVector;
            }
            
            // Orgional
            // Vector3 ReferencePoint = GetReferncePoint();
            // RotateVertex(ReferencePoint , ref vertextPositionTextureArr[0].Position);
            // STOPPED HERE        
        }

        protected virtual void SetUpVertices(Vector3 position, Vector2 size , Matrix WorldMatrix)
        {
            if (orientation == "Ground")
            {
                vertextPositionTextureArr[0] = new VertexPositionTexture(
                    new Vector3(0, 0, 0) + position, new Vector2(0, 0));
                vertextPositionTextureArr[1] = new VertexPositionTexture(
                    new Vector3(size.X, 0, 0) + position, new Vector2(1, 0));
                vertextPositionTextureArr[2] = new VertexPositionTexture(
                    new Vector3(0, 0, size.Y) + position, new Vector2(0, 1));
                vertextPositionTextureArr[3] = new VertexPositionTexture(
                    new Vector3(size.X, 0, size.Y) + position, new Vector2(1, 1));
            }
            else if (orientation == "Face")
            {
                vertextPositionTextureArr[0] = new VertexPositionTexture(
                    new Vector3(0, 0, 0) + position, new Vector2(0, 0));
                vertextPositionTextureArr[1] = new VertexPositionTexture(
                    new Vector3(size.X, 0, 0) + position, new Vector2(1, 0));
                vertextPositionTextureArr[2] = new VertexPositionTexture(
                    new Vector3(0, -size.Y, 0) + position, new Vector2(0, 1));
                vertextPositionTextureArr[3] = new VertexPositionTexture(
                    new Vector3(size.X, -size.Y, 0) + position, new Vector2(1, 1));
            }
            else if (orientation == "Side")
            {
                vertextPositionTextureArr[0] = new VertexPositionTexture(
                    new Vector3(0, 0, 0) + position, new Vector2(0, 0));
                vertextPositionTextureArr[1] = new VertexPositionTexture(
                    new Vector3(0, 0, -size.X) + position, new Vector2(1, 0));
                vertextPositionTextureArr[2] = new VertexPositionTexture(
                    new Vector3(0, -size.Y, 0) + position, new Vector2(0, 1));
                vertextPositionTextureArr[3] = new VertexPositionTexture(
                    new Vector3(0, -size.Y, -size.X) + position, new Vector2(1, 1));
            }

            // Rotate The texture with specified angle 
            RotateVector(WorldMatrix);
        }

        public virtual void Draw(GraphicsDevice graphicsDevice)
        {
            basicEffect = new BasicEffect(graphicsDevice, null);
            basicEffect.LightingEnabled = true;
            this.orientation = orientation;
            this.size = size;
            SetUpVertices(position, size);
            // Transparent - Begin (Alpha-Blending the Texture)
            graphicsDevice.RenderState.AlphaBlendEnable = true;
            graphicsDevice.RenderState.SourceBlend = Blend.SourceAlpha;
            graphicsDevice.RenderState.DestinationBlend = Blend.InverseSourceAlpha;
            // Manipulating Texture
            graphicsDevice.VertexDeclaration = new VertexDeclaration
                (graphicsDevice, VertexPositionTexture.VertexElements);
            basicEffect.World = Matrix.Identity;
            basicEffect.View = chaseCamera.viewMatrix;
            basicEffect.Projection = chaseCamera.projectionMatrix;
            basicEffect.Texture = texture;
            basicEffect.TextureEnabled = true;
            // Begin - BasicEffect
            basicEffect.Begin();
            foreach (EffectPass pass in basicEffect.CurrentTechnique.Passes)
            {
                basicEffect.EnableDefaultLighting();
                pass.Begin();
                graphicsDevice.DrawUserPrimitives<VertexPositionTexture>
                    (PrimitiveType.TriangleStrip, vertextPositionTextureArr, 0, 2);
                pass.End();
            }
            // End - BasicEffect
            basicEffect.End();
            // End - Transparent
            graphicsDevice.RenderState.AlphaBlendEnable = false;
        }

        public virtual void Draw(Vector3 position , GraphicsDevice graphicsDevice)
        {
            basicEffect = new BasicEffect(graphicsDevice, null);
            basicEffect.LightingEnabled = true;
            this.orientation = orientation;
            this.size = size;
            SetUpVertices(position, size);
            // Transparent - Begin (Alpha-Blending the Texture)
            graphicsDevice.RenderState.AlphaBlendEnable = true;
            graphicsDevice.RenderState.SourceBlend = Blend.SourceAlpha;
            graphicsDevice.RenderState.DestinationBlend = Blend.InverseSourceAlpha;
            // Manipulating Texture
            graphicsDevice.VertexDeclaration = new VertexDeclaration
                (graphicsDevice, VertexPositionTexture.VertexElements);
            basicEffect.World = Matrix.Identity;
            basicEffect.View = chaseCamera.viewMatrix;
            basicEffect.Projection = chaseCamera.projectionMatrix;
            basicEffect.Texture = texture;
            basicEffect.TextureEnabled = true;
            // Begin - BasicEffect
            basicEffect.Begin();
            foreach (EffectPass pass in basicEffect.CurrentTechnique.Passes)
            {
                basicEffect.EnableDefaultLighting();
                pass.Begin();
                graphicsDevice.DrawUserPrimitives<VertexPositionTexture>
                    (PrimitiveType.TriangleStrip, vertextPositionTextureArr, 0, 2);
                pass.End();
            }
            // End - BasicEffect
            basicEffect.End();
            // End - Transparent
            graphicsDevice.RenderState.AlphaBlendEnable = false;
        }

        public virtual void Draw(Vector3 centralizedPosition, GraphicsDevice graphicsDevice, Matrix WorldMatrix)
        {
            basicEffect = new BasicEffect(graphicsDevice, null);
            basicEffect.LightingEnabled = true;
            SetUpVertices(centralizedPosition, size, WorldMatrix);
            // Transparent - Begin (Alpha-Blending the Texture)
            graphicsDevice.RenderState.AlphaBlendEnable = true;
            graphicsDevice.RenderState.SourceBlend = Blend.SourceAlpha;
            graphicsDevice.RenderState.DestinationBlend = Blend.InverseSourceAlpha;
            // Manipulating Texture
            graphicsDevice.VertexDeclaration = new VertexDeclaration
                (graphicsDevice, VertexPositionTexture.VertexElements);
            basicEffect.World = Matrix.Identity;
            basicEffect.View = chaseCamera.viewMatrix;
            basicEffect.Projection = chaseCamera.projectionMatrix;
            basicEffect.Texture = texture;
            basicEffect.TextureEnabled = true;            
            // Begin - BasicEffect
            basicEffect.Begin();
            foreach (EffectPass pass in basicEffect.CurrentTechnique.Passes)
            {
                basicEffect.EnableDefaultLighting();
                pass.Begin();
                graphicsDevice.DrawUserPrimitives<VertexPositionTexture>
                    (PrimitiveType.TriangleStrip, vertextPositionTextureArr, 0, 2);
                pass.End();
            }
            // End - BasicEffect
            basicEffect.End();
            // End - Transparent
            graphicsDevice.RenderState.AlphaBlendEnable = false;
        }
    }
}
