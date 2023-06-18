using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Room 
{
    public static int MinSize = 4;
    public static int MaxSize = 7;
    public readonly Vector2Int Position;
    public readonly Vector2Int Dimensions;
    public int Area => Dimensions.x * Dimensions.y;
    public Vector2 CenterOfRoom => Position + ((Vector2)Dimensions / 2.0f);
    public float Center => CenterOfRoom.magnitude;
    protected static Objects monstersToSpawn;
    protected static Objects itemsToSpawn;
    protected static Transform Dungeon;

    private Stack<Vector2> usedCell = new Stack<Vector2>();

    public Room(Vector2Int pos, Vector2Int dim, Objects mon, Objects items, Transform dungeon)
    {
        Position = pos;
        Dimensions = dim;
        monstersToSpawn = mon;
        itemsToSpawn = items;
        Dungeon = dungeon;
    }

    public List<Vector2Int> DoorTo(Room room)
    {
        var path = room.CenterOfRoom - CenterOfRoom;
        var xint = Mathf.RoundToInt(path.x);
        var yint = Mathf.RoundToInt(path.y);
        var pathint = new Vector2Int(xint, yint);
        var doorlist = new List<Vector2Int>();

        while (Mathf.Abs(pathint.x) != 0)
        {
            doorlist.Add(new Vector2Int((int)CenterOfRoom.x, (int)CenterOfRoom.y) + pathint);
            pathint.x -= System.Math.Sign(pathint.x);
        }

        while (Mathf.Abs(pathint.y) != 0)
        {
            doorlist.Add(new Vector2Int((int)CenterOfRoom.x, (int)CenterOfRoom.y) + pathint);
            pathint.y -= System.Math.Sign(pathint.y);
        }

        return doorlist;
    }

    public bool Overlap(int xpos, int ypos, int width, int height)
    {
        if (Position.x + Dimensions.x < xpos || Position.x > xpos + width) return false;
        if (Position.y + Dimensions.y < ypos || Position.y > ypos + height) return false;
        return true;
    }

    public virtual void SpawnObjects()
    {
        SpawnItems();
        SpawnMonsters();
    }

    private void SpawnMonsters()
    {
        var rate = (float)Area / (MaxSize * MaxSize);
        for (int x = 0; x < Dimensions.x; x++)
        {
            for (int y = 0; y < Dimensions.y; y++)
            {
                var newPos = GetPos(x, y);
                var r = Random.value;
                if (r > 0.78f)
                {
                    Object.Instantiate(monstersToSpawn.GetSpawnObject(), newPos, Quaternion.identity, Dungeon);
                }
            }
        }
    }

    private void SpawnItems()
    {
        for (int i = 0; i < Dimensions.x; i++)
        {
            for (int j = 0; j < Dimensions.y; j++)
            {
                var pos = new Vector3(Position.x + i, 0, Position.y + j) + (Vector3.one * 0.5f);
                var r = Random.value;
                if (r > 0.68f)
                {
                    Object.Instantiate(itemsToSpawn.GetSpawnObject(), pos, Quaternion.identity, Dungeon);
                }
            }
        }
    }

    private Vector3 GetPos(int x, int y)
    {
        var pos = new Vector3(Position.x + x + (Random.value - 0.5f), 0.5f, Position.y + y + (Random.value - 0.5f));

        return pos;
    }
}
