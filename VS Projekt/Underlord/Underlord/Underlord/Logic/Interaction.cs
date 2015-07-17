﻿using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Underlord.Entity;

namespace Underlord.Logic
{
    static class Interaction
    {
        static Vars_Func.GameState gameState = Vars_Func.GameState.Ingame;
        static Vector2 indexOfMiddleHexagonForRoomCreation = new Vector2(0, 0);
        static int radius = 0;
        static int counter = 0;
        static float timeCounter = 0.0f;
        static Vector2 oldMousePosition;
        static List<Vector2> toBeRoomPositions = new List<Vector2>();

        #region Properties
        public static Vars_Func.GameState GameState
        {
            get { return gameState; }
            set { gameState = value; }
        }
        #endregion

        public static void Update(GameTime gameTime, Environment.Map map, Vector2 mouseover, MouseState mouseState, MouseState lastMouseState, KeyboardState keyboard, Renderer.GUI gui)
        {
            timeCounter += gameTime.ElapsedGameTime.Milliseconds;

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
                    indexOfMiddleHexagonForRoomCreation = new Vector2(0, 0);
                    radius = 0;
                    counter = 0;
                    gameState = Vars_Func.GameState.Build;
                    timeCounter = 0;
                }
                //switch to Mine Mode if "M" ist pressed
                if (keyboard.IsKeyDown(Keys.M))
                {
                    indexOfMiddleHexagonForRoomCreation = new Vector2(0, 0);
                    radius = 0;
                    counter = 0;
                    gameState = Vars_Func.GameState.Mine;
                    timeCounter = 0;
                }
                //switch to MergeRooms Mode if "T" ist pressed
                if (keyboard.IsKeyDown(Keys.T))
                {
                    indexOfMiddleHexagonForRoomCreation = new Vector2(0, 0);
                    radius = 0;
                    counter = 0;
                    gameState = Vars_Func.GameState.MergeRooms;
                    timeCounter = 0;
                }
                //switch to DeleteRoom Mode if "Z" ist pressed
                if (keyboard.IsKeyDown(Keys.Z))
                {
                    indexOfMiddleHexagonForRoomCreation = new Vector2(0, 0);
                    radius = 0;
                    counter = 0;
                    gameState = Vars_Func.GameState.DeleteRoom;
                    timeCounter = 0;
                }
            }
            switch (gameState)
            {
                case Vars_Func.GameState.MainMenue:
                    break;
                #region Ingame
                case Vars_Func.GameState.Ingame:
                    //colors the hexagon at mouseposition in Ingame mode yellow
                    map.getHexagonAt(mouseover).Color = Color.Yellow;
                    //only ten clicks per second
                    if (timeCounter > 100)
                    {
                        //try to select object that is currently at mouseposition
                        if (lastMouseState.LeftButton == ButtonState.Released &&
                            mouseState.LeftButton == ButtonState.Pressed)
                        {
                            if (map.getHexagonAt(mouseover).Obj != null)
                            {
                                switch ((map.getHexagonAt(mouseover).Obj.getThingTyp()))
                                {
                                    case Vars_Func.ThingTyp.Wall:
                                        gui.SelectedThingTyp = Vars_Func.ThingTyp.Wall;
                                        gui.Wall = (Wall)map.getHexagonAt(mouseover).Obj;
                                        break;
                                    case Vars_Func.ThingTyp.Nest:
                                        gui.SelectedThingTyp = Vars_Func.ThingTyp.Nest;
                                        gui.Nest = (Nest)map.getHexagonAt(mouseover).Obj;
                                        break;
                                    case Vars_Func.ThingTyp.DungeonCreature:
                                        gui.SelectedThingTyp = Vars_Func.ThingTyp.DungeonCreature;
                                        break;
                                    case Vars_Func.ThingTyp.HeroCreature:
                                        gui.SelectedThingTyp = Vars_Func.ThingTyp.HeroCreature;
                                        break;
                                    case Vars_Func.ThingTyp.NeutralCreature:
                                        gui.SelectedThingTyp = Vars_Func.ThingTyp.NeutralCreature;
                                        break;
                                    case Vars_Func.ThingTyp.HQCreature:
                                        gui.SelectedThingTyp = Vars_Func.ThingTyp.HQCreature;
                                        break;
                                }
                            }
                            else
                            {
                                gui.SelectedThingTyp = Vars_Func.ThingTyp.length;
                            }
                        }
                    }
                    break;
                #endregion
                case Vars_Func.GameState.Save:
                    break;
                case Vars_Func.GameState.Load:
                    break;
                #region CreateRoom
                case Vars_Func.GameState.CreateRoom:
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
                        oldMousePosition = indexOfMiddleHexagonForRoomCreation;
                        if (counter == 0)
                        {
                            //colors the hexagon at mouseposition in CreateRoom mode blue
                            map.getHexagonAt(mouseover).Color = Color.Blue;
                            if(lastMouseState.LeftButton == ButtonState.Released &&
                                mouseState.LeftButton == ButtonState.Pressed &&
                                (map.getHexagonAt(mouseover).Obj == null || map.getHexagonAt(mouseover).Obj.getThingTyp() != Vars_Func.ThingTyp.Wall) && 
                                map.getHexagonAt(mouseover).RoomNumber == 0)
                            {
                                indexOfMiddleHexagonForRoomCreation = mouseover;
                                ++counter;
                                timeCounter = 0;
                            }
                        }
                        //determines the hexagons that the new room would have
                        else if (counter == 1)
                        {
                            Vector2 newMousePosition = mouseover;
                            if (oldMousePosition != newMousePosition)
                            {
                                oldMousePosition = newMousePosition;
                                radius = Vars_Func.computeDistance(indexOfMiddleHexagonForRoomCreation, mouseover, map);
                                toBeRoomPositions = determineToBeRoomPositions(indexOfMiddleHexagonForRoomCreation, radius, map);
                                foreach (Vector2 pos in toBeRoomPositions)
                                {
                                    map.getHexagonAt(pos).Color = Color.Blue;
                                }
                            }
                            //second click determines the radius for the new room and creates the room
                            if (lastMouseState.LeftButton == ButtonState.Released && 
                                mouseState.LeftButton == ButtonState.Pressed)
                            {
                                radius = Vars_Func.computeDistance(indexOfMiddleHexagonForRoomCreation, mouseover, map);
                                map.Rooms.Add(new Environment.Room(indexOfMiddleHexagonForRoomCreation, radius, map));
                                indexOfMiddleHexagonForRoomCreation = new Vector2(0, 0);
                                radius = 0;
                                counter = 0;
                                timeCounter = 0;
                            }
                        }
                    }
                    break;
                #endregion
                #region Build
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
                        if (lastMouseState.LeftButton == ButtonState.Released && 
                            mouseState.LeftButton == ButtonState.Pressed)
                        {
                            //place nest only when there is a room at mouseposition and the room doesn't have a nest already
                            //the neighbors of the hexagon at mouseposition must be in the same room
                            if (map.getHexagonAt(mouseover).RoomNumber != 0 && map.Rooms.ElementAt(map.getHexagonAt(mouseover).RoomNumber - 1).NestType == Vars_Func.NestTyp.length)
                            {
                                bool placeable = true;
                                for (int i = 0; i < 6; ++i)
                                {
                                    if (map.getHexagonAt(mouseover).RoomNumber != map.getHexagonAt(map.getHexagonAt(mouseover).Neighbors[i]).RoomNumber) placeable = false;
                                }
                                if (placeable)
                                {
                                    new Nest(Vars_Func.NestTyp.Beetle, mouseover, map.getHexagonAt(mouseover), map, map.getHexagonAt(mouseover).Neighbors[3]);
                                    map.Rooms.ElementAt(map.getHexagonAt(mouseover).RoomNumber - 1).NestType = Vars_Func.NestTyp.Beetle;
                                }
                            }
                        }
                    }
                    break;
                #endregion
                #region Mine
                case Vars_Func.GameState.Mine:
                    //colors the hexagon at mouseposition in Mine mode red
                    map.getHexagonAt(mouseover).Color = Color.Red;
                    //only ten clicks per second
                    if (timeCounter > 100)
                    {
                        //back to Ingame Mode
                        if (keyboard.IsKeyDown(Keys.Escape))
                        {
                            gameState = Vars_Func.GameState.Ingame;
                            timeCounter = 0;
                        }
                        //try to mine a wall at mouseposition
                        if (mouseState.LeftButton == ButtonState.Pressed)
                        {
                            //mine only if there is a wall
                            if (map.getHexagonAt(mouseover).Obj != null && map.getHexagonAt(mouseover).Obj.getThingTyp() == Vars_Func.ThingTyp.Wall &&
                                ((Wall)map.getHexagonAt(mouseover).Obj).Typ != Vars_Func.WallTyp.HQ)
                            {
                                map.getHexagonAt(mouseover).Obj = null;
                            }
                        }
                    }
                    break;
                #endregion
                #region MergeRooms
                case Vars_Func.GameState.MergeRooms:
                    //colors the hexagon at mouseposition in MergeRooms mode blue
                    map.getHexagonAt(mouseover).Color = Color.Blue;
                    //only ten clicks per second
                    if (timeCounter > 100)
                    {
                        //back to Ingame Mode
                        if (keyboard.IsKeyDown(Keys.Escape))
                        {
                            gameState = Vars_Func.GameState.Ingame;
                            timeCounter = 0;
                        }
                        //try to merge rooms
                        if (lastMouseState.LeftButton == ButtonState.Released && 
                            mouseState.LeftButton == ButtonState.Pressed)
                        {
                            if (map.getHexagonAt(mouseover).RoomNumber != 0)
                            {
                                for (int i = 0; i < 6; ++i)
                                {
                                    Environment.Room room;
                                    //if the selected hexagon and the neighbor hexagon have different roomnumbers
                                    //and the neighbor hexagon is a room
                                    //and there isn't a nest in the room of the neighbor hexagon
                                    if (map.getHexagonAt(mouseover).RoomNumber != map.getHexagonAt(map.getHexagonAt(mouseover).Neighbors[i]).RoomNumber &&
                                        map.getHexagonAt(map.getHexagonAt(mouseover).Neighbors[i]).RoomNumber != 0 &&
                                        map.Rooms.ElementAt(map.getHexagonAt(map.getHexagonAt(mouseover).Neighbors[i]).RoomNumber - 1).NestType == Vars_Func.NestTyp.length)
                                    {
                                        //than get the room of the neighbor hexagon
                                        //and merge it in the room of the selected hexagon
                                        room = map.Rooms.ElementAt(map.getHexagonAt(map.getHexagonAt(mouseover).Neighbors[i]).RoomNumber - 1);
                                        map.Rooms.ElementAt(map.getHexagonAt(mouseover).RoomNumber - 1).mergeRoom(room, map);
                                    }
                                }
                            }
                            timeCounter = 0;
                        }
                    }
                    break;
                #endregion
                #region DeleteRoom
                case Vars_Func.GameState.DeleteRoom:
                    //colors the hexagon at mouseposition in DeleteRoom mode blue
                    map.getHexagonAt(mouseover).Color = Color.Blue;
                    if (timeCounter > 100)
                    {
                        //back to Ingame Mode
                        if (keyboard.IsKeyDown(Keys.Escape))
                        {
                            gameState = Vars_Func.GameState.Ingame;
                            timeCounter = 0;
                        }
                        //try to delete a room
                        if (lastMouseState.LeftButton == ButtonState.Released && 
                            mouseState.LeftButton == ButtonState.Pressed)
                        {
                            //if there is a room
                            //and there isn't a nest in the room
                            if (map.getHexagonAt(mouseover).RoomNumber != 0 &&
                                map.Rooms.ElementAt(map.getHexagonAt(mouseover).RoomNumber - 1).NestType == Vars_Func.NestTyp.length)
                            {
                                //then delete that room
                                map.Rooms.ElementAt(map.getHexagonAt(mouseover).RoomNumber - 1).deleteRoom(map);
                            }
                            timeCounter = 0;
                        }
                    }
                    break;
                #endregion
            }
        }

        public static List<Vector2> determineToBeRoomPositions(Vector2 middleHexagonIndexNumber, int radius, Environment.Map map)
        {
            List<Vector2> list = new List<Vector2>();
            //create a list with only the middle hexagon
            if (radius == 0)
            {
                list.Add(middleHexagonIndexNumber);
            }
            //create a list through a broad-first-search
            else
            {
                list.Add(middleHexagonIndexNumber); //add middle hexagon
                Queue<Vector2> queue = new Queue<Vector2>(); //create a queue
                queue.Enqueue(middleHexagonIndexNumber); //add middle hexagon to queue
                queue.Enqueue(new Vector2(radius + map.getPlanelength(), 0)); //add dummy-element to queue (x-value is a value that can't be reached with coordinated of the map)
                map.getHexagonAt(middleHexagonIndexNumber).Visited = true; //set visited for the middle element at true
                while (queue.Count != 1)
                {
                    Vector2 tmp = queue.Dequeue(); //get the first element of the queue
                    if (tmp.X == map.getPlanelength() + 1) break; //stop if the element is the dummy-element with an x-value that sasy that the radius is reached
                    //if element is a map-element
                    else if (tmp.X < map.getPlanelength())
                    {
                        //for all neighbors
                        for (int i = 0; i < 6; ++i)
                        {
                            Vector2 neighbor = map.getHexagonAt(tmp).Neighbors[i];
                            //which weren't visited already
                            if (map.getHexagonAt(neighbor).Visited == false)
                            {
                                map.getHexagonAt(neighbor).Visited = true; //set visited at true
                                //when there isn't an object on the hexagon or the object isn't a wall
                                //and the hexagon isn't already a room
                                if ((map.getHexagonAt(neighbor).Obj == null || map.getHexagonAt(neighbor).Obj.getThingTyp() != Logic.Vars_Func.ThingTyp.Wall) && map.getHexagonAt(neighbor).RoomNumber == 0)
                                {
                                    queue.Enqueue(neighbor); //add the neighbor to the queue
                                    list.Add(neighbor); //and add the neighbor to the room
                                }
                            }
                        }
                    }
                    //if element is the dummy-element with radius isn't reached
                    else queue.Enqueue(new Vector2(tmp.X - 1, 0));
                }
                //set visited for all hexagon at false (for the next use of searching)
                foreach (Environment.Hexagon hex in map.getMapHexagons())
                {
                    if (hex.Visited == true) hex.Visited = false;
                }
            }
            return list;
        }
    }
}
