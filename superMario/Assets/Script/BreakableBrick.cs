using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableBrick : MonoBehaviour
{
    public GameObject fragment;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
            if (collision.gameObject.GetComponent<MarioController>().isBig)
            {
                Instantiate(fragment, transform.position + new Vector3(0.25f, 0, 0), Quaternion.Euler(0, 0, 30.0f));
                Instantiate(fragment, transform.position + new Vector3(-0.25f, 0, 0), Quaternion.Euler(0, 0, -30.0f));
                Instantiate(fragment, transform.position + new Vector3(0, 0.25f, 0), Quaternion.Euler(0, 0, 60.0f));
                Instantiate(fragment, transform.position + new Vector3(0, -0.25f, 0), Quaternion.Euler(0, 0, -60.0f));
                Destroy(gameObject);
            }
    }
}
