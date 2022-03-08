using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrickFragment : MonoBehaviour
{
    private  float upForce;
    private  float rotateSpeed;

    // Start is called before the first frame update
    void Start()
    {
        upForce = Random.Range(15.0f, 20.0f);
        rotateSpeed = Random.Range(-360.0f, 360.0f);
        GetComponent<Rigidbody2D>().AddForce(new Vector2(Random.Range(-0.3f, 0.3f), 1.0f) * upForce, ForceMode2D.Impulse);
        Invoke(nameof(Disappear), 1.0f);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(rotateSpeed * Time.deltaTime * Vector3.forward);
    }

    void Disappear()
    {
        Destroy(gameObject);
    }
}
