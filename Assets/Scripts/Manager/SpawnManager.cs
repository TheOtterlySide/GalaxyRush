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
    public bool gameRunning;
    
    // Start is called before the first frame update
    void Start()
    {
        SetEnemyCounter();
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

    void SetEnemyCounter()
    {
        EnemyCounter = Random.Range(0, 10);
        SpawnPoints.Add(SpawnPoint);
        SpawnPoints.Add(SpawnPoint2);
        SpawnPoints.Add(SpawnPoint3);
    }

    void SpawnEnemies()
    {
        int tempCounter = 0;
        Quaternion tempAngle;
        Vector3 tempPos;
        
        for (int i = 0; i < EnemyCounter; i++)
        {
            tempCounter = Random.Range(0, 3);
            tempAngle = Quaternion.identity;
            tempPos = SpawnPoints[tempCounter].transform.position;
            Instantiate(enemyPrefab, tempPos, tempAngle);
            enemyPrefab.SetActive(true);
        }

    }

    void SpawnPowerup()
    {
        Instantiate(powerUpPrefab, SpawnPoints[1].transform.position, Quaternion.identity);
        powerUpPrefab.SetActive(true);
    }

    public void DeleteFromGM(string tag)
    {
        var gameObjects =  GameObject.FindGameObjectsWithTag (tag);
 
        for(var i = 0 ; i < gameObjects.Length ; i ++)
            Destroy(gameObjects[i]);
    }

}
