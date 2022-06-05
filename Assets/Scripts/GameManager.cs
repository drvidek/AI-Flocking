using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]  public enum GameState { pregame, game, pause, postgame}
public class GameManager : MonoBehaviour
{
    [SerializeField] public static GameState currentGameState;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (currentGameState != GameState.pause)
                currentGameState = GameState.pause;
            else
                currentGameState = GameState.game;
        }
    }

    public static bool IsPaused()
    {
        return currentGameState == GameState.pause;
    }
    
}
