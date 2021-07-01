using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Tile : MonoBehaviour
{
    [SerializeField] string character = null;
    [SerializeField] Color litWall;
    [SerializeField] Color unlitWall;
    [SerializeField] Color litFloor;
    [SerializeField] Color unlitFloor;
    [SerializeField] Color characterColor;
    public bool walkable;
    public bool isWall;
    [SerializeField] bool transparent;
    public bool visible;
    public bool explored;

    SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        UpdateTile();
    }


    public void UpdateTile()
    {
        TextMeshPro textMeshPro = GetComponentInChildren<TextMeshPro>();
        if (!explored)
        {
            spriteRenderer.color = Color.black;
            return;
        }
        if (visible)
        {
            if (isWall)
            {
                spriteRenderer.color = litWall;
            }
            else
            {
                spriteRenderer.color = litFloor;
            }
            textMeshPro.text = character;
            textMeshPro.color = characterColor;
        }
        else
        {
            if (isWall)
            {
                spriteRenderer.color = unlitWall;
            }
            else
            {
                spriteRenderer.color = unlitFloor;
            }
            textMeshPro.text = character;
            textMeshPro.color = characterColor;
        }
    }

    public void UpdateTile(string _character, Color _backgroundColor, Color _characterColor , bool _walkable)
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
        isWall = false;
    }
    public void SetToWall()
    {
        UpdateTile("#", new Color32(105, 105, 105, 255), Color.black, false);
        isWall = true;
    }
}
