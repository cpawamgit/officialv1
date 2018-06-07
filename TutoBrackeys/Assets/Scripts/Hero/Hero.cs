using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Hero : Unite
{
    //public int AAdamage; //auto attack damage

    private void Start()
    {
        GameManager2.Instance.playerInGameScene++;
    }

}
