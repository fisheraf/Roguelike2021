using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStates : MonoBehaviour
{
    Engine engine;

    public enum GameState { PlayerTurn, EnemyTurn, DropItem}
    public GameState gameState = GameState.PlayerTurn;

    private void Start()
    {
        engine = GetComponent<Engine>();
    }

    public void ChangeGameState(GameState gs)
    {
        gameState = gs;
        if(gs == GameState.EnemyTurn)
        {
            engine.gameMap.FOV();
            engine.enemyManager.EnemyTurn();
        }

        Debug.Log("Game state change to " + gs);
    }
}
