using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using enemy;

public class normalEnemy : EnemyController
{
    [Header("Components")]
    public Sprite[] walkAnim;
    public SpriteRenderer mySpriteRenderer;
    public Sprite daedAnim;
    public Rigidbody2D rid;
    public BoxCollider2D col;

    [Header("Frame")]
    public int currentFrame = 1;
    public int framesPerSecond = 2;

    private float secondsPerFrame;
    private GameManagement game;
    private AudioSource TheDie;
    // Start is called before the first frame update
    void Start()
    {
        marioScript = GameObject.FindWithTag("Player").GetComponent<MarioController>();
        game = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManagement>();
        secondsPerFrame = 1.0f / framesPerSecond;
        Invoke("NextFrame", secondsPerFrame);
        TheDie = GetComponent<AudioSource>();
    }

    void NextFrame()
    {
        currentFrame = (currentFrame + 1) % walkAnim.Length;
        mySpriteRenderer.sprite = walkAnim[currentFrame];
        Invoke("NextFrame", secondsPerFrame);
    }

    private void Update()
    {
        move();
        checkCollision();
    }
    public void die()
    {
        TheDie.Play();
        game.updateScore(100);
        moveSpeed = 0;
        mySpriteRenderer.sprite = daedAnim;
        rid.simulated = false;
        col.enabled = false;
        CancelInvoke();
        Invoke("destroy", 0.2f);
    }

    public void unusualDie()
    {
        TheDie.Play();
        moveSpeed = 0;
        game.updateScore(100);
        //mySpriteRenderer.sprite = daedAnim;
        rid.velocity = new Vector2(0, 0);
        rid.AddForce(Vector2.up * 3, ForceMode2D.Impulse);
        gameObject.transform.localScale = new Vector3(1, -1, 1);
        col.enabled = false;
        CancelInvoke();
        Invoke("destroy", 2f);
    }
    private void destroy()
    {
        Destroy(gameObject);
    }
    // Update is called once per frame

    private void OnCollisionEnter2D(Collision2D collision)
    {
       
        if (collision.gameObject.tag.Equals("Player"))
        {
            marioScript.die();
        }else if ((collision.gameObject.tag.Equals("enemy")))
        {
            unusualDie();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag.Equals("Player") && collision.gameObject.GetComponent<MarioController>().isInvincible)
            unusualDie();
    }
}
