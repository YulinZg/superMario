using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pipe : MonoBehaviour
{
    public bool isFirst;
    private MarioController mario;
    private bool canIn = false;

    // Start is called before the first frame update
    void Start()
    {
        mario = GameObject.FindGameObjectWithTag("Player").GetComponent<MarioController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (canIn)
        {
            if (isFirst && Input.GetKeyDown(KeyCode.DownArrow))
            {
                mario.isBlink = true;
                mario.rid.simulated = false;
                mario.col.enabled = false;
                mario.tri.enabled = false;
                mario.rid.velocity = Vector2.zero;
                StartCoroutine(Move(-2.0f, 2.0f));
            }
            if (!isFirst)
            {
                mario.isBlink = true;
                mario.rid.simulated = false;
                mario.col.enabled = false;
                mario.tri.enabled = false;
                mario.rid.velocity = Vector2.zero;
                StartCoroutine(Move(2.0f, 2.0f));
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
            canIn = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
            canIn = false;
    }

    IEnumerator Move(float distance, float duration)
    {
        if (!isFirst)
            mario.gameObject.transform.position = new Vector3(156.0f, -2.6f);
        float time = 0;
        Vector3 start = mario.gameObject.transform.position;
        while (time < duration)
        {
            mario.gameObject.transform.position = Vector3.Lerp(start, new Vector3(start.x, start.y + distance, start.z), time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        if (isFirst)
            mario.gameObject.transform.position = new Vector3(-5.5f, -6.5f);
        mario.rid.simulated = true;
        mario.col.enabled = true;
        mario.tri.enabled = true;
        mario.isBlink = false;
    }
}
