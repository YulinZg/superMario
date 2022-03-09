using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableBrick : MonoBehaviour
{
    public GameObject fragment;
    public GameObject killEnemy;
    public float upForce;
    private Rigidbody2D rigidBody;

    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (!collision.gameObject.GetComponent<MarioController>().isBig)
            {
                rigidBody.AddForce(Vector2.up * upForce, ForceMode2D.Impulse);
            }
                
            else
            {
                GameObject mario = GameObject.FindGameObjectWithTag("Player");
                mario.GetComponent<AudioSource>().clip = mario.GetComponent<MarioController>().breakBrick;
                mario.GetComponent<AudioSource>().Play();
                Instantiate(killEnemy, transform.position + Vector3.up, Quaternion.identity);
                Instantiate(fragment, transform.position + new Vector3(0.25f, 0, 0), Quaternion.Euler(0, 0, 30.0f));
                Instantiate(fragment, transform.position + new Vector3(-0.25f, 0, 0), Quaternion.Euler(0, 0, -30.0f));
                Instantiate(fragment, transform.position + new Vector3(0, 0.25f, 0), Quaternion.Euler(0, 0, 60.0f));
                Instantiate(fragment, transform.position + new Vector3(0, -0.25f, 0), Quaternion.Euler(0, 0, -60.0f));
                Destroy(transform.parent.gameObject);
            }
        }
        else if (collision.gameObject.CompareTag("enemy"))
        {
            collision.GetComponent<normalEnemy>().unusualDie();
        }
    }
}
