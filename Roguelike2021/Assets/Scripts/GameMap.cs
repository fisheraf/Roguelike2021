using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMap : MonoBehaviour
{
    [Tooltip("Needs to be multiple of 2")]
    public int mapWidth = 90;
    [Tooltip("Needs to be multiple of 2")]
    public int mapHeight = 50;

    Tile floorTile;
    Tile wallTile;

    [SerializeField] GameObject floorTileObject;
    [SerializeField] GameObject wallTileObject;


    public GameObject[,] map;

    private void Awake()
    {
        floorTile = floorTileObject.GetComponent<Tile>();
        wallTile = wallTileObject.GetComponent<Tile>();

        map = new GameObject[mapWidth, mapHeight];

        FillMap();
    }


    void FillMap()
    {
        for (int x = 0; x < mapWidth; x++)
        {
            for (int y = 0; y < mapHeight; y++)
            {
                map[x, y] = wallTileObject;
            }
        }

        map[2, 2] = floorTileObject;
        map[3, 2] = floorTileObject;
        map[4, 2] = floorTileObject;

        ShowMap();
    }

    void ShowMap()
    {
        for (int x = 0; x < mapWidth; x++)
        {
            for (int y = 0; y < mapHeight; y++)
            {
                Instantiate(map[x,y], new Vector2(x, y), Quaternion.identity, gameObject.transform);
            }
        }
    }
}
