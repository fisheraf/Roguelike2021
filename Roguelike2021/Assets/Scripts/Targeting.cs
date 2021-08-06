using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Targeting : MonoBehaviour
{
    Engine engine;
    public GameObject highlight;
    Vector2 worldPoint;
    public Vector2Int target;

    private void Start()
    {
        engine = GetComponent<Engine>();    
    }

    private void Update()
    {
        worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        worldPoint = new Vector2(worldPoint.x + .5f, worldPoint.y + .5f);
    }

    public bool targetSelected = false;

    public IEnumerator TargetSelect(bool entityRequired)
    {
        targetSelected = false;
        highlight.GetComponent<SpriteRenderer>().enabled = true;

        while (targetSelected == false)
        {
            engine.gameStates.ChangeGameState(GameStates.GameState.Targeting);

            //highlight target cell
            highlight.transform.position = new Vector3((int)worldPoint.x, (int)worldPoint.y, -1);
            highlight.GetComponentInChildren<SpriteRenderer>().enabled = true;


            if (Input.GetKeyDown(KeyCode.Escape))
            {
                highlight.GetComponent<SpriteRenderer>().enabled = false;
                engine.gameStates.ChangeGameState(GameStates.GameState.PlayerTurn);
                yield break;
            }

            else if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space))
            {
                target = new Vector2Int((int)worldPoint.x, (int)worldPoint.y);

                if (engine.gameMap.map[target.x, target.y].visible)
                {
                    if (entityRequired)
                    {
                        foreach (GameObject entity in engine.gameMap.entities)
                        {
                            if ((int)entity.transform.position.x == target.x && (int)entity.transform.position.y == target.y)
                            {
                                targetSelected = true;
                            }
                        }
                        yield return null;
                    }
                    else
                    {
                        targetSelected = true;
                    }
                }
                else
                {
                    engine.uIManager.NewMessage("Target not in range.");
                    yield return null;
                }
                highlight.GetComponent<SpriteRenderer>().enabled = false;
            }
            yield return null;
        }
    }
}
