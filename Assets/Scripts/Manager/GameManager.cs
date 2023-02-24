using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    internal static GameManager Instance;
    // Start is called before the first frame update
    [SerializeField] private bool GameRunning;
    [SerializeField] private Player playerObject;
    [SerializeField] public static float highscore;
    void Start()
    {
        GameRunning = true;
    }

    // Update is called once per frame
    void Update()
    {
        CheckForPlayer();
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
        }
    }

    public static void updateScore(int Score)
    {
        highscore += Score;
    }
}
