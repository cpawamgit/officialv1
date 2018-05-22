using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using HeroNetworkPlayer = NetworkPlayer;
using HeroNetworkManager = NetworkManager;


public class UIPFCController : MonoBehaviour {

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
    public List<GameObject> animImageP1 = new List<GameObject>();
    public List<GameObject> animImageP2 = new List<GameObject>();
    public GameObject buttons;
    public GameObject winAnim;
    public GameObject looseAnim;



    private HeroNetworkPlayer localPlayer;
    public HeroNetworkPlayer m_localPlayer
    { get { return localPlayer; } }

    private HeroNetworkPlayer otherPlayer;
    public HeroNetworkPlayer m_otherPlayer
    { get { return otherPlayer; } }

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


    public void AddPlayer(HeroNetworkPlayer player)
    {
        Debug.Log("AddPlayer");


        if (player.hasAuthority)
        {
            localPlayer = player;
        }
        else
            otherPlayer = player;

    }







    public void OnClickButton(int choice)// 1 = rock, 2 = paper, 3 = scissors
    {
        localPlayer.GetPFCChoice(choice);
    }


    



    public void UpdateButtons()
    {
        for (int i = 0; i < buttonsList.Count; i++)
        {
            if (buttonsList[i].interactable)
                buttonsList[i].interactable = false;
            else
                buttonsList[i].interactable = true;
        }
    }


    public void PFCWinAnim(int p1Choice, int p2Choice, int winner)
    {
        Animator anim = GetComponent<Animator>();

        Debug.Log("PFCWinAnim");

        buttons.SetActive(false);

        Debug.Log("p1Choice -1 = " + (p1Choice - 1));
        Debug.Log("p2Choice -1 = " + (p2Choice - 1));
        Debug.Log("animImageP1[p1Choice -1] = " + (animImageP1[p1Choice - 1]));
        Debug.Log("animImageP2[p2Choice -1] = " + (animImageP2[p2Choice - 1]));

        animImageP1[p1Choice -1].SetActive(true);
        animImageP2[p2Choice -1].SetActive(true);

        anim.Play("PFCAnim");

        if (winner == -1)
        {
            Debug.Log("Draw anim");
        }
        else if (winner == localPlayer.playerID)
        {
            winAnim.SetActive(true);
            anim.SetTrigger("win");
        }
        else 
        {
            looseAnim.SetActive(true);
            anim.SetTrigger("loose");
        }

    }

}
