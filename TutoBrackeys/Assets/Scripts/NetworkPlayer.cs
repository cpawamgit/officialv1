using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System;
using HeroNetworkManager = NetworkManager;


public class NetworkPlayer : NetworkBehaviour
{

    public event Action<NetworkPlayer> syncVarsChanged;

    //Server only event
    public event Action<NetworkPlayer> choiceDone;



    [SyncVar]
    [HideInInspector]
    public int PFCchoice = 0;
    [SyncVar]
    [HideInInspector]
    public int Score;

    [SerializeField]
    protected GameObject m_PFCPlayerUI;
    [SerializeField]
    protected GameObject m_hero;

    public SceneFader sceneFader;



    // Set on the server only
    [SyncVar(hook = "OnHasInitialized")]
    private bool m_Initialized = false;
    [SyncVar]
    private int m_PlayerID;

    private HeroNetworkManager m_NetManager;


    /// <summary>
    /// Gets this player's id
    /// </summary>
    public int playerID
    { get {return m_PlayerID;} }


    /// <summary>
    /// Get the PFCPlayerObject associate with this player
    /// </summary>
    public PFCPlayer PFCPlayerObject
    { get; private set; }

    public NetworkPlayer s_LocalPlayer
    { get; private set; }






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

        //recupère le nom, la liste des unités sélectionnée... dans un fichier de sauvegarde (playerDataManager)
    }




    [ClientRpc]
    public void RpcFadeIn()
    {
        //sceneFader.FadeTo( );
    }


    /// <summary>
    /// Called when we enter MapRomainScene
    /// </summary>
    [Client]
    public void OnEnterMapRomainScene()  
    {
        Debug.Log("OnEnterMapRomainScene");

        if (!hasAuthority)
            return;

    }


    /// <summary>
    /// Called when we enter PFCScene
    /// </summary>
    [Client]
    public void OnEnterPFCScene()
    {
        Debug.Log("OnEnterPFCScene");

        CreatePFCPlayerUI();
    }

    /// <summary>
    /// Create our lobby object
    /// </summary>
    private void CreatePFCPlayerUI()
    {
        PFCPlayerObject = Instantiate(m_PFCPlayerUI).GetComponent<PFCPlayer>();
        PFCPlayerObject.Init(this);
    }

    private void OnHasInitialized(bool value)
    {
        if (!m_Initialized && value)
        {
            m_Initialized = value;
            CreatePFCPlayerUI();

           
            
        }
    }
}
