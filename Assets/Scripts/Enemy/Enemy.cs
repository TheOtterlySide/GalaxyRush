using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float enemySpeed;
    [SerializeField] private int enemyScore;
    // Start is called before the first frame update
    void Start()
    {
        DecideSpeed();
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.down * (enemySpeed * Time.deltaTime));
    }

    void DecideSpeed()
    {
        enemySpeed = Random.Range(1, 9);
    }
    
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.transform.CompareTag("Bullet"))
        {
            GameManager.updateScore(enemyScore);
            DestroySelf();
        }

        if (other.transform.CompareTag("Player"))
        {
            GameManager.updateScore(-enemyScore);
            DestroySelf();
        }

        if (other.transform.CompareTag("Wall"))
        {
            DestroySelf();
        }
    }
    
    void DestroySelf()
    {
        gameObject.SetActive(false);
        Destroy(gameObject);
    }
}
