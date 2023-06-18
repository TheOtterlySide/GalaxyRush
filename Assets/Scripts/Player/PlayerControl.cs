using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    
    [SerializeField] private GameObject spawnPointBullet;
    [SerializeField] private Bullet playerBullet;
    public void UpdatePosition(float xPos, float yPos, float playerSpeed)
    {
        transform.Translate(new Vector3(xPos,yPos) * (playerSpeed * Time.deltaTime));
    }
    
    public void ShootingBullet()
    {
        Instantiate(playerBullet, spawnPointBullet.transform.position, Quaternion.identity);
    }
}
