using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;


namespace TerrainNamespace
{
	public struct VertexPositionNormalColor
	{
		public Vector3 Position;
		public Color Color;
		public Vector3 Normal;

		public static int SizeInBytes = 7 * 4;
		public static VertexElement[] VertexElements = new VertexElement[]
              {
                  new VertexElement( 0, 0, VertexElementFormat.Vector3, VertexElementMethod.Default, VertexElementUsage.Position, 0 ),
                  new VertexElement( 0, sizeof(float) * 3, VertexElementFormat.Color, VertexElementMethod.Default, VertexElementUsage.Color, 0 ),
                  new VertexElement( 0, sizeof(float) * 4, VertexElementFormat.Vector3, VertexElementMethod.Default, VertexElementUsage.Normal, 0 ),
              };
	}

	public class Terrain
	{
		GraphicsDeviceManager graphics;
		GraphicsDevice device;

		int terrainWidth;
		int terrainLength;
		float[,] heightData;

		VertexBuffer terrainVertexBuffer;
		IndexBuffer terrainIndexBuffer;
		VertexDeclaration terrainVertexDeclaration;

		Effect effect;

		const float rotationSpeed = 0.3f;
		const float moveSpeed = 30.0f;
		MouseState originalMouseState;

		Texture2D grassTexture;

		ContentManager Content;


		public Terrain(ContentManager content, GraphicsDeviceManager graphics)
		{
			device = graphics.GraphicsDevice;
			this.Content = content;

			effect = Content.Load<Effect>("Shaders\\terrainEffect");

			LoadVertices();

			LoadTextures();
		}

		private void LoadVertices()
		{
			Texture2D heightMap = Content.Load<Texture2D>("Images\\heightMap"); LoadHeightData(heightMap);


			VertexPositionNormalTexture[] terrainVertices = SetUpTerrainVertices();
			int[] terrainIndices = SetUpTerrainIndices();
			terrainVertices = CalculateNormals(terrainVertices, terrainIndices);
			CopyToTerrainBuffers(terrainVertices, terrainIndices);
			terrainVertexDeclaration = new VertexDeclaration(device, VertexPositionNormalTexture.VertexElements);
		}

		private void LoadTextures()
		{

			grassTexture = Content.Load<Texture2D>("Images\\grass");
		}


		private void LoadHeightData(Texture2D heightMap)
		{
			float minimumHeight = float.MaxValue;
			float maximumHeight = float.MinValue;

			terrainWidth = heightMap.Width;
			terrainLength = heightMap.Height;

			Color[] heightMapColors = new Color[terrainWidth * terrainLength];
			heightMap.GetData(heightMapColors);

			heightData = new float[terrainWidth, terrainLength];
			for (int x = 0; x < terrainWidth; x++)
				for (int y = 0; y < terrainLength; y++)
				{
					heightData[x, y] = heightMapColors[x + y * terrainWidth].R;
					if (heightData[x, y] < minimumHeight) minimumHeight = heightData[x, y];
					if (heightData[x, y] > maximumHeight) maximumHeight = heightData[x, y];
				}

			for (int x = 0; x < terrainWidth; x++)
				for (int y = 0; y < terrainLength; y++)
					heightData[x, y] = (heightData[x, y] - minimumHeight) / (maximumHeight - minimumHeight) * 50.0f;
		}

		private VertexPositionNormalTexture[] SetUpTerrainVertices()
		{
			VertexPositionNormalTexture[] terrainVertices = new VertexPositionNormalTexture[terrainWidth * terrainLength];

			for (int x = 0; x < terrainWidth; x++)
			{
				for (int y = 0; y < terrainLength; y++)
				{
                    float value = 1000.0f;
					terrainVertices[x + y * terrainWidth].Position = new Vector3(x*value - 1000, heightData[x, y] - 15.0f, -y*value + 100000);
					terrainVertices[x + y * terrainWidth].TextureCoordinate.X = (float)x / 30.0f;
					terrainVertices[x + y * terrainWidth].TextureCoordinate.Y = (float)y / 30.0f;
				}
			}

			return terrainVertices;
		}

		private int[] SetUpTerrainIndices()
		{
			int[] indices = new int[(terrainWidth - 1) * (terrainLength - 1) * 6];
			int counter = 0;
			for (int y = 0; y < terrainLength - 1; y++)
			{
				for (int x = 0; x < terrainWidth - 1; x++)
				{
					int lowerLeft = x + y * terrainWidth;
					int lowerRight = (x + 1) + y * terrainWidth;
					int topLeft = x + (y + 1) * terrainWidth;
					int topRight = (x + 1) + (y + 1) * terrainWidth;

					indices[counter++] = topLeft;
					indices[counter++] = lowerRight;
					indices[counter++] = lowerLeft;

					indices[counter++] = topLeft;
					indices[counter++] = topRight;
					indices[counter++] = lowerRight;
				}
			}

			return indices;
		}

		private VertexPositionNormalTexture[] CalculateNormals(VertexPositionNormalTexture[] vertices, int[] indices)
		{
			for (int i = 0; i < vertices.Length; i++)
				vertices[i].Normal = new Vector3(0, 0, 0);

			for (int i = 0; i < indices.Length / 3; i++)
			{
				int index1 = indices[i * 3];
				int index2 = indices[i * 3 + 1];
				int index3 = indices[i * 3 + 2];

				Vector3 side1 = vertices[index1].Position - vertices[index3].Position;
				Vector3 side2 = vertices[index1].Position - vertices[index2].Position;
				Vector3 normal = Vector3.Cross(side1, side2);

				vertices[index1].Normal += normal;
				vertices[index2].Normal += normal;
				vertices[index3].Normal += normal;
			}

			for (int i = 0; i < vertices.Length; i++)
				vertices[i].Normal.Normalize();

			return vertices;
		}

		private void CopyToTerrainBuffers(VertexPositionNormalTexture[] vertices, int[] indices)
		{
			terrainVertexBuffer = new VertexBuffer(device, vertices.Length * VertexPositionNormalTexture.SizeInBytes, BufferUsage.WriteOnly);
			terrainVertexBuffer.SetData(vertices);

			terrainIndexBuffer = new IndexBuffer(device, typeof(int), indices.Length, BufferUsage.WriteOnly);
			terrainIndexBuffer.SetData(indices);
		}
		public void Draw(Matrix viewMatrix, Matrix projectionMatrix)
		{
			effect.CurrentTechnique = effect.Techniques["Textured"];
			effect.Parameters["xTexture"].SetValue(grassTexture);

            Matrix worldMatrix = Matrix.Identity * Matrix.CreateScale(20) ;
			effect.Parameters["xWorld"].SetValue(worldMatrix);
            effect.Parameters["xView"].SetValue(viewMatrix);
			effect.Parameters["xProjection"].SetValue(projectionMatrix);

			effect.Parameters["xEnableLighting"].SetValue(true);
			effect.Parameters["xAmbient"].SetValue(0.4f);
			effect.Parameters["xLightDirection"].SetValue(new Vector3(-0.5f, -1, -0.5f));

			effect.Begin();
			foreach (EffectPass pass in effect.CurrentTechnique.Passes)
			{
				pass.Begin();

				device.Vertices[0].SetSource(terrainVertexBuffer, 0, VertexPositionNormalTexture.SizeInBytes);
				device.Indices = terrainIndexBuffer;
				device.VertexDeclaration = terrainVertexDeclaration;

				int noVertices = terrainVertexBuffer.SizeInBytes / VertexPositionNormalTexture.SizeInBytes;
				int noTriangles = terrainIndexBuffer.SizeInBytes / sizeof(int) / 3;
				device.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, noVertices, 0, noTriangles);

				pass.End();
			}
			effect.End();
		}
	}
}
