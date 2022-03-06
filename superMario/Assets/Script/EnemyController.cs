using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    // Start is called before the first frame update
    [Header("Movement")]
    public float moveSpeed = 7f;
    public Vector2 dir;

    [Header("Player")]
    public MarioController marioScript;

    [Header("Ray")]
    public LayerMask tileLayer;
    public LayerMask enemyLayer;
    public float checkLength = 1.0f;
    public Vector2 checkDir;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void move()
    {
        
    }

    void changeDir()
    {

    }

    void checkEnemy()
    {
        bool isHitEnemy = Physics2D.Raycast(transform.position, checkDir, checkLength, enemyLayer);
        if (isHitEnemy)
        {

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
