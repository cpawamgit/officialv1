using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class UpdateScore : NetworkBehaviour {

    public Text P1;
    public Text P2;

    private int scoreP1 = 0;
    private int scoreP2 = 0;

    public void RaiseScore(int i)
    {
        if (i == 1)
        {
            scoreP1++;
            P1.text = scoreP1.ToString();
        }
        if(i == 2)
        {
            scoreP2++;
            P2.text = scoreP2.ToString();
        }
    }
}
