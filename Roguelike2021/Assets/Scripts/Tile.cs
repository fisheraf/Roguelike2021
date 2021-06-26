using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Tile : MonoBehaviour
{
    [SerializeField] string chararacter = null;
    [SerializeField] Color backgroundColor = Color.grey;
    [SerializeField] Color characterColor = Color.black;
    bool walkable;
    bool transparent;
    bool dark;

    private void Awake()
    {
        GetComponent<SpriteRenderer>().color = backgroundColor;
        TextMeshPro textMeshPro = GetComponentInChildren<TextMeshPro>();
        textMeshPro.text = chararacter;
        textMeshPro.color = characterColor;
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
