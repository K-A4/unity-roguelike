using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public struct Cell
{
    public Vector2Int position;
    public bool isWall;
}

public class DungeonGeneration : MonoBehaviour
{
    public static DungeonGeneration Instance;
    public static int Level = 0;
    public static List<Cell> OpenSpace = new List<Cell>();
    public Transform Player;
    public Transform Door;
    public bool SpawnObjects;
    public UnityEvent OnDungeonCreate;

    [SerializeField] private int mapX;
    [SerializeField] private int mapY;

    //private List<GameObject> level = new List<GameObject>();
    [SerializeField] private List<Room> rooms = new List<Room>();
    [SerializeField] private List<Objects> monstersToSpawn = new List<Objects>();
    [SerializeField] private List<Objects> itemsToSpawn = new List<Objects>();
    [SerializeField] private GameObject wall;
    [SerializeField] private GameObject ceil;

    private Cell[,] cells;
    private List<Cell> BossDoors = new List<Cell>();
    public static Objects Monsters ;
    public static Objects Items ;

    private void Awake()
    {
        Monsters = monstersToSpawn[Level];
        Items = itemsToSpawn[Level];
        Instance = this;
    }

    private void Start()
    {
        GenerateDungeon();
    }

    [ContextMenu("IncreaseLevel")]
    public void IncreaseLevel()
    {
        Level = (Level + 1) % monstersToSpawn.Count;
        Monsters = monstersToSpawn[Level];
        Items = itemsToSpawn[Level];
        UIText.Instance.SendText("DUNGEON LEVEL INCREASED");
    }

    [ContextMenu("GenerateDungeon")]
    public void GenerateDungeon()
    {
        CreateCells();
        PlaceBossRoom(mapX / 2 - 4, mapY / 2 - 4, 4);
        StartCoroutine(GenerateDungeonCoroutine(true));
    }

    [ContextMenu("RegenerateDungeon")]
    public void RegenerateDungeon()
    {
        CreateCells();
        PlaceBossRoom(mapX / 2 - 4, mapY / 2 - 4, 4);
        StartCoroutine(GenerateDungeonCoroutine(false));
    }

    public void DestroyDungeon()
    {
        for (int x = 0; x < mapX; x++)
        {
            for (int y = 0; y < mapY; y++)
            {
                cells[x, y].isWall = true;
            }
        }

        rooms.Clear();

        for (int i = transform.childCount - 1; i >= 0; i--)
        {
            Destroy(transform.GetChild(i).gameObject);
        }
    }

    private IEnumerator GenerateDungeonCoroutine(bool setPlayer)
    {
        for (int i = 0; i < 100; i++)
        {
            RandomRoomPlace();
        }

        rooms = rooms.OrderBy(r => r.Center).ToList();
        var corridors = new List<Vector2Int>();

        if (setPlayer)
        {
            var roomPos = rooms[0].CenterOfRoom;
            Player.transform.position = new Vector3(roomPos.x, 0.5f, roomPos.y);
        }

        for (int i = 0; i < rooms.Count - 1; i++)
        {
            corridors.AddRange(rooms[i].DoorTo(rooms[i + 1]));
        }

        foreach (var corridor in corridors)
        {
            cells[corridor.x, corridor.y].isWall = false;
            OpenSpace.Add(cells[corridor.x, corridor.y]);
        }

        for (int i = 0; i < mapX; i++)
        {
            for (int j = 0; j < mapY; j++)
            {
                if (IsNeibhoursWalls(i, j))
                {
                    corridors.Add(new Vector2Int(i, j));
                }
            }
        }

        foreach (var corridor in corridors)
        {
            cells[corridor.x, corridor.y].isWall = false;
        }

        foreach (var item in cells)
        {

            if (item.isWall)
            {
                var cube = GameObject.Instantiate(wall);
                cube.transform.position = new Vector3(transform.position.x + item.position.x, transform.position.y, transform.position.z + item.position.y);
                cube.transform.SetParent(transform);
            }
            
        }

        foreach (var cell in BossDoors)
        {
            if (!cells[cell.position.x, cell.position.y].isWall)
            {
                var cube = Instantiate(wall);
                cube.transform.position = new Vector3(transform.position.x + cell.position.x, 0, transform.position.z + cell.position.y);
                cube.transform.SetParent(Door);
                var newPos = cube.transform.localPosition;
                newPos.y = 0;
                cube.transform.localPosition = newPos;
            }
        }

        for (int i = 1; i < rooms.Count; i++)
        {
            if (SpawnObjects)
            {
                rooms[i].SpawnObjects();
            }
            yield return null;
        }

        OnDungeonCreate?.Invoke();

        yield break;
    }

    private void CreateCells()
    {
        cells = new Cell[mapX, mapY];
        DestroyDungeon();
        for (int i = 1; i < Door.childCount; i++)
        {
            Destroy(Door.GetChild(i).gameObject);
        }
        
        for (int i = 0; i < mapX; i++)
        {
            for (int j = 0; j < mapY; j++)
            {
                var newCell = new Cell() { position = new Vector2Int(i, j), isWall = true };
                cells[i, j] = newCell;
            }
        }
    }

    private bool IsNeibhoursWalls(int i, int j)
    {
        int neibx;
        int neiby;
        for (int nx = -1; nx <= 1; nx++)
        {
            for (int ny = -1; ny <= 1; ny++)
            {
                neibx = i + nx;
                neiby = j + ny;

                if (neibx >= 0 && neibx < mapX && neiby >= 0 && neiby < mapY)
                {
                    if (!cells[neibx, neiby].isWall)
                    {
                        return false;
                    }
                }
            }
        }
        return true;
    }

    private void RandomRoomPlace()
    {
        //print((Mathf.Pow(Random.value, 0.5f) - 0.5f) * 2);
        //Random.value * mapX
        //(mapX / 2) + (((Mathf.Pow(Random.value, 0.5f) - 0.5f) * 2) * (mapX / 2))
        var rXpos = Mathf.RoundToInt(Random.value * mapX);
        var rYpos = Mathf.RoundToInt(Random.value * mapY);
        var r = Random.value;
        var rDx = Mathf.RoundToInt(Room.MinSize + r * (Room.MaxSize - Room.MinSize));
        r = Random.value;
        var rDy = Mathf.RoundToInt(Room.MinSize + r * (Room.MaxSize - Room.MinSize));

        if (CanPlaceRoom(rXpos, rYpos, rDx, rDy))
        {
            PlaceRectRoom(rXpos, rYpos, rDx, rDy);
            rooms.Add(new Room(new Vector2Int(rXpos, rYpos), new Vector2Int(rDx, rDy), Monsters, Items, transform));
        }
    }

    private bool CanPlaceRoom(int xpos, int ypos, int width, int height)
    {
        foreach (var room in rooms)
        {
            if (room.Overlap(xpos, ypos, width, height))
            {
                return false;
            }
        }

        if (xpos + width >= mapX || xpos == 0) return false;
        if (ypos + height >= mapY || ypos == 0) return false;

        return true;
    }

    private void PlaceRectRoom(int xpos, int ypos, int width, int height)
    {
        for (int x = xpos; x < xpos + width; x++)
        {
            for (int y = ypos; y < ypos + height; y++)
            {
                cells[x, y].isWall = false;
                OpenSpace.Add(cells[x, y]);
            }
        }
    }

    private void PlaceBossRoom(int xpos, int ypos, int radius)
    {
        PlaceRectRoom(xpos, ypos, radius * 2, radius * 2);
        var bossRoom = new BossRoom(new Vector2Int(xpos, ypos), new Vector2Int(radius * 2, radius * 2), Monsters, Items, transform);
        rooms.Add(bossRoom);
        BossDoors.Clear();
        for (int i = 0; i < radius * 2; i++)
        {
            var upperx = xpos + i;
            var uppery = ypos - 1;
            BossDoors.Add(cells[upperx, uppery]);
            BossDoors.Add(cells[upperx, uppery + (radius * 2) + 1]);
            BossDoors.Add(cells[uppery, upperx]);
            BossDoors.Add(cells[uppery + (radius * 2) + 1, upperx]);
        }

        for (int x = xpos; x < xpos + (radius * 2); x++)
        {
            for (int y = ypos; y < ypos + (radius * 2); y++)
            {
                var inCircle = Vector2.Distance(new Vector2(x - xpos + 0.5f, y - ypos + 0.5f), new Vector2(radius, radius)) < radius;

                if (!inCircle)
                {
                    cells[x, y].isWall = true;
                }
                else
                {
                    BossRoom.OpenSpace.Add(cells[x, y]);
                }
            }
        }
        //print(BossRoom.OpenSpace.Count);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        foreach (var item in BossRoom.OpenSpace)
        {
            var pos = new Vector3(item.position.x, 0.0f, item.position.y);
            Gizmos.DrawCube(pos + (Vector3.one * 0.5f), Vector3.one * 0.5f);
        }
    }
}
