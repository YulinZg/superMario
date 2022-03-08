using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingCoin : MonoBehaviour
{
    public Sprite[] sprites;
    public float interval;
    private SpriteRenderer spriteRenderer;
    private float timer = 0;
    private int index = 0;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (sprites.Length != 0)
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
    }
}
