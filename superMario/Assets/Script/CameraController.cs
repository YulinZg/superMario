using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private float smoothing;
    private Transform target;
    private MarioController mario;

    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
        mario = target.gameObject.GetComponent<MarioController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!mario.isDead)
            if (transform.position != target.position)
                MoveCamera();
    }

    void MoveCamera()
    {
        Vector3 targetPos = target.position;
        if (target.position.y < -4.0f && target.position.x > -8.0f && target.position.x < 9.0f)
        {
            targetPos = new Vector3(0.5f, -13.5f, -10.0f);
        }
        else
        {
            targetPos.x = Mathf.Clamp(targetPos.x, 5.0f, 203.0f);
            targetPos.y = 3.0f;
            targetPos.z = -10.0f;
        }
        transform.position = Vector3.Lerp(transform.position, targetPos, smoothing);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag.Equals("enemy"))
        {
            collision.gameObject.GetComponent<enemy.EnemyController>().moveSpeed = 1f;
        }
    }
}
