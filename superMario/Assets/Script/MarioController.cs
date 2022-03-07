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
        onGround = Physics2D.Raycast(transform.position, Vector2.down, groundLength, groundLayer);
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
            Debug.DrawRay(transform.position + new Vector3(i / 60f - 0.25f, -0.1f, 0), Vector2.down * groundLength);
<<<<<<< HEAD
            if (hit.collider && hit.collider.GetComponent<NormalEnemy>())
            {
                if (hit.collider.CompareTag("enemy"))
                    hit.collider.GetComponent<NormalEnemy>().die();
=======
            if (hit.collider && hit.collider.GetComponent<normalEnemy>())
            {
                if (hit.collider.CompareTag("enemy"))
                    hit.collider.GetComponent<normalEnemy>().die();
>>>>>>> 16525a6eda6d6387df49d421214e109af80b3073
                rid.velocity = new Vector2(rid.velocity.x, 0);
                rid.AddForce(Vector2.up * 2, ForceMode2D.Impulse);
            }
            if (hit.collider && hit.collider.GetComponent<TurtleEnemy>())
            {
                if (hit.collider.CompareTag("enemy") && !hit.collider.GetComponent<TurtleEnemy>().isShell)
                    hit.collider.GetComponent<TurtleEnemy>().die();
                else if (hit.collider.CompareTag("enemy") && hit.collider.GetComponent<TurtleEnemy>().isShell)
                {
                    if(i >= 20)
                    {
                        hit.collider.GetComponent<TurtleEnemy>().shellMoveDir = new Vector3(1,0,0);
                    }
                    else if (i < 10)
                    {
                        hit.collider.GetComponent<TurtleEnemy>().shellMoveDir = new Vector3(-1, 0, 0);
                    }
                    hit.collider.gameObject.layer = LayerMask.NameToLayer("shell");
                    hit.collider.GetComponent<TurtleEnemy>().canShellMove();
                }
                rid.velocity = new Vector2(rid.velocity.x, 0);
                rid.AddForce(Vector2.up * 2, ForceMode2D.Impulse);
            }
        }

    }
}
