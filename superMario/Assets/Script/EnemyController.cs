using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    // Start is called before the first frame update
    [Header("Movement")]
    public float moveSpeed = 1f;
    public Vector3 dir = new Vector3(-1,0,0);

    [Header("Player")]
    public MarioController marioScript;

    [Header("Ray")]
    public Vector3 rayOffset = new Vector3(-0.4f,0,0);
    public LayerMask collisionLayer;
    public float checkLength = 1.0f;
    public Vector2 checkDir = new Vector2(-1,0);
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        move();
        checkCollision();
        //checkTile();
        //Debug.DrawRay(transform.position + rayOffset, checkDir * checkLength);
    }

    void move()
    {
        transform.position += dir * Time.deltaTime * moveSpeed;
    }

    void changeDir()
    {
        dir.x *= -1;
        checkDir.x *= -1;
        rayOffset *= -1;
        transform.localScale = new Vector3(dir.x, 1, 1);
    }

    void checkCollision()
    {
        bool isHitEnemy = Physics2D.Raycast(transform.position + rayOffset, checkDir, checkLength, collisionLayer);
        if (isHitEnemy)
        {
            changeDir();   
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag.Equals("Player"))
        {
            marioScript.die();
        }
    }
}
