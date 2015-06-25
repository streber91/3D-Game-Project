using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Underlord.Entity;
using Underlord.Environment;

namespace Underlord.Logic
{
    static class Interaction
    {
        static Vars_Func.GameState gameState = Vars_Func.GameState.Ingame;
        static Vector2 indexOfMiddleHexagonForRoomCreation = new Vector2(0, 0);
        static int radius = 0;
        static int counter = 0;
        static float timeCounter = 0.0f;

        #region Properties
        public static Vars_Func.GameState GameState
        {
            get { return gameState; }
            set { gameState = value; }
        }
        #endregion

        public static void Update(GameTime gameTime, float timeSinceLastUpdate, Map map, Vector2 mouseover, MouseState mouseState, KeyboardState keyboard)
        {
            timeCounter += timeSinceLastUpdate;
            
            switch (gameState)
            {
                case Vars_Func.GameState.MainMenue:
                    break;
                case Vars_Func.GameState.Ingame:
                    map.getHexagonAt(mouseover.X, mouseover.Y).Color = Color.Yellow;// getPlaneHexagons()[(int)(mouseover.X * planeLength + mouseover.Y)].setColor(Color.Brown);
                    if (timeCounter > 100)
                    {
                        if (keyboard.IsKeyDown(Keys.R))
                        {
                            gameState = Vars_Func.GameState.CreateRoom;
                            timeCounter = 0;
                        }
                    }
                    break;
                case Vars_Func.GameState.Save:
                    break;
                case Vars_Func.GameState.Load:
                    break;
                case Vars_Func.GameState.CreateRoom:
                    
                    map.getHexagonAt(mouseover.X, mouseover.Y).Color = Color.Purple;// getPlaneHexagons()[(int)(mouseover.X * planeLength + mouseover.Y)].setColor(Color.Brown);
                    //Color the Hexagons around the coursor too
                    /*Vector2[] neigbors = map.getHexagonAt(mouseover.X, mouseover.Y).getNeighbors(); //getPlaneHexagons()[(int)(mouseover.X * planeLength + mouseover.Y)].getNeighbors();
                    foreach (Vector2 hex in neigbors)
                    {
                        map.getHexagonAt(hex.X, hex.Y).Color = Color.Blue;
                    }*/
                    if (timeCounter > 100)
                    {
                        if (keyboard.IsKeyDown(Keys.Escape))
                        {
                            indexOfMiddleHexagonForRoomCreation = new Vector2(0, 0);
                            radius = 0;
                            counter = 0;
                            gameState = Vars_Func.GameState.Ingame;
                            timeCounter = 0;
                        }
                        if (counter == 0 && mouseState.LeftButton == ButtonState.Pressed)
                        {
                            //if (map.getHexagonAt(mouseover.X, mouseover.Y).Obj
                            indexOfMiddleHexagonForRoomCreation = mouseover;
                            ++counter;
                            timeCounter = 0;
                        }
                        else if (counter == 1 && mouseState.LeftButton == ButtonState.Pressed)
                        {
                            radius = Math.Max((int)Math.Abs(indexOfMiddleHexagonForRoomCreation.X - mouseover.X), (int)Math.Abs(indexOfMiddleHexagonForRoomCreation.Y - mouseover.Y));
                            map.Rooms.Add(new Room(indexOfMiddleHexagonForRoomCreation, radius, map));
                            indexOfMiddleHexagonForRoomCreation = new Vector2(0, 0);
                            radius = 0;
                            counter = 0;
                            timeCounter = 0;
                        }
                    }
                    break;
                case Vars_Func.GameState.Build:
                    break;
                case Vars_Func.GameState.Mine:
                    break;
            }
        }
    }
}
