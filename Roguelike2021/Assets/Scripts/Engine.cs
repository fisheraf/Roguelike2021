using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BayatGames.SaveGameFree;
using System.Diagnostics;
using Debug = UnityEngine.Debug;

public class Engine : MonoBehaviour
{
    public GameObject player;

    public GameMap gameMap;
    public GameStates gameStates;
    public UIManager uIManager;
    public Inventory inventory;
    public EnemyManager enemyManager;
    public Targeting targeting;
    public LevelManager levelManager;


    int totalTiles;
    bool[] isWalkableSingle;
    bool[] tileIsWallSingle;
    bool[] tileVisibleSingle;
    bool[] tileExploredSingle;

    private void Awake()
    {
        player = GameObject.Find("Player");

        gameMap = GetComponent<GameMap>();
        gameStates = GetComponent<GameStates>();
        uIManager = GetComponent<UIManager>();
        inventory = GetComponent<Inventory>();
        enemyManager = GetComponent<EnemyManager>();
        targeting = GetComponent<Targeting>();
        levelManager = GetComponent<LevelManager>();
    }

    // Start is called before the first frame update
    void Start()
    {
        totalTiles = gameMap.mapHeight * gameMap.mapWidth;

        isWalkableSingle = new bool[totalTiles];
        tileIsWallSingle = new bool[totalTiles];
        tileVisibleSingle = new bool[totalTiles];
        tileExploredSingle = new bool[totalTiles];
    }

    // Update is called once per frame
    void Update()
    {
        if (player == null) { player = GameObject.Find("Player"); }
    }

    

    public void SaveProgress()
    {
        Stopwatch sw = new Stopwatch();
        sw.Start();

        Debug.Log("Game saved");

        SaveGame.Save<Vector3>("Player Location", player.transform.position);

        Fighter playerFighter = player.GetComponent<Fighter>();
        SaveGame.Save<int>("maxHP", playerFighter.maxHP);
        SaveGame.Save<int>("hp", playerFighter.HP);
        SaveGame.Save<int>("defense", playerFighter.baseDefense);
        SaveGame.Save<int>("power", playerFighter.basePower);

        /*Level playerLevel = playerObject.GetComponent<Level>();
        SaveGame.Save<int>("currentLevel", playerLevel.currentLevel);
        SaveGame.Save<int>("currentXP", playerLevel.currentXP);

        SaveGame.Save<int>("dungeonLevel", dungeonLevel);
        */

        List<int> inventoryItemIDNumber = new List<int>();
        for (int j = 0; j < inventory.inventory.Count; j++)
        {
            inventoryItemIDNumber.Add(inventory.inventory[j].GetComponent<Item>().itemNumber);
        }
        SaveGame.Save<List<int>>("Inventory", inventoryItemIDNumber);

        /*
        Equipment equipment = FindObjectOfType<Equipment>();
        equipment.SaveEquipped();
        SaveGame.Save<List<int>>("Player Eq", equipment.eqIDNumber);
        SaveGame.Save<List<bool>>("isEquippedList", equipment.isEquippedList);
        */

        //map
        
        /*
        bool[] hasEntitySingle = new bool[x];
        bool[] hasItemSingle = new bool[x];
        */

        int i = 0;

        for (int x = 0; x < gameMap.mapWidth; x++)
        {
            for (int y = 0; y < gameMap.mapHeight; y++)
            {
                //Debug.Log(i + "," + x + "," + y);
                isWalkableSingle[i] = gameMap.map[x, y].walkable;
                tileIsWallSingle[i] = gameMap.map[x, y].isWall;
                tileVisibleSingle[i] = gameMap.map[x, y].visible;
                tileExploredSingle[i] = gameMap.map[x, y].explored;
                /*
                hasEntitySingle[i] = gameMap.hasEntity[x, y];
                hasItemSingle[i] = gameMap.hasItem[x, y];
                */

                i++;
            }
        }

        SaveGame.Save<bool[]>("isWalkable", isWalkableSingle);
        SaveGame.Save<bool[]>("tileIsWall", tileIsWallSingle);
        SaveGame.Save<bool[]>("tileVisible", tileVisibleSingle);
        SaveGame.Save<bool[]>("tileExplored", tileExploredSingle);
        /*
        SaveGame.Save<bool[]>("hasEntity", hasEntitySingle);
        SaveGame.Save<bool[]>("hasItem", hasItemSingle);
        */


        //entities
        //CreateEntity(int x, int y, int z, int spriteNumber, Color32 color, string name, int HP, int defense, int power)

        List<int> xPositonEntity = new List<int>();
        List<int> yPositionEntity = new List<int>();
        List<int> entityNumber = new List<int>();
        List<int> hpe = new List<int>();

        foreach (GameObject entity in gameMap.entities)
        {
            xPositonEntity.Add((int)entity.transform.position.x);
            yPositionEntity.Add((int)entity.transform.position.y);
            entityNumber.Add(entity.GetComponent<Entity>().entityNumber);
            hpe.Add(entity.GetComponent<Fighter>().HP);
        }

        SaveGame.Save<List<int>>("xpos", xPositonEntity);
        SaveGame.Save<List<int>>("ypos", yPositionEntity);
        SaveGame.Save<List<int>>("entityNumber", entityNumber);
        SaveGame.Save<List<int>>("hpe", hpe);
        //SaveGame.Save<List<GameObject>>("equipment", equipment);

        //deadentities


        //items
        List<int> xPostionItem = new List<int>();
        List<int> yPositionItem = new List<int>();
        List<int> itemIDNumber = new List<int>();

        itemIDNumber.Clear();
        for (int j = 0; j < gameMap.items.Count; j++)
        {
            itemIDNumber.Add(gameMap.items[j].GetComponent<Item>().itemNumber);

            xPostionItem.Add((int)gameMap.items[j].transform.position.x);
            yPositionItem.Add((int)gameMap.items[j].transform.position.y);
        }

        SaveGame.Save<List<int>>("itemIDNumber", itemIDNumber);
        SaveGame.Save<List<int>>("xposi", xPostionItem);
        SaveGame.Save<List<int>>("yposi", yPositionItem);

        //stairs
        /*SaveGame.Save<int>("stairX", (int)stairsObj.transform.position.x);
        SaveGame.Save<int>("stairY", (int)stairsObj.transform.position.y);
        SaveGame.Save<bool>("stairSeen", stairsObj.GetComponent<Stairs>().hasBeenSeen);
        */

        SaveGame.Save<List<string>>("Message Log", uIManager.messageLogList);

        sw.Stop();
        Debug.Log("Saving took:" + sw.Elapsed);
    }

    public void LoadProgress() {
        Stopwatch sw = new Stopwatch();
        sw.Start();
                
        foreach (GameObject e in gameMap.entities)
        {
            Destroy(e);
        }
        gameMap.entities.Clear();
        foreach (GameObject i in gameMap.items)
        {
            Destroy(i);
        }
        gameMap.items.Clear();
        foreach (GameObject i in inventory.inventory)
        {
            Destroy(i);
        }
        inventory.inventory.Clear();

        uIManager.ClearMessages();

        Debug.Log("Game Loaded");
        //player
        player = null;
        Vector3 playerLocation = SaveGame.Load<Vector3>("Player Location");
        gameMap.CreatePlayer((int)playerLocation.x, (int)playerLocation.y, (int)playerLocation.z,
            SaveGame.Load<int>("hp"), 
            SaveGame.Load<int>("maxHP"),
            SaveGame.Load<int>("defense"),
            SaveGame.Load<int>("power"));

        player = GameObject.Find("Player");

        //Level playerLevel = playerObject.GetComponent<Level>();

        //playerLevel.currentLevel = SaveGame.Load<int>("currentLevel");
        //playerLevel.currentXP = SaveGame.Load<int>("currentXP");

        //dungeonLevel = SaveGame.Load<int>("dungeonLevel");

        uIManager.SetPlayerHealth(SaveGame.Load<int>("hp"), SaveGame.Load<int>("maxHP"));
        //uIManager.SetUIText();

        
        List<int> inventoryItemIDNumber = SaveGame.Load<List<int>>("Inventory");

        for (int i = 0; i < inventoryItemIDNumber.Count; i++)
        {
            if (inventoryItemIDNumber[i] < 100)
            {
                gameMap.CreateItem(-20, 0, inventoryItemIDNumber[i]);
                Debug.Log(inventoryItemIDNumber[i]);
                inventory.inventory.Add(gameMap.items[i]);
                gameMap.items.RemoveAt(0);
            }
            else if (inventoryItemIDNumber[i] < 200)
            {
                //CreateEquipment(xposi[i], yposi[i], itemIDNumber[i]);
            }
        }
        inventory.SetUIOrder();

        //Equipment equipment = FindObjectOfType<Equipment>();
        //equipment.eqIDNumber = SaveGame.Load<List<int>>("Player Eq");
        //equipment.isEquippedList = SaveGame.Load<List<bool>>("isEquippedList");
        //equipment.LoadEquipped();


        //map
        isWalkableSingle = SaveGame.Load<bool[]>("isWalkable");
        tileIsWallSingle = SaveGame.Load<bool[]>("tileIsWall");
        tileVisibleSingle = SaveGame.Load<bool[]>("tileVisible");
        tileExploredSingle = SaveGame.Load<bool[]>("tileExplored");      
        
        /*
        hasEntitySingle = SaveGame.Load<bool[]>("hasEntity");
        hasItemSingle = SaveGame.Load<bool[]>("hasItem");
        */

        gameMap.FillMap();

        for (int i = 0; i < totalTiles; i++)
        {
            int x = i / gameMap.mapHeight;
            int y = i % gameMap.mapHeight;

            //Debug.Log(i + "," + x + "," + y);s

            gameMap.map[x, y].walkable = isWalkableSingle[i];
            gameMap.map[x, y].isWall = tileIsWallSingle[i];
            if(!gameMap.map[x, y].isWall) { gameMap.map[x, y].SetToFloor(); }
            gameMap.map[x, y].visible = tileVisibleSingle[i];
            gameMap.map[x, y].explored = tileExploredSingle[i];            

            /* 
            gameMap.hasEntity[x, y] = hasEntitySingle[i];
            gameMap.hasItem[x, y] = hasItemSingle[i];
            */
        }

        



        //entities
        
        List<int> xpos = SaveGame.Load<List<int>>("xpos");
        List<int> ypos = SaveGame.Load<List<int>>("ypos");
        List<int> entityNumber = SaveGame.Load<List<int>>("entityNumber");
        List<int> hpe = SaveGame.Load<List<int>>("hpe");

        for (int i = 0; i < xpos.Count; i++)  
        {
            if(i == 0) { continue; } //skip player
            gameMap.CreateEntity(xpos[i], ypos[i], entityNumber[i]);

            gameMap.entities[i].GetComponent<Fighter>().HP = hpe[i];
        }


        //deadentities


        //items        
        
        List<int> xposi = SaveGame.Load<List<int>>("xposi");
        List<int> yposi = SaveGame.Load<List<int>>("yposi");
        List<int> itemIDNumber = SaveGame.Load<List<int>>("itemIDNumber");       

        for (int i = 0; i < xposi.Count; i++)
        {
            if (itemIDNumber[i] < 100)
            {
                gameMap.CreateItem(xposi[i], yposi[i], itemIDNumber[i]);
            }
            else if (itemIDNumber[i] < 200)
            {
                //CreateEquipment(xposi[i], yposi[i], itemIDNumber[i]);
            }

        }

        //equipment.LoadEquipped();

        //entities = SaveGame.Load<List<GameObject>>("entities");
        //deadEntities = SaveGame.Load<List<GameObject>>("deadEntities");
        //items = SaveGame.Load<List<GameObject>>("items");
        //equipment = SaveGame.Load<List<GameObject>>("equipment");


        //stairs
        /*
        int stairX = SaveGame.Load<int>("stairX");
        int stairY = SaveGame.Load<int>("stairY");
        bool stairSeen = SaveGame.Load<bool>("stairSeen");

        CreateStairs(new Vector2(stairX, stairY));
        stairsObj.GetComponent<Stairs>().hasBeenSeen = stairSeen;
        */

        //inventory.UpdatePlayerStats();

        List<string> list = SaveGame.Load<List<string>>("Message Log");
        foreach (string s in list)
        {
            uIManager.NewMessage(s);
        }




        gameMap.FOV();
        sw.Stop();
        Debug.Log("Loading took:" + sw.Elapsed);
    }
}
