using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using DG.Tweening;

public class UIManager : MonoBehaviour
{
    public Engine engine;

    public TextMeshProUGUI[] messageLog;
    public List<string> messageLogList = new List<string>();

    public Slider healthSlider;
    public TextMeshProUGUI healthValues;
    public TextMeshProUGUI entityText;

    public GameObject inventoryPanel;
    public GameObject levelPanel;


    private void Start()
    {
        engine = GetComponent<Engine>();
    }

    private void LateUpdate()
    {
        MouseOverTooltips();
    }

    public void NewMessage(string message)
    {
        messageLogList.Add(message);

        for (int i = messageLog.Length - 1; i > 0; i--)
        {
            messageLog[i].text = messageLog[i - 1].text;
        }
        messageLog[0].text = message;
    }

    public void SetPlayerHealth(int currentHP, int maxHP)
    {
        healthSlider.maxValue = maxHP;
        healthSlider.value = currentHP;
        healthValues.text = (currentHP + "/" + maxHP);
    }

    private void MouseOverTooltips()
    {
        Vector3 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //fixed center issue
        worldPoint = new Vector3(worldPoint.x + .5f, worldPoint.y + .5f);
        Vector2Int worldPointInt = new Vector2Int((int)worldPoint.x, (int)worldPoint.y);
        worldPoint = new Vector3(worldPointInt.x, worldPointInt.y);
        bool mouseIsOverSomething = false;

        //foreach player, item, dead entity other object that dont have entity
        foreach (GameObject item in engine.gameMap.items)
        {
            if (item.transform.position.Equals(worldPoint))
            {
                if (engine.gameMap.map[worldPointInt.x, worldPointInt.y].visible)
                {
                    MouseOverText(item.GetComponent<Item>().name);
                    mouseIsOverSomething = true;

                    //flavor text?
                }
            }
        }
        foreach (GameObject item in engine.inventory.inventory)
        {
            if (item.transform.position.Equals(worldPoint))
            {
                MouseOverText(item.GetComponent<Item>().name);
                mouseIsOverSomething = true;

                //flavor text?
            }
        }

        foreach (GameObject entity in engine.gameMap.entities)
        {
            //if (entity.transform.position.Equals(worldPoint))   z error
            if (entity.transform.position.x == worldPoint.x && entity.transform.position.y == worldPoint.y)
            {
                if (engine.gameMap.map[worldPointInt.x, worldPointInt.y].visible)
                {
                    MouseOverText(entity.GetComponent<Entity>().name);
                    mouseIsOverSomething = true;

                    //flavor text?
                }
            }
        }


        /*
        foreach (GameObject deadEntity in deadEntities)
        {
            if (deadEntity.transform.position.x == (int)worldPoint.x && deadEntity.transform.position.y == (int)worldPoint.y)
            {
                if (gameMap.tileVisible[(int)worldPoint.x, (int)worldPoint.y])
                {
                    uiManager.MouseOverText(deadEntity.GetComponent<Entity>().name);
                    mouseIsOverSomething = true;

                    //flavor text?
                }
            }
        }*/

        if (!mouseIsOverSomething)
        {
            MouseOverText(null);
        }
    }

    public void MouseOverText(string text)
    {
        entityText.text = text;
    }

    public void ClearMessages()
    {
        messageLogList.Clear();
        foreach (TextMeshProUGUI tm in messageLog)
        {
            tm.text = "";
        }
    }

    public void OpenLevelPanel()
    {
        inventoryPanel.transform.DORotate(new Vector3(0, -90, 0), .5f);
        inventoryPanel.SetActive(false);
        levelPanel.SetActive(true);
        levelPanel.transform.rotation = Quaternion.Euler(0, 90, 0);
        levelPanel.transform.DORotate(new Vector3(0, 0, 0), .5f);
    }

    public void OpenInventoryPanel()
    {
        levelPanel.transform.DORotate(new Vector3(0, -90, 0), .5f);
        levelPanel.SetActive(false);
        inventoryPanel.SetActive(true);
        inventoryPanel.transform.rotation = Quaternion.Euler(0, 90, 0);
        inventoryPanel.transform.DORotate(new Vector3(0, 0, 0), .5f);
    }
}
