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
using Underlord.Logic;
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
        int planeLength, minimapSize, mapDrawWidth;
        float hexagonSideLength, interWaveTime;
        KeyboardState keyboard;
        MouseState mouseState;
        MouseState lastMouseState;
        Vector3 mousePosition;
        Vector2 indexOfMiddleHexagon;
        Minimap minimap;
        WaveController wavecontroller;

        Vector3 temp;
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
            hexagonSideLength = 1; //dont change
            mapDrawWidth = 10; //dont go over 15
            planeLength = 50; //need an even number!
            minimapSize = 5 * planeLength; //in pixel
            frameTimeCounter = 0;
            frames = 0;
            drawFrame = 0;
            updateTimeCounter = 0;
            updates = 0;
            drawUpdates = 0;
            interWaveTime = 60000 * 2.0f; //in ms

            Vars_Func.loadContent(Content);
            map = new Map(planeLength, Logic.Vars_Func.HexTyp.Sand, true, hexagonSideLength);
            Mapgenerator.generateMap(map, planeLength, (int)(planeLength / 10), (int)(planeLength / 5));
            wavecontroller = new WaveController(interWaveTime);

            //cameraposition HQ.X * 1.5, HQ.Y * 1.75
            camera = new Camera(new Vector3((map.HQPosition.X * 1.5f) + 1, (map.HQPosition.Y * 1.75f) - 10 + 0.875f, 11), new Vector3((map.HQPosition.X * 1.5f) + 1, (map.HQPosition.Y * 1.75f) + 0.875f, 0),
                                Vector3.UnitZ, GraphicsDevice.Viewport.AspectRatio, 0.5f, 1000.0f, planeLength, hexagonSideLength);
            view = camera.View;
            projection = camera.Projection;
            keyboard = Keyboard.GetState();
            mouseState = Mouse.GetState();
            mousePosition = Vars_Func.mousepos(GraphicsDevice, mouseState, projection, view);
            //minimap = new Minimap(map, new Vector2(graphics.PreferredBackBufferWidth - minimapSize, 0), new Vector2(minimapSize, minimapSize));
            GUI.createGUI();
            System.Diagnostics.Debug.WriteLine("HQ " + map.HQPosition);

            //camera.Position = new Vector3(1.6f*map.HQPosition.X, 1.45f*map.HQPosition.Y, camera.Position.Z);

            base.Initialize();
            Interaction.Game = this;
        }

        public void reinitialize()
        {
            GUI.SelectedThingTyp = Vars_Func.ThingTyp.length;
            hexagonSideLength = 1; //dont change
            mapDrawWidth = 10; //dont go over 15
            planeLength = 50; //need an even number!
            minimapSize = 5 * planeLength; //in pixel
            frameTimeCounter = 0;
            frames = 0;
            drawFrame = 0;
            updateTimeCounter = 0;
            updates = 0;
            drawUpdates = 0;
            interWaveTime = 60000 * 2.0f; //in ms

            map = new Map(planeLength, Logic.Vars_Func.HexTyp.Sand, true, hexagonSideLength);
            Mapgenerator.generateMap(map, planeLength, (int)(planeLength / 10), (int)(planeLength / 5));
            wavecontroller = new WaveController(interWaveTime);

            camera = new Camera(new Vector3((map.HQPosition.X * 1.5f) + 1, (map.HQPosition.Y * 1.75f) - 10 + 0.875f, 11), new Vector3((map.HQPosition.X * 1.5f) + 1, (map.HQPosition.Y * 1.75f) + 0.875f, 0),
                                Vector3.UnitZ, GraphicsDevice.Viewport.AspectRatio, 0.5f, 1000.0f, planeLength, hexagonSideLength);
            view = camera.View;
            projection = camera.Projection;
            keyboard = Keyboard.GetState();
            mouseState = Mouse.GetState();
            mousePosition = Vars_Func.mousepos(GraphicsDevice, mouseState, projection, view);
            minimap = new Minimap(map, new Vector2(graphics.PreferredBackBufferWidth - minimapSize, 2), new Vector2(minimapSize, minimapSize));
            System.Diagnostics.Debug.WriteLine("HQ " + map.HQPosition);

            base.Initialize();
            Interaction.Game = this;
            GC.Collect();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            effect = new BasicEffect(GraphicsDevice);
            font = Content.Load<SpriteFont>("Fonts/Augusta");
        }

        protected override void UnloadContent()
        {

        }

        protected override void Update(GameTime gameTime)
        {
            if (temp != map.HQDrawPositon)
            {
                //System.Diagnostics.Debug.WriteLine("HQ " + map.HQDrawPositon);
                temp = map.HQDrawPositon;
            }
            // updates per second
            updateTimeCounter += gameTime.ElapsedGameTime.Milliseconds;
            ++updates;
            if (updateTimeCounter >= 1000)
            {
                drawUpdates = updates;
                updates = 0;
                updateTimeCounter -= 1000;
            }

            //// DON'T TOUCH THIS////
            keyboard = Keyboard.GetState();
            lastMouseState = mouseState;
            mouseState = Mouse.GetState();
            mousePosition = Vars_Func.mousepos(GraphicsDevice, mouseState, projection, view);
            camera.Update(gameTime, gameTime.ElapsedGameTime.Milliseconds, mouseState);
            view = camera.View;
            map.update(gameTime, gameTime.ElapsedGameTime.Milliseconds);
            //// DON'T TOUCH THIS////


            indexOfMiddleHexagon = Vars_Func.gridColision(camera.Target, planeLength, hexagonSideLength);
            Vector2 mouseover = Vars_Func.gridColision(mousePosition, planeLength, hexagonSideLength);

            Vars_Func.resetHexagonColors(map);

            wavecontroller.update(gameTime, map);

            Interaction.Update(gameTime, map, mouseover, mouseState, lastMouseState, keyboard);
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            //Frames per second
            frameTimeCounter += gameTime.ElapsedGameTime.Milliseconds;
            ++frames;
            if (frameTimeCounter >= 1000)
            {
                drawFrame = frames;
                frames = 0;
                frameTimeCounter -= 1000;
            }

            GraphicsDevice.Clear(Color.CornflowerBlue);
            effect.View = camera.View;
            effect.Projection = camera.Projection;
            effect.VertexColorEnabled = true;
            effect.CurrentTechnique.Passes[0].Apply();
            map.DrawModel(camera, indexOfMiddleHexagon, camera.Target, mapDrawWidth);


            spriteBatch.Begin();

            if (Interaction.GameState != Vars_Func.GameState.MainMenu &&
                Interaction.GameState != Vars_Func.GameState.Highscore &&
                Interaction.GameState != Vars_Func.GameState.Tutorial)
            {
                minimap.drawMinimap(spriteBatch, indexOfMiddleHexagon);
            }
            GUI.Draw(spriteBatch, font, mouseState);
            spriteBatch.DrawString(font, mouseState.X.ToString() + " : " + mouseState.Y.ToString(), new Vector2(20, 200), Color.Black);
            spriteBatch.DrawString(font, mousePosition.X.ToString() + " : " + mousePosition.Y.ToString() + " : " + mousePosition.Z.ToString(), new Vector2(20, 220), Color.Black);
            spriteBatch.DrawString(font, "FPS: " + drawFrame.ToString(), new Vector2(20, 240), Color.Black);
            spriteBatch.DrawString(font, "UPS: " + drawUpdates.ToString(), new Vector2(20, 260), Color.Black);

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
