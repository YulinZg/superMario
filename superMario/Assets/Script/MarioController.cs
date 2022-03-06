using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarioController : MonoBehaviour
{
    //[Header("Sprite")]
    //public Sprite jumpSprite;
    //public Sprite standSprite;
    //public Sprite stopSprite;

    [Header("Movement")]
    public float moveSpeed = 10f;
    public float jumpForce = 10f;
    public Vector2 dir;
    public bool isChangingDir;

    [Header("Physics")]
    public float linearDrag = 1.5f;
    public float gravity = 1f;
    public float fallMultiplier = 5f;
    public float maxSpeed = 7f;

    [Header("Collision")]
    public bool onGround = false;
    public float groundLength = 0.3f;

    [Header("Components")]
    public Rigidbody2D rid;
    public Animator animator;
    public LayerMask groundLayer;
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
        move(dir.x);
        modifyPhysics();
    }
    // Update is called once per frame
    void Update()
    {
        dir = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        isOnGround();
        jump();
    }

    private void move(float dir)
    {
        rid.AddForce(Vector2.right * dir * moveSpeed);
        if (Mathf.Abs(rid.velocity.x) > 0.2 && onGround)
            animator.SetBool("isRunning", true);
        else if (rid.velocity.x <= 0.2 && onGround)
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
        //if (onGround && !isChangingDir)
        //{
        //    animator.SetBool("isOnGround", true);
        //    //mySprite.sprite = standSprite;
        //}
        //else if (!onGround && !isChangingDir)
        //{
        //    animator.SetBool("isOnGround", false);
        //    //mySprite.sprite = jumpSprite;
        //}
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
            moveSpeed = 2f;
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
}
