using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.IO;
using UnityEngine.SceneManagement;

public class SaveManager : MonoBehaviour
{
    public static SaveManager instance;

    [SerializeField] string filename;
    [SerializeField] bool isEncrptData;
    [SerializeField] string code;

    [SerializeField] private GameData gameData;
    private List<ISaveManager> saveManagers;
    private FileDataHandler fileDataHandler;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        fileDataHandler = new FileDataHandler(Application.persistentDataPath, filename, isEncrptData, code);
        saveManagers = FindAllSaveManagers();

        LoadGame();
    }

    [ContextMenu("Delete Save File")]
    public void DeleteSaveData()
    {
        fileDataHandler = new FileDataHandler(Application.persistentDataPath, filename, isEncrptData, code);
        fileDataHandler.Delete();
    }

    public void NewGame()
    {
        gameData = new GameData();
        fileDataHandler.Save(gameData);
    }

    public void LoadGame()
    {
        gameData = fileDataHandler.Load();

        if (gameData == null)
        {
            NewGame();
        }

        foreach (ISaveManager manager in saveManagers)
        {
            manager.LoadData(gameData);
        }
    }

    public void SaveGame()
    {
        foreach(ISaveManager manager in saveManagers)
        {
            manager.SaveData(ref gameData);
        }

        fileDataHandler.Save(gameData);
    }

    private List<ISaveManager> FindAllSaveManagers()
    {
        IEnumerable<ISaveManager> saveManagers = FindObjectsOfType<MonoBehaviour>(true).OfType<ISaveManager>();
        return new List<ISaveManager>(saveManagers);
    }

    public bool HaveSaveData()
    {
        if(fileDataHandler.Load() != null)
        {
            return true;
        }
        return false;
    }
}