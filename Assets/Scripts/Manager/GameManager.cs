using System.Collections.Generic;
using System.IO;
using System.Linq;
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
    
    
    private HighscoreEntry HGE;
    private InputField inputFieldPlayName;
    [SerializeField] private GameObject GO_inputfield;
    [SerializeField] private GameObject ScoreBoard;
    private string playerName;
    private List<HighscoreEntry> highScorePlayerList;
    private string highscoreFile = "data.json";

    [SerializeField] private GameObject GO_Highscore1;
    [SerializeField] private GameObject GO_Highscore2;
    [SerializeField] private GameObject GO_Highscore3;

    [SerializeField] private SpawnManager SpawnManager;

    void Start()
    {
        GameRunning = true;
        inputFieldPlayName = GO_inputfield.GetComponent<InputField>();
        SetupWalls();
        SpawnManager.gameRunning = true;
    }

    // Update is called once per frame
    void Update()
    {
        CheckForPlayer();
        updateScore(scorePerSecond * Time.deltaTime);
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
            playerTime.text = Time.time.ToString();
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
        Load();
        Save();
        
    }

    List<HighscoreEntry> sortHighscoreList()
    {
        highScorePlayerList = highScorePlayerList.OrderBy(o=>o.highscore).ToList();
        return highScorePlayerList;
    }

    void ShowHighscoreTable()
    {
        var GO_Highscore1T = GO_Highscore1.AddComponent<Text>();
        var GO_Highscore2T = GO_Highscore1.AddComponent<Text>();
        var GO_Highscore3T = GO_Highscore1.AddComponent<Text>();

        GO_Highscore1T.text = highScorePlayerList[0].highscore + " " + highScorePlayerList[0].username;
        GO_Highscore2T.text = highScorePlayerList[1].highscore + " " + highScorePlayerList[1].username;
        GO_Highscore3T.text = highScorePlayerList[2].highscore + " " + highScorePlayerList[2].username;

    }
    
    private void Load()
    {
        string json = loadHighscoreList();
        EndScene.SetActive(false);
        ScoreBoard.SetActive(true);
        
        if (json != null)
        {
            highScorePlayerList = JsonUtility.FromJson<List<HighscoreEntry>>(json);
        }
        else
        {
            highScorePlayerList = new List<HighscoreEntry>();
        }
  
    }

    void Save()
    {
        var tempstore = sortHighscoreList();
        for (int i = 0; i < tempstore.Count; i++)
        {
            if (HGE.highscore > tempstore[i].highscore)
            {
                tempstore.Insert(i,HGE);
            }
        }

        if (tempstore.Count > 10)
        {
            tempstore.RemoveAt(11);
        }
        string json = JsonUtility.ToJson(tempstore);
        WriteToFile(json);
    }
    
    
    string loadHighscoreList()
    {
        string path = GetFilePath(highscoreFile);
        if (File.Exists(path))
        {
            using (StreamReader reader = new StreamReader(path))
            {
                string json = reader.ReadToEnd();
                return json;
            }
        }
        else
        {
            return null;
        }
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
    
    private string GetFilePath(string fileName)
    {
        return Application.persistentDataPath + "/" + fileName;
    }
}

public class HighscoreEntry
{
    public string username { get; set; }
    public float highscore { get; set; }
}