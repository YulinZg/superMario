using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HiddenBrick : MonoBehaviour
{
    public float upForce;
    public GameObject obj;
    public float upOffSet;
    public Sprite emptySprite;
    public float dropTime;
    public float setStaticTime;
    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rigidBody;
    private bool isEmpty = false;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        rigidBody = GetComponent<Rigidbody2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isEmpty && collision.gameObject.CompareTag("Player"))
        {
            rigidBody.AddForce(Vector2.up * upForce, ForceMode2D.Impulse);
            isEmpty = true;
            spriteRenderer.color = new Color(1, 1, 1, 1);
            spriteRenderer.sprite = emptySprite;
            Invoke(nameof(Drop), dropTime);
            Invoke(nameof(SetStatic), setStaticTime);
        }
        else if (!isEmpty && collision.gameObject.CompareTag("enemy"))
        {
            collision.GetComponent<normalEnemy>().die();
        }
    }

    void Drop()
    {
        Instantiate(obj, transform.position + new Vector3(0, upOffSet, 0), Quaternion.identity);
    }

    void SetStatic()
    {
        rigidBody.bodyType = RigidbodyType2D.Static;
        transform.localPosition = Vector3.zero;
    }
}
