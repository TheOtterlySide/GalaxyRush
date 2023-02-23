using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    [SerializeField] 
    private float speed;

    [SerializeField] 
    private float lifeTime;
    
    
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
        Debug.Log(other);
        if (other.gameObject.name == "Player")
        {
            DestroySelf();    
        }
    }
}
