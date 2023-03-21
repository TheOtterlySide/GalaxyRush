using System;
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

    void Start()
    {
        GameRunning = true;
        inputFieldPlayName = GO_inputfield.GetComponent<InputField>();
        SetupWalls();
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
            GameRunning = false;
            GameEnd();
        }
    }

    public static void updateScore(float Score)
    {
        highscore += Score;
    }

    private void updateUI()
    {
        scoreLabel.text = "Score: " + Mathf.Round(highscore).ToString();
        playerLife.text = playerObject.playerLife.ToString();
        playerTime.text = Time.deltaTime.ToString();
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
        loadHighscoreList();
    }

    List<HighscoreEntry> sortHighscoreList()
    {
        highScorePlayerList = highScorePlayerList.OrderBy(o=>o.highscore).ToList();
        return highScorePlayerList;
    }
    
    public void Load()
    {
        string json = loadHighscoreList();
        HGE = JsonUtility.FromJson<HighscoreEntry>(json);
    }

    void Save()
    {
        var tempstore = sortHighscoreList();
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
            Debug.LogWarning("File not found");
        }

        return "Success";
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