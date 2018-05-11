using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class GameMaster : NetworkBehaviour
{

    #region Singleton
    public static GameMaster Instance;
    private void Awake()
    {
        if (Instance != null)
            Destroy(this);

        Instance = this;
    }
    #endregion

    public UIPFCController UIPFCController;

    private List<PlayerConnection> playersConnected;
    public List<PlayerConnection> m_PlayersConnected
    { get { return playersConnected; } }

    private PlayerConnection localPlayer;
    public PlayerConnection m_LocalPlayer
    { get { return localPlayer; } }


    private PlayerConnection otherPlayer;
    public PlayerConnection m_OtherPlayer
    { get { return otherPlayer; } }



    void Start()
    {
        DontDestroyOnLoad(Instance.gameObject);

       
    }

    [ClientRpc]
    public void RpcSetPlayersConnectedList(List<PlayerConnection> playersConnectedList)
    {
        //Debug.Log("SetPlayersConnectedList");

        //playersConnected = playersConnectedList;

        //for (int i = 0; i < playersConnected.Count; i++)
        //{
        //    if (playersConnected[i].hasAuthority)
        //    {
        //        localPlayer = playersConnected[i];
        //    }
        //    else
        //    {
        //        otherPlayer = playersConnected[i];
        //    }
        //}

        //UIPFCController.SetUINames();
    }
}
