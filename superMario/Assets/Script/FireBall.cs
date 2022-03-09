using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBall : MonoBehaviour
{
    public float destroyTime;

    [Header("Frame")]
    public int currentFrame = 3;
    public int framesPerSecond = 8;

    [Header("Components")]
    public Sprite[] fireAnim;
    public SpriteRenderer mySpriteRenderer;
    public Rigidbody2D rid;

    private float froce = 6.0f;
    private float secondsPerFrame;

    private MarioController marioScript;

    // Start is called before the first frame update
    void Start()
    {
        marioScript = GameObject.FindWithTag("Player").GetComponent<MarioController>();
        if(marioScript.gameObject.transform.localScale.x > 0)
        {
            rid.velocity = new Vector2(marioScript.gameObject.GetComponent<Rigidbody2D>().velocity.x, 0);
            rid.AddForce(Vector3.right * froce, ForceMode2D.Impulse);
        }  
        else if (marioScript.gameObject.transform.localScale.x < 0)
        {
            rid.velocity = new Vector2(marioScript.gameObject.GetComponent<Rigidbody2D>().velocity.x, 0);
            rid.AddForce(Vector3.left * froce, ForceMode2D.Impulse);
        }
            
        rid.AddForce(Vector3.up * froce/2, ForceMode2D.Impulse);
        secondsPerFrame = 1.0f / framesPerSecond;
        Invoke("NextFrame", secondsPerFrame);
        Invoke("Disappear", destroyTime);
    }

    void NextFrame()
    {
        currentFrame = (currentFrame + 1) % fireAnim.Length;
        mySpriteRenderer.sprite = fireAnim[currentFrame];
        Invoke("NextFrame", secondsPerFrame);
    }

    // Update is called once per frame
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            if (collision.gameObject.GetComponent<normalEnemy>())
                collision.gameObject.GetComponent<normalEnemy>().unusualDie();
            if (collision.gameObject.GetComponent<TurtleEnemy>())
                collision.gameObject.GetComponent<TurtleEnemy>().fireDie();
            Destroy(gameObject);
        }
    }

    void Disappear()
    {
        Destroy(gameObject);
    }
}
