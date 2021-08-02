using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Diagnostics;
using Debug = UnityEngine.Debug;
using Random = UnityEngine.Random;
using TMPro;

public class GameMap : MonoBehaviour
{
    public Stopwatch timer;

    [Tooltip("Needs to be multiple of 2")]
    public int mapWidth = 90;
    [Tooltip("Needs to be multiple of 2")]
    public int mapHeight = 50;

    public int maxRoomSize = 10;
    public int minRoomSize = 5;
    public int maxNumberOfRooms = 50;
    public int maxMonstersPerRoom = 3;
    public int maxItemsPerRoom = 3;

    public int fovRadius;

    int numberOfRooms;

    [SerializeField] GameObject tileObject;

    public Tile[,] map;

    Grid2 grid2;

    public GameObject player;

    [Tooltip("Place enemy gameobjects here")]
    public GameObject[] enemyPrefab;
    public GameObject[] itemPrefab;

    public List<GameObject> entities = new List<GameObject>();
    public List<GameObject> items = new List<GameObject>();

    Engine engine;

    private void Awake()
    {
        player = GameObject.Find("Player");//going to create and place later
        grid2 = GetComponent<Grid2>();
        engine = GetComponent<Engine>();

        map = new Tile[mapWidth, mapHeight];

        InstantiateTiles();

        MakeMap(maxNumberOfRooms, minRoomSize, maxRoomSize, mapWidth, mapHeight);
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            //clear game objects
            timer = new Stopwatch();
            for (int x = 0; x < mapWidth; x++)
            {
                for (int y = 0; y < mapHeight; y++)
                {
                    map[x, y].SetToWall();
                }
            }
            timer.Stop();
            Debug.Log("Clearing map took:" + timer.Elapsed);

            MakeMap(maxNumberOfRooms, minRoomSize, maxRoomSize, mapWidth, mapHeight);
        }
    }

    void MakeMap(int maxNumberOfRooms, int minRoomSize, int maxRoomSize, int mapWidth, int mapHeight)
    {
        timer = new Stopwatch();
        timer.Start();

        FillMap();
        List<Rect> rooms = new List<Rect>();
        numberOfRooms = 0;

        for (int r = 0; r < maxNumberOfRooms; r++)
        {
            //random room size
            int w = Random.Range(minRoomSize, maxRoomSize);
            int h = Random.Range(minRoomSize, maxRoomSize);
            //random position inside map
            int x = Random.Range(1, mapWidth - w - 1);//stopped left side edge cases
            int y = Random.Range(0, mapHeight - h - 2);//stopped top side edge cases

            //added spacing between rooms
            Rect newRoom = new Rect(x - 1, y + 1, w + 2, h + 2);

            //check overlapping rooms
            bool overlapping = false;
            for (int i = 0; i < rooms.Count; i++)
            {
                if (newRoom.Overlaps(rooms[i]))
                {
                    overlapping = true;
                    break;
                }
            }
            if (!overlapping)
            {
                RectInt newRoomRect = new RectInt(Mathf.RoundToInt(newRoom.xMin + 1),
                    Mathf.RoundToInt(newRoom.yMin + 1),
                    Mathf.RoundToInt(newRoom.width - 2),
                    Mathf.RoundToInt(newRoom.height - 2));
                CreateRoom(newRoomRect);

                //center of room for tunnels
                int newX = Mathf.RoundToInt(newRoomRect.center.x);
                int newY = Mathf.RoundToInt(newRoomRect.center.y);

                if (numberOfRooms == 0)
                {
                    player.transform.position = new Vector2((int)newRoomRect.center.x, (int)newRoomRect.center.y);
                    entities.Add(player);
                }
                else
                {
                    //connect rooms with tunnel
                    int prevX = Mathf.RoundToInt(rooms[(numberOfRooms - 1)].center.x);
                    int prevY = Mathf.RoundToInt(rooms[(numberOfRooms - 1)].center.y);


                    //randomly pick tunnel direction
                    if (Random.Range(0, 1) == 1)
                    {
                        CreateVerticalTunnel(prevX, newX, prevY);
                        CreateHorizontalTunnel(prevY, newY, newX);
                    }
                    else
                    {
                        CreateVerticalTunnel(prevY, newY, prevX);
                        CreateHorizontalTunnel(prevX, newX, newY);
                    }
                }

                PlaceEntities(newRoomRect);
                PlaceItems(newRoomRect);

                numberOfRooms++;
                rooms.Add(newRoom);
            }
        }

        timer.Stop();
        Debug.Log("Creating map took:" + timer.Elapsed);

        ClearVisibleTiles();
        FOV();
        SetWalkable();
        grid2.CreateGrid();        
        UpdateWalkable();     
    }


    void InstantiateTiles()
    {
        for (int x = 0; x < mapWidth; x++)
        {
            for (int y = 0; y < mapHeight; y++)
            {
                GameObject newTile = Instantiate(tileObject, new Vector2(x, y), Quaternion.identity, gameObject.transform.GetChild(0));
                map[x, y] = newTile.GetComponent<Tile>();
            }
        }
    }
    void FillMap()
    {
        for (int x = 0; x < mapWidth; x++)
        {
            for (int y = 0; y < mapHeight; y++)
            {
                map[x, y].SetToWall();
            }
        }
    }

    void CreateRoom(RectInt rectInt)
    {
        for (int x = rectInt.xMin; x < rectInt.xMax; x++)
        {
            for (int y = rectInt.yMin; y < rectInt.yMax; y++)
            {
                map[x, y].SetToFloor();
            }
        }
    }

    void CreateHorizontalTunnel(int x1, int x2, int y)
    {
        for (int x = Mathf.Min(x1, x2); x < Mathf.Max(x1, x2) + 1; x++)
        {
            map[x, y].SetToFloor();
        }
    }

    void CreateVerticalTunnel(int y1, int y2, int x)
    {
        for (int y = Mathf.Min(y1, y2); y < Mathf.Max(y1, y2) + 1; y++)
        {
            map[x, y].SetToFloor();
        }
    }

    void PlaceEntities(RectInt rectInt)
    {
        for (int i = 0; i <= Random.Range(0, maxMonstersPerRoom); i++)
        {
            int x = Random.Range(rectInt.x, rectInt.x + rectInt.width);
            int y = Random.Range(rectInt.y, rectInt.y + rectInt.height);

            foreach (GameObject entity in entities)
            {
                if(entity.transform.position.x == x && entity.transform.position.y == y) { return; }
            }

            {
                if (Random.Range(0, 100) > 80)
                {
                    CreateEntity(x, y, 1);
                }
                else
                {
                    CreateEntity(x, y, 0);
                }
            }
        }
    }

    void CreateEntity(int x, int y, int entityNumber)
    {
        GameObject entity = Instantiate(enemyPrefab[entityNumber], new Vector2(x, y), Quaternion.identity, gameObject.transform.GetChild(1));
        entity.name = enemyPrefab[entityNumber].name;
        //entity.entityNumber = entityNumber;

        entities.Add(entity);
    }

    void PlaceItems(RectInt rectInt)
    {
        for (int i = 0; i <= Random.Range(0, maxItemsPerRoom); i++)
        {
            int x = Random.Range(rectInt.x, rectInt.x + rectInt.width);
            int y = Random.Range(rectInt.y, rectInt.y + rectInt.height);

            foreach (GameObject item in items)
            {
                if (item.transform.position.x == x && item.transform.position.y == y) { return; }
            }

            {
                if (Random.Range(0, 100) > 80)
                {
                    CreateItem(x, y, 1);
                }
                else
                {
                    CreateItem(x, y, 0);
                }
            }
        }
    }

    void CreateItem(int x, int y, int itemNumber)
    {
        GameObject itemObject = Instantiate(itemPrefab[itemNumber], new Vector2(x, y), Quaternion.identity, gameObject.transform.GetChild(2));
        itemObject.name = itemPrefab[itemNumber].name;
        //entity.entityNumber = entityNumber;

        items.Add(itemObject);
    }


    public Tuple<bool, string> IsBlocked(int x, int y)
    {
        if (!map[x, y].walkable) { return Tuple.Create(true, map[x,y].name); }
        else
        {
            foreach (GameObject entity in entities)
            {
                if (entity.transform.position.x == x && entity.transform.position.y == y)
                {
                    return Tuple.Create(true, entity.name);
                }
            }
            return Tuple.Create(false, "");
        }
    }


    public int DiagDistance(int x, int y, int x1, int y1)
    {
        return (int)Vector2.Distance(new Vector2(x, y), new Vector2(x1, y1));
    }


    /// <summary>
    /// FOV credit to http://www.roguebasin.com/index.php/Eligloscode Raycasting
    /// </summary>
    public void FOV()
    {
        ClearVisibleTiles();

        float x, y;
        for (int i = 0; i < 360; i++)
        {
            x = (float)Math.Cos(i * .01745f);
            y = (float)Math.Sin(i * .01745f);
            DoFov(x, y);
        }
        RenderTiles();
    }

    void DoFov(float x, float y)
    {
        float ox, oy;
        ox = (float)player.transform.position.x + .5f;
        oy = (float)player.transform.position.y + .5f;
        for (int i = 0; i < fovRadius; i++)
        {
            map[(int)ox, (int)oy].visible = true;
            map[(int)ox, (int)oy].explored = true;
            if (map[(int)ox, (int)oy].isWall)
            {
                return;
            }
            ox += x;
            oy += y;
        }
    }

    void ClearVisibleTiles()
    {
        for (int x = 0; x < mapWidth; x++)
        {
            for (int y = 0; y < mapHeight; y++)
            {
                map[x, y].visible = false;
                //map[x, y].hasEntity = false;
                //map[x, y].walkable = true;
            }
        }
    }


    void RenderTiles()//don't render whole map?
    {
        timer = new Stopwatch();
        timer.Start();

        foreach (GameObject entity in entities)
        {
            entity.GetComponent<TextMeshPro>().enabled = false;
        }
        foreach (GameObject item in items)
        {
            item.GetComponent<TextMeshPro>().enabled = false;
        }

        for (int x = 0; x < mapWidth; x++)
        {
            for (int y = 0; y < mapHeight; y++)
            {
                map[x, y].UpdateTile();
                if(map[x,y].visible)
                {
                    foreach (GameObject entity in entities)
                    {
                        if (entity.transform.position.x == x && entity.transform.position.y == y)
                        {
                            entity.GetComponent<TextMeshPro>().enabled = true;
                        }                        
                    }
                    foreach (GameObject item in items)
                    {
                        if (item.transform.position.x == x && item.transform.position.y == y)
                        {
                            item.GetComponent<TextMeshPro>().enabled = true;
                        }
                    }
                }
            }
        }

        timer.Stop();
        //Debug.Log("Render took:" + timer.Elapsed);
    }


    public void SetWalkable()
    {
        foreach (Tile t in map)
        {
            if (t.isWall)
            {
                t.walkable = false;
            }
            else
            {
                t.walkable = true;
            }
        }
    }

    public void UpdateWalkable()
    {
        foreach (Tile t in map)
        {
            if(!t.isWall)  
            {
                t.walkable = true;
            }        
        }

        foreach (GameObject entity in entities)
        {
            if(entity.name == "Player") { continue; }
            
            map[(int)entity.transform.position.x, (int)entity.transform.position.y].walkable = false;
        }

        grid2.UpdateGrid();
    }
}
