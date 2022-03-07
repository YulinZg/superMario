using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brick : MonoBehaviour
{
    public Sprite[] sprites;
    public Sprite emptySprite;
    public float interval;
    public float invokeTime;
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
            spriteRenderer.sprite = sprites[index];
            index++;
            if (index == sprites.Length)
                index = 0;
            timer = 0;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!isEmpty)
        {
            Invoke(nameof(Drop), invokeTime);
        }
    }

    void Drop()
    {
        isEmpty = true;
        spriteRenderer.sprite = emptySprite;
        rigidBody.bodyType = RigidbodyType2D.Static;
        transform.position = Vector3.zero;
    }
}
