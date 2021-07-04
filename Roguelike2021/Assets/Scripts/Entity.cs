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
        if(gameMap.IsBlocked((int)gameObject.transform.position.x + x, (int)gameObject.transform.position.y + y).Item1)
        {
            Debug.Log("Tile blocked by " + gameMap.IsBlocked((int)gameObject.transform.position.x + x, (int)gameObject.transform.position.y + y).Item2);
            return;
        }
        gameObject.transform.position = new Vector2(gameObject.transform.position.x + x, gameObject.transform.position.y + y);
        gameStates.gameState = GameStates.GameState.EnemyTurn;
        gameMap.EnemyTurn();
    }
}
