using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    [SerializeField] 
    private float speed = 200f;

    [SerializeField] 
    private float lifeTime = 5f;
    
    
    internal void DestroySelf()
    {
        gameObject.SetActive(false);
        Destroy(gameObject);
    }
    
    private void Awake()
    {
        Invoke("DestroySelf", lifeTime);
    }
    
    private void Update()
    {
        transform.Translate(speed * Time.deltaTime * Vector2.down);
    }
    
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.name == "Player")
        {
            DestroySelf();    
        }
    }
}
