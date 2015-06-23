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
using Underlord.Entity;
using Underlord.Renderer;
using Underlord.Environment;

namespace Underlord
{
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        BasicEffect effect;
        Matrix view, projection;
        Map map;
        Camera camera;
        SpriteFont font;
        int planeLength;
        float hexagonSideLength;
        MouseState mouseState;
        Vector3 mousePosition;
        Vector2 indexOfMiddleHexagon;
        List<Thing> mapObjects;

        float updateTimeCounter, updates, drawUpdates;
        float frameTimeCounter, frames, drawFrame;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = 1366;
            graphics.PreferredBackBufferHeight = 768;
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            hexagonSideLength = 1; //do not change
            planeLength = 10; //need an even number!
            frameTimeCounter = 0;
            frames = 0;
            drawFrame = 0;
            updateTimeCounter = 0;
            updates = 0;
            drawUpdates = 0;
            Vars_Func.loadContent(Content);
            map = new Map(planeLength, Entity.Vars_Func.HexTyp.Sand, true, hexagonSideLength);
            //mapObjects = Logic.Mapgenerator.generateMap();

            //for (int i = 0; i < planeLength; ++i)
            //{
            //    for (int j = 0; j < planeLength; ++j)
            //    {
            //        mapObjects.Add(new Wall(new Vector2(i, j), Vars_Func.TypWall.Stone, 300, map, wall));
            //    }
            //}
            camera = new Camera(new Vector3(0, -10, 15), new Vector3(0, 0, 0), Vector3.UnitZ, GraphicsDevice.Viewport.AspectRatio, 0.5f, 1000.0f, planeLength, hexagonSideLength);
            view = camera.View;
            projection = camera.Projection;
            mouseState = Mouse.GetState();
            mousePosition = Vars_Func.mousepos(GraphicsDevice, mouseState, projection, view);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            effect = new BasicEffect(GraphicsDevice);
            font = Content.Load<SpriteFont>("font");
        }

        protected override void UnloadContent()
        {

        }

        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();
            /*for (int i = 0; i < 2500000; ++i)
            {
                float tmp = 20;
                tmp /= 5;
                tmp += 6;
                tmp *= 11;
                tmp -= 10;
            }*/
            updateTimeCounter += gameTime.ElapsedGameTime.Milliseconds;
            ++updates;
            if (updateTimeCounter >= 1000)
            {
                drawUpdates = updates;
                updates = 0;
                updateTimeCounter -= 1000;
            }
            mouseState = Mouse.GetState();
            mousePosition = Vars_Func.mousepos(GraphicsDevice, mouseState, projection, view);
            camera.Update(gameTime, gameTime.ElapsedGameTime.Milliseconds, mouseState);
            view = camera.View;
            foreach (Hexagon hex in map.getMapHexagons())
            {
                hex.Color = Color.White;
            }
            Vector2 mouseover = Vars_Func.gridColision(mousePosition, planeLength, hexagonSideLength);
            Vector2[] neigbors = map.getHexagonAt(mouseover.X, mouseover.Y).getNeighbors(); //getPlaneHexagons()[(int)(mouseover.X * planeLength + mouseover.Y)].getNeighbors();
            map.getHexagonAt(mouseover.X, mouseover.Y).Color = Color.Blue;// getPlaneHexagons()[(int)(mouseover.X * planeLength + mouseover.Y)].setColor(Color.Brown);
            foreach (Vector2 hex in neigbors)
            {
                map.getHexagonAt(hex.X, hex.Y).Color = Color.Blue;
            }
            indexOfMiddleHexagon = Vars_Func.gridColision(camera.Target, planeLength, hexagonSideLength);
            map.getHexagonAt(indexOfMiddleHexagon.X, indexOfMiddleHexagon.Y).Color = Color.Purple;

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            frameTimeCounter += gameTime.ElapsedGameTime.Milliseconds;
            ++frames;
            if (frameTimeCounter >= 1000)
            {
                drawFrame = frames;
                frames = 0;
                frameTimeCounter -= 1000;
            }
            GraphicsDevice.Clear(Color.Blue);
            effect.View = camera.View;
            effect.Projection = camera.Projection;
            effect.VertexColorEnabled = true;
            effect.CurrentTechnique.Passes[0].Apply();
            map.DrawModel(camera, indexOfMiddleHexagon, camera.Target);
            spriteBatch.Begin();
            spriteBatch.DrawString(font, mouseState.X.ToString() + " : " + mouseState.Y.ToString(), new Vector2(10, 10), Color.White);
            spriteBatch.DrawString(font, mousePosition.X.ToString() + " : " + mousePosition.Y.ToString() + " : " + mousePosition.Z.ToString(), new Vector2(10, 25), Color.White);
            spriteBatch.DrawString(font, "FPS: " + drawFrame.ToString(), new Vector2(10, 40), Color.White);
            spriteBatch.DrawString(font, "UPS: " + drawUpdates.ToString(), new Vector2(10, 55), Color.White);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
