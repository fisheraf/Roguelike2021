using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{    
    public float speed = 1f;
    public Vector3[] path;
    int targetIndex;
    Engine engine;
    Entity entity;


    void Start()
    {
        engine = FindObjectOfType<Engine>();
        entity = GetComponent<Entity>();       
    }

    private void Update()
    {
        
    }

    public void RequestPathForUnit()
    {        
        path = FindObjectOfType<PathFinding>().FindPath(transform.position, engine.player.transform.position);        
    }


    public void OnDrawGizmos()
    {
        if (path != null && !entity.isDead)
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
