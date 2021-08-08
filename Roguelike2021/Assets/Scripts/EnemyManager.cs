using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class EnemyManager : MonoBehaviour
{
    Engine engine;
    Grid2 grid2;

    List<GameObject> entities;


    private void Start()
    {
        engine = GetComponent<Engine>();
        grid2 = FindObjectOfType<Grid2>();

        entities = engine.gameMap.entities;
    }


    public void EnemyTurn()
    {
        Stopwatch sw = new Stopwatch();
        sw.Start();

        foreach (GameObject entity in entities)
        {        
            if (entity.GetComponent<Entity>().isDead) { continue; }
            if (entity.name == "Player") { continue; }            
           
            //Debug.Log("The " + entity + " ponders the meaning of life.");
            entity.GetComponent<Entity>().hasActed = false;


            if (entity.GetComponent<Entity>().isConfused)
            {
                entity.GetComponent<Entity>().confusedTurnsLeft--;
                if (entity.GetComponent<Entity>().confusedTurnsLeft < 0)
                {
                    engine.uIManager.NewMessage("The " + entity.name + " is no longer confused");
                    entity.GetComponent<Entity>().isConfused = false;
                    entity.GetComponent<Entity>().hasActed = true;
                }

                List<Vector2> rTarget = new List<Vector2>();
                rTarget.Clear();

                for (int x = (int)entity.transform.position.x - 1; x < (int)entity.transform.position.x + 1; x++)
                {
                    for (int y = (int)entity.transform.position.y - 1; y < (int)entity.transform.position.y + 1; y++)
                    {

                        if (engine.gameMap.map[x,y].walkable)
                        {
                            rTarget.Add(new Vector2(x, y));
                        }
                    }
                }
                rTarget.Add(new Vector2((int)entity.transform.position.x, (int)entity.transform.position.y));
                int randomNumber = UnityEngine.Random.Range(0, rTarget.Count);

                Vector3 randomTarget = rTarget[randomNumber];
                entity.transform.position = randomTarget;
                Debug.Log(randomTarget);
                entity.GetComponent<Entity>().hasActed = true;
            }


            if (engine.gameMap.map[(int)entity.transform.position.x, (int)entity.transform.position.y].visible)
            {
                bool playerInRange = false;
                    
                List<Node> neighbors = grid2.GetNeighbours(new Node(true, entity.transform.position, (int)entity.transform.position.x, (int)entity.transform.position.y));
                foreach (Node neighbor in neighbors)
                {                            
                    if ((Vector2)engine.player.transform.position == (Vector2)neighbor.worldPosition)
                    {
                        playerInRange = true;
                    }
                }

                if (playerInRange && !entity.GetComponent<Entity>().hasActed)
                {
                    //Debug.Log(entity.name + " attacks player");
                    entity.GetComponent<Fighter>().attack(engine.player.GetComponent<Fighter>());
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
                    engine.gameMap.UpdateWalkable();
                    entity.GetComponent<Entity>().hasActed = true;
                    continue;
                }                    
            }            
        }        
        engine.gameStates.ChangeGameState(GameStates.GameState.PlayerTurn);
        sw.Stop();
        Debug.Log("Enemy turn took:" + sw.Elapsed);
    }
}
