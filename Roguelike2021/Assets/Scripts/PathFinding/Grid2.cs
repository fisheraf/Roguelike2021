using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid2 : MonoBehaviour
{
    public bool displayGridGizmos;
    public Vector2 gridWorldSize;
    public float nodeRadius;
    Node[,] grid;

    float nodeDiameter = .45f;
    int gridSizeX, gridSizeY;

    public GameMap gameMap;

    void Awake()
    {
        gameMap = FindObjectOfType<GameMap>();
        nodeDiameter = nodeRadius * 2;
    }

    public int MaxSize
    {
        get
        {
            return gridSizeX * gridSizeY;
        }
    }

    public void CreateGrid()
    {
        gridSizeX = gameMap.mapWidth;
        gridSizeY = gameMap.mapHeight;

        grid = new Node[gridSizeX, gridSizeY];
        Vector3 worldBottomLeft = new Vector3(0, 0, -1f);

        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {                
                Vector3 worldPoint = worldBottomLeft + ((Vector3.right * x) + (Vector3.up * y));
                bool walkable = gameMap.map[x,y].walkable;
                grid[x, y] = new Node(walkable, worldPoint, x, y);
            }
        }        
    }

    public void UpdateGrid()
    {
        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {                
                grid[x,y].walkable = gameMap.map[x, y].walkable;                
            }
        }
    }

    public List<Node> GetNeighbours(Node node)
    {
        List<Node> neighbours = new List<Node>();

        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if (x == 0 && y == 0)
                    continue;

                int checkX = node.gridX + x;
                int checkY = node.gridY + y;

                if (checkX >= 0 && checkX < gridSizeX && checkY >= 0 && checkY < gridSizeY)
                {
                    neighbours.Add(grid[checkX, checkY]);
                }
            }
        }

        return neighbours;
    }


    public Node NodeFromWorldPoint(Vector3 worldPosition)
    {
        //Debug.Log(worldPosition.x + "," + worldPosition.y);
        int x = (int)worldPosition.x;
        int y = (int)worldPosition.y;

        return grid[x, y];
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(new Vector3((gameMap.mapWidth / 2) - .5f, (gameMap.mapHeight / 2) - .5f, 0), new Vector3(gridWorldSize.x, gridWorldSize.y, 1));

        if (grid != null && displayGridGizmos)
        {
            foreach (Node n in grid)
            {
                Gizmos.color = (n.walkable) ? new Color(.5f, .5f, 1, .2f) : new Color(1, 0, 0, .2f);

                Gizmos.DrawCube(new Vector3(n.worldPosition.x , n.worldPosition.y , n.worldPosition.z), Vector3.one * (nodeDiameter - .1f));
            }
        }
    }
}
