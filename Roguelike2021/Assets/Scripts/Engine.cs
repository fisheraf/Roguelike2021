using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Engine : MonoBehaviour
{
    public GameObject player;

    public GameMap gameMap;
    public GameStates gameStates;
    public UIManager uIManager;
    public Inventory inventory;
    public EnemyManager enemyManager;

    private void Awake()
    {
        player = GameObject.Find("Player");

        gameMap = GetComponent<GameMap>();
        gameStates = GetComponent<GameStates>();
        uIManager = GetComponent<UIManager>();
        inventory = GetComponent<Inventory>();
        enemyManager = GetComponent<EnemyManager>();
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
