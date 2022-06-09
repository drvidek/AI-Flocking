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
    public static List<Bullet> bullets = new List<Bullet>();
    static int _loadFile = -1;

    private void Start()
    {
        GameObject.Find("Player").TryGetComponent<PlayerMain>(out _player);

        if (_loadFile != -1)
        {
            LoadGameFromFile(_loadFile);
            _loadFile = -1;
        }
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

    [SerializeField] private Flock[] flocks;

    #region Save + Load
    public void SaveGame()
    {
        string score = GlobalScore.GetScore();

        string _playerData = CreatePlayerSaveString();

        string flockData = "";

        for (int i = 0; i < flocks.Length; i++)
        {
            flockData = flockData + flocks[i].SaveAgentTransform();
        }

        string bulletData = CreateBulletSaveString();

        string _saveFile = score + _playerData + flockData;

        HandleGameSaveFile.WriteSaveFile(_saveFile);
    }

    public void LoadGameFromFile(int file)
    {
        //fetch an array of game = 0, player = 1, flock 0-3 = 2-5

        //loading
        if (file < HandleGameSaveFile.saveSlots)
        {
            string[] loadParents = HandleGameSaveFile.ReadSaveFile(file);

            if (loadParents.Length > 0)
            {
                ParseParentArray(loadParents);
                HandleGameSaveFile.WriteContinueFile(PackParentArraysToString(loadParents));
            }
        }
        else    //continuing
        {
            string[] continueParents = HandleGameSaveFile.ReadContinueFile();

            if (continueParents.Length > 0)
            {
                ParseParentArray(continueParents);
            }
        }
    }

    public void ParseParentArray(string[] loadFiles)
    {
        //0 = score
        GlobalScore.ResetScore();
        GlobalScore.IncreaseScore(int.Parse(loadFiles[0]), new Vector2(-1000, -1000));

        //1 - player
        string[] playerArray = loadFiles[1].Split(':');
        ParsePlayerArray(playerArray);

        //2-5 = flocks
        int flockStart = 2;
        for (int i = flockStart; i < loadFiles.Length; i++)
        {
            if (loadFiles[i] != "")
            {
                string[] agentEntries = loadFiles[i].Split('~');
                flocks[i - flockStart].LoadAgents(agentEntries);
            }
        }
    }

    public string CreatePlayerSaveString()
    {
        string _playerData = "|";

        Vector3 _playerVelocity = (Vector3)_player.Velocity;

        _playerData = _playerData
            + _player.transform.position.ToString() + ":"   //0
            + _player.transform.up.ToString() + ":" //1
            + _playerVelocity.ToString() + ":"     //2
            + _player.BoostDelay.ToString();        //3
        return _playerData;
    }

    public string CreateBulletSaveString()
    {
        string bulletData = "|";

        foreach (Bullet bullet in bullets)
        {
            bulletData = bulletData
                + bullet.transform.position.ToString() + ":"   //0
            + bullet.direction.ToString() + ":" //1
            + bullet.spd.ToString() + ":"     //2
            + bullet.tag.ToString()        //4
            + "~";
        }

        return bulletData;
    }

    public void ParseBulletArray()
    {
        //bullet.transform.position = (_shotSpawnPoint != null) ? _shotSpawnPoint.position : transform.position;
        //bullet.direction = _dir;
        //bullet.tag = tag;
        //bullet.spd = _shotSpeed;
        //bullet.power = _shotPower;
        //bullet.scale = _shotSize;

        //if tag = enemy, scale and color = 3 and red
        //else scale = 1 and colour = white

    }

    public void ParsePlayerArray(string[] playerArray)
    {
        Transform playerTransform = _player.transform;
        playerTransform.position = MathExt.StringToVector3(playerArray[0]);
        playerTransform.up = MathExt.StringToVector3(playerArray[1]);

        Vector3 _playerVelocity = MathExt.StringToVector3(playerArray[2]);

        _player.Velocity = (Vector2)_playerVelocity;
        _player.BoostDelay = float.Parse(playerArray[3]);
        _player.UpdateBoostGUI();
    }

    public string PackParentArraysToString(string[] parents)
    {
        string packedString = "";
        foreach (string piece in parents)
        {
            packedString = packedString + piece + "|";
        }

        return packedString;
    }

    #endregion

    #region Scene Changing
    public static void ChangeScene(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
    }
    public void QueueLoad(int file)
    {
        _loadFile = file;
    }
    #endregion

    public void QuitGame()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

}
