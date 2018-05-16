using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;


public class UIPFCController : NetworkBehaviour {

    #region Singleton


    /// <summary>
    /// Gets the UIPFCController instance if it exists
    /// </summary>
    public static UIPFCController Instance
    {
        get;
        protected set;
    }

    public static bool InstanceExists
    {
        get { return Instance != null; }
    }

    #endregion


    private GameMaster gameMaster;

    public List<Button> buttonsList = new List<Button>();

    
    public List<Text> playerName = new List<Text>();
    public List<Text> playerScore = new List<Text>();

    [SerializeField]
    protected RectTransform panel;

    private NetworkPlayer localPlayer;


    /// <summary>
    /// Initialize our singleton
    /// </summary>
    protected void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    /// <summary>
    /// Clear the singleton
    /// </summary>
    protected void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }


    public void AddPlayer(PFCPlayer player)
    {
        Debug.Log("AddPlayer to panel");

        localPlayer = player.m_NetPlayer;

        player.transform.SetParent(panel, false);
    }


    //public void SetUI()
    //{
    //    gameMaster = GameMaster.Instance;

    //    Debug.Log("SetUI");


    //    playerName[0].text = "Player " + gameMaster.m_LocalPlayer.playerID.ToString();
    //    playerName[1].text = "Player " + gameMaster.m_OtherPlayer.playerID.ToString();

    //    playerScore[0].text = "0";
    //    playerScore[1].text = "0";
    //}


    public void OnClickButton(int choice)// 1 = rock, 2 = paper, 3 = scissors
    {
        gameMaster = GameMaster.Instance;

        localPlayer.PFCchoice = choice;

        CmdAreAllChoicesDone();

        UpdateButtons();


    }

    //[ClientRpc]
    //private int RpcGetPFCChoice()
    //{
    //    return localPlayer.PFCchoice;
    //}


    [Command]
    private void CmdAreAllChoicesDone()
    {

        //List<int> choices = new List<int>();

        //for (int i = 0; i < 2; i++)
        //{
        //    choices[i] = RpcGetPFCChoice();
        //}


        //for (int i = 0; i < choices.Count; i++)
        //{
        //    if (choices[i] == 0)
        //        return;
        //    else
        //        CompareChoices();
        //}
    }

    private void CompareChoices()
    {
        Debug.Log("CompareChoices");
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
