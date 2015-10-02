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
        Vars_Func.GameState gamestate;
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
        MouseState lastMouseState;
        Vector3 mousePosition;
        Vector2 indexOfMiddleHexagon;
        Minimap minimap;

        bool showIngameMenu, buttonIsPressed, reinitializeDone;

        float updateTimeCounter, updates, drawUpdates;
        float frameTimeCounter, frames, drawFrame;
        float timeCounter;
        float minDelayTime;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = 1366;
            graphics.PreferredBackBufferHeight = 768;
            Content.RootDirectory = "Content";
            //IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            gamestate = Vars_Func.GameState.StartMenu;
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
            showIngameMenu = false;
            buttonIsPressed = false;
            reinitializeDone = false;
            timeCounter = 0.0f;
            minDelayTime = 100.0f;

            Vars_Func.loadContent(Content);
            map = new Map(planeLength, Logic.Vars_Func.HexTyp.Sand, true, hexagonSideLength);
            Mapgenerator.generateMap(map, planeLength, (int)(planeLength / 10), (int)(planeLength / 5), false);

            //cameraposition HQ.X * 1.5, HQ.Y * 1.75
            camera = new Camera(new Vector3((map.HQPosition.X * 1.5f) + 1, (map.HQPosition.Y * 1.75f) - 10 + 0.875f, 11), new Vector3((map.HQPosition.X * 1.5f) + 1, (map.HQPosition.Y * 1.75f) + 0.875f, 0),
                                Vector3.UnitZ, GraphicsDevice.Viewport.AspectRatio, 0.5f, 1000.0f, planeLength, hexagonSideLength);
            view = camera.View;
            projection = camera.Projection;
            keyboard = Keyboard.GetState();
            mouseState = Mouse.GetState();
            mousePosition = Vars_Func.mousepos(GraphicsDevice, mouseState, projection, view);
            
            
            minimap = new Minimap(map, new Vector2(graphics.PreferredBackBufferWidth - minimapSize, 0), new Vector2(minimapSize, minimapSize));
            BasicGUI.createGUI();
            StartMenu_GUI.createGUI();
            MainMenu_GUI.createGUI();
            Highscore_GUI.createGUI();
            Setting_GUI.createGUI();
            IngameMenu_GUI.createGUI();
            Confirm_GUI.createGUI();
            GameOver_GUI.createGUI();
            IntroMenu_GUI.createGUI();
            //InsertNameGUI.createGUI();

            base.Initialize();
            Interaction.Game = this;
            reinitializeDone = true;
        }

        public void reinitialize()
        {
            Player.Gold = 1000;
            Player.Mana = 2000;
            Player.Food = 200;
            Player.Score = 0;
            Spells.SummonImpCost = 0;
            BasicGUI.SelectedThingTyp = Vars_Func.ThingTyp.length;
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
            timeCounter = 0.0f;
            WaveController.restart();

            map = new Map(planeLength, Logic.Vars_Func.HexTyp.Sand, true, hexagonSideLength);
            Mapgenerator.generateMap(map, planeLength, (int)(planeLength / 10), (int)(planeLength / 5), false);

            camera = new Camera(new Vector3((map.HQPosition.X * 1.5f) + 1, (map.HQPosition.Y * 1.75f) - 10 + 0.875f, 11), new Vector3((map.HQPosition.X * 1.5f) + 1, (map.HQPosition.Y * 1.75f) + 0.875f, 0),
                                Vector3.UnitZ, GraphicsDevice.Viewport.AspectRatio, 0.5f, 1000.0f, planeLength, hexagonSideLength);
            view = camera.View;
            projection = camera.Projection;
            keyboard = Keyboard.GetState();
            mouseState = Mouse.GetState();
            mousePosition = Vars_Func.mousepos(GraphicsDevice, mouseState, projection, view);
            minimap = new Minimap(map, new Vector2(graphics.PreferredBackBufferWidth - minimapSize, 0), new Vector2(minimapSize, minimapSize));
            

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
            timeCounter += gameTime.ElapsedGameTime.Milliseconds;

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
            //mousePosition = Vars_Func.mousepos(GraphicsDevice, mouseState, projection, view);
            //camera.Update(gameTime, gameTime.ElapsedGameTime.Milliseconds, mouseState);
            //view = camera.View;
            //map.update(gameTime, gameTime.ElapsedGameTime.Milliseconds);
            ////// DON'T TOUCH THIS////
            
            //indexOfMiddleHexagon = Vars_Func.gridColision(camera.Target, planeLength, hexagonSideLength);
            //Vector2 mouseover = Vars_Func.gridColision(mousePosition, planeLength, hexagonSideLength);

            //Vars_Func.resetHexagonColors(map);

            //wavecontroller.update(gameTime, map);

            //Interaction.Update(gameTime, map, mouseover, mouseState, lastMouseState, keyboard);
            //GUI.update(gameTime, map, mouseState);
            if (Interaction.GameState == Vars_Func.GameState.GameOver)
            {
                gamestate = Vars_Func.GameState.GameOver;
            }

            switch (gamestate)
            {
                #region Startmenu
                case Vars_Func.GameState.StartMenu:
                    StartMenu_GUI.update(gameTime, mouseState, keyboard);
                    if (timeCounter > minDelayTime)
                    {
                        if (StartMenu_GUI.getGUI_Button() != null && StartMenu_GUI.getGUI_Button().Typ == Vars_Func.GUI_Typ.StartButton)
                        {
                            gamestate = Vars_Func.GameState.MainMenu;
                            timeCounter = 0;
                        }
                    }
                    break;
                #endregion
                #region Mainmenu
                case Vars_Func.GameState.MainMenu:
                    MainMenu_GUI.update(gameTime, mouseState, keyboard);
                    Highscore_GUI.restGUI();
                    Setting_GUI.restGUI();
                    GameOver_GUI.restGUI();
                    IntroMenu_GUI.restGUI();
                    showIngameMenu = false;

                    if (MainMenu_GUI.getGUI_Button() != null)
                    {
                        if (timeCounter > minDelayTime)
                        {
                            switch (MainMenu_GUI.getGUI_Button().Typ)
                            {
                                case Vars_Func.GUI_Typ.NewGameButton:
                                    reinitialize();
                                    //gamestate = Vars_Func.GameState.Ingame;
                                    gamestate = Vars_Func.GameState.Intro;
                                    timeCounter = 0;
                                    //MainMenu_GUI.restGUI();
                                    break;
                                case Vars_Func.GUI_Typ.SettingsButton:
                                    gamestate = Vars_Func.GameState.Settings;
                                    timeCounter = 0;
                                    break;
                                case Vars_Func.GUI_Typ.HighScoreButton:
                                    gamestate = Vars_Func.GameState.Highscore;
                                    timeCounter = 0;
                                    break;
                                case Vars_Func.GUI_Typ.QuitButton:
                                    timeCounter = 0;
                                    this.Exit();
                                    break;
                                default: break;
                            }
                        }
                    }
                    break;
                #endregion
                #region Ingame
                case Vars_Func.GameState.Ingame:
                    MainMenu_GUI.restGUI();
                    Setting_GUI.restGUI();
                    Highscore_GUI.restGUI();
                    Confirm_GUI.restGUI();
                    GameOver_GUI.restGUI();

                    IntroMenu_GUI.restGUI();
                        if (keyboard.IsKeyDown(Keys.Tab) && !buttonIsPressed)
                        {
                            buttonIsPressed = true;
                            showIngameMenu = !showIngameMenu;
                        }
                        if (!keyboard.IsKeyDown(Keys.Tab) && buttonIsPressed)
                        {
                            buttonIsPressed = false;
                        }

                    // Add ingame menu
                    if (showIngameMenu)
                    {
                        IngameMenu_GUI.update(gameTime, mouseState, keyboard);
                        if (IngameMenu_GUI.getGUI_Button() != null)
                        {
                            switch (IngameMenu_GUI.getGUI_Button().Typ)
                            {
                                case Vars_Func.GUI_Typ.NewGameButton:
                                    showIngameMenu = false;
                                    IngameMenu_GUI.restGUI();
                                    break;
                                case Vars_Func.GUI_Typ.SettingsButton:
                                    gamestate = Vars_Func.GameState.Settings;
                                    break;
                                case Vars_Func.GUI_Typ.HighScoreButton:
                                    gamestate = Vars_Func.GameState.Highscore;
                                    break;
                                case Vars_Func.GUI_Typ.StartButton:
                                    //Player.saveScore();
                                    //reinitialize();
                                    gamestate = Vars_Func.GameState.Confirm;
                                    break;
                                default: break;
                            }
                        }
                    }
                    else
                    {
                        mousePosition = Vars_Func.mousepos(GraphicsDevice, mouseState, projection, view);
                        camera.Update(gameTime, gameTime.ElapsedGameTime.Milliseconds, mouseState);
                        view = camera.View;

                        map.update(gameTime, gameTime.ElapsedGameTime.Milliseconds, this);
                        //// DON'T TOUCH THIS////
                        indexOfMiddleHexagon = Vars_Func.gridColision(camera.Target, planeLength, hexagonSideLength);
                        Vector2 mouseover = Vars_Func.gridColision(mousePosition, planeLength, hexagonSideLength);
                        
                        Vars_Func.resetHexagonColors(map);

                        WaveController.update(gameTime, map);
                        BasicGUI.update(gameTime, map, mouseState);
                        Interaction.Update(gameTime, map, mouseover, mouseState, lastMouseState, keyboard);
                    }
                    break;
                #endregion
                #region Highscore
                case Vars_Func.GameState.Highscore:
                    Highscore_GUI.update(gameTime, mouseState, keyboard);
                    if (showIngameMenu)
                    {
                        IngameMenu_GUI.restGUI();
                        if (timeCounter > minDelayTime)
                        {
                            if (Highscore_GUI.getGUI_Button() != null && Highscore_GUI.getGUI_Button().Typ == Vars_Func.GUI_Typ.StartButton)
                            {
                                gamestate = Vars_Func.GameState.Ingame;
                                timeCounter = 0;
                            }
                        }
                    }
                    else
                    {
                        MainMenu_GUI.restGUI();
                        if (timeCounter > minDelayTime)
                        {
                            if (Highscore_GUI.getGUI_Button() != null && Highscore_GUI.getGUI_Button().Typ == Vars_Func.GUI_Typ.StartButton)
                            {
                                gamestate = Vars_Func.GameState.MainMenu;
                                timeCounter = 0;
                            }
                        }
                    }
                    break;
                #endregion
                #region Settings
                case Vars_Func.GameState.Settings:
                    Setting_GUI.update(gameTime, mouseState, lastMouseState, keyboard);
                    if (showIngameMenu)
                    {
                        IngameMenu_GUI.restGUI();
                        if (timeCounter > minDelayTime)
                        {
                            if (Setting_GUI.getGUI_Button() != null && Setting_GUI.getGUI_Button().Typ == Vars_Func.GUI_Typ.StartButton)
                            {
                                gamestate = Vars_Func.GameState.Ingame;
                                timeCounter = 0;
                            }
                        }
                    }
                    else
                    {
                        MainMenu_GUI.restGUI();
                        if (timeCounter > minDelayTime)
                        {
                            if (Setting_GUI.getGUI_Button() != null && Setting_GUI.getGUI_Button().Typ == Vars_Func.GUI_Typ.StartButton)
                            {
                                gamestate = Vars_Func.GameState.MainMenu;
                                timeCounter = 0;
                            }
                        }
                    }

                    break;
                #endregion
                #region Confirm
                case Vars_Func.GameState.Confirm:
                    Confirm_GUI.update(gameTime, mouseState, keyboard);
                    if (timeCounter > minDelayTime)
                    {
                        if (Confirm_GUI.getGUI_Button() != null && Confirm_GUI.getGUI_Button().Typ == Vars_Func.GUI_Typ.NewGameButton)
                        {
                            showIngameMenu = false;
                            IngameMenu_GUI.restGUI();
                            gamestate = Vars_Func.GameState.Ingame;
                            //showIngameMenu = false;
                            //IngameMenu_GUI.restGUI();
                            //gamestate = Vars_Func.GameState.GameOver;
                            timeCounter = 0;
                        }
                        if (Confirm_GUI.getGUI_Button() != null && Confirm_GUI.getGUI_Button().Typ == Vars_Func.GUI_Typ.QuitButton)
                        {
                            Player.saveScore();
                            reinitialize();
                            IntroMenu_GUI.createGUI();
                            MainMenu_GUI.createGUI();
                            Highscore_GUI.createGUI();
                            gamestate = Vars_Func.GameState.MainMenu;
                            //gamestate = Vars_Func.GameState.InsertNameConfirm;
                            //showIngameMenu = false;
                            //IngameMenu_GUI.restGUI();
                            //gamestate = Vars_Func.GameState.Ingame;
                            timeCounter = 0;
                        }
                    }
                    break;
                #endregion
                #region GameOver
                case Vars_Func.GameState.GameOver:
                    GameOver_GUI.update(gameTime, mouseState, keyboard);
                    if (timeCounter > minDelayTime)
                    {
                        if (GameOver_GUI.getGUI_Button() != null && GameOver_GUI.getGUI_Button().Typ == Vars_Func.GUI_Typ.NewGameButton)
                        {
                            showIngameMenu = false;
                            Player.saveScore();
                            reinitialize();
                            MainMenu_GUI.createGUI();
                            IngameMenu_GUI.restGUI();
                            Highscore_GUI.createGUI();
                            gamestate = Vars_Func.GameState.Ingame;
                            //gamestate = Vars_Func.GameState.InserNameReset;
                            //timeCounter = 0;
                        }

                        if (GameOver_GUI.getGUI_Button() != null && GameOver_GUI.getGUI_Button().Typ == Vars_Func.GUI_Typ.QuitButton)
                        {
                            Player.saveScore();
                            this.Exit();
                            //gamestate = Vars_Func.GameState.InserNameQuit;
                            //timeCounter = 0;
                        }
                    }
                    break;
                #endregion
                #region Intro
                case Vars_Func.GameState.Intro:
                    IntroMenu_GUI.update(gameTime, mouseState, keyboard);
                    if (timeCounter > minDelayTime)
                    {
                        if (IntroMenu_GUI.getGUI_Button() != null && IntroMenu_GUI.getGUI_Button().Typ == Vars_Func.GUI_Typ.NewGameButton)
                        {
                            MainMenu_GUI.restGUI();
                            reinitialize();
                            Setting_GUI.ShowHelp = true;
                            gamestate = Vars_Func.GameState.Ingame;
                            timeCounter = 0;

                        }
                        if (IntroMenu_GUI.getGUI_Button() != null && IntroMenu_GUI.getGUI_Button().Typ == Vars_Func.GUI_Typ.QuitButton)
                        {
                            MainMenu_GUI.restGUI();
                            reinitialize();
                            Setting_GUI.ShowHelp = false;
                            gamestate = Vars_Func.GameState.Ingame;
                            timeCounter = 0;

                        }
                    }
                    break;
                #endregion
                #region Insert Name After Confirm
                case Vars_Func.GameState.InsertNameConfirm:
                    InsertNameGUI.update(gameTime, mouseState, keyboard);
                    if (timeCounter > minDelayTime)
                    {
                        if (InsertNameGUI.getGUI_Button() != null && InsertNameGUI.getGUI_Button().Typ == Vars_Func.GUI_Typ.QuitButton)
                        {
                            //Retrun the player name 
                            string PlayerName = InsertNameGUI.getPlayerName();

                            //TODO: pass on to the Player class

                            Player.saveScore();
                            reinitialize();
                            IntroMenu_GUI.createGUI();
                            MainMenu_GUI.createGUI();
                            gamestate = Vars_Func.GameState.MainMenu;
                            timeCounter = 0;
                        }
                    }
                    break;
                #endregion
                #region Insert Name Before Reset
                case Vars_Func.GameState.InserNameReset:
                    InsertNameGUI.update(gameTime, mouseState, keyboard);
                    if (timeCounter > minDelayTime)
                    {
                        if (InsertNameGUI.getGUI_Button() != null && InsertNameGUI.getGUI_Button().Typ == Vars_Func.GUI_Typ.QuitButton)
                        {
                            //Retrun the player name 
                            string PlayerName = InsertNameGUI.getPlayerName();

                            //TODO: pass on to the Player class

                            showIngameMenu = false;
                            Player.saveScore();
                            reinitialize();
                            MainMenu_GUI.createGUI();
                            IngameMenu_GUI.restGUI();
                            gamestate = Vars_Func.GameState.Ingame;
                            timeCounter = 0;
                        }
                    }
                    break;
                #endregion
                #region Insert Name Before Quit
                case Vars_Func.GameState.InserNameQuit:
                    InsertNameGUI.update(gameTime, mouseState, keyboard);
                    if (timeCounter > minDelayTime)
                    {
                        if (InsertNameGUI.getGUI_Button() != null && InsertNameGUI.getGUI_Button().Typ == Vars_Func.GUI_Typ.QuitButton)
                        {
                            timeCounter = 0;
                            //Retrun the player name 
                            string PlayerName = InsertNameGUI.getPlayerName();

                            //TODO: pass on to the Player class

                            Player.saveScore();
                            this.Exit();

                        }
                    }
                    break;
                #endregion
                default:
                    break;
            }
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

            GraphicsDevice.Clear(Color.Black);
            effect.View = camera.View;
            effect.Projection = camera.Projection;
            effect.VertexColorEnabled = true;
            effect.CurrentTechnique.Passes[0].Apply();
            map.DrawModel(camera, indexOfMiddleHexagon, camera.Target, mapDrawWidth);

            spriteBatch.Begin();

            //if (Interaction.GameState != Vars_Func.GameState.MainMenu &&
            //    Interaction.GameState != Vars_Func.GameState.Highscore &&
            //    Interaction.GameState != Vars_Func.GameState.Tutorial)
            //{
            //    minimap.drawMinimap(spriteBatch, indexOfMiddleHexagon);
            //}
            //GUI.Draw(spriteBatch, font, mouseState, camera, map);
            //GUI.Draw(spriteBatch, font, minimap, indexOfMiddleHexagon);
            ////spriteBatch.DrawString(font, mouseState.X.ToString() + " : " + mouseState.Y.ToString(), new Vector2(20, 200), Color.Black);
            ////spriteBatch.DrawString(font, mousePosition.X.ToString() + " : " + mousePosition.Y.ToString() + " : " + mousePosition.Z.ToString(), new Vector2(20, 220), Color.Black);
            //spriteBatch.DrawString(font, "FPS: " + drawFrame.ToString(), new Vector2(20, 240), Color.Black);
            //spriteBatch.DrawString(font, "UPS: " + drawUpdates.ToString(), new Vector2(20, 260), Color.Black);

            switch (gamestate)
            {
                #region Startmenu
                case Vars_Func.GameState.StartMenu:
                    StartMenu_GUI.Draw(spriteBatch, font);
                    break;
                #endregion
                #region Mainmenu
                case Vars_Func.GameState.MainMenu:
                    // Draw the old one unil menu is ready 
                    if (!MainMenu_GUI.UpdateReady)
                    {
                        StartMenu_GUI.Draw(spriteBatch, font);
                    }
                    else
                    {
                        MainMenu_GUI.Draw(spriteBatch, font);
                    }
                    break;
                #endregion
                #region Ingame
                case Vars_Func.GameState.Ingame:
                    if (reinitializeDone)
                    {
                        //minimap.drawMinimap(spriteBatch, indexOfMiddleHexagon);
                        BasicGUI.Draw(spriteBatch, font, minimap, indexOfMiddleHexagon);
                        //spriteBatch.DrawString(font, mouseState.X.ToString() + " : " + mouseState.Y.ToString(), new Vector2(20, 200), Color.Black);
                        //spriteBatch.DrawString(font, mousePosition.X.ToString() + " : " + mousePosition.Y.ToString() + " : " + mousePosition.Z.ToString(), new Vector2(20, 220), Color.Black);
                        //spriteBatch.DrawString(font, "FPS: " + drawFrame.ToString(), new Vector2(20, 240), Color.Black);
                        //spriteBatch.DrawString(font, "UPS: " + drawUpdates.ToString(), new Vector2(20, 260), Color.Black);

                        if (showIngameMenu)
                        {
                            if (!IngameMenu_GUI.UpdateReady)
                            {
                                //Do nothing
                            }
                            else
                            {
                                IngameMenu_GUI.Draw(spriteBatch, font);
                            }

                        }
                    }
                    else
                    {
                        MainMenu_GUI.Draw(spriteBatch, font);
                    }
                    break;
                #endregion
                #region Highscore
                case Vars_Func.GameState.Highscore:
                    if (showIngameMenu)
                    {
                        if (!Highscore_GUI.UpdateReady)
                        {
                            IngameMenu_GUI.Draw(spriteBatch, font);
                        }
                        else
                        {
                            Highscore_GUI.Draw(spriteBatch, font);
                        }
                    }
                    else
                    {
                        if (!Highscore_GUI.UpdateReady)
                        {
                            MainMenu_GUI.Draw(spriteBatch, font);
                        }
                        else
                        {
                            Highscore_GUI.Draw(spriteBatch, font);
                        }
                    }
                    break;
#endregion
                #region Settings
                case Vars_Func.GameState.Settings:
                    if (showIngameMenu)
                    {
                        if (!Setting_GUI.UpdateReady)
                        {
                            IngameMenu_GUI.Draw(spriteBatch, font);
                        }
                        else
                        {
                            Setting_GUI.Draw(spriteBatch, font);
                            if (Setting_GUI.UseFullscreen && !this.graphics.IsFullScreen)
                            {
                                this.graphics.ToggleFullScreen();
                            }
                            else if (!Setting_GUI.UseFullscreen && this.graphics.IsFullScreen)
                            {
                                this.graphics.ToggleFullScreen();
                            }
                        }
                    }
                    else
                    {
                        if (!Setting_GUI.UpdateReady)
                        {
                            MainMenu_GUI.Draw(spriteBatch, font);
                        }
                        else
                        {
                            Setting_GUI.Draw(spriteBatch, font);
                            if (Setting_GUI.UseFullscreen && !this.graphics.IsFullScreen)
                            {
                                this.graphics.ToggleFullScreen();
                            }
                            else if (!Setting_GUI.UseFullscreen && this.graphics.IsFullScreen)
                            {
                                this.graphics.ToggleFullScreen();
                            }
                        }
                    }
                    break;
                #endregion
                #region Confirm
                case Vars_Func.GameState.Confirm:
                    if (!Confirm_GUI.UpdateReady)
                    {
                        BasicGUI.Draw(spriteBatch, font, minimap, indexOfMiddleHexagon);
                        IngameMenu_GUI.Draw(spriteBatch, font);
                    }
                    else
                    {
                        BasicGUI.Draw(spriteBatch, font, minimap, indexOfMiddleHexagon);
                        Confirm_GUI.Draw(spriteBatch, font);
                    }
                    break;
                #endregion
                #region GameOver
                case Vars_Func.GameState.GameOver:
                    if (!GameOver_GUI.UpdateReady)
                    {
                        BasicGUI.Draw(spriteBatch, font, minimap, indexOfMiddleHexagon);
                    }
                    else
                    {
                        BasicGUI.Draw(spriteBatch, font, minimap, indexOfMiddleHexagon);
                        GameOver_GUI.Draw(spriteBatch, font);
                    }

                    break;
                #endregion
                #region Intro
                case Vars_Func.GameState.Intro:
                    if (!IntroMenu_GUI.UpdateReady)
                    {
                        MainMenu_GUI.Draw(spriteBatch, font);
                    }
                    else
                    {
                        IntroMenu_GUI.Draw(spriteBatch, font);
                    }
                    break;
                #endregion
                #region Insert Name After Confirm
                case Vars_Func.GameState.InsertNameConfirm:
                    if (!InsertNameGUI.UpdateReady)
                    {
                        BasicGUI.Draw(spriteBatch, font, minimap, indexOfMiddleHexagon);
                        Confirm_GUI.Draw(spriteBatch, font);
                    }
                    else
                    {
                        BasicGUI.Draw(spriteBatch, font, minimap, indexOfMiddleHexagon);
                        InsertNameGUI.Draw(spriteBatch, font);
                    }
                    break;
                #endregion
                #region Insert Name Before Reset
                case Vars_Func.GameState.InserNameReset:
                    if (!InsertNameGUI.UpdateReady)
                    {
                        BasicGUI.Draw(spriteBatch, font, minimap, indexOfMiddleHexagon);
                        GameOver_GUI.Draw(spriteBatch, font);
                    }
                    else
                    {
                        BasicGUI.Draw(spriteBatch, font, minimap, indexOfMiddleHexagon);
                        InsertNameGUI.Draw(spriteBatch, font);
                    }
                    break;
                #endregion
                #region Insert Name Before Quit
                case Vars_Func.GameState.InserNameQuit:
                    if (!InsertNameGUI.UpdateReady)
                    {
                        BasicGUI.Draw(spriteBatch, font, minimap, indexOfMiddleHexagon);
                        GameOver_GUI.Draw(spriteBatch, font);
                    }
                    else
                    {
                        BasicGUI.Draw(spriteBatch, font, minimap, indexOfMiddleHexagon);
                        InsertNameGUI.Draw(spriteBatch, font);
                    }
                    break;
                #endregion
                default:
                    break;
            }
            spriteBatch.Draw(Vars_Func.getCursor(), new Vector2(mouseState.X, mouseState.Y), Color.White);

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
