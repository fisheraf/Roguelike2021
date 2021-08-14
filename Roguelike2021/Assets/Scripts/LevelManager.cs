using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public int currentLevel;
    public int currentXP;
    public int levelUpBase;
    public int levelUpFactor;

    Engine engine;

    private void Start()
    {
        engine = FindObjectOfType<Engine>();
    }

    public int experienceToNextLevel()
    {
        return levelUpBase + (currentLevel * levelUpFactor);
    }

    public void addXP(int XP)
    {
        currentXP += XP;

        if (currentXP >= experienceToNextLevel())
        {
            currentXP -= experienceToNextLevel();
            currentLevel++;
            FindObjectOfType<UIManager>().NewMessage("You feel stronger! You reached level: " + currentLevel);
            engine.gameStates.ChangeGameState(GameStates.GameState.LevelUp);
        }
    }

    public void StatChange(int i)
    {
        switch (i)
        {
            case 0:                
                engine.player.GetComponent<Fighter>().maxHP += 20;
                engine.player.GetComponent<Fighter>().HP += 20;
                engine.uIManager.SetPlayerHealth(engine.player.GetComponent<Fighter>().HP, engine.player.GetComponent<Fighter>().maxHP);
                break;
            case 1:
                engine.player.GetComponent<Fighter>().basePower += 1;
                break;
            case 2:
                engine.player.GetComponent<Fighter>().baseDefense += 1;
                break;
        }

        engine.gameStates.ChangeGameState(GameStates.GameState.PlayerTurn);
    }
}
