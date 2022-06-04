using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState { pregame, game, pause, postgame}
public class GameManager : MonoBehaviour
{
    public static GameState currentGameState;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static bool IsPaused()
    {
        return currentGameState == GameState.pause;
    }
}
