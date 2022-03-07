using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using enemy;

public class NormalEnemy : EnemyController
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
    // Start is called before the first frame update
    void Start()
    {
        marioScript = GameObject.FindWithTag("Player").GetComponent<MarioController>();
        secondsPerFrame = 1.0f / framesPerSecond;
        Invoke("NextFrame", secondsPerFrame);
    }

    void NextFrame()
    {
        currentFrame = (currentFrame + 1) % walkAnim.Length;
        mySpriteRenderer.sprite = walkAnim[currentFrame];
        Invoke("NextFrame", secondsPerFrame);
    }

    public void die()
    {
        moveSpeed = 0;
        mySpriteRenderer.sprite = daedAnim;
        rid.simulated = false;
        col.enabled = false;
        CancelInvoke();
        Invoke("destroy", 0.2f);
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
            Debug.Log("normal£¡£¡");
            marioScript.die();
        }else if ((collision.gameObject.tag.Equals("enemy")))
        {
            die();
        }
    }
}
