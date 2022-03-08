using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mushroom : MonoBehaviour
{
    public float moveSpeed = 1f;
    public Vector3 dir = new Vector3(1, 0, 0);
    public Vector3 rayOffset = new Vector3(0.4f, 0, 0);
    public LayerMask collisionLayer;
    public float checkLength = 0.05f;
    public Vector2 checkDir = new Vector2(1, 0);
    public float riseDistance = 1.0f;
    public float riseTime = 1.0f;

    private bool isShown = false;
    private Rigidbody2D rigidBody;

    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        StartCoroutine(Rise(riseDistance, riseTime));
    }

    // Update is called once per frame
    void Update()
    {
        if (isShown)
        {
            transform.position += moveSpeed * Time.deltaTime * dir;
            if (Physics2D.Raycast(transform.position + rayOffset, checkDir, checkLength, collisionLayer))
            {
                dir.x *= -1;
                checkDir.x *= -1;
                rayOffset *= -1;
                transform.localScale = new Vector3(dir.x, 1, 1);
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
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
        isShown = true;
        rigidBody.simulated = true;
    }
}