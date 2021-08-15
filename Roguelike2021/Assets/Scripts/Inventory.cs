using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using System.Linq;

public class Inventory : MonoBehaviour
{    
    Engine engine;
    public List<GameObject> inventory = new List<GameObject>();
    public TextMeshProUGUI[] inventorySlots;

    // Start is called before the first frame update
    void Start()
    {        
        engine = FindObjectOfType<Engine>();        
    }

    public void PickUpItem()
    {
        bool itemPickedUp = false;

        foreach (GameObject item in engine.gameMap.items)
        {
            if (item.transform.position.Equals((Vector2)engine.player.transform.position))
            {                
                for (int i = 0; i < inventorySlots.Length; i++)
                {
                    if(inventorySlots[i].text == "")
                    {
                        inventory.Add(item);
                        //item.transform.position = inventorySlots[i].gameObject.transform.position;
                        inventorySlots[i].text = item.GetComponent<TextMeshPro>().text;
                        inventorySlots[i].color = item.GetComponent<TextMeshPro>().color;
                        break;
                    }
                    if (i == inventorySlots.Length - 1)
                    {
                        Debug.Log("Inventory full.");
                        return;
                    }
                    else
                    {
                        continue;
                    }

                }

                engine.uIManager.NewMessage("You pick up " + item.name);
                item.transform.position = new Vector2(-20, 0);
                engine.gameMap.items.Remove(item);                
                itemPickedUp = true;
                engine.gameStates.ChangeGameState(GameStates.GameState.EnemyTurn);
                break;
            }
        }

        if (!itemPickedUp)
        {
            engine.uIManager.NewMessage("There is nothing to pickup.");
        }
    }

    public void UseItem(int number)
    {
        try
        {
            if (inventory[number].GetComponent<Item>().UseItem())
            {
                GameObject item = inventory[number];
                inventory.Remove(item);
                Destroy(item);
                SetUIOrder();
                engine.gameStates.ChangeGameState(GameStates.GameState.EnemyTurn);
            }
        }
        catch (ArgumentOutOfRangeException)
        {
            engine.uIManager.NewMessage("There is no item in that slot.");
            return;
        }
    }

    public void DropItem(int number)
    {
        try
        {
            GameObject item = inventory[number];
            engine.gameMap.items.Add(item);
            item.transform.position = new Vector3(engine.player.transform.position.x, engine.player.transform.position.y, 0);
            inventory.Remove(item);
            SetUIOrder();
            engine.uIManager.NewMessage("You drop " + item.name);
            engine.gameStates.ChangeGameState(GameStates.GameState.EnemyTurn);            
        }
        catch (ArgumentOutOfRangeException)
        {
            engine.uIManager.NewMessage("There is no item in that slot.");
            return;
        }
    }

    public void SetUIOrder()
    {
        foreach (TextMeshProUGUI slot in inventorySlots)
        {
            slot.text = "";
        }
        for (int i = 0; i < inventory.Count; i++)
        {
            inventorySlots[i].text = inventory[i].GetComponent<TextMeshPro>().text;
            inventorySlots[i].color = inventory[i].GetComponent<TextMeshPro>().color;
        }
    }
}
