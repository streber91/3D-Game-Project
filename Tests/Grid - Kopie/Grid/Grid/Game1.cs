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
        MouseState mousestate;
        SpriteFont font;
        Vector3 mouseposition;
        float hexagonsidelength;
        int planelength;
        Vector2 indexOfMiddleHexagon;

        Model dummyWall;


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
            hexagonsidelength = 1;
            planelength = 10; //need an even number!

            dummyWall = Content.Load<Model>("Models//dummyWall_HEX_01");
            plane = new Plane(planelength, hexagonsidelength, dummyWall);
            camera = new Camera(new Vector3(0, 0, 15), new Vector3(0, 0, 0), Vector3.Up, GraphicsDevice.Viewport.AspectRatio, 0.5f, 1000.0f, planelength, hexagonsidelength);
            IsMouseVisible = true;
            mousestate = Mouse.GetState();

            view = camera.View;
            projection = camera.Projection; 
            
            
            mouseposition = GraphicsDevice.Viewport.Unproject(new Vector3(mousestate.X, mousestate.Y, 0), projection, view, Matrix.Identity);

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
            font = Content.Load<SpriteFont>("testfont");

            //dummyWall = Content.Load<Model>("Models//dummyWall_HEX_01");

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

            mousestate = Mouse.GetState();
            mouseposition = mousepos();
            camera.Update(gameTime, gameTime.ElapsedGameTime.Milliseconds, mousestate);
            view = camera.View;

            foreach (Hexagon hex in plane.getPlaneHexagons())
            {
                hex.setColor(hex.getStdColor());
            }
            
            Vector2 mouseover = gridColision(mouseposition);
            Vector2[] neigbors = plane.getPlaneHexagons()[(int)(mouseover.X * planelength + mouseover.Y)].getNeighbors();

            plane.getPlaneHexagons()[(int)(mouseover.X * planelength + mouseover.Y)].setColor(Color.Brown);
            foreach (Vector2 hex in neigbors)
            {
                plane.getPlaneHexagons()[(int)(hex.X * planelength + hex.Y)].setColor(Color.Brown);
            }

            indexOfMiddleHexagon = gridColision(camera.getCameraTarget());
            plane.getPlaneHexagons()[(int)(indexOfMiddleHexagon.X * planelength + indexOfMiddleHexagon.Y)].setColor(Color.Purple);

            base.Update(gameTime);
        }
        
        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Blue);
            effect.View = camera.View;
            effect.Projection = camera.Projection;
            effect.VertexColorEnabled = true;

            

            effect.CurrentTechnique.Passes[0].Apply();
            //plane.Draw(GraphicsDevice, indexOfMiddleHexagon, camera.getCameraTarget());

            plane.DrawModel(camera, indexOfMiddleHexagon, camera.getCameraTarget());


            spriteBatch.Begin();

            spriteBatch.DrawString(font, mousestate.X.ToString() + " : " + mousestate.Y.ToString(), new Vector2(10, 10), Color.White);
            spriteBatch.DrawString(font, mouseposition.X.ToString() + " : " + mouseposition.Y.ToString() + " : " + mouseposition.Z.ToString(), new Vector2(10, 25), Color.White);

            spriteBatch.End();

            base.Draw(gameTime);
        }

        private Vector3 mousepos()
        {
            Vector3 vec1 = GraphicsDevice.Viewport.Unproject(new Vector3(mousestate.X, mousestate.Y, 1), projection, view, Matrix.Identity);
            Vector3 vec2 = GraphicsDevice.Viewport.Unproject(new Vector3(mousestate.X, mousestate.Y, 0), projection, view, Matrix.Identity);
            float a = -vec1.Z / (vec2.Z - vec1.Z);
            Vector3 mousepos = new Vector3(a * (vec2.X - vec1.X) + vec1.X, a * (vec2.Y - vec1.Y) + vec1.Y, 0.0f);

            return mousepos;
        }

        private Vector2 gridColision(Vector3 mouse)
        {
            float mouseX = mouse.X;
            float mouseY = mouse.Y;
            int X = 0;
            int Y = 0;
            while (mouseX < 0) mouseX = mouseX + (planelength * 1.5f * hexagonsidelength);
            while (mouseY < 0) mouseY = mouseY + (planelength * 2 * 0.875f * hexagonsidelength);
            mouseX = mouseX % (planelength * 1.5f * hexagonsidelength);
            mouseY = mouseY % (planelength * 2 * 0.875f * hexagonsidelength);

            while (mouseX >= 1.5f * hexagonsidelength)
            {
                mouseX -= 1.5f;
                ++X;
            }
            while (mouseY >= 0.875f * hexagonsidelength)
            {
                mouseY -= 0.875f;
                ++Y;
            }

            if (mouseX <= hexagonsidelength / 2)
            {
                if ((X + Y) % 2 == 0)
                {
                    if (0.875 >= mouseX * 1.75 + mouseY) --X;
                    if (X < 0) X += planelength;
                }
                else
                {
                    if (0 >= mouseX * 1.75 - mouseY) --X;
                    if (X < 0) X += planelength;
                }
            }
            if (X % 2 != 0)
            {
                --Y;
                if (Y < 0) Y += planelength * 2;
            }

            return new Vector2(X, (int)(Y / 2));
        }
    }
}
