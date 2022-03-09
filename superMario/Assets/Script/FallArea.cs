using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallArea : MonoBehaviour
{
    private GameManagement game;

    // Start is called before the first frame update
    void Start()
    {
        game = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManagement>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
            if (!collision.gameObject.GetComponent<MarioController>().isDead)
            {
                collision.gameObject.GetComponent<MarioController>().isDead = true;
                game.showGameOver();
            }
        Destroy(collision.gameObject);
    }
}
