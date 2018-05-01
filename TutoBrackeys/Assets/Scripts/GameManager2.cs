using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager2 : MonoBehaviour {

    public GameObject gameOverUI;
    public GameObject completeLevelUI;

    public static bool GameIsOver;


    private void Start()
    {
        GameIsOver = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown("e"))
            GameOver();
    }

    public void GameOver()   
    {
        if (GameIsOver)
            return;

        gameOverUI.SetActive(true);
        GameIsOver = true;
    }

    public void WinLevel()
    {
        GameIsOver = true;
        completeLevelUI.SetActive(true);
    }


}
