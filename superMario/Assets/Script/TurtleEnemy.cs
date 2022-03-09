using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using enemy;

public class TurtleEnemy : EnemyController
{
    [Header("Components")]
    public Sprite[] walkAnim;
    public SpriteRenderer mySpriteRenderer;
    public Sprite shellAnim;
    public Rigidbody2D rid;
    public BoxCollider2D col;

    [Header("Frame")]
    public int currentFrame = 1;
    public int framesPerSecond = 2;

    [Header("Shell")]
    public float shellMoveSpeed;
    public Vector3 shellMoveDir;
    public bool isShell = false;
    public bool isShellMoving = false;

    private float secondsPerFrame;
    private GameManagement game;
    // Start is called before the first frame update
    void Start()
    {
        marioScript = GameObject.FindWithTag("Player").GetComponent<MarioController>();
        game = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManagement>();
        secondsPerFrame = 1.0f / framesPerSecond;
        Invoke("NextFrame", secondsPerFrame);
    }
    void NextFrame()
    {
        currentFrame = (currentFrame + 1) % walkAnim.Length;
        mySpriteRenderer.sprite = walkAnim[currentFrame];
        Invoke("NextFrame", secondsPerFrame);
    }

    void Update()
    {
        checkCollision();
        if (!isShell)
            move();
        else if(isShellMoving && isShell)
        {
            shellMove();
            //rid.velocity = new Vector2(rid.velocity.x, 0);
            //checkDir.x = shellMoveDir.x;
        }
        //checkTile();
        //Debug.DrawRay(transform.position + rayOffset, checkDir * checkLength);
    }
    public void die()
    {
        col.size = new Vector2(col.size.x, 0.61f);
        col.offset = new Vector2(0, -0.06f);
        isShell = true;
        //rid.constraints = RigidbodyConstraints2D.FreezeAll;
        mySpriteRenderer.sprite = shellAnim;
        game.updateScore(100);
        CancelInvoke();
    }

    public void canShellMove()
    {
        game.updateScore(100);
        collisionLayer = 1 << LayerMask.NameToLayer("Ground");
        checkDir.x = shellMoveDir.x;
        isShellMoving = true;
    }
    public void shellMove()
    {
        transform.position += shellMoveDir * Time.deltaTime * shellMoveSpeed;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag.Equals("Player") && !isShell)
        {
            marioScript.die();
        }
        else if (collision.gameObject.tag.Equals("Player") && isShell)
        {
            canShellMove();
            gameObject.layer = LayerMask.NameToLayer("shell");
            shellMoveDir = collision.gameObject.GetComponent<MarioController>().dir;
            checkDir.x = collision.gameObject.GetComponent<MarioController>().dir.x;
            if (Mathf.Sign(checkDir.x) != Mathf.Sign(rayOffset.x))
                rayOffset *= -1;
        }
    }

    new protected void changeDir()
    {
         shellMoveDir.x *= -1;
         dir.x *= -1;
         checkDir.x *= -1;
         rayOffset *= -1;
         transform.localScale = new Vector3(dir.x, 1, 1);
    }

    new protected void checkCollision()
    {
            
        bool isHitEnemy = Physics2D.Raycast(transform.position + rayOffset, checkDir, checkLength, collisionLayer);
        Debug.DrawRay(transform.position + rayOffset, checkDir * checkLength);
        if (isHitEnemy)
        {
            changeDir();
        }
    }

    public void fireDie()
    {
        mySpriteRenderer.sprite = shellAnim;
        CancelInvoke();
        isShell = true;
        rid.velocity = new Vector2(0, 0);
        rid.AddForce(Vector2.up * 3, ForceMode2D.Impulse);
        gameObject.transform.localScale = new Vector3 (1, -1, 1);
        col.enabled = false;
        Invoke("destroy", 2f);
    }

    private void destroy()
    {
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag.Equals("Player") && collision.gameObject.GetComponent<MarioController>().isInvincible)
            fireDie();
    }
    // Update is called once per frame
}
