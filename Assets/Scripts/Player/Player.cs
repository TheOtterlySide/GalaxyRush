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
    
    
    public GameObject WallsLeft;
    public GameObject WallsRight;
    public GameObject WallsTop;
    public GameObject WallsBottom;

    private Vector3 boundLeft;
    private Vector3 boundRight;
    private Vector3 boundTop;
    private Vector3 boundBottom;


    void Start()
    {
        
        SetupBoundaries();
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
        if (xPos < boundRight.x && xPos > boundLeft.x && yPos > boundBottom.y && yPos < boundTop.y)
        {
            transform.Translate(new Vector3(xPos,yPos) * playerSpeed * Time.deltaTime);
        }
    }

    void SetupBoundaries()
    {
        boundLeft = WallsLeft.transform.position;
        boundRight = WallsRight.transform.position;
        boundTop = WallsTop.transform.position;
        boundBottom = WallsBottom.transform.position;

    }
    
    void OnCollisionEnter(Collision collision) 
    {
        Debug.Log("COLLISSION");
        if(collision.gameObject.name == "Left")  // or if(gameObject.CompareTag("YourWallTag"))
        {
            playerSpeed = 0;
        }
    }
}
