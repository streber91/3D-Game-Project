using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Underlord.Environment
{
    class Room
    {
        List<Vector2> room = new List<Vector2>();

        public Room(Vector2 middleHexagonIndexNumber, int radius, Map map)
        {
            room.Add(middleHexagonIndexNumber);
            Queue<Vector2> queue = new Queue<Vector2>();
            queue.Enqueue(middleHexagonIndexNumber);
            queue.Enqueue(new Vector2(radius+1000000,0));
            map.getHexagonAt(middleHexagonIndexNumber).Visited = true;
            int roomNumber = map.Rooms.Count + 1;
            map.getHexagonAt(middleHexagonIndexNumber).RoomNumber = roomNumber;
            while (queue.Count != 1)
            {
                Vector2 tmp = queue.Dequeue();
                if (tmp.X == 1000000) break;
                else if (tmp.X < 1000000)
                {
                    for (int i = 0; i < 6; ++i)
                    {
                        Vector2 neighbor = map.getHexagonAt(tmp).getNeighbors()[i];
                        if (map.getHexagonAt(neighbor).Visited == false)
                        {
                            map.getHexagonAt(neighbor).Visited = true;
                            if (map.getHexagonAt(neighbor).Obj == null || map.getHexagonAt(neighbor).Obj.getThingTyp() != Entity.Vars_Func.ThingTyp.Wall)
                            {
                                queue.Enqueue(neighbor);
                                map.getHexagonAt(neighbor).RoomNumber = roomNumber;
                                room.Add(neighbor);
                            }
                        }
                    }
                }
                else queue.Enqueue(new Vector2(tmp.X - 1, 0));
            }
            foreach (Hexagon hex in map.getMapHexagons())
            {
                if (hex.Visited == true) hex.Visited = false;
            }
        }

        public void mergeRoom(Room r)
        {
            foreach (Vector2 x in r.room)
            {
                this.room.Add(x);
            }
        }
    }
}
