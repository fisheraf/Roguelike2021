using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Entity : MonoBehaviour
{
    [SerializeField] string chararacter = null;
    [SerializeField] Color color = Color.white;

    GameMap gameMap;
    GameStates gameStates;

    private void Awake()
    {
        gameMap = FindObjectOfType<GameMap>();
        gameStates = FindObjectOfType<GameStates>();

        TextMeshPro textMeshPro = GetComponent<TextMeshPro>();
        textMeshPro.text = chararacter;
        textMeshPro.color = color;
    }

    public void Move(int x, int y)
    {
        Vector2Int targetTile = new Vector2Int((int)transform.position.x + x, (int)transform.position.y + y);


        if(gameMap.IsBlocked(targetTile.x, targetTile.y).Item1)
        {
            foreach (GameObject entity in gameMap.entities)
            {
                if(targetTile.x == entity.transform.position.x && targetTile.y == entity.transform.position.y)
                {
                    Debug.Log("You tickle the " + entity.name + ".");

                    gameStates.gameState = GameStates.GameState.EnemyTurn;
                    gameMap.EnemyTurn();
                    return;
                }
            }
            Debug.Log("Tile blocked by " + gameMap.IsBlocked(targetTile.x, targetTile.y).Item2 + ".");
            return;
        }
        gameObject.transform.position = new Vector2(targetTile.x, targetTile.y);
        gameStates.gameState = GameStates.GameState.EnemyTurn;
        gameMap.EnemyTurn();
    }
}
