using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossRoom : Room
{
    public static List<Cell> OpenSpace = new List<Cell>();
    public BossRoom(Vector2Int pos, Vector2Int dim, Objects mon, Objects items, Transform dungeon) : base(pos, dim, mon, items, dungeon)
    {
    }

    public override void SpawnObjects()
    {
        
    }
}
