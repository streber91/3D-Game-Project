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
using Underlord.Logic;

// Add temporary Animation
using Underlord.Animation;
using AnimationAux;

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
        float hexagonSideLength;
        KeyboardState keyboard;
        MouseState mouseState;
        Vector3 mousePosition;
        Vector2 indexOfMiddleHexagon;
        Minimap minimap;

        float updateTimeCounter, updates, drawUpdates;
        float frameTimeCounter, frames, drawFrame;

        // Temporary
            /// <summary>
            /// The animated model we are displaying
            /// </summary>
            private AnimationModel impModel = null;

            /// <summary>
            /// This model is loaded solely for the dance animation
            /// </summary>
            private AnimationModel impWalkAnimation = null;
            private AnimationModel impGrableAnimation = null;

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
            mapDrawWidth = 10; //dont go over 15
            hexagonSideLength = 1; //do not change
            planeLength = 50; //need an even number!
            minimapSize = 240; //in pixel
            frameTimeCounter = 0;
            frames = 0;
            drawFrame = 0;
            updateTimeCounter = 0;
            updates = 0;
            drawUpdates = 0;
            Vars_Func.loadContent(Content);
            map = new Map(planeLength, Entity.Vars_Func.HexTyp.Sand, true, hexagonSideLength);
            Logic.Mapgenerator.generateMap(map, planeLength, (int)(planeLength / 10), (int)(planeLength / 5));

            camera = new Camera(new Vector3(0, -10, 15), new Vector3(0, 0, 0), Vector3.UnitZ, GraphicsDevice.Viewport.AspectRatio, 0.5f, 1000.0f, planeLength, hexagonSideLength);
            view = camera.View;
            projection = camera.Projection;
            keyboard = Keyboard.GetState();
            mouseState = Mouse.GetState();
            mousePosition = Vars_Func.mousepos(GraphicsDevice, mouseState, projection, view);
            minimap = new Minimap(map, new Vector2(graphics.PreferredBackBufferWidth - minimapSize, 0), new Vector2(minimapSize, minimapSize));

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            effect = new BasicEffect(GraphicsDevice);
            font = Content.Load<SpriteFont>("font");

            //Temporary
                // Load the model we will display
                impModel = new AnimationModel("AnimationModels//minion_ANI_grabbling_01");
                impModel.LoadContent(Content);

                // Load the model that has an animation clip it in
                impGrableAnimation = new AnimationModel("AnimationModels//minion_ANI_grabbling_01");
                impGrableAnimation.LoadContent(Content);

                impWalkAnimation = new AnimationModel("AnimationModels//minion_ANI_walk_simple_02");
                impWalkAnimation.LoadContent(Content);

                AnimationClip walkClip = impWalkAnimation.Clips[0];

                // And play the clip
                AnimationPlayer player = impModel.PlayClip(walkClip);
                player.Looping = true;
        }

        protected override void UnloadContent()
        {

        }

        protected override void Update(GameTime gameTime)
        {
            // just a test
            //for (int i = 0; i < 3500000; ++i)
            //{
            //    float tmp = 20;
            //    tmp /= 5;
            //    tmp += 6;
            //    tmp *= 11;
            //    tmp -= 10;
            //}

            updateTimeCounter += gameTime.ElapsedGameTime.Milliseconds;
            ++updates;
            if (updateTimeCounter >= 1000)
            {
                drawUpdates = updates;
                updates = 0;
                updateTimeCounter -= 1000;
            }
            keyboard = Keyboard.GetState();
            mouseState = Mouse.GetState();
            mousePosition = Vars_Func.mousepos(GraphicsDevice, mouseState, projection, view);
            camera.Update(gameTime, gameTime.ElapsedGameTime.Milliseconds, mouseState);
            view = camera.View;
            map.update(gameTime, gameTime.ElapsedGameTime.Milliseconds);
            //reset the color of all hexagons
            foreach (Hexagon hex in map.getMapHexagons())
            {
                hex.Color = Color.White;
            }
            switch(Interaction.GameState)
            {
                case Vars_Func.GameState.CreateRoom:
                    foreach (Hexagon hex in map.getMapHexagons())
                    {
                        //colors the room-hexagons in CreateRoom mode
                        if (hex.RoomNumber == 1) hex.Color = Color.Red;
                        else if (hex.RoomNumber == 2) hex.Color = Color.Yellow;
                        else if (hex.RoomNumber == 3) hex.Color = Color.Green;
                        else if (hex.RoomNumber != 0) hex.Color = Color.Blue;
                    }
                    break;
                case Vars_Func.GameState.MergeRooms:
                    foreach (Hexagon hex in map.getMapHexagons())
                    {
                        //colors the room-hexagons in MergeRooms mode
                        if (hex.RoomNumber == 1) hex.Color = Color.Red;
                        else if (hex.RoomNumber == 2) hex.Color = Color.Yellow;
                        else if (hex.RoomNumber == 3) hex.Color = Color.Green;
                        else if (hex.RoomNumber != 0) hex.Color = Color.Blue;
                    }
                    break;
                case Vars_Func.GameState.Build:
                    foreach (Hexagon hex in map.getMapHexagons())
                    {
                        //colors the room-hexagons in Build mode
                        if (hex.RoomNumber == 1) hex.Color = Color.Red;
                        else if (hex.RoomNumber == 2) hex.Color = Color.Yellow;
                        else if (hex.RoomNumber == 3) hex.Color = Color.Green;
                        else if (hex.RoomNumber != 0) hex.Color = Color.Blue;
                    }
                    break;
            }
            indexOfMiddleHexagon = Vars_Func.gridColision(camera.Target, planeLength, hexagonSideLength);
            //Color the Hexagon in the middle of the Screen purple
            //map.getHexagonAt(indexOfMiddleHexagon.X, indexOfMiddleHexagon.Y).Color = Color.Purple;

            Vector2 mouseover = Vars_Func.gridColision(mousePosition, planeLength, hexagonSideLength);
            Interaction.Update(gameTime, map, mouseover, mouseState, keyboard);
           
            // Temporary
                /// Update the knight
                /// 
            

                impModel.Update(gameTime);
            
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
            map.DrawModel(camera, indexOfMiddleHexagon, camera.Target, mapDrawWidth);
            spriteBatch.Begin();
            spriteBatch.DrawString(font, mouseState.X.ToString() + " : " + mouseState.Y.ToString(), new Vector2(10, 10), Color.White);
            spriteBatch.DrawString(font, mousePosition.X.ToString() + " : " + mousePosition.Y.ToString() + " : " + mousePosition.Z.ToString(), new Vector2(10, 25), Color.White);
            spriteBatch.DrawString(font, "FPS: " + drawFrame.ToString(), new Vector2(10, 40), Color.White);
            spriteBatch.DrawString(font, "UPS: " + drawUpdates.ToString(), new Vector2(10, 55), Color.White);

            //Texture2D test = Content.Load<Texture2D>("TEST");
            //Rectangle rec = new Rectangle(0, 0, 1366, 144);
            //Rectangle rec2 = new Rectangle(0, 144, 180, 624);
            //Rectangle rec3 = new Rectangle(180, 648, 1186, 120);
            //spriteBatch.Draw(test, rec, Color.Red);
            //spriteBatch.Draw(test, rec2, Color.Black);
            //spriteBatch.Draw(test, rec3, Color.Green);

            
             //Temporary
                Matrix impMatrix = Matrix.Identity *
                Matrix.CreateScale(0.1f) *
                Matrix.CreateRotationX(MathHelper.PiOver2) *
                Matrix.CreateRotationY(0) *
                Matrix.CreateRotationZ(0) *
                Matrix.CreateTranslation(new Vector3(0, 0, 0.5f));

                /// Draw the knight
                impModel.Draw(camera, impMatrix);
            
            minimap.drawMinimap(spriteBatch);
            
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
