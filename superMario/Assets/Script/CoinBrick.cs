using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinBrick : MonoBehaviour
{
    public float upForce;
    public GameObject obj;
    public int number = 10;
    public float upOffSet;
    public Sprite emptySprite;
    public float dropTime;
    public float setStaticTime;
    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rigidBody;
    private bool isEmpty = false;
    private BoxCollider2D trigger;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        rigidBody = GetComponent<Rigidbody2D>();
        trigger = GetComponent<BoxCollider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isEmpty && collision.gameObject.CompareTag("Player"))
        {
            AudioClip audioclip = Resources.Load("Audios/Gold") as AudioClip;
            AudioSource audioSource = gameObject.GetComponent<AudioSource>();

            if (audioSource == null)
            {
                audioSource = gameObject.AddComponent<AudioSource>();
            }

            audioSource.clip = audioclip;
            audioSource.Play();
            if (number > 1)
            {
                trigger.enabled = false;
                number--;
                rigidBody.AddForce(Vector2.up * upForce, ForceMode2D.Impulse);
                Invoke(nameof(Drop), dropTime);
                Invoke(nameof(SetTrigger), setStaticTime);
            }
            else 
            {
                rigidBody.AddForce(Vector2.up * upForce, ForceMode2D.Impulse);
                Invoke(nameof(Drop), dropTime);
                isEmpty = true;
                spriteRenderer.sprite = emptySprite;
                Invoke(nameof(SetStatic), setStaticTime);
            }
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

    void SetTrigger()
    {
        trigger.enabled = true;
    }
}
