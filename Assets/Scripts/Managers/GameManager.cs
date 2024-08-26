using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static LoadSaveManager.GameStateData;

public class GameManager : Singleton<GameManager>
{
    int enemyCount;
    public int wave;
    public Text enemyDisplay;
    public Text waveDisplay;

    public PlayerController cref;
    protected override void Awake()
    {
        base.Awake();
    }
    private void Start()
    {
        if (bShouldLoad)
        {
            LoadGame();
            bShouldLoad = false;
        }
        SaveGame();
        enemyCount = FindObjectsOfType<Enemy>().Length;
        enemyDisplay.text = "Enemies Left: " + enemyCount.ToString();
        waveDisplay.text = "Current Wave: " + wave.ToString();
    }
    public void Quit()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
    public void EnemyDefeated()
    {
        enemyCount--;
        enemyDisplay.text = "Enemies Left: " + enemyCount.ToString();
        if (enemyCount <= 0)
        {
            wave++;
            SaveGame();
        }
    }

    public static LoadSaveManager StateManager
    {
        get
        {
            if (!statemanager)
                statemanager = Instance.GetComponent<LoadSaveManager>();
            return statemanager;
        }
    }
    // Internal reference to Saveload Game Manager
    private static LoadSaveManager statemanager = null;

    // Should load from save game state on level load, or just restart level from defaults
    private static bool bShouldLoad = false;

    public void SaveGamePrepare()
    {
        DataGlobal dataGlobal = StateManager.gameState.global;

        dataGlobal.clearedWave = wave;
    }

    public void LoadGameComplete()
    {
        DataGlobal data = StateManager.gameState.global;
        wave = data.clearedWave;
    }

    public void SaveGame()
    {
        cref.SaveGamePrepare();
        SaveGamePrepare();
        Debug.Log("Autosaving...");
        Debug.Log(Application.persistentDataPath);
        StateManager.Save(Application.persistentDataPath + "/SaveGame.xml");
    }

    public void LoadGame()
    {
        bShouldLoad = true;
        StateManager.Load(Application.persistentDataPath + "/SaveGame.xml");
        cref.LoadGameComplete();
        LoadGameComplete();
    }
}
