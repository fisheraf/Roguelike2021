using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Entity : MonoBehaviour
{
    [SerializeField] string chararacter = null;
    [SerializeField] Color color = Color.white;

    GameMap gameMap;

    private void Awake()
    {
        gameMap = FindObjectOfType<GameMap>();

        TextMeshPro textMeshPro = GetComponent<TextMeshPro>();
        textMeshPro.text = chararacter;
        textMeshPro.color = color;
    }

    public void Move(int x, int y)
    {
        if(gameMap.isBlocked((int)gameObject.transform.position.x + x, (int)gameObject.transform.position.y + y))
        {
            Debug.Log("tile blocked");
            return;
        }
        gameObject.transform.position = new Vector2(gameObject.transform.position.x + x, gameObject.transform.position.y + y);
    }

}
