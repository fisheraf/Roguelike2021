using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStates : MonoBehaviour
{
    public enum GameState { PlayerTurn, EnemyTurn}
    public GameState gameState = GameState.PlayerTurn;
}
