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

    private List<PlayerConnection> connectedPlayers;
    public List<PlayerConnection> m_PlayersConnected
    { get { return connectedPlayers; } }

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


    public void SetPlayersConnectedList(List<PlayerConnection> playersConnectedList)
    {
        Debug.Log("SetPlayersConnectedList");

        connectedPlayers = playersConnectedList;

        for (int i = 0; i < connectedPlayers.Count; i++)
        {
            if (connectedPlayers[i].hasAuthority)
            {
                localPlayer = connectedPlayers[i];
                Debug.Log("localPlayer = " + localPlayer);
            }
            else
            {
                otherPlayer = connectedPlayers[i];
                Debug.Log("otherPlayer = " + otherPlayer);
            }
        }

        UIPFCController.SetUINames();
    }
}
