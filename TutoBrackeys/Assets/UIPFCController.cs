using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;


public class UIPFCController : NetworkBehaviour {

    private GameMaster gameMaster;

    public List<Button> buttonsList = new List<Button>();


    public List<Text> playerName = new List<Text>();
    public List<Text> playerScore = new List<Text>();





    void Start ()
    {
        gameMaster = GameMaster.Instance;
    }


    public void SetUINames()
    {
        Debug.Log(" playerName[0].text = " + playerName[0].text);
        Debug.Log(" gameMaster.m_LocalPlayer.playerID = " + gameMaster.m_LocalPlayer.playerID);




        playerName[0].text = "Player : " + gameMaster.m_LocalPlayer.playerID.ToString();
        playerName[1].text = "Player : " + gameMaster.m_OtherPlayer.playerID.ToString();

    }


    public void OnClickButton(int choice)// 1 = rock, 2 = paper, 3 = scissors
    {

        gameMaster.m_LocalPlayer.PFCchoice = choice;

        UpdateButtons();

        for (int i = 0; i < gameMaster.m_PlayersConnected.Count; i++)
        {
            if (gameMaster.m_PlayersConnected[i].PFCchoice == 0)
                return;
            else
                CompareChoices();
        }
    }


    [Server]
    private void CompareChoices()
    {

    }



    private void UpdateButtons()
    {
        for (int i = 0; i < buttonsList.Count; i++)
        {
            if (buttonsList[i].interactable)
                buttonsList[i].interactable = false;
            else
                buttonsList[i].interactable = true;
        }
    }
}
