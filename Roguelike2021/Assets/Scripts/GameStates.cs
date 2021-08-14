using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStates : MonoBehaviour
{
    Engine engine;

    public enum GameState { PlayerTurn, EnemyTurn, DropItem, Targeting, LevelUp}
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
        if (lastState == GameState.LevelUp && gs == GameState.EnemyTurn)
        {
            gameState = GameState.LevelUp;
            return;
        }

        if (gs == GameState.EnemyTurn)
        {
            engine.gameMap.FOV();
            engine.enemyManager.EnemyTurn();
        }
        if(gs == GameState.LevelUp)
        {
            engine.uIManager.OpenLevelPanel();
        }
        if (gs != GameState.LevelUp && lastState == GameState.LevelUp)
        {
            engine.uIManager.OpenInventoryPanel();
        }


        //Debug.Log("Game state change to " + gs);
    }

    public void LastGameState()
    {
        ChangeGameState(lastState);
    }
}
