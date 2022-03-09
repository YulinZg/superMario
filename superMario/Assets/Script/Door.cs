using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public GameManagement game;
    public AudioSource bgm;
    public AudioSource door;
    public AudioClip winEffect;
    // Start is called before the first frame update
    void Start()
    {
        game = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManagement>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag.Equals("Player"))
        {
            door.clip = winEffect;
            door.Play();
            bgm.Stop();
            collision.gameObject.SetActive(false);
            //collision.gameObject.GetComponent<SpriteRenderer>().sortingOrder = 0;
            game.showWin();
        }
    }

    private void win()
    {

    }
}
