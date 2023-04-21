using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    internal static GameManager Instance;
    // Start is called before the first frame update
    [SerializeField] private bool GameRunning;
    [SerializeField] private Player playerObject;
    public static float highscore;
    [SerializeField] private int scorePerSecond;


    [SerializeField] private GameObject Wall_Left;
    [SerializeField] private GameObject Wall_Right;
    [SerializeField] private GameObject Wall_Top;
    [SerializeField] private GameObject Wall_Bottom;
    
    [SerializeField] private Camera MainCamera; //be sure to assign this in the inspector to your main camera
    private Vector2 screenBounds;

    [SerializeField] private Text scoreLabel;
    [SerializeField] private Text playerLife;
    [SerializeField] private Text playerTime;

    [SerializeField] private GameObject EndScene;
    [SerializeField] private GameObject GO_inputfield;
    [SerializeField] private GameObject GO_inputfieldHeadline;
    
    private HighscoreEntry HGE = new();
    private InputField inputFieldPlayName;
    
    [SerializeField] private GameObject ScoreBoard;
    private string playerName;
    private List<HighscoreEntry> highScorePlayerList = new();
    private string highscoreFile = "data.json";

    [SerializeField] private Text GO_Highscore1;
    [SerializeField] private Text GO_Highscore2;
    [SerializeField] private Text GO_Highscore3;
    private List<Text> TextList = new List<Text>();

    [SerializeField] private SpawnManager SpawnManager;

    void Start()
    {
        GameRunning = true;
        inputFieldPlayName = GO_inputfield.GetComponent<InputField>();
        SetupWalls();
        fillTextList();
        SpawnManager.gameRunning = true;
    }

    // Update is called once per frame
    void Update()
    {
        CheckForPlayer();
        updateScore(scorePerSecond * Time.time);
        updateUI();
    }
    private void Awake()
    {
        if (Instance == null) 
        {
            Instance = this;
        }
        else if (Instance != this) 
        {
            Destroy(gameObject);
        }
    }

    private void fillTextList()
    {
        TextList.Add(GO_Highscore1);
        TextList.Add(GO_Highscore2);
        TextList.Add(GO_Highscore3);
    }

    void CheckForPlayer()
    {
        if (playerObject.playerAlive == false) 
        {
            SpawnManager.gameRunning = false;
            GameEnd();
        }
    }

    public static void updateScore(float Score)
    {
        highscore += Score;
        if (highscore <= 0)
        {
            highscore = 0;
        }
    }

    private void updateUI()
    {
        if (playerObject.playerAlive)
        {
            scoreLabel.text = "Score: " + Mathf.Round(highscore).ToString();
            playerLife.text = playerObject.playerLife.ToString();
            playerTime.text = (Mathf.Round(Time.time * 100f) / 100f).ToString();
        }
       
    }

    void SetupWalls()
    {
        Vector3 stageDimensions = Camera.main.ViewportToWorldPoint(new Vector3(1, 1, Camera.main.nearClipPlane));
        Wall_Left.transform.position = new Vector3(-stageDimensions.x - 0.5f, 0, 0);
        Wall_Right.transform.position = new Vector3(stageDimensions.x + 0.5f, 0,0);
        Wall_Top.transform.position = new Vector3(0,stageDimensions.y + 0.5f,0);
        Wall_Bottom.transform.position = new Vector3(0, -stageDimensions.y - 0.5f, 0);
    }

    void GameEnd()
    {
        EndScene.SetActive(true);
    }

    public void setInputName()
    {
        playerName = inputFieldPlayName.text;
        HGE.username = playerName; 
        HGE.highscore = highscore.ToString("0.");
        highScorePlayerList = loadHighscoreList();
        highScorePlayerList.Add(HGE);
        Load();
        Save();
        ShowHighscoreTable();
    }

    List<HighscoreEntry> sortHighscoreList()
    {
        highScorePlayerList = highScorePlayerList.OrderByDescending(o=>o.highscore).ToList();
        return highScorePlayerList;
    }

    void ShowHighscoreTable()
    {
        for (int i = 0; i < TextList.Count; i++)
        {
            TextList[i].text = highScorePlayerList[i].highscore + " " + highScorePlayerList[i].username;
        }

    }
    
    private void Load()
    {
        
        EndScene.SetActive(false);
        GO_inputfield.SetActive(false);
        GO_inputfieldHeadline.SetActive(false);
        ScoreBoard.SetActive(true);
    }

    void Save()
    {
        var tempstore = sortHighscoreList();

        if (tempstore.Count > 10)
        {
            tempstore.RemoveAt(11);
        }
        
        string json = Newtonsoft.Json.JsonConvert.SerializeObject(tempstore);
        WriteToFile(json);
    }
    
    
    List<HighscoreEntry> loadHighscoreList()
    {
        string path = GetFilePath(highscoreFile);

        if (File.Exists(path))
        {
            var tempCheck = isFileEmpty(path);
            if (tempCheck == false)
            {
                using (StreamReader reader = File.OpenText(path))
                {
                    JsonSerializer serializer = new JsonSerializer();
                    var tempStore = (List<HighscoreEntry>)serializer.Deserialize(reader, typeof(List<HighscoreEntry>));
                    return tempStore;
                }
            }

        }  //Cant Load File

        return new List<HighscoreEntry>();
    }
    void WriteToFile(string jsonHighscoreList)
    {
        string path = GetFilePath(highscoreFile);
        FileStream fileStream = new FileStream(path, FileMode.Create);

        using (StreamWriter writer = new StreamWriter(fileStream))
        {
            writer.Write(jsonHighscoreList);
        }
    }

    private bool isFileEmpty(string fileName)
    {
        var info = new FileInfo(fileName);
        
        if (info.Length == 0)
        {
            return true;
        }

        return false;
    }
    private string GetFilePath(string fileName)
    {
        return Application.persistentDataPath + "/" + fileName;
    }
}
[Serializable]
public class HighscoreEntry
{
    public string username { get; set; }
    public string highscore { get; set; }
}