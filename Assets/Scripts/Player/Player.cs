using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private float playerSpeed;
    [SerializeField] private float playerDMG;
    [SerializeField] private float playerXPos;
    [SerializeField] private float playerYPos;
    
    public bool playerAlive;
    public int playerLife;
    public bool gameRunning;
    
    [SerializeField] private bool playerPowerStatus;
    [SerializeField] private float playerShootCooldown;
    [SerializeField] private float playerShootTime;
    [SerializeField] private float playerPowerCooldown;
    [SerializeField] private float playerPowerTime;

    [SerializeField] public PlayerControl controls;
    
    void Start()
    {
        playerPowerStatus = false;
        playerAlive = true;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (playerAlive && gameRunning)
        {
            if (Input.GetKey("left") || Input.GetKey("right") || Input.GetKey("up") || Input.GetKey("down"))
            {
                playerXPos = Input.GetAxis("Horizontal");
                playerYPos = Input.GetAxis("Vertical");
                controls.UpdatePosition(playerXPos, playerYPos, playerSpeed);
            }
        }
        
    }

    private void Update()
    {
        if (playerAlive & gameRunning)
        {
            playerShootTime += Time.deltaTime;
            if (Input.GetButton("Fire1") && playerShootTime > playerShootCooldown)
            {
                playerShootTime = 0;
                controls.ShootingBullet();
            }

            if (playerPowerStatus)
            {
                playerPowerTime += Time.deltaTime;
                if (playerPowerTime > playerPowerCooldown)
                {
                    playerPowerTime = 0;
                    OnPowerUp(false);
                }
            }
        }
    }

    void OnPowerUp(bool powerUp)
    {
        if (powerUp == true && playerPowerStatus == false)
        {
            playerPowerStatus = true;
            playerSpeed *= 2;
        }
        else
        {
            playerPowerStatus = false;
            playerPowerTime = 0;
            playerSpeed /= 2;
        }
    }



    void OnCollisionEnter2D(Collision2D collision) 
    {
        if (collision.gameObject.CompareTag("PowerUp"))
        {
            OnPowerUp(true);
        }

        if (collision.gameObject.tag is "Enemy" or "BulletEnemy")
        {
            HandleLife();
        }
    }

    void HandleLife()
    {
        if (playerLife != 0)
        {
            playerLife--;
        }
        else
        {
            playerLife = 0;
            playerAlive = false;
        }
    }
}
