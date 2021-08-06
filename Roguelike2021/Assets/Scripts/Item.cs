using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class Item : MonoBehaviour
{
    [SerializeField] string chararacter = null;
    [SerializeField] Color color = Color.white;
    
    [SerializeField] int value;
    public enum ItemType { Null, HealingPotion, LightningScroll, FireballScroll, ConfusionScroll }
    public ItemType itemType = ItemType.Null;

    Engine engine;

    TextMeshPro textMeshPro;

    private void Awake()
    {
        engine = FindObjectOfType<Engine>();

        textMeshPro = GetComponent<TextMeshPro>();

        textMeshPro.text = chararacter;
        textMeshPro.color = color;
    }

    public bool UseItem()
    {
        if (itemType == ItemType.HealingPotion)
        {
            Fighter fighter = engine.player.GetComponent<Fighter>();
            if (fighter.HP == fighter.maxHP)
            {
                //Debug.Log("full HP");
                engine.uIManager.NewMessage("You are already at full health.");
                return false;
            }
            else
            {
                int healthGained = 0;
                if (fighter.maxHP - fighter.HP < value)
                {
                    healthGained = (fighter.maxHP - fighter.HP);
                }
                else
                {
                    healthGained = value;
                }
                //gameMap.uIManager.NewMessage("You gain <size=200%><voffset=-.2em ><#023788>" + healthGained + "</size></voffset></color> health back from the potion.");
                engine.uIManager.NewMessage("You gain " + healthGained + " health back from the potion.");
                fighter.heal(value);

                return true;
            }
        }
        if (itemType == ItemType.LightningScroll)
        {
            Fighter fighter = engine.player.GetComponent<Fighter>();//add + magic damage from player skill?

            GameObject target = null;
            int closestDistance = 7;
            foreach (GameObject entity in engine.gameMap.entities)
            {
                if (entity.name == "Player") { continue; }
                if (engine.gameMap.map[(int)entity.transform.position.x, (int)entity.transform.position.y].visible)
                {
                    int distanceToEntity = engine.gameMap.DiagDistance((int)engine.player.transform.position.x, (int)engine.player.transform.position.y, (int)entity.transform.position.x, (int)entity.transform.position.y);
                    if (distanceToEntity < closestDistance)
                    {
                        target = entity;
                        closestDistance = distanceToEntity;
                    }
                }
            }
            if (target != null)
            {
                engine.uIManager.NewMessage("You strike " + target.name + " with a bolt of lightning");
                target.GetComponent<Fighter>().takeDamage(value);
                return true;
            }
            else
            {
                engine.uIManager.NewMessage("No targets in range.");
                return false;
            }
        }
        if (itemType == ItemType.FireballScroll)
        {
            StartCoroutine(Targeting(false));
        }
        if(itemType == ItemType.ConfusionScroll)
        {
            StartCoroutine(Targeting(true));
        }
        return false;

    }

    IEnumerator Targeting(bool entityRequired)
    {
        StartCoroutine(engine.targeting.TargetSelect(entityRequired));
        while (engine.targeting.targetSelected == false)
        {
            yield return null;
        }

        if (itemType == ItemType.FireballScroll)
        {
            Fireball(engine.targeting.target);
        }
        if (itemType == ItemType.ConfusionScroll)
        {
            Confuse(engine.targeting.target);
        }
    }

    void Fireball(Vector2Int target)
    {
        //Debug.Log("Fireball cast at " + target);
        int radius = 3;

        for (int i = engine.gameMap.entities.Count - 1; i >= 0; i--)
        {
            int distanceToEntity = engine.gameMap.DiagDistance(target.x, target.y, (int)engine.gameMap.entities[i].transform.position.x, (int)engine.gameMap.entities[i].transform.position.y);
            //Debug.Log(i + "," + distanceToEntity);
            if (distanceToEntity <= radius)
            {
                engine.uIManager.NewMessage("The " + engine.gameMap.entities[i].name + " is engulfed in flames.");
                engine.gameMap.entities[i].GetComponent<Fighter>().takeDamage(value);
            }
        }

        ConsumeItem();
    }


    void Confuse(Vector2Int target)
    {
        foreach (GameObject entity in engine.gameMap.entities)
        {
            if ((target.x == (int)entity.transform.position.x) && (target.y == (int)entity.transform.position.y))
            {
                engine.uIManager.NewMessage("The " + entity.name + " becomes confused.");
                entity.GetComponent<Entity>().isConfused = true;
                entity.GetComponent<Entity>().confusedTurnsLeft = value;
            }
        }

        ConsumeItem();
    }

    private void ConsumeItem()
    {
        engine.inventory.inventory.Remove(this.gameObject);
        engine.inventory.SetUIOrder();
        engine.gameStates.ChangeGameState(GameStates.GameState.EnemyTurn);
    }
}
