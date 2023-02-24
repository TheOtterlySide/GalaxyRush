using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
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
        transform.Translate(speed * Time.deltaTime * Vector3.up);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            DestroySelf();
        }
        
    }

}
