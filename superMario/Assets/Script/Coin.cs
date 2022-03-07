using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    public float force;
    public float disappearTime;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Rigidbody2D>().AddForce(Vector2.up * force, ForceMode2D.Impulse);
        Invoke(nameof(Disappear), disappearTime);
    }

    void Disappear()
    {
        Destroy(gameObject);
    }
}
