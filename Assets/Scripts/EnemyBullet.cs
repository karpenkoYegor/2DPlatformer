using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    private float speed = 10f;

    void Start()
    {
        this.gameObject.GetComponent<Rigidbody2D>().velocity = transform.right * speed;
    }
    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            CharacterController2D.health--;
        }
        if (other.gameObject.CompareTag("Enemy"))
        { }
        else
        {
            Destroy(this.gameObject);
        }
    }
}
