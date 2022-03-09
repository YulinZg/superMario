using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManagement : MonoBehaviour
{
    [Header("UI")]
    public Text score;
    public Text coins;
    public GameObject panel;
    public GameObject winPanel;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
            SceneManager.LoadScene(0);
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            //Debug.Log(1111);
            Application.Quit();
        }

    }
    
    public void updateCoins(int num)
    {
        int temp = int.Parse(coins.text);
        temp += num;
        coins.text = temp.ToString();
    }

    public void updateScore(int num)
    {
        int temp = int.Parse(score.text);
        temp += num;
        score.text = temp.ToString();
    }

    public void showGameOver()
    {
        panel.SetActive(true);
    }

    public void showWin()
    {
        winPanel.SetActive(true);
    }
}
