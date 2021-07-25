using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class EnemyManager : MonoBehaviour
{    
    GameMap gameMap;
    Grid2 grid2;
    GameStates gameStates;

    List<GameObject> entities;
    GameObject player;


    private void Start()
    {
        gameMap = FindObjectOfType<GameMap>();
        grid2 = FindObjectOfType<Grid2>();
        gameStates = FindObjectOfType<GameStates>();

        entities = gameMap.entities;
        player = gameMap.player;
    }


    public void EnemyTurn()
    {
        Stopwatch sw = new Stopwatch();
        sw.Start();

        foreach (GameObject entity in entities)
        {        
            if (entity.GetComponent<Entity>().isDead) { continue; }

            if (entity.name != "Player")
            {
                //Debug.Log("The " + entity + " ponders the meaning of life.");
                entity.GetComponent<Entity>().hasActed = false;

                if (gameMap.map[(int)entity.transform.position.x, (int)entity.transform.position.y].visible)
                {
                    bool playerInRange = false;
                    
                    List<Node> neighbors = grid2.GetNeighbours(new Node(true, entity.transform.position, (int)entity.transform.position.x, (int)entity.transform.position.y));
                    foreach (Node neighbor in neighbors)
                    {                            
                        if ((Vector2)player.transform.position == (Vector2)neighbor.worldPosition)
                        {
                            playerInRange = true;
                        }
                    }

                    if (playerInRange && !entity.GetComponent<Entity>().hasActed)
                    {
                        //Debug.Log(entity.name + " attacks player");
                        entity.GetComponent<Fighter>().attack(player.GetComponent<Fighter>());
                        entity.GetComponent<Entity>().hasActed = true;
                        continue;
                    }
                    else if (!playerInRange && !entity.GetComponent<Entity>().hasActed)
                    {                        
                        entity.GetComponent<Unit>().RequestPathForUnit();
                        if(entity.GetComponent<Unit>().path.Length > 0)
                        {
                            entity.transform.position = entity.GetComponent<Unit>().path[0];
                        }
                        gameMap.UpdateWalkable();
                        entity.GetComponent<Entity>().hasActed = true;
                        continue;
                    }                    
                }
            }
        }
        gameStates.gameState = GameStates.GameState.PlayerTurn;
        sw.Stop();
        Debug.Log("Enemy turn took:" + sw.Elapsed);
    }
}
