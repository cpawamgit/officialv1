using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System;
using HeroNetworkManager = NetworkManager;
using HeroNetworkPlayer = NetworkPlayer;


public class NetworkPlayer : NetworkBehaviour
{

    public event Action<NetworkPlayer> syncVarsChanged;

    //Server only event
    public event Action<NetworkPlayer> activesPlayersEvent;

    //Server only event
    public event Action<NetworkPlayer> choiceDone;


    [SyncVar(hook = "InitUI")]
    private int PFCScore = 0;
    public int GetPFSScore
    { get { return PFCScore; } }

    public int PFCChoice = 0;

    [SerializeField]
    protected GameObject m_PFCPlayerUI;
    [SerializeField]
    protected GameObject m_hero;

    public SceneFader sceneFader;

    private int winner;

    public GameObject scenePooler;


  
    [SyncVar]
    private int m_PlayerID;

    private HeroNetworkManager m_NetManager;


    /// <summary>
    /// Gets this player's id
    /// </summary>
    public int playerID
    { get { return m_PlayerID; } }


    ///// <summary>
    ///// Get the PFCPlayerObject associate with this player
    ///// </summary>
    //public PFCPlayer PFCPlayer
    //{ get; private set; }

    public NetworkPlayer s_LocalPlayer
    { get; private set; }




    private void Update()
    {
        if (Input.GetKeyDown("a"))
        {
            if (hasAuthority)
            {
                CmdGameState();

            }
        }
    }



    [Server]
    public void SetPlayerID(int playerID)
    {
        this.m_PlayerID = playerID;
    }

    /// <summary>
    /// Register us with the NetworkManager
    /// </summary>
    [Client]
    public override void OnStartClient()
    {
        DontDestroyOnLoad(this.gameObject);

        if (m_NetManager == null)
            m_NetManager = HeroNetworkManager.Instance;

        base.OnStartClient();

        Debug.Log("Client Network Player start");

        m_NetManager.RegisterNetworkPlayer(this);
    }


    /// <summary>
    /// Set initial values
    /// </summary>
    [Client]
    public override void OnStartLocalPlayer()       // called automatically on start and only on the local player
    {
        base.OnStartLocalPlayer();
        Debug.Log("OnStartLocalPlayer");
        s_LocalPlayer = this;


        CmdActivesPlayers();


        //recupère le nom, la liste des unités sélectionnée... dans un fichier de sauvegarde (playerDataManager)
    }

    [Command]
    private void CmdActivesPlayers()
    {
        Debug.Log("CmdActivesPlayers");

        //connected = true;

        if (m_NetManager.playerCount >= 2)
        {
            m_NetManager.ProgressToPFCScene();
        }

    }

    [Command]
    public void CmdGameState()
    {
        m_NetManager.ProgressToGame();
    }

    [ClientRpc]
    public void RpcFadeIn()
    {
        //sceneFader.FadeTo( );
    }


    /// <summary>
    /// Called when we enter MapRomainScene
    /// </summary>
    public void OnEnterMapRomainScene()
    {
        if (!isServer)
            return;
        if (!hasAuthority)
            return;
        Debug.Log("OnEnterMapRomainScene");
        if (isServer)
        {
            GameManager2.Instance.gameObject.AddComponent<MyObjectPooler>();
        }
        Instantiate(scenePooler);
        SendGoToPooler();
        if (isServer)
            MyObjectPooler.Instance.Init();
    }

    public void SendGoToPooler()
    {
        Debug.Log("Entering SendGoToPooler");
        Debug.Log("selectedAgentPool" + AgentSelector.Instance.selectedAgentPool.Count);
        foreach (Pool poolObject in AgentSelector.Instance.selectedAgentPool)
        {
            Debug.Log("Foreach SendGoToPooler");
            CmdSendPool(poolObject);
        }
    }

    [Command]
    public void CmdSendPool(Pool poolbject)
    {
        Debug.Log("Entering CmdSendPool");
        if (poolbject == null)
        {
            Debug.Log("poolObject == NULL !!!");
        }
        MyObjectPooler.Instance.FillPoolList(poolbject);
    }

    /// <summary>
    /// Called when we enter PFCScene
    /// </summary>
    public void OnEnterPFCScene()
    {
        Debug.Log("OnEnterPFCScene");

        UIPFCController.Instance.AddPlayer(this);

        InitUI(0);
    }



    public void InitUI(int newScore)
    {
        Debug.Log("PFCScore's hook");

        PFCScore = newScore;


        for (int i = 0; i < m_NetManager.connectedPlayers.Count; i++)
        {
            UIPFCController.Instance.playerName[i].text = "Player " + m_NetManager.connectedPlayers[i].m_PlayerID.ToString();
            UIPFCController.Instance.playerScore[i].text = m_NetManager.connectedPlayers[i].PFCScore.ToString();
        }
    }


    public override void OnNetworkDestroy()
    {
        Debug.Log("NetworkPlayer call OnNetworkDestroy");

        base.OnNetworkDestroy();

        if (m_NetManager != null)
        {
            m_NetManager.DeregisterNetworkPlayer(this);
        }
    }




    public void GetPFCChoice(int choice)
    {
        CmdSetPFCChoice(choice);

        UIPFCController.Instance.UpdateButtons();

        CmdAreAllChoicesDone();
    }



    [Command]
    private void CmdSetPFCChoice(int choice)
    {
        Debug.Log("CmdSetPFCChoice", this);
        PFCChoice = choice;
    }



    [Command]
    private void CmdAreAllChoicesDone()
    {
        foreach (HeroNetworkPlayer player in HeroNetworkManager.Instance.connectedPlayers)
        {
            if (player.PFCChoice == 0)
            {
                return;
            }
        }
        CmdCompareChoices();
    }





    [Command]
    private void CmdCompareChoices()
    {
        Debug.Log("CompareChoices");
        int localChoice = UIPFCController.Instance.m_localPlayer.PFCChoice;
        int otherChoice = UIPFCController.Instance.m_otherPlayer.PFCChoice;

        winner = -1;


        if (localChoice == otherChoice)
        {
            Debug.Log("DRAW !!!!");
        }
        else
        {
            switch (localChoice)
            {
                case 1:
                    if (otherChoice == 2)
                    {
                        UIPFCController.Instance.m_otherPlayer.PFCScore++;
                        winner = 1;
                    }
                    if (otherChoice == 3)
                    {
                        UIPFCController.Instance.m_localPlayer.PFCScore++;
                        winner = 0;
                    }
                    break;

                case 2:
                    if (otherChoice == 1)
                    {
                        UIPFCController.Instance.m_localPlayer.PFCScore++;
                        winner = 0;
                    }
                    if (otherChoice == 3)
                    {
                        UIPFCController.Instance.m_otherPlayer.PFCScore++;
                        winner = 1;
                    }
                    break;

                case 3:
                    if (otherChoice == 1)
                    {
                        UIPFCController.Instance.m_otherPlayer.PFCScore++;
                        winner = 1;
                    }
                    if (otherChoice == 2)
                    {
                        UIPFCController.Instance.m_localPlayer.PFCScore++;
                        winner = 0;
                    }
                    break;
            }
        }
        
        RpcLaunchPFCAnim(localChoice, otherChoice, winner);

        localChoice = 0;
        otherChoice = 0;
    }

    [ClientRpc]
    private void RpcLaunchPFCAnim(int p1Choice, int p2Choice, int winner)
    {
        Debug.Log("RpcLaunchPFCAnim");
        UIPFCController.Instance.PFCWinAnim(p1Choice, p2Choice, winner);
    }

}
