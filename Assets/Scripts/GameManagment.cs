using TMPro;
using UnityEngine;
using System.Data;
using System.IO;
using System;


public class GameManagment : MonoBehaviour
{

	// This is your table to hold the result set:
	static DataTable dataTable = new DataTable();    public GameObject block;
    public float maxX;
    public Transform spawnPoint;
    public float spawnRate;

    bool gameStarted = false;

    public GameObject tapText;
    public TextMeshProUGUI scoreText;

    static int score = 0;

    void Start()
    {
        HandleLoad();
        scoreText.text = "Best score: " + score.ToString();
    }


    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !gameStarted)
        {
            StartSpawning();
            gameStarted = true;
            scoreText.text = "0";
            tapText.SetActive(false);
        }
    }


    private void StartSpawning() 
    {
        InvokeRepeating("SpawnBlock", 0.5f, spawnRate);
    }

    private void SpawnBlock()
    {
        Vector3 spawnPos = spawnPoint.position;

        spawnPos.x = UnityEngine.Random.Range(-maxX, maxX);

        Instantiate(block, spawnPos, Quaternion.identity);

        score++;

        scoreText.text = score.ToString(); 
    }
    private static SaveData _savedata = new SaveData();
    

    [System.Serializable]
    public struct SaveData
    {
        public PlayerSaveData PlayerData;
    }

    public static void HandleSave()
    {
        Save();
    }

    public static void Save()
    {
        if (_savedata.PlayerData.bestScore < score)
        {
            _savedata.PlayerData.bestScore = score;
            File.WriteAllText(SaveFileName(), JsonUtility.ToJson(_savedata.PlayerData, true));
        }
    }
    public static void FirstSave()
    {
        _savedata.PlayerData.bestScore = score;
        File.WriteAllText(SaveFileName(), JsonUtility.ToJson(_savedata.PlayerData, true));
    }
    public void Load(PlayerSaveData data)
    {
        score = _savedata.PlayerData.bestScore;
    }
    public void HandleLoad() {
        try 
        {
            File.ReadAllText(SaveFileName());
        } catch {
            FirstSave();
        }
        string savecontent = File.ReadAllText(SaveFileName());
        _savedata = JsonUtility.FromJson<SaveData>(savecontent);
        Load(_savedata.PlayerData);
    }

    [System.Serializable]
    public struct PlayerSaveData
    {
        public int bestScore;
    }

    public static string SaveFileName()
    {
        string saveFile = Application.persistentDataPath + "/save" + ".save";
        return saveFile;
    }
}
