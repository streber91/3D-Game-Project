using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;

namespace Underlord.Environment
{
    class Room
    {
        List<Vector2> room = new List<Vector2>();
        Logic.Vars_Func.NestTyp nestType;

        #region Properties
        public Logic.Vars_Func.NestTyp NestType
        {
            get { return nestType; }
            set { nestType = value; }
        }
        #endregion

        #region Constructor
        public Room(Vector2 middleHexagonIndexNumber, int radius, Map map)
        {
            nestType = Logic.Vars_Func.NestTyp.length;
            //create nest with only the middle hexagon
            if (radius == 0)
            {
                room.Add(middleHexagonIndexNumber);
                map.getHexagonAt(middleHexagonIndexNumber).RoomNumber = map.Rooms.Count + 1;
            }
            //create nest through a broad-first-search
            else
            {
                room.Add(middleHexagonIndexNumber); //add middle hexagon
                Queue<Vector2> queue = new Queue<Vector2>(); //create a queue
                queue.Enqueue(middleHexagonIndexNumber); //add middle hexagon to queue
                queue.Enqueue(new Vector2(radius + map.getPlanelength(), 0)); //add dummy-element to queue (x-value is a value that can't be reached with coordinated of the map)
                map.getHexagonAt(middleHexagonIndexNumber).Visited = true; //set visited for the middle element at true
                int roomNumber = map.Rooms.Count + 1; //set the number for the new room at map.Room.Count + 1, because roomNumber = 0 means there is no room
                map.getHexagonAt(middleHexagonIndexNumber).RoomNumber = roomNumber; //set the roomNumber for the middle hexagon
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
                                    map.getHexagonAt(neighbor).RoomNumber = roomNumber; //set the roomNumber of the neighbor
                                    room.Add(neighbor); //and add the neighbor to the room
                                }
                            }
                        }
                    }
                    //if element is the dummy-element with radius isn't reached
                    else queue.Enqueue(new Vector2(tmp.X - 1, 0));
                }
                //set visited for all hexagon at false (for the next use of searching)
                foreach (Hexagon hex in map.getMapHexagons())
                {
                    if (hex.Visited == true) hex.Visited = false;
                }
            }
        }
        #endregion

        public void mergeRoom(Room r, Map map)
        {
            int oldRoomNumber = map.getHexagonAt(r.room[0]).RoomNumber;
            int newRoomNumber = map.getHexagonAt(this.room[0]).RoomNumber;
            foreach (Vector2 x in r.room)
            {
                map.getHexagonAt(x).RoomNumber = newRoomNumber;
                this.room.Add(x);
            }
            map.Rooms.RemoveAt(oldRoomNumber - 1);
            for (int i = oldRoomNumber - 1; i < map.Rooms.Count; ++i)
            {
                for (int j = 0; j < map.Rooms[i].room.Count; ++j)
                {
                    map.getHexagonAt(map.Rooms[i].room[j]).RoomNumber -= 1;
                }
            }
        }

        public void deleteRoom(Map map)
        {
            int oldRoomNumber = map.getHexagonAt(this.room[0]).RoomNumber;
            foreach (Vector2 x in this.room)
            {
                map.getHexagonAt(x).RoomNumber = 0;
            }
            map.Rooms.RemoveAt(oldRoomNumber - 1);
            for (int i = oldRoomNumber - 1; i < map.Rooms.Count; ++i)
            {
                for (int j = 0; j < map.Rooms[i].room.Count; ++j)
                {
                    map.getHexagonAt(map.Rooms[i].room[j]).RoomNumber -= 1;
                }
            }
        }
    }
}
