using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private GameObject SpawnPoint;
    [SerializeField] private GameObject SpawnPoint2;
    [SerializeField] private GameObject SpawnPoint3;
    [SerializeField] private GameObject SpawnArea_L;
    [SerializeField] private GameObject SpawnArea_R;
    
    private List<GameObject> SpawnPoints = new List<GameObject>();

    private Transform sp1;
    private Transform sp2;
    private Transform sp3;

    private int EnemyCounter;

    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private GameObject powerUpPrefab;
    [SerializeField] private float powerUpSpawn;
    [SerializeField] private float powerUpDelay;
    
    [SerializeField] private float enemySpawn;
    [SerializeField] private float enemyDelay;
    [SerializeField] private float enemyPauseSpawn;
    public bool gameRunning;
    
    // Start is called before the first frame update
    void Start()
    {
        SetupSpawnArea();
        SpawnEnemies();
    }

    // Update is called once per frame
    void Update()
    {
        if (gameRunning)
        {
            if (Time.time >= powerUpSpawn)
            {
                powerUpSpawn = Time.time + powerUpDelay;
                SpawnPowerup();
            }

            if (Time.time >= enemySpawn)
            {
                enemySpawn = Time.time + enemyDelay;
                SpawnEnemies();
            }
        }
    }

    void SetupSpawnArea()
    {
        Vector3 stageDimensions = Camera.main.ViewportToWorldPoint(new Vector3(1, 1, Camera.main.nearClipPlane));
        SpawnArea_L.transform.position = new Vector3(-stageDimensions.x - 0.5f, stageDimensions.y + 0.5f, 0);
        SpawnArea_R.transform.position = new Vector3(stageDimensions.x + 0.5f, stageDimensions.y + 0.5f,0);
    }
    void SetEnemyCounter()
    {
        EnemyCounter = Random.Range(2, 10);
        SpawnPoints.Add(SpawnPoint);
        SpawnPoints.Add(SpawnPoint2);
        SpawnPoints.Add(SpawnPoint3);
    }

    void SpawnEnemies()
    {
        int tempCounter = 0;
        Quaternion tempAngle;
        Vector3 tempPos;
        
        EnemyCounter = Random.Range(2, 10);
        
        for (int i = 0; i < EnemyCounter; i++)
        {
            tempCounter = Random.Range(0, 3);
            tempAngle = Quaternion.identity;
            tempPos.x = Random.Range(SpawnArea_L.transform.position.x, SpawnArea_R.transform.position.x);
            tempPos.y = SpawnArea_L.transform.position.y;
            tempPos.z = SpawnArea_L.transform.position.z;
            float waitTime = 4;
            
            while (enemyPauseSpawn < waitTime)
            {
                enemyPauseSpawn += Time.deltaTime;
            }
            
            Instantiate(enemyPrefab, tempPos, tempAngle);
            enemyPrefab.SetActive(true);
        }

    }

    void SpawnPowerup()
    {
        Vector3 tempPos;
        tempPos.x = Random.Range(SpawnArea_L.transform.position.x, SpawnArea_R.transform.position.x);
        tempPos.y = SpawnArea_L.transform.position.y;
        tempPos.z = SpawnArea_L.transform.position.z;
        Instantiate(powerUpPrefab, tempPos, Quaternion.identity);
        powerUpPrefab.SetActive(true);
    }

    public void DeleteFromGM(string tag)
    {
        var gameObjects =  GameObject.FindGameObjectsWithTag (tag);
 
        for(var i = 0 ; i < gameObjects.Length ; i ++)
            Destroy(gameObjects[i]);
    }

}
