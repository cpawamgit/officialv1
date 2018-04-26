using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public static GameManager2 GameManager;
    public GameManager2 gameManager;

    public static int Money;
    public int startMoney = 400;
  
    public static float Lives;
    public int startLives = 20;

    public static int Rounds;


    void Start()
    {
        GameManager = gameManager;
        Money = startMoney;
        Lives = startLives;
        Rounds = 0;
    }

    public static void DecreaseLife()
    {
        Lives--;
        Lives = Mathf.Clamp(Lives, 0f, Mathf.Infinity);

        if (Lives <= 0)
        {
            GameManager.GameOver();
        }
    }

   
}
