using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Item : MonoBehaviour
{
    [SerializeField] string chararacter = null;
    [SerializeField] Color color = Color.white;
    
    [SerializeField] int value;
    public enum ItemType { Null, HealingPotion, LightningScroll }
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
        if(itemType == ItemType.HealingPotion)
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
        return false;
    }
}
