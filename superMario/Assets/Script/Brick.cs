using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brick : MonoBehaviour
{
    public GameObject obj;
    public float upOffSet;
    public Sprite[] sprites;
    public Sprite emptySprite;
    public float interval;
    public float dropTime;
    public float setStaticTime;
    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rigidBody;
    private float timer = 0;
    private int index = 0;
    private bool isEmpty = false;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        rigidBody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isEmpty && sprites.Length != 0)
            Animate();
    }

    void Animate()
    {
        timer += Time.deltaTime;
        if (timer >= interval)
        {
            index++;
            if (index == sprites.Length)
                index = 0;
            spriteRenderer.sprite = sprites[index];
            timer = 0;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!isEmpty && collision.gameObject.CompareTag("Player"))
        {
            isEmpty = true;
            spriteRenderer.sprite = emptySprite;
            Invoke(nameof(Drop), dropTime);
            Invoke(nameof(SetStatic), setStaticTime);
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
