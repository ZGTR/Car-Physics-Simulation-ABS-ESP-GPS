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
	public class Road
	{
		private BasicEffect basicEffect;
		private Texture2D road;
        private List<VertexPositionNormalTexture> trackVertices;
        private List<List<VertexPositionNormalTexture>> textLayersList;
        public List<List<Vector3>> LayersList;
		private VertexDeclaration myVertexDeclaration;
        public int RoadLayersCount = 1; 
		
		public void NRoad()
		{	   
		}

        public void Initialize(List<List<Vector3>> LayersList)
        {
            this.LayersList = LayersList;
            textLayersList = new List<List<VertexPositionNormalTexture>>();
            RoadLayersCount = this.LayersList.Count ;
            for (int i = 0; i < LayersList.Count; i++)
            {
                List<Vector3> extendedTrackPoints = GenerateTrackPoints(LayersList[i]);
                LayersList[i].Clear();
                textLayersList.Add(GenerateTrackVertices(extendedTrackPoints));
                LayersList[i] = ConvertToVector3List(textLayersList[i]);                
            }
        }

        private List<Vector3> ConvertToVector3List(List<VertexPositionNormalTexture> dummyList)
        {
            List<Vector3> ListToReturn = new List<Vector3>();
            for (int i = 0; i < dummyList.Count; i++)
            {
                ListToReturn.Add(dummyList[i].Position);
            }
            return ListToReturn;            
        }

        private Vector3 CR3D(Vector3 v1, Vector3 v2, Vector3 v3, Vector3 v4, float amount)
        {
            Vector3 result = new Vector3();

            result.X = MathHelper.CatmullRom(v1.X, v2.X, v3.X, v4.X, amount);
            result.Y = MathHelper.CatmullRom(v1.Y, v2.Y, v3.Y, v4.Y, amount);
            result.Z = MathHelper.CatmullRom(v1.Z, v2.Z, v3.Z, v4.Z, amount);

            return result;
        }

        private List<Vector3> InterpolateCR(Vector3 v1, Vector3 v2, Vector3 v3, Vector3 v4)
        {
            List<Vector3> list = new List<Vector3>();
            int detail = 20;
            for (int i = 0; i < detail; i++)
            {
                Vector3 newPoint = CR3D(v1, v2, v3, v4, (float)i / (float)detail);
                list.Add(newPoint);
            }
            return list;
        }

        private List<Vector3> GenerateTrackPoints(List<Vector3> basePoints)
        {
            basePoints.Add(basePoints[0]);
            basePoints.Add(basePoints[1]);
            basePoints.Add(basePoints[2]);

            List<Vector3> allPoints = new List<Vector3>();

            for (int i = 1; i < basePoints.Count - 2; i++)
            {
                List<Vector3> part = InterpolateCR(basePoints[i - 1], basePoints[i], basePoints[i + 1], basePoints[i + 2]);
                allPoints.AddRange(part);
            }

            return allPoints;
        }

        public List<List<Vector3>> GetTrackVertices()
        {
            return LayersList;
        }

        private List<VertexPositionNormalTexture> GenerateTrackVertices(List<Vector3> basePoints)
        {
            float halfTrackWidth = 5.0f;
            float textureLenght = 5.0f;

            float distance = 0;
            List<VertexPositionNormalTexture> verticesList = new List<VertexPositionNormalTexture>();

            for (int i = 1; i < basePoints.Count - 1; i++)
            {
                Vector3 carDir = basePoints[i + 1] - basePoints[i];
                Vector3 sideDir = Vector3.Cross(new Vector3(0, 1, 0), carDir);
                sideDir.Normalize();

                Vector3 outerPoint = basePoints[i] + sideDir * halfTrackWidth;
                Vector3 innerPoint = basePoints[i] - sideDir * halfTrackWidth;

                VertexPositionNormalTexture vertex;
                vertex = new VertexPositionNormalTexture(innerPoint, new Vector3(0, 1, 0), new Vector2(0, distance / textureLenght));
                verticesList.Add(vertex);
                vertex = new VertexPositionNormalTexture(outerPoint, new Vector3(0, 1, 0), new Vector2(1, distance / textureLenght));
                verticesList.Add(vertex);
                distance += carDir.Length();
            }

            VertexPositionNormalTexture extraVert = verticesList[0];
            extraVert.TextureCoordinate.Y = distance / textureLenght;
            verticesList.Add(extraVert);

            extraVert = verticesList[1];
            extraVert.TextureCoordinate.Y = distance / textureLenght;
            verticesList.Add(extraVert);

            return verticesList;
        }

        public void LoadContent(GraphicsDevice device, ContentManager Content)
        {
            basicEffect = new BasicEffect(device, null);
            road = Content.Load<Texture2D>("Images\\road");
            myVertexDeclaration = new VertexDeclaration(device, VertexPositionNormalTexture.VertexElements);
        }

        public void Draw(BasicCamera fpsCam, GraphicsDevice device)
        {
            for (int i = 0; i < RoadLayersCount; i++)
            {
                //render track
                basicEffect.World = Matrix.CreateScale(50) * Matrix.Identity;
                basicEffect.View = fpsCam.viewMatrix;
                basicEffect.Projection = fpsCam.projectionMatrix;

                basicEffect.Texture = road;
                basicEffect.TextureEnabled = true;
                basicEffect.VertexColorEnabled = false;

                basicEffect.Begin();
                foreach (EffectPass pass in basicEffect.CurrentTechnique.Passes)
                {
                    pass.Begin();
                    device.VertexDeclaration = myVertexDeclaration;
                    device.DrawUserPrimitives<VertexPositionNormalTexture>
                        (PrimitiveType.TriangleStrip, textLayersList[i].ToArray(), 0, textLayersList[i].Count - 2);
                    pass.End();
                }
                basicEffect.End();
            }

        }

	}
}
