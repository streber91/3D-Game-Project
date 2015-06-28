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
                    //colors the hexagon at mouseposition in Ingame mode yellow
                    map.getHexagonAt(mouseover).Color = Color.Yellow;
                    if (timeCounter > 100)
                    {
                        //switch to CreateRoom Mode in "R" is pressed
                        if (keyboard.IsKeyDown(Keys.R))
                        {
                            gameState = Vars_Func.GameState.CreateRoom;
                            timeCounter = 0;
                        }
                        //switch to Build Mode if "N" ist pressed
                        if (keyboard.IsKeyDown(Keys.N))
                        {
                            gameState = Vars_Func.GameState.Build;
                            timeCounter = 0;
                        }
                    }
                    break;
                case Vars_Func.GameState.Save:
                    break;
                case Vars_Func.GameState.Load:
                    break;
                case Vars_Func.GameState.CreateRoom:
                    //colors the hexagon at mouseposition in CreateRoom mode blue
                    map.getHexagonAt(mouseover).Color = Color.Blue;
                    //color the hexagons around the coursor too
                    /*Vector2[] neigbors = map.getHexagonAt(mouseover.X, mouseover.Y).getNeighbors();
                    foreach (Vector2 hex in neigbors)
                    {
                        map.getHexagonAt(hex.X, hex.Y).Color = Color.Blue;
                    }*/
                    //only ten clicks per second
                    if (timeCounter > 100)
                    {
                        //back to Ingame mode and reset of values
                        if (keyboard.IsKeyDown(Keys.Escape))
                        {
                            indexOfMiddleHexagonForRoomCreation = new Vector2(0, 0);
                            radius = 0;
                            counter = 0;
                            gameState = Vars_Func.GameState.Ingame;
                            timeCounter = 0;
                        }
                        //first click determines the middle of the new room
                        if (counter == 0 && mouseState.LeftButton == ButtonState.Pressed)
                        {
                            if (map.getHexagonAt(mouseover).Obj == null || map.getHexagonAt(mouseover).Obj.getThingTyp() != Vars_Func.ThingTyp.Wall)
                            {
                                indexOfMiddleHexagonForRoomCreation = mouseover;
                                ++counter;
                            }
                            timeCounter = 0;
                        }
                        //second click determines the radius for the new room and creates the room
                        else if (counter == 1 && mouseState.LeftButton == ButtonState.Pressed)
                        {
                            radius = Vars_Func.computeDistance(indexOfMiddleHexagonForRoomCreation, mouseover, map);
                            map.Rooms.Add(new Room(indexOfMiddleHexagonForRoomCreation, radius, map));
                            indexOfMiddleHexagonForRoomCreation = new Vector2(0, 0);
                            radius = 0;
                            counter = 0;
                            timeCounter = 0;
                        }
                    }
                    break;
                case Vars_Func.GameState.Build:
                    //colors the hexagon at mouseposition in Build mode purple
                    map.getHexagonAt(mouseover).Color = Color.Purple;
                    //only ten clicks per second
                    if (timeCounter > 100)
                    {
                        //back to Ingame Mode
                        if (keyboard.IsKeyDown(Keys.Escape))
                        {
                            gameState = Vars_Func.GameState.Ingame;
                            timeCounter = 0;
                        }
                        //try to place a nest at mouseposition
                        if (mouseState.LeftButton == ButtonState.Pressed)
                        {
                            //place nest only when there is a room at mouseposition and the room doesn't have a nest already
                            if (map.getHexagonAt(mouseover).RoomNumber != 0 && map.Rooms.ElementAt(map.getHexagonAt(mouseover).RoomNumber - 1).NestType == Vars_Func.NestTyp.length)
                            {
                                map.Nests.Add(new Nest(Vars_Func.NestTyp.Beetle, mouseover, map.getHexagonAt(mouseover)));
                                map.Rooms.ElementAt(map.getHexagonAt(mouseover).RoomNumber - 1).NestType = Vars_Func.NestTyp.Beetle;
                            }
                        }
                    }
                    break;
                case Vars_Func.GameState.Mine:
                    break;
            }
        }
    }
}
