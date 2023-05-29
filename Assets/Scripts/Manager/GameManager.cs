using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    internal static GameManager Instance;
    // Start is called before the first frame update
    [SerializeField] private bool GameRunning;
    [SerializeField] private Player playerObject;
    public static float highscore;
    [SerializeField] private int scorePerSecond;

    #region Walls

    [SerializeField] private GameObject Wall_Left;
    [SerializeField] private GameObject Wall_Right;
    [SerializeField] private GameObject Wall_Top;
    [SerializeField] private GameObject Wall_Bottom;

    #endregion
    
    
    [SerializeField] private Camera MainCamera; //be sure to assign this in the inspector to your main camera
    private Vector2 screenBounds;

    #region UI

    [SerializeField] private Text scoreLabel;
    [SerializeField] private Text playerLife;
    [SerializeField] private Text playerTime;
    
    [SerializeField] private GameObject playerLife1;
    [SerializeField] private GameObject playerLife2;
    [SerializeField] private GameObject playerLife3;

    private SpriteRenderer playerLife_1;
    private SpriteRenderer playerLife_2;
    private SpriteRenderer playerLife_3;
    #endregion
    

    

    #region Highscore

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

    private float storedTime;
    #endregion

    #region Pause

    [SerializeField] private GameObject PauseMenu;

    #endregion
    [SerializeField] private SpawnManager SpawnManager;
    [SerializeField] private GameObject Game;

    void Start()
    {
        GameRunning = true;
        inputFieldPlayName = GO_inputfield.GetComponent<InputField>();
        playerLife_1 = playerLife1.GetComponent<SpriteRenderer>();
        playerLife_2 = playerLife2.GetComponent<SpriteRenderer>();
        playerLife_3 = playerLife3.GetComponent<SpriteRenderer>();
        SetupWalls();
        fillTextList();
        SpawnManager.gameRunning = true;
        playerObject.gameRunning = true;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        CheckForPlayer();
        updateUI();


        if (Input.GetButton("Cancel"))
        {
            Pause(true);
        }
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
            playerObject.gameRunning = false;
           
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
            updateLife();
            playerTime.text = (Mathf.Round(Time.time * 100f / 100f)).ToString();
        }

    }

    void updateLife()
    {
        switch (playerObject.playerLife)
        {
            case 0:
                playerLife_1.color = Color.red;
                playerLife_2.color = Color.red;
                playerLife_3.color = Color.red;
                break;
            case 1:
                playerLife_1.color = Color.green;
                playerLife_2.color = Color.red;
                playerLife_3.color = Color.red;
                break;
            case 2:
                playerLife_1.color = Color.green;
                playerLife_2.color = Color.green;
                playerLife_3.color = Color.red;
                break;
            case 3:
                playerLife_1.color = Color.green;
                playerLife_2.color = Color.green;
                playerLife_3.color = Color.green;
                break;
            default:
                break;
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
        storedTime = float.Parse(playerTime.text);
        updateLife();
        EndScene.SetActive(true);
        inputFieldPlayName.ActivateInputField();
        inputFieldPlayName.Select();
        playerObject.SelfDestroy();
        SpawnManager.DeleteFromGM("Enemy");
        SpawnManager.DeleteFromGM("PowerUp");
    }

    public void setInputName()
    {
        playerName = inputFieldPlayName.text;
        HGE.username = playerName;
        highscore += scorePerSecond * storedTime;
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

    bool isFileEmpty(string fileName)
    {
        var info = new FileInfo(fileName);
        
        if (info.Length == 0)
        {
            return true;
        }

        return false;
    }
    string GetFilePath(string fileName)
    {
        return Application.persistentDataPath + "/" + fileName;
    }

    public void Pause(bool pauseStatus)
    {
        if (pauseStatus == true)
        {
            Time.timeScale = 0.0f;
            PauseMenu.SetActive(true);
            playerObject.gameRunning = false;
        }

        else
        {
            Time.timeScale = 1.0f;
            PauseMenu.SetActive(false);
            playerObject.gameRunning = true;
        }
    }

    public void MainMenu()
    {
        PauseMenu.SetActive(false);
        Destroy(Game);
        SceneManager.LoadScene("Main Menu");
    }
}
[Serializable]
public class HighscoreEntry
{
    public string username { get; set; }
    public string highscore { get; set; }
}