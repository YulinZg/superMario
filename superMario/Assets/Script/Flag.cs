using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flag : MonoBehaviour
{
    public GameObject mario;
    public GameObject flag;
    public bool canMove;
    public GameManagement game;
    private AudioSource TheWin;
    // Start is called before the first frame update
    void Start()
    {
        game = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManagement>();
        TheWin = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (canMove)
        {
            flag.transform.position = mario.transform.position + new Vector3(-0.3f,1,0);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag.Equals("Player"))
        {
            TheWin.Play();
            if (collision.gameObject.transform.position.y >= 5.5)
                game.updateScore(5000);
            else if (collision.gameObject.transform.position.y >= 3)
                game.updateScore(3000);
            else if (collision.gameObject.transform.position.y >= 1)
                game.updateScore(2000);
            else
                game.updateScore(1000);
            collision.gameObject.GetComponent<MarioController>().isTouchFlag = true;
            canMove = true;
            //Debug.Log(11111);
        }
    }
}
