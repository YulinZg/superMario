using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarioController : MonoBehaviour
{
    //[Header("Sprite")]
    //public Sprite jumpSprite;
    //public Sprite standSprite;
    //public Sprite stopSprite;
    [Header("Mario State")]
    public bool isDead = false;

    [Header("Movement")]
    public float moveSpeed = 10f;
    public float jumpForce = 10f;
    public Vector2 dir;
    public bool isChangingDir;

    [Header("Physics")]
    public float linearDrag = 1.5f;
    public float gravity = 1f;
    public float fallMultiplier = 4f;
    public float maxSpeed = 7f;

    [Header("Collision")]
    public bool onGround = false;
    public float groundLength = 0.3f;

    [Header("Components")]
    public BoxCollider2D col;
    public Rigidbody2D rid;
    public Animator animator;
    public LayerMask groundLayer;
    public LayerMask enemyLayer;
    public SpriteRenderer mySprite;
    
   
    //private enum Status
    //{
    //    stand,
    //    dead,
    //    run,
    //    back,
    //    jump,
    //    eat
    //}

    //private Status statu;
    void Start()
    {
        AudioClip audioclip = Resources.Load("Audios/Nintendo - Super Mario bros. theme") as AudioClip;
        AudioSource audioSource = gameObject.GetComponent<AudioSource>();

        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        audioSource.clip = audioclip;
        audioSource.loop = true;
        audioSource.Play();
    }

    private void FixedUpdate()
    {
        if (isDead)
            return;
        move(dir.x);
        modifyPhysics();
    }
    // Update is called once per frame
    void Update()
    {
        if (isDead)
            return;
        dir = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        isOnGround();
        checkDamage();
        jump();
    }

    public void die()
    {
        isDead = true;
        animator.Play("dead");
        rid.velocity = new Vector2(0, 0);
        rid.AddForce(Vector2.up * 4, ForceMode2D.Impulse);
        col.enabled = false;
        Debug.Log("die");
        AudioClip audioclip = Resources.Load("Audios/Die") as AudioClip;
        AudioSource audioSource = gameObject.GetComponent<AudioSource>();

        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        audioSource.clip = audioclip;
        audioSource.Play();
    }
    private void move(float dir)
    {
        rid.AddForce(Vector2.right * dir * moveSpeed);
        if (Mathf.Abs(rid.velocity.x) > 0.2 && onGround)
            animator.SetBool("isRunning", true);
        else if (Mathf.Abs(rid.velocity.x) <= 0.2 && onGround)
            animator.SetBool("isRunning", false);
        if (dir < 0)
            transform.localScale = new Vector3(-1, 1, 1);
        else if (dir > 0)
            transform.localScale = new Vector3(1, 1, 1);
        if (Mathf.Abs(rid.velocity.x) > maxSpeed)
        {
            rid.velocity = new Vector2(Mathf.Sign(rid.velocity.x) * maxSpeed, rid.velocity.y);
        }
    }

    private void jump()
    {
        if (onGround && Input.GetButtonDown("Jump"))
        {
            animator.Play("jump");
            rid.velocity = new Vector2(rid.velocity.x, 0);
            rid.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);  
        }
    }

    private void isOnGround()
    {
        onGround = Physics2D.Raycast(transform.position + new Vector3( -0.32f, -0.1f, 0), Vector2.down, groundLength, groundLayer) ||
                   Physics2D.Raycast(transform.position + new Vector3( 0.3f, -0.1f, 0), Vector2.down, groundLength, groundLayer);
        //Debug.DrawRay(transform.position, Vector2.down * groundLength);
        animator.SetBool("isOnGround", onGround);
    }

    private void modifyPhysics()
    {
        isChangingDir = (dir.x > 0 && rid.velocity.x < 0) || (dir.x < 0 && rid.velocity.x > 0) && onGround;
        if (onGround && !animator.GetCurrentAnimatorStateInfo(0).IsName("jump"))
        {
            rid.gravityScale = gravity;
            moveSpeed = 10f;
            if (isChangingDir)
            {
                rid.drag = linearDrag;
                animator.Play("stop");
            }
            else
            {
                rid.drag = 0f;
            }
        }
        else if (!onGround)
        {
            rid.gravityScale = gravity * 1.5f;
            rid.drag = linearDrag * 0.15f;
            moveSpeed = 3f;
            if (rid.velocity.y < 0)
            {
                rid.gravityScale = gravity * fallMultiplier;
            }
            else if (rid.velocity.y > 0 && !Input.GetButton("Jump"))
            {
                rid.gravityScale = gravity * fallMultiplier / 2;
            }
        }
    }

    private void checkDamage()
    {
        for (int i = 0; i < 30; i++)
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position + new Vector3(i / 50f - 0.3f, -0.1f, 0), Vector2.down, groundLength, enemyLayer);
            Debug.DrawRay(transform.position + new Vector3(i / 50f - 0.3f, -0.3f, 0), Vector2.down * groundLength);
            if (hit.collider && hit.collider.GetComponent<normalEnemy>())
            {
                if (hit.collider.CompareTag("enemy"))
                    hit.collider.GetComponent<normalEnemy>().die();
                rid.velocity = new Vector2(rid.velocity.x, 0);
                rid.AddForce(Vector2.up * 2, ForceMode2D.Impulse);
            }
            if (hit.collider && hit.collider.GetComponent<TurtleEnemy>())
            {
                if (hit.collider.CompareTag("enemy") && !hit.collider.GetComponent<TurtleEnemy>().isShell)
                    hit.collider.GetComponent<TurtleEnemy>().die();
                else if (hit.collider.CompareTag("enemy") && hit.collider.GetComponent<TurtleEnemy>().isShell)
                {
                    if(i >= 15)
                    {
                        hit.collider.GetComponent<TurtleEnemy>().shellMoveDir = new Vector3(1,0,0);
                        hit.collider.GetComponent<TurtleEnemy>().checkDir.x = 1;
                    }
                    else if (i < 15)
                    {
                        hit.collider.GetComponent<TurtleEnemy>().shellMoveDir = new Vector3(-1, 0, 0);
                        hit.collider.GetComponent<TurtleEnemy>().checkDir.x = -1; 
                    }
                    hit.collider.gameObject.layer = LayerMask.NameToLayer("shell");
                    if (Mathf.Sign(hit.collider.GetComponent<TurtleEnemy>().checkDir.x) != Mathf.Sign(hit.collider.GetComponent<TurtleEnemy>().rayOffset.x))
                        hit.collider.GetComponent<TurtleEnemy>().rayOffset *= -1;
                    hit.collider.GetComponent<TurtleEnemy>().canShellMove();
                }
                rid.velocity = new Vector2(rid.velocity.x, 0);
                rid.AddForce(Vector2.up * 2, ForceMode2D.Impulse);
            }
        }

    }
}
