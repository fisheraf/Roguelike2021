using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public Transform target;
    public float speed = 1f;
    public Vector3[] path;
    int targetIndex;
    Entity entity;
    GameMap gameMap;


    void Start()
    {  
        target = GameObject.Find("Player").transform;
        entity = GetComponent<Entity>();
        gameMap = FindObjectOfType<GameMap>();        
    }

    private void Update()
    {
        
    }

    public void RequestPathForUnit()
    {        
        path = FindObjectOfType<PathFinding>().FindPath(transform.position, target.position);        
    }


    public void OnDrawGizmos()
    {
        if (path != null)
        {
            for (int i = targetIndex; i < path.Length; i++)
            {
                Gizmos.color = new Color(0, 1, 0, .2f);
                Gizmos.DrawCube(new Vector3(path[i].x, path[i].y), Vector3.one);

                if (i == targetIndex)
                {
                    Gizmos.DrawLine(transform.position, path[i]);
                }
                else
                {
                    Gizmos.DrawLine(path[i - 1], path[i]);
                }
            }
        }
    }
}
