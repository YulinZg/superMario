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
    public bool isBig = false;
    public bool isBlink = false;
    public bool canFire = false;
    public bool isInvincible = false;

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
    public BoxCollider2D tri;
    public Rigidbody2D rid;
    public Animator animator;
    public LayerMask groundLayer;
    public LayerMask enemyLayer;
    public SpriteRenderer mySprite;
    public RuntimeAnimatorController bigMario;
    public RuntimeAnimatorController smallMario;
    public Sprite midSprite;
    public GameObject fireBall;


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
        if (!isDead && !isBlink)
        {
            move(dir.x);
            modifyPhysics();
        }

    }
    // Update is called once per frame
    void Update()
    {
        if (!isDead && !isBlink)
        {
            if (Mathf.Abs(rid.velocity.x) <= 0.2 && onGround && !animator.GetCurrentAnimatorStateInfo(0).IsName("dead"))
                animator.Play("idle");

            dir = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
            isOnGround();
            jump();
            checkDamage();

            if (canFire && Input.GetKeyDown(KeyCode.X))
            {
                Instantiate(fireBall, transform.position + new Vector3(0,0.1f,0), Quaternion.identity);
            }
        }

    }

    public void die()
    {
        if (isBig)
        {
            if (canFire)
            {
                canFire = false;
                mySprite.color = Color.white;
            }
            else
            {
                animator.Play("dead");
                rid.velocity = new Vector2(0, 0);
                col.enabled = false;
                rid.simulated = false;
                StartCoroutine(DoBlinks(40, 1.5f / 40));
                Invoke("changeToSmall", 1.7f);
            }
            
        }
        else
        {
            isDead = true;
            rid.velocity = new Vector2(0, 0);
            col.enabled = false;
            animator.Play("dead");
            rid.AddForce(Vector2.up * 6, ForceMode2D.Impulse);
            Debug.Log("die");
        }

    }

    private void changeToSmall()
    {

        col.size = new Vector2(0.6591296f, 0.7877641f);
        col.offset = new Vector2(-0.01010871f, -0.007165909f);
        isBig = false;
        col.enabled = true;
        rid.simulated = true;
        animator.runtimeAnimatorController = smallMario;
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
        onGround = Physics2D.Raycast(transform.position + new Vector3(-0.32f, -0.1f, 0), Vector2.down, groundLength, groundLayer) ||
                   Physics2D.Raycast(transform.position + new Vector3(0.3f, -0.1f, 0), Vector2.down, groundLength, groundLayer);
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
        if (!isDead && !isBlink)
        {
            for (int i = 0; i < 20; i++)
            {
                RaycastHit2D hit;
                if (isBig)
                {
                    hit = Physics2D.Raycast(transform.position + new Vector3(i / 40f - 0.25f, 0, 0), Vector2.down, groundLength, enemyLayer);
                    Debug.DrawRay(transform.position + new Vector3(i / 40f - 0.25f, 0, 0), Vector2.down * groundLength);
                }
                else
                {
                    hit = Physics2D.Raycast(transform.position + new Vector3(i / 40f - 0.25f, -0.1f, 0), Vector2.down, groundLength, enemyLayer);
                    Debug.DrawRay(transform.position + new Vector3(i / 40f - 0.25f, -0.1f, 0), Vector2.down * groundLength);
                }
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
                        if (i >= 10)
                        {
                            hit.collider.GetComponent<TurtleEnemy>().shellMoveDir = new Vector3(1, 0, 0);
                            hit.collider.GetComponent<TurtleEnemy>().checkDir.x = 1;
                        }
                        else if (i < 10)
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

    IEnumerator DoBlinks(int numBlinks, float time)
    {
        isBlink = true;
        for (int i = 0; i < numBlinks; i++)
        {
            mySprite.enabled = !mySprite.enabled;
            yield return new WaitForSeconds(time);
        }
        isBlink = false;
        mySprite.enabled = true;
    }
    public void big()
    {
        if (!isBig)
        {
            //animator.Play("dead");
            //animator.Play("idle");
            isBig = true;
            col.enabled = false;
            tri.enabled = false;
            rid.simulated = false;
            rid.velocity = new Vector2(0, 0);
            StartCoroutine(DoBlinks(40, 1.5f / 40));
            Invoke("changeToBig", 1.7f);
        }

    }

    private void changeToBig()
    {
        col.size = new Vector2(0.55f, 1.2f);
        col.offset = new Vector2(0, 0.3f);
        tri.size = new Vector2(0.65f, 1f);
        tri.offset = new Vector2(0, 0.2f);
        col.enabled = true;
        tri.enabled = true;
        rid.simulated = true;
        animator.runtimeAnimatorController = bigMario;

    }

    public void changeToFire()
    {
        if (isBig)
        {
            canFire = true;
            mySprite.color = Color.red;      
        }
    }

    IEnumerator invincibleBlinks(int numBlinks, float time)
    {
        //isBlink = true;
        for (int i = 0; i < numBlinks; i++)
        {
            if(i % 2 == 0)
                mySprite.color = Color.red;
            else
                mySprite.color = Color.white;
            yield return new WaitForSeconds(time);
        }
        isInvincible = false;
        if (canFire)
            mySprite.color = Color.red;
        else
            mySprite.color = Color.white;
    }

    public void invincible()
    {
        isInvincible = true;
        StartCoroutine(invincibleBlinks(400, 20f / 400));
    }
}
