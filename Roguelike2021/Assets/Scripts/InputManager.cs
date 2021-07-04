using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    float timer;
    [SerializeField] Entity player;
    GameMap gameMap;
    GameStates gameStates;

    private void Awake()
    {
        gameMap = FindObjectOfType<GameMap>();
        gameStates = FindObjectOfType<GameStates>();
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        if (timer > .2 && gameStates.gameState == GameStates.GameState.PlayerTurn)
        {
            PlayerMovement();
        }
    }

    void PlayerMovement()
    {
        //down left
        if (Input.GetKey(KeyCode.Keypad1))
        {
            player.Move(-1, -1);
        }
        //down
        if (Input.GetKey(KeyCode.Keypad2))
        {
            player.Move(0, -1);
        }
        //down right
        if (Input.GetKey(KeyCode.Keypad3))
        {
            player.Move(1, -1);
        }
        //left
        if (Input.GetKey(KeyCode.Keypad4))
        {
            player.Move(-1, 0);
        }
        //wait
        if (Input.GetKeyDown(KeyCode.Keypad5))
        {

        }
        //right
        if (Input.GetKey(KeyCode.Keypad6))
        {
            player.Move(1, 0);
        }
        //up left
        if (Input.GetKey(KeyCode.Keypad7))
        {
            player.Move(-1, 1);
        }
        //up
        if (Input.GetKey(KeyCode.Keypad8))
        {
            player.Move(0, 1);
        }
        //up right
        if (Input.GetKey(KeyCode.Keypad9))
        {
            player.Move(1, 1);
        }
        //delay input
        if (Input.anyKey)
        {
            timer = 0;
            gameMap.FOV();//switch to player turn end
        }

    }
}
