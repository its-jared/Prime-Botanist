using UnityEngine;
using System.Linq;
using System.Collections.Generic;

public class SaveSystemManager : MonoBehaviour
{
    [Header("File Storage Config")]
    [SerializeField]
    private string fileName;

    public static SaveSystemManager instance { get; private set; }

    private GameSettings gameSettings;
    private List<ISaveSystem> saveObjects;
    private FileDataHandler dataHandler;

    void Awake()
    {
        if (instance != null) return;
        instance = this;
    }

    void Start()
    {
        dataHandler = new FileDataHandler(Application.persistentDataPath, fileName);
        saveObjects = findAllSaveSystemObjects();

        LoadGame();    
    }

    void OnApplicationQuit()
    {
        SaveGame();    
    }

    public void NewGame()
    {
        gameSettings = new GameSettings();
    }

    public void LoadGame()
    {
        gameSettings = dataHandler.Load();

        // If no data has been found, init a new blank game. 
        if (gameSettings == null)
        {
            Debug.Log("Game settings now found, initing a new game.");
            NewGame();
        }

        foreach (ISaveSystem saveSystemObj in saveObjects)
        {
            saveSystemObj.LoadSettings(gameSettings);
        }
    }

    public void SaveGame()
    {
        foreach (ISaveSystem saveSystemObj in saveObjects)
        {
            saveSystemObj.SaveSettings(ref gameSettings);
        }

        dataHandler.Save(gameSettings);
    }

    [System.Obsolete]
    private List<ISaveSystem> findAllSaveSystemObjects()
    {
        IEnumerable<ISaveSystem> saveSystemObjects = FindObjectsOfType<MonoBehaviour>().OfType<ISaveSystem>();
        return new List<ISaveSystem>(saveSystemObjects);
    }
}
