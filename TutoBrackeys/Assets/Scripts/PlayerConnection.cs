using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerConnection : NetworkBehaviour
{
    public SceneFader sceneFader;

    [SyncVar]
    [HideInInspector]
    public int PFCchoice = 0;
    [SyncVar]
    [HideInInspector]
    public int Score;


    [SyncVar]
    private int m_PlayerID;

    public int playerID
    { get {return m_PlayerID;} }




 

    private void Start()
    {
        DontDestroyOnLoad(this.gameObject);
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
        base.OnStartClient();

        Debug.Log("Client Network Player start");

        NetworkManager.Instance.RegisterNetworkPlayer(this);
    }

    [ClientRpc]
    public void RpcFadeIn()
    {
        //sceneFader.FadeTo( );
    }


    [Client]
    public void OnEnterGameScene()
    {
        if (!hasAuthority)
            return;
        
        // fait des bails quand on arrive dans une new scene
    }
}
