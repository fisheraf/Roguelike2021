using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStates : MonoBehaviour
{
    Engine engine;

    public enum GameState { PlayerTurn, EnemyTurn, DropItem, Targeting}
    public GameState gameState = GameState.PlayerTurn;

    GameState lastState;

    private void Start()
    {
        engine = GetComponent<Engine>();
    }

    public void ChangeGameState(GameState gs)
    {
        lastState = gameState;
        gameState = gs;
        if(gs == GameState.EnemyTurn)
        {
            engine.gameMap.FOV();
            engine.enemyManager.EnemyTurn();
        }

        //Debug.Log("Game state change to " + gs);
    }

    public void LastGameState()
    {
        ChangeGameState(lastState);
    }
}
