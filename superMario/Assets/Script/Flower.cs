using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flower : MonoBehaviour
{
    public float riseDistance = 1.0f;
    public float riseTime = 1.0f;
    public Sprite[] sprites;
    public float interval;
    private SpriteRenderer spriteRenderer;
    private float timer = 0;
    private int index = 0;
    private GameManagement game;
    // Start is called before the first frame update
    void Start()
    {
        game = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManagement>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        StartCoroutine(Rise(riseDistance, riseTime));
    }

    // Update is called once per frame
    void Update()
    {
        if (sprites.Length != 0)
            Animate();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<MarioController>().changeToFire();
            game.updateScore(2000);
            Destroy(gameObject);
        }
    }

    IEnumerator Rise(float distance, float duration)
    {
        float time = 0;
        Vector3 start = transform.position;
        while (time < duration)
        {
            transform.position = Vector3.Lerp(start, new Vector3(start.x, start.y + distance, start.z), time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        transform.position = new Vector3(start.x, start.y + distance, start.z);
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
