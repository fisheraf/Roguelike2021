using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Entity : MonoBehaviour
{
    [SerializeField] string chararacter = null;
    [SerializeField] Color color = Color.white;

    private void Awake()
    {
        TextMeshPro textMeshPro = GetComponent<TextMeshPro>();
        textMeshPro.text = chararacter;
        textMeshPro.color = color;
    }

    public void Move(int x, int y)
    {
        gameObject.transform.position = new Vector2(gameObject.transform.position.x + x, gameObject.transform.position.y + y);
    }

}
