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
    public static string bulletPrefabPath = "Prefabs/Bullet";

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
    public void SaveGame(int file)
    {
        string score = GlobalScore.GetScore();

        string _playerData = CreatePlayerSaveString();

        string flockData = "";

        for (int i = 0; i < flocks.Length; i++)
        {
            flockData = flockData + flocks[i].SaveAgentTransform();
        }

        string bulletData = CreateBulletSaveString();

        string _saveFile = score + _playerData + flockData + bulletData;

        HandleGameSaveFile.WriteSaveFile(_saveFile, file);
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
                Debug.Log("Parsed array and loaded");
                HandleGameSaveFile.WriteContinueFile(PackParentArraysToString(loadParents));
                Debug.Log("Wrote Continue File");
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
        Debug.Log("Completed Load");

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
        for (int i = flockStart; i < loadFiles.Length - 1; i++)
        {
            if (loadFiles[i] != "")
            {
                string[] agentEntries = loadFiles[i].Split('~');
                flocks[i - flockStart].LoadAgents(agentEntries);
            }
        }

        //6 = bullets
        //unpack the string into an array
        string[] bulletArray = loadFiles[6].Split('~');
        ParseBulletArray(bulletArray);
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

        if (bullets.Count > 0)
            foreach (Bullet bullet in bullets)
            {
                Vector3 _dir = (Vector3)bullet.direction;

                bulletData = bulletData
                    + bullet.transform.position.ToString() + ":"   //0
                + _dir.ToString() + ":" //1
                + bullet.spd.ToString() + ":"     //2
                + bullet.tag.ToString()        //3
                + "~";
            }

        if (bulletData.Length > 1)
        bulletData = bulletData.Remove(bulletData.Length - 1);

        return bulletData;
    }

    public void ParseBulletArray(string[] bulletArray)
    {
        for (int i = 0; i < bullets.Count; i++)
        {
            Destroy(bullets[i].gameObject);
        }

        bullets.Clear();

        GameObject prefab = Resources.Load(bulletPrefabPath) as GameObject;

        Debug.Log(bulletArray.Length + " bullets to load");
        Debug.Log(prefab.name + "  Prefab Loaded");

        for (int i = 0; i < bulletArray.Length; i++)
        {
            if (bulletArray[i] != "")
            {
                string[] bulletDetails = bulletArray[i].Split(':');

                foreach (string item in bulletDetails)
                {
                    Debug.Log(item);
                }

                GameObject bulletObject = Instantiate( //creates a clone of gameobject or prefab
                    prefab,  // this is the prefab
                    MathExt.StringToVector3(bulletDetails[0]),
                    new Quaternion(0, 0, 0, 0)
                    );
                Bullet bullet = bulletObject.GetComponent<Bullet>();

                Vector3 _direction = MathExt.StringToVector3(bulletDetails[1]);

                bullet.direction = (Vector2)_direction;
                bullet.spd = float.Parse(bulletDetails[2]);
                bullet.tag = bulletDetails[3];

                bullet.scale = bullet.tag == "Player" ? 1 : 3;
                bullet.color = bullet.tag == "Player" ? Color.white : Color.red;

                bullet.ApplyProperties();

                Debug.Log(bullet.ToString() + " " + i + " loaded");

            }
            else
                Debug.Log("Invalid bullet");
        }
        Debug.Log("Bullets loaded");
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
        packedString = packedString.Remove(packedString.Length - 1);

        return packedString;
    }

    #endregion

    #region Scene Changing
    public static void ChangeScene(int sceneIndex)
    {
        if (bullets.Count > 0)
            bullets.Clear();
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
