using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable] public enum GameState { pregame, game, pause, postgame }
public class GameManager : MonoBehaviour
{
    [SerializeField] public static GameState currentGameState = GameState.game;
    [SerializeField] public GameObject _pausePanel;
    [SerializeField] private PlayerMain _player;

    private void Start()
    {
        GameObject.Find("Player").TryGetComponent<PlayerMain>(out _player);
    }

    // Update is called once per frame
    void Update()
    {
        if (_player != null && Input.GetKeyDown(KeyBinds.keys["Pause"]) && !_player.Dead)
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

    public static void ChangeScene(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
    }

    [SerializeField] private Flock[] flocks;

    public void SaveGame()
    {
        string flockData = "";

        for (int i = 0; i < flocks.Length; i++)
        {
            flockData = flockData + flocks[i].SaveAgentTransform();
        }

        RecordGameState.WriteSaveFile(_player.transform, flockData);
    }

    public void LoadGame(int file)
    {
        //fetch an array of player, flock0-3
        string[] loadFiles = RecordGameState.ReadSaveFile(file);

        if (loadFiles.Length > 0)
        {
            Transform playerTransform = _player.transform;

            string[] playerTrans = loadFiles[0].Split(':');
            playerTransform.position = MathExt.StringToVector3(playerTrans[0]);
            playerTransform.up = MathExt.StringToVector3(playerTrans[1]);

            for (int i = 1; i < loadFiles.Length; i++)
            {
                string[] agentEntries = loadFiles[i].Split('~');
                flocks[i - 1].LoadAgents(agentEntries);
            }

            //return true;
        }
        //else
        //return false;
    }

    public void QuitGame()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

}
