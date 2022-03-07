using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    public float force;
    public float disappearTime;
    public Sprite[] sprites;
    public float interval;
    private SpriteRenderer spriteRenderer;
    private float timer = 0;
    private int index = 0;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        GetComponent<Rigidbody2D>().AddForce(Vector2.up * force, ForceMode2D.Impulse);
        Invoke(nameof(Disappear), disappearTime);
    }

    // Update is called once per frame
    void Update()
    {
        if (sprites.Length != 0)
            Animate();
    }

    void Disappear()
    {
        Destroy(gameObject);
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
}
