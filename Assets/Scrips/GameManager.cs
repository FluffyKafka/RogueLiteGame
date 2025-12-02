using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour, ISaveManager
{
    public static GameManager instance;
    [SerializeField] private CheckPoint[] checkPointArray;
    public CheckPoint lastCheckPoint = null;

    public bool isPlayerRemainingExist;
    public string playerRemainingSceneName;
    public int playerLeftCurrency;
    [SerializeField] private GameObject playerRemainingPrefab;
    [SerializeField] private Transform playerRemainingTransform;

    [SerializeField] private string preGameSceneName;
    [SerializeField] public float pauseTimeSpeedDivider = 100f;//暂停时将游戏速度降低到极低

    public Transform playerInitTransform = null;

    public bool isBattle = false;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(instance.gameObject);
        }
        else
        {
            instance = this;
        }

        checkPointArray = FindObjectsOfType<CheckPoint>();
    }

    private void Start()
    {
        if(playerInitTransform != null)
        {
            PlayerManager.instance.player.transform.position = playerInitTransform.position;
        }
    }

    public void CheckPointLoad()
    {
        checkPointArray = FindObjectsOfType<CheckPoint>();
    }

    public void RestartGame()
    {
        SaveManager.instance.DeleteSaveData();
        SceneManager.LoadScene(preGameSceneName);
    }

    public void RestartGame_PlayerRemaining()
    {
        SaveManager.instance.NewGameWithPlayerRemaining(playerRemainingSceneName, playerLeftCurrency);
        SceneManager.LoadScene(preGameSceneName);
    }

    public void LoadData(GameData _data)
    {
        isPlayerRemainingExist = _data.isPlayerRemainingExist;
        playerLeftCurrency = _data.playerLeftCurrency;
        playerRemainingSceneName = _data.playerRemainingSceneName;

        if(isPlayerRemainingExist && playerRemainingSceneName == SceneManager.GetActiveScene().name)
        {
            PlayerRemaining playerRemaining = 
                Instantiate(playerRemainingPrefab, playerRemainingTransform.position, Quaternion.identity)
                .GetComponentInChildren<PlayerRemaining>();
            playerRemaining.Setup(playerLeftCurrency);
        }
    }

    public void SaveData(ref GameData _data)
    {
        _data.isPlayerRemainingExist = isPlayerRemainingExist;
        _data.playerLeftCurrency = playerLeftCurrency;
        _data.playerRemainingSceneName = playerRemainingSceneName;
    }

    public CheckPoint TryGetClosestCheckPointToPlayer()
    {
        CheckPoint closest = null;
        float minDistance = float.PositiveInfinity;
        foreach(CheckPoint checkPoint in checkPointArray)
        {
            if(checkPoint.isCheck)
            {
                float distance = Vector2.Distance(PlayerManager.instance.player.transform.position, checkPoint.transform.position);
                if (distance < minDistance)
                {
                    closest = checkPoint;
                    minDistance = distance;
                }
            }
        }
        return closest;
    }

    public void SetPauseGame(bool _isPause)
    {
        if(!CanPause())
        {
            Debug.Log("Cannot Pause");
            return;
        }

        if(_isPause)
        {
            Time.timeScale = 1 / pauseTimeSpeedDivider;
        }
        else
        {
            Time.timeScale = 1f;
        }
    }
    public void SetPauseGameForce(bool _isPause)
    {
        if (_isPause)
        {
            Time.timeScale = 0f;
        }
        else
        {
            Time.timeScale = 1f;
        }
    }

    public bool CanPause()
    {
        return !isBattle;
    }

    public void SaveAndExit()
    {
        SaveManager.instance.SaveGame();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
    Application.Quit();
#endif
    }
}
