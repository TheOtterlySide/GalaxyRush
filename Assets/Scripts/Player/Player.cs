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
    
    [SerializeField] private bool playerPowerStatus;

    [SerializeField] private Bullet playerBullet;
    [SerializeField] private float playerShootCooldown;
    [SerializeField] private float playerShootTime;
    [SerializeField] private float playerPowerCooldown;
    [SerializeField] private float playerPowerTime;
    
    void Start()
    {
        playerPowerStatus = false;
        playerAlive = true;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (playerAlive)
        {
            if (Input.GetKey("left") || Input.GetKey("right") || Input.GetKey("up") || Input.GetKey("down"))
            {
                playerXPos = Input.GetAxis("Horizontal");
                playerYPos = Input.GetAxis("Vertical");
                UpdatePosition(playerXPos, playerYPos);
            }
        }
        
    }

    private void Update()
    {
        if (playerAlive)
        {
            playerShootTime += Time.deltaTime;
            if (Input.GetButton("Fire1") && playerShootTime > playerShootCooldown)
            {
                playerShootTime = 0;
                ShootingBullet();
            }

            if (playerPowerStatus == true)
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

    void UpdatePosition(float xPos, float yPos)
    {
            transform.Translate(new Vector3(xPos,yPos) * (playerSpeed * Time.deltaTime));
    }

    void OnCollisionEnter2D(Collision2D collision) 
    {
        if (collision.gameObject.tag == "PowerUp")
        {
            OnPowerUp(true);
        }

        if (collision.gameObject.tag == "Enemy" || collision.gameObject.tag == "BulletEnemy")
        {
            HandleLife();
        }
    }

    void ShootingBullet()
    {
        Instantiate(playerBullet, gameObject.transform.position, Quaternion.identity);
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
