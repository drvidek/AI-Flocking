using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]  public enum GameState { pregame, game, pause, postgame}
public class GameManager : MonoBehaviour
{
    [SerializeField] public static GameState currentGameState = GameState.game;
    [SerializeField] public GameObject _pausePanel;
    
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyBinds.keys["Pause"]))
        {
            TogglePause();
        }
    }

    public void TogglePause()
    {
        if (_pausePanel != null)
        {
            if (currentGameState == GameState.game)
            {
                currentGameState = GameState.pause;
                _pausePanel.SetActive(true);
            }
            else if (currentGameState == GameState.pause && _pausePanel.activeSelf)
            {
                currentGameState = GameState.game;
                _pausePanel.SetActive(false);
            }
        }

    }

    public static bool IsPaused()
    {
        return currentGameState == GameState.pause;
    }

    public void ChangeScene(int sceneIndex)
    {
        
        SceneManager.LoadScene(sceneIndex);
    }

    public void SaveGame()
    {
        PlayerSave.WriteSaveFile(GameObject.Find("Player").transform);
    }

    public void LoadGame(int file)
    {
        PlayerSave.ReadSaveFile(GameObject.Find("Player").transform,file);
    }

    public void QuitGame()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

}
