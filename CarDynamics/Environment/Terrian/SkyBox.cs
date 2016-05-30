using System;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using CameraViewer;
using Microsoft.Xna.Framework.Content;

namespace CarDynamics.Terrian
{
    class SkyBox
    {
        // constant definitions
        private const float BOUNDARY = 1600.0f;

        // load and access PositionColor.fx shader
        private Effect positionColorEffect;    // shader object
        private EffectParameter positionColorEffectWVP; // to set display matrix for window

        // load and access Texture.fx shader
        private Effect textureEffect;          // shader object                 
        private EffectParameter textureEffectWVP;       // cumulative matrix w*v*p 
        private EffectParameter textureEffectImage;     // texture parameter

        Texture2D frontTexture, backTexture, groundTexture,
        leftTexture, rightTexture, skyTexture;



        // vertex types and buffers
        private VertexDeclaration positionColor;
        private VertexDeclaration positionColorTexture;

        // ground vertices and texture
        VertexPositionColorTexture[]
            groundVertices = new VertexPositionColorTexture[4];
        private Texture2D grassTexture;



        private const float EDGE = BOUNDARY * 2.0f;
        private VertexPositionColorTexture[] skyVertices = new
                VertexPositionColorTexture[4];

        //MY Variables

        GameWindow Window;
        GraphicsDevice graphics;
        //BasicCamera cam;
        ContentManager Content;

        public SkyBox(ContentManager Content, GraphicsDevice graphic, GameWindow window)//, BasicCamera camera)
        {
            this.graphics = graphic;
            this.Content = Content;
            this.Window = window;
            InitializeBaseCode();
            InitializeGround();
            InitializeSkybox();

        }

        public void LoadContent()
        {
            // load texture
            grassTexture = Content.Load<Texture2D>("Images\\grass");
            frontTexture = Content.Load<Texture2D>("Images\\front");
            backTexture = Content.Load<Texture2D>("Images\\back");
            leftTexture = Content.Load<Texture2D>("Images\\left");
            rightTexture = Content.Load<Texture2D>("Images\\right");
            groundTexture = Content.Load<Texture2D>("Images\\ground2");
            skyTexture = Content.Load<Texture2D>("Images\\sky");
        }


        private void InitializeSkybox()
        {
            Vector3 pos = Vector3.Zero;
            Vector2 uv = Vector2.Zero;
            Color color = Color.White;

            const float MAX = 0.997f; // offset to remove white seam at image edge
            const float MIN = 0.003f; // offset to remove white seam at image edge

            // set position, image, and color data for each vertex in rectangle

            pos.X = +EDGE; pos.Y = -EDGE; uv.X = MIN; uv.Y = MAX; // Bottom R
            skyVertices[0] = new VertexPositionColorTexture(pos, color, uv);

            pos.X = +EDGE; pos.Y = +EDGE; uv.X = MIN; uv.Y = MIN; // Top R
            skyVertices[1] = new VertexPositionColorTexture(pos, color, uv);

            pos.X = -EDGE; pos.Y = -EDGE; uv.X = MAX; uv.Y = MAX; // Bottom L
            skyVertices[2] = new VertexPositionColorTexture(pos, color, uv);

            pos.X = -EDGE; pos.Y = +EDGE; uv.X = MAX; uv.Y = MIN; // Top L
            skyVertices[3] = new VertexPositionColorTexture(pos, color, uv);
        }

        /// <summary>
        /// This method is called when the program begins to set game application
        /// properties such as status bar title and draw mode.  It initializes the  
        /// camera viewer projection, vertex types, and shaders.
        /// </summary>
        private void InitializeBaseCode()
        {
            // set status bar in PC Window (there is none for the Xbox 360)
            //Window.Title = "Microsoft® XNA Game Studio Creator's Guide, Second Edition";

            // see both sides of objects drawn

            graphics.RenderState.CullMode = CullMode.None;



            // initialize vertex types
            positionColor = new VertexDeclaration(graphics,
                                        VertexPositionColor.VertexElements);
            positionColorTexture = new VertexDeclaration(graphics,
                                        VertexPositionColorTexture.VertexElements);

            // load PositionColor.fx and set global params
            positionColorEffect = Content.Load<Effect>("Shaders\\PositionColor");
            positionColorEffectWVP = positionColorEffect.Parameters["wvpMatrix"];

            // load Texture.fx and set global params
            textureEffect = Content.Load<Effect>("Shaders\\Texture");
            textureEffectWVP = textureEffect.Parameters["wvpMatrix"];
            textureEffectImage = textureEffect.Parameters["textureImage"];
        }

        /// <summary>
        /// Set vertices for rectangular surface that is drawn using a triangle strip.
        /// </summary>
        private void InitializeGround()
        {
            const float BORDER = BOUNDARY * 5;
            Vector2 uv = new Vector2(0.0f, 0.0f);
            Vector3 pos = new Vector3(0.0f, 0.0f, 0.0f);
            Color color = Color.White;

            //the height of ground
            float height = -5.0f;
            // top left
            uv.X = 0.0f; uv.Y = 0.0f; pos.X = -BORDER; pos.Y = height; pos.Z = -BORDER;
            groundVertices[0] = new VertexPositionColorTexture(pos, color, uv);

            // bottom left
            uv.X = 0.0f; uv.Y = 1.0f; pos.X = -BORDER; pos.Y = height; pos.Z = BORDER;
            groundVertices[1] = new VertexPositionColorTexture(pos, color, uv);

            // top right
            uv.X = 1.0f; uv.Y = 0.0f; pos.X = BORDER; pos.Y = height; pos.Z = -BORDER;
            groundVertices[2] = new VertexPositionColorTexture(pos, color, uv);

            // bottom right
            uv.X = 1.0f; uv.Y = 1.0f; pos.X = BORDER; pos.Y = height; pos.Z = BORDER;
            groundVertices[3] = new VertexPositionColorTexture(pos, color, uv);
        }


        private void PositionColorShader(PrimitiveType primitiveType,
                                        VertexPositionColor[] vertexData,
                                        int numPrimitives)
        {
            positionColorEffect.Begin(); // begin using PositionColor.fx
            positionColorEffect.Techniques[0].Passes[0].Begin();

            // set drawing format and vertex data then draw primitive surface
            graphics.VertexDeclaration = positionColor;
            graphics.DrawUserPrimitives<VertexPositionColor>(
                                    primitiveType, vertexData, 0, numPrimitives);

            positionColorEffect.Techniques[0].Passes[0].End();
            positionColorEffect.End();  // stop using PositionColor.fx
        }

        /// <summary>
        /// Draws textured primitive objects using Texture.fx shader. 
        /// </summary>
        /// <param name="primitiveType">Object type drawn with vertex data.</param>
        /// <param name="vertexData">Array of vertices.</param>
        /// <param name="numPrimitives">Total primitives drawn.</param>
        private void TextureShader(PrimitiveType primitiveType,
                                   VertexPositionColorTexture[] vertexData,
                                   int numPrimitives)
        {
            textureEffect.Begin(); // begin using Texture.fx
            textureEffect.Techniques[0].Passes[0].Begin();

            // set drawing format and vertex data then draw surface
            graphics.VertexDeclaration = positionColorTexture;
            graphics.DrawUserPrimitives
                                    <VertexPositionColorTexture>(
                                    primitiveType, vertexData, 0, numPrimitives);

            textureEffect.Techniques[0].Passes[0].End();
            textureEffect.End(); // stop using Textured.fx
        }

        /// <summary>
        /// Triggers drawing of ground with texture shader.
        /// </summary>
        private void DrawGround(BasicCamera cam)
        {
            // 1: declare matrices
            Matrix world, translation;

            // 2: initialize matrices
            translation = Matrix.CreateTranslation(0.0f, 0.0f, 0.0f);

            // 3: build cumulative world matrix using I.S.R.O.T. sequence
            // identity, scale, rotate, orbit(translate & rotate), translate
            world = translation;

            // 4: set shader parameters
            textureEffectWVP.SetValue(world * cam.viewMatrix * cam.projectionMatrix);
            textureEffectImage.SetValue(groundTexture);

            // 5: draw object - primitive type, vertex data, # primitives
            TextureShader(PrimitiveType.TriangleStrip, groundVertices, 2);
        }

        private void DrawSkybox(BasicCamera cam)
        {
            const float DROP = -1.2f;

            // 1: declare matrices and set defaults
            Matrix world;
            Matrix rotationY = Matrix.CreateRotationY(0.0f);
            Matrix rotationX = Matrix.CreateRotationX(0.0f);
            Matrix translation = Matrix.CreateTranslation(0.0f, 0.0f, 0.0f);
            Matrix camTranslation // move skybox with camera
                                = Matrix.CreateTranslation(
                                  cam.position.X, cam.position.Y, cam.position.Z);

            // 2: set transformations and also texture for each wall
            for (int i = 0; i < 5; i++)
            {
                switch (i)
                {
                    case 0: // BACK
                        translation = Matrix.CreateTranslation(0.0f, DROP, EDGE);
                        textureEffectImage.SetValue(backTexture);
                        break;
                    case 1: // RIGHT
                        translation = Matrix.CreateTranslation(-EDGE, DROP, 0.0f);
                        rotationY = Matrix.CreateRotationY(-(float)Math.PI / 2.0f);
                        textureEffectImage.SetValue(rightTexture);
                        break;
                    case 2: // FRONT
                        translation = Matrix.CreateTranslation(0.0f, DROP, -EDGE);
                        rotationY = Matrix.CreateRotationY((float)Math.PI);
                        textureEffectImage.SetValue(frontTexture);
                        break;
                    case 3: // LEFT
                        translation = Matrix.CreateTranslation(EDGE, DROP, 0.0f);
                        rotationY = Matrix.CreateRotationY((float)Math.PI / 2.0f);
                        textureEffectImage.SetValue(leftTexture);
                        break;
                    case 4: // SKY
                        translation = Matrix.CreateTranslation(0.0f, EDGE + DROP, 0.0f);
                        rotationX = Matrix.CreateRotationX(-(float)Math.PI / 2.0f);
                        rotationY =
                        Matrix.CreateRotationY(3.0f * MathHelper.Pi / 2.0f);
                        textureEffectImage.SetValue(skyTexture);
                        break;
                }
                // 3: build cumulative world matrix using I.S.R.O.T. sequence
                world = rotationX * rotationY * translation * camTranslation;

                // 4: set shader variables
                textureEffectWVP.SetValue(world * cam.viewMatrix
                                                * cam.projectionMatrix);

                // 5: draw object - primitive type, vertices, # primitives
                TextureShader(PrimitiveType.TriangleStrip, skyVertices, 2);
            }
        }







        public void Draw(BasicCamera cam)
        {
            DrawGround(cam);
            DrawSkybox(cam);
        }
    }
}
