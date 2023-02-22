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

    [SerializeField] private Bullet playerBullet;
    [SerializeField] private float playerShootCooldown;
    [SerializeField] private float playerShootTime;
    
    void Start()
    {
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Input.GetKey("left") || Input.GetKey("right") || Input.GetKey("up") || Input.GetKey("down"))
        {
            playerXPos = Input.GetAxis("Horizontal");
            playerYPos = Input.GetAxis("Vertical");
            UpdatePosition(playerXPos, playerYPos);
        }
    }

    private void Update()
    {
        playerShootTime += Time.deltaTime;
        if (Input.GetButton("Fire1") && playerShootTime > playerShootCooldown)
        {
            playerShootTime = 0;
            ShootingBullet();
        }
    }

    void SetSpeed(bool powerUp)
    {
        if (powerUp == true)
        {
            playerSpeed *= 2;
        }
        else
        {
            playerSpeed /= 2;
        }
    }

    void UpdatePosition(float xPos, float yPos)
    {
            transform.Translate(new Vector3(xPos,yPos) * playerSpeed * Time.deltaTime);
    }

    void OnCollisionEnter(Collision collision) 
    {
        if(collision.gameObject.name == "Left")  // or if(gameObject.CompareTag("YourWallTag"))
        {
            playerSpeed = 0;
        }
    }

    void ShootingBullet()
    {
        Debug.Log(gameObject.transform.position);
        Debug.Log("Test");
        print("Test");
        Instantiate(playerBullet, gameObject.transform.position, Quaternion.identity);
    }
}
