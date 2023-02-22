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
    
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey("left") || Input.GetKey("right") || Input.GetKey("up") || Input.GetKey("down"))
        {
            playerXPos = Input.GetAxis("Horizontal");
            playerYPos = Input.GetAxis("Vertical");
            UpdatePosition(playerXPos, playerYPos);
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
}
