using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Tile : MonoBehaviour
{
    [SerializeField] string character = null;
    [SerializeField] Color backgroundColor = Color.grey;
    [SerializeField] Color characterColor = Color.black;
    public bool walkable;
    [SerializeField] bool transparent;
    [SerializeField] bool dark;

    private void Awake()
    {
        UpdateTile(character, backgroundColor, characterColor, false);
    }

    public void UpdateTile(string _character, Color _backgroundColor, Color _characterColor, bool _walkable)
    {
        GetComponent<SpriteRenderer>().color = _backgroundColor;
        TextMeshPro textMeshPro = GetComponentInChildren<TextMeshPro>();
        textMeshPro.text = _character;
        textMeshPro.color = _characterColor;
        walkable = _walkable;
    }

    public void SetToFloor()
    {
        UpdateTile(".", new Color32(155, 155, 155, 255), Color.black, true);
    }
    public void SetToWall()
    {
        UpdateTile("#", new Color32(155, 155, 155, 255), Color.black, false);
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
