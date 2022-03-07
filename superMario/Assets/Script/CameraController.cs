using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private float smoothing;
    private Transform target;

    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (target != null)
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
            targetPos.x = Mathf.Clamp(targetPos.x, 2.5f, 205.5f);
            targetPos.y = 1.5f;
            targetPos.z = -10.0f;
        }
        transform.position = Vector3.Lerp(transform.position, targetPos, smoothing);
    }
}