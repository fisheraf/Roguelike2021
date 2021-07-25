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

    GameMap gameMap;
    GameStates gameStates;
    EnemyManager enemyManager;

    TextMeshPro textMeshPro;

    private void Awake()
    {
        gameMap = FindObjectOfType<GameMap>();
        gameStates = FindObjectOfType<GameStates>();
        enemyManager = FindObjectOfType<EnemyManager>();

        textMeshPro = GetComponent<TextMeshPro>();

        textMeshPro.text = chararacter;
        textMeshPro.color = color;
    }

    public void Move(int x, int y)
    {
        Vector2Int targetTile = new Vector2Int((int)transform.position.x + x, (int)transform.position.y + y);


        //if(gameMap.IsBlocked(targetTile.x, targetTile.y).Item1)
        if(!gameMap.map[targetTile.x, targetTile.y].walkable)
        {
            foreach (GameObject entity in gameMap.entities)
            {
                if(targetTile.x == entity.transform.position.x && targetTile.y == entity.transform.position.y)
                {
                    if (entity.name == "Player")
                    {
                        Debug.Log("The player bides its time...");
                        gameStates.gameState = GameStates.GameState.EnemyTurn;
                        enemyManager.EnemyTurn();
                        return;
                    }

                    Debug.Log("You tickle the " + entity.name + ".");
                    GetComponent<Fighter>().attack(entity.GetComponent<Fighter>());

                    gameStates.gameState = GameStates.GameState.EnemyTurn;
                    enemyManager.EnemyTurn();
                    return;
                }
            }
            Debug.Log("Tile blocked by " + gameMap.IsBlocked(targetTile.x, targetTile.y).Item2 + ".");
            return;
        }
        
        gameObject.transform.position = new Vector3(targetTile.x, targetTile.y, -5);        

        gameStates.gameState = GameStates.GameState.EnemyTurn;
        enemyManager.EnemyTurn();
    }

    public void KillEntity()
    {
        textMeshPro.text = "x";
        textMeshPro.color = Color.red;

        name = "Dead " + name;
        gameMap.entities.Remove(this.gameObject);
        //FindObjectOfType<GameMap>().deadEntities.Add(gameObject);
        isDead = true;
        gameMap.UpdateWalkable();
    }
}
