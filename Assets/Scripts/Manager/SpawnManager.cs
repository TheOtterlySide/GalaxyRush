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
    [SerializeField] private float powerUpCooldown;
    [SerializeField] private float powerUpActve;

    // Start is called before the first frame update
    void Start()
    {
        SetEnemyCounter();
        SpawnEnemies();
    }

    // Update is called once per frame
    void Update()
    {
        powerUpCooldown += Time.deltaTime;
        if (powerUpCooldown > powerUpActve)
        {
            SpawnPowerup();
        }
    }

    void SetEnemyCounter()
    {
        EnemyCounter = Random.Range(1, 10);
        SpawnPoints.Add(SpawnPoint);
        SpawnPoints.Add(SpawnPoint2);
        SpawnPoints.Add(SpawnPoint3);
    }

    void SpawnEnemies()
    {
        int tempCounter = 0;
        Quaternion tempAngle;
        Vector3 tempPos;
        float offset;
        for (int i = 0; i < EnemyCounter; i++)
        {
            Debug.Log("Heres");
            tempCounter = Random.Range(1, 3);
            tempAngle = Quaternion.identity;
            tempPos = SpawnPoints[tempCounter].transform.position;
            Instantiate(enemyPrefab, tempPos, tempAngle);
        }

    }

    void SpawnPowerup()
    {
        Instantiate(powerUpPrefab, SpawnPoints[1].transform.position, Quaternion.identity);
    }

}
