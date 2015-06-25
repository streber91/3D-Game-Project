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
            map.getHexagonAt(middleHexagonIndexNumber.X, middleHexagonIndexNumber.Y).Visited = true;
            int roomNumber = map.Rooms.Count + 1;
            map.getHexagonAt(middleHexagonIndexNumber.X, middleHexagonIndexNumber.Y).RoomNumber = roomNumber;
            while (queue.Peek() != null)
            {
                Vector2 tmp = queue.Dequeue();
                if (tmp.X == 1000000) break;
                else if (tmp.X < 1000000)
                {
                    for (int i = 0; i < 6; ++i)
                    {
                        Vector2 neighbor = map.getHexagonAt(tmp.X, tmp.Y).getNeighbors()[i];
                        if (map.getHexagonAt(neighbor.X, neighbor.Y).Visited == false)
                        {
                            queue.Enqueue(neighbor);
                            map.getHexagonAt(neighbor.X, neighbor.Y).Visited = true;
                            map.getHexagonAt(neighbor.X, neighbor.Y).RoomNumber = roomNumber;
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
