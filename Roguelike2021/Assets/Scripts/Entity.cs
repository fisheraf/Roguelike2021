using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Entity : MonoBehaviour
{
    [SerializeField] string chararacter = null;
    [SerializeField] Color color = Color.white;

    public bool hasActed = true;
    public bool isDead = false;
    public bool isConfused = false;
    public int confusedTurnsLeft;

    Engine engine;

    TextMeshPro textMeshPro;

    private void Awake()
    {
        engine = FindObjectOfType<Engine>();

        textMeshPro = GetComponent<TextMeshPro>();

        textMeshPro.text = chararacter;
        textMeshPro.color = color;
    }

    public void Move(int x, int y)
    {
        Vector2Int targetTile = new Vector2Int((int)transform.position.x + x, (int)transform.position.y + y);


        //if(gameMap.IsBlocked(targetTile.x, targetTile.y).Item1)
        if(!engine.gameMap.map[targetTile.x, targetTile.y].walkable)
        {
            foreach (GameObject entity in engine.gameMap.entities)
            {
                if(targetTile.x == entity.transform.position.x && targetTile.y == entity.transform.position.y)
                {
                    if (entity.name == "Player")
                    {
                        Debug.Log("The player bides its time...");
                        engine.gameStates.ChangeGameState(GameStates.GameState.EnemyTurn);
                        return;
                    }

                    //Debug.Log("You tickle the " + entity.name + ".");
                    GetComponent<Fighter>().attack(entity.GetComponent<Fighter>());

                    engine.gameStates.ChangeGameState(GameStates.GameState.EnemyTurn);
                    return;
                }
            }
            Debug.Log("Tile blocked by " + engine.gameMap.IsBlocked(targetTile.x, targetTile.y).Item2 + ".");
            return;
        }
        
        gameObject.transform.position = new Vector3(targetTile.x, targetTile.y, -5);

        engine.gameStates.ChangeGameState(GameStates.GameState.EnemyTurn);
    }

    public void KillEntity()
    {
        textMeshPro.text = "x";
        textMeshPro.color = Color.red;

        name = "Dead " + name;
        engine.gameMap.entities.Remove(this.gameObject);
        //FindObjectOfType<GameMap>().deadEntities.Add(gameObject);
        isDead = true;
        engine.gameMap.UpdateWalkable();
    }
}
