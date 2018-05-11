using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerController : NetworkBehaviour
{

    //public Text textP1;
    //public Text textP2;

    //public Text ScoreP1;
    //public Text ScoreP2;

    //public Canvas P1Canvas;
    //public Canvas P2Canvas;

    //public Text Rock;
    //public Text Leaf;
    //public Text Scissors;

    //public Image image;

    //[SyncVar(hook = "UpdateRockText")]
    //public string rockText;

    //[HideInInspector]
    //[SyncVar]
    //public int scoreP1 = 0;
    //[HideInInspector]
    //[SyncVar]
    //public int scoreP2 = 0;


    //private int playerNumber;

    //[HideInInspector]
    //[SyncVar(hook = "RaiseScoreP1")]
    //public string p1Select = null;
    //[HideInInspector]
    //[SyncVar(hook = "RaiseScoreP2")]
    //public string p2Select = null;

    //[HideInInspector]
    //[SyncVar(hook = "RaiseScoreP1")]
    //public string p1Score = null;
    //[HideInInspector]
    //[SyncVar(hook = "RaiseScoreP2")]
    //public string p2Score = null;

    //void Start()
    //{
    //    if (SceneManager.GetActiveScene().name == "scene1")
    //    {
    //        //Pop l UI
    //    }

    //    GameMaster.AddNewPlayer(this);

    //    GameMaster.playerNumber++;
    //    playerNumber = GameMaster.playerNumber;

    //    Debug.Log("PLAYER NUMBER : " + playerNumber);
    //    if (playerNumber == 1)
    //    {
    //        P1Canvas.gameObject.SetActive(true);
    //        Debug.Log("P1texte");
    //    }
    //    if (playerNumber == 2)
    //    {
    //        P2Canvas.gameObject.SetActive(true);
    //        Debug.Log("P2texte");
    //    }

    //    //if (hasAuthority)
    //    //{
    //    //    image.gameObject.SetActive(true);
    //    //}
    //    //if (!hasAuthority)
    //    //{
    //    //    image.gameObject.SetActive(false);
    //    //}

    //}

    //void Update()
    //{
    //    if (!isLocalPlayer)
    //    {
    //        return;
    //    }
    //    if (p1Select != null && p2Select != null)
    //    {
    //        CmdCheckWinner();
    //    }

    //}

    //[Command]
    //void CmdCheckWinner()
    //{
    //    /*
    //    if (p1Select== p2Select)
    //    {
    //        p1Score = "Draw on this round";
    //        p2Score = "Draw on this round";
    //    }
    //    */
    //}

    //[Command]
    //void CmdP1WinRound()
    //{
    //    scoreP1++;
    //    p1Score = "PLAYER 1 SCORE : " + scoreP1;
    //    Debug.Log("p1 push A");
    //}

    //[Command]
    //void CmdP2WinRound()
    //{
    //    scoreP2++;
    //    p2Score = "PLAYER 2 SCORE : " + scoreP2;
    //    Debug.Log("p2 push A");
    //}


    //void OnClick(string str)
    //{
    //    Debug.Log("playerNumber = " + playerNumber);
    //    if (playerNumber == 1)
    //    {
    //        p1Select = "Player " + playerNumber + " : " + str;
    //        rockText = "SELECTED by Player " + playerNumber;
    //        Debug.Log("RockText = " + rockText, this);
    //    }
    //    if (playerNumber == 2)
    //    {
    //        p2Select = "Player " + playerNumber + " : " + str;
    //        rockText = "SELECTED by Player " + playerNumber;
    //    }
    //}

    //void UpdateRockText(string rockText)
    //{
    //    Rock.text = rockText;
    //}

  

    //void RaiseScoreP1(string p1Score)
    //{
    //    textP1.text = p1Score;
    //}

    //void RaiseScoreP2(string p2Score)
    //{
    //    textP2.text = p2Score;
    //}
}
