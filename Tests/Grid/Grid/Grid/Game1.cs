using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Grid
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        BasicEffect effect;
        Matrix view, projection;
        Plane plane;
        Camera camera;
        SpriteFont spriteFont;
        

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            plane = new Plane(10, 1.0f);
            camera = new Camera(new Vector3(7, 4, 15), new Vector3(7, 4, 0), Vector3.Up);

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            effect = new BasicEffect(GraphicsDevice);
            projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4, GraphicsDevice.Viewport.AspectRatio, 0.5f, 1000.0f);
            spriteFont = Content.Load<SpriteFont>("Font");
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            camera.Update(gameTime, gameTime.ElapsedGameTime.Milliseconds);
            view = Matrix.CreateLookAt(camera.getCameraPosition(), camera.getCameraTarget(), camera.getUpVector());
            

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            effect.View = view;
            effect.Projection = projection;
            effect.VertexColorEnabled = true;

            effect.CurrentTechnique.Passes[0].Apply();
            plane.Draw(gameTime, GraphicsDevice);
            spriteBatch.Begin(0, null, null, null, RasterizerState.CullNone, effect);
            for (int i = 0; i < plane.getSideLength(); ++i)
            {
                for (int j = 0; j < plane.getSideLength(); ++j)
                {
                    int tmp = i*plane.getSideLength()+j;
                    Vector3 indexPosition3D = Vector3.Transform(plane.getPlaneHexagons().ElementAt(tmp).get3DPosition(), view);
                    Vector2 indexPosition2D = new Vector2(indexPosition3D.X, indexPosition3D.Y);
                    spriteBatch.DrawString(spriteFont, plane.getPlaneHexagons().ElementAt(tmp).getIndexNumber().ToString(), indexPosition2D, Color.White, 0, Vector2.Zero, 0.10f, 0, indexPosition3D.Z);
                }
            }

            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
