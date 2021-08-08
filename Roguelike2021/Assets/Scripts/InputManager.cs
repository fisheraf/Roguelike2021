using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    float timer;
    Engine engine;

    private void Awake()
    {
        engine = GetComponent<Engine>();
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {   
        timer += Time.deltaTime;

        if (timer > .01 && engine.gameStates.gameState == GameStates.GameState.PlayerTurn)
        {
            PlayerMovement();
            Actions();
        }
        if(timer > .01 && engine.gameStates.gameState == GameStates.GameState.DropItem)
        {
            DropItem();
        }
        /*if(engine.gameStates.gameState == GameStates.GameState.Targeting)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                engine.gameStates.ChangeGameState(GameStates.GameState.PlayerTurn);
            }
        }*/
    }

    void PlayerMovement()
    {
        //down left
        if (Input.GetKeyDown(KeyCode.Keypad1))
        {
            engine.player.GetComponent<Entity>().Move(-1, -1);
        }
        //down
        if (Input.GetKeyDown(KeyCode.Keypad2))
        {
            engine.player.GetComponent<Entity>().Move(0, -1);
        }
        //down right
        if (Input.GetKeyDown(KeyCode.Keypad3))
        {
            engine.player.GetComponent<Entity>().Move(1, -1);
        }
        //left
        if (Input.GetKeyDown(KeyCode.Keypad4))
        {
            engine.player.GetComponent<Entity>().Move(-1, 0);
        }
        //wait
        if (Input.GetKeyDown(KeyCode.Keypad5))
        {
            engine.player.GetComponent<Entity>().Move(0, 0);
        }
        //right
        if (Input.GetKeyDown(KeyCode.Keypad6))
        {
            engine.player.GetComponent<Entity>().Move(1, 0);
        }
        //up left
        if (Input.GetKeyDown(KeyCode.Keypad7))
        {
            engine.player.GetComponent<Entity>().Move(-1, 1);
        }
        //up
        if (Input.GetKeyDown(KeyCode.Keypad8))
        {
            engine.player.GetComponent<Entity>().Move(0, 1);
        }
        //up right
        if (Input.GetKeyDown(KeyCode.Keypad9))
        {
            engine.player.GetComponent<Entity>().Move(1, 1);
        }
    }

    void Actions() 
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            engine.inventory.PickUpItem();
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            timer = 0;
            engine.gameStates.ChangeGameState(GameStates.GameState.DropItem);
        }

        
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            engine.inventory.UseItem(0);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            engine.inventory.UseItem(1);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            engine.inventory.UseItem(2);
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            engine.inventory.UseItem(3);
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            engine.inventory.UseItem(4);
        }
        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            engine.inventory.UseItem(5);
        }
        if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            engine.inventory.UseItem(6);
        }
        if (Input.GetKeyDown(KeyCode.Alpha8))
        {
            engine.inventory.UseItem(7);
        }
        if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            engine.inventory.UseItem(8);
        }
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            engine.inventory.UseItem(9);
        }
        if (Input.GetKeyDown(KeyCode.Minus))
        {
            engine.inventory.UseItem(10);
        }
        if (Input.GetKeyDown(KeyCode.Equals))
        {
            engine.inventory.UseItem(11);
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            engine.SaveProgress();
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            engine.LoadProgress();
        }
    }

    void DropItem()
    {
        if (Input.GetKeyDown(KeyCode.D))
        {
            timer = 0;
            engine.gameStates.ChangeGameState(GameStates.GameState.PlayerTurn);
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            engine.inventory.DropItem(0);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            engine.inventory.DropItem(1);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            engine.inventory.DropItem(2);
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            engine.inventory.DropItem(3);
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            engine.inventory.DropItem(4);
        }
        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            engine.inventory.DropItem(5);
        }
        if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            engine.inventory.DropItem(6);
        }
        if (Input.GetKeyDown(KeyCode.Alpha8))
        {
            engine.inventory.DropItem(7);
        }
        if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            engine.inventory.DropItem(8);
        }
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            engine.inventory.DropItem(9);
        }
        if (Input.GetKeyDown(KeyCode.Minus))
        {
            engine.inventory.DropItem(10);
        }
        if (Input.GetKeyDown(KeyCode.Equals))
        {
            engine.inventory.DropItem(11);
        }
    }
}
