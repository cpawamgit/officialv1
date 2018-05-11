using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.Networking.Match;
using System;


public enum ActualSceneState
{
    None,
    Game,
    Menu,
    PFC,
}

public class NetworkManager : UnityEngine.Networking.NetworkManager
{

    #region Singleton
    public static NetworkManager Instance;
    private void Awake()
    {
        if (Instance != null)
            Destroy(this);

        Instance = this;
    }
    #endregion


    private ActualSceneState m_ActualSceneState;

    public string sceneToLoad;
    [SerializeField]
    public GameObject m_NetworkPlayerPrefab;

    public List<PlayerConnection> connectedPlayers
    { get; private set; }

    /// <summary>
    /// Maximum number of players in a multiplayer game
    /// </summary>
    [SerializeField]
    protected int m_MultiplayerMaxPlayers = 2;


    /// <summary>
    /// Called on clients and server when the scene changes
    /// </summary>
    public event Action<bool, string> sceneChanged;




    void Start()
    {

        Debug.Log("Start dans NetworkManager");

        connectedPlayers = new List<PlayerConnection>();
    }

    private void Update()
    {
        if (m_ActualSceneState != ActualSceneState.None)
        {
            if (m_ActualSceneState == ActualSceneState.PFC)
            {

                Debug.Log("GameMaster.Instance = " + GameMaster.Instance);
                Debug.Log("connectedPlayers = " + connectedPlayers);

                GameMaster.Instance.SetPlayersConnectedList(connectedPlayers);
            }
        }

        m_ActualSceneState = ActualSceneState.None;
    }


    public void RegisterNetworkPlayer(PlayerConnection playerConnection)
    {
        connectedPlayers.Add(playerConnection);

        if (NetworkServer.active)
        {
            UpdatePlayersIDs();
        }
    }



    private void UpdatePlayersIDs()
    {
        for (int i = 0; i < connectedPlayers.Count; i++)
        {
            connectedPlayers[i].SetPlayerID(i);
        }

        if (connectedPlayers.Count == 2)
        {
            Debug.Log("UpdatePlayersIDs call ServerChangeScene");
            ServerChangeScene(sceneToLoad);
            matchMaker.SetMatchAttributes(matchInfo.networkId, false, 0, (success, info) => Debug.Log("Match hidden")); //Unlist
        }
    }



    #region NetworkEvent

    /// <summary>
    /// Gets the NetworkPlayer object for a given connection
    /// </summary>
    public static PlayerConnection GetPlayerForConnection(NetworkConnection conn)
    {
        return conn.playerControllers[0].gameObject.GetComponent<PlayerConnection>();
    }



    public override void  ServerChangeScene(string newSceneName)
    {
        Debug.Log("ServerChangeScene");

        base.ServerChangeScene(newSceneName);
    }



    public override void OnServerSceneChanged(string sceneName)
    {
        Debug.Log("OnServerSceneChanged");

        base.OnServerSceneChanged(sceneName);

        if (sceneChanged != null)
        {
            sceneChanged(true, sceneName);
        }

        if (sceneName == "PFCscene")
        {
            m_ActualSceneState = ActualSceneState.PFC;
        }

        //if (sceneChanged != null)               // Event call chez les clients pour afficher un message
        //{
        //    sceneChanged(true, sceneName);
        //}
    }

  



    public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId)
    {
        // Intentionally not calling base here - we want to control the spawning of prefabs
        Debug.Log("OnServerAddPlayer");

       
        GameObject newPlayer = Instantiate<GameObject>(m_NetworkPlayerPrefab);
        DontDestroyOnLoad(newPlayer);
        NetworkServer.AddPlayerForConnection(conn, newPlayer.gameObject, playerControllerId); //associate prefab with his connection
        
    }






    public override void OnClientError(NetworkConnection conn, int errorCode)
    {
        Debug.Log("OnClientError");

        base.OnClientError(conn, errorCode);

        //if (clientError != null)                // Event call chez les clients pour afficher un message
        //{
        //    clientError(conn, errorCode);
        //}
    }

    public override void OnClientConnect(NetworkConnection conn)
    {
        Debug.Log("OnClientConnect");

        ClientScene.Ready(conn);
        ClientScene.AddPlayer(0);

        //if (clientConnected != null)                // Event call chez les clients pour afficher un message
        //{
        //    clientConnected(conn);
        //}
    }

    public override void OnClientDisconnect(NetworkConnection conn)
    {
        Debug.Log("OnClientDisconnect");

        base.OnClientDisconnect(conn);

        //if (clientDisconnected != null)                // Event call chez les clients pour afficher un message
        //{
        //    clientDisconnected(conn);
        //}
    }

    public override void OnServerError(NetworkConnection conn, int errorCode)
    {
        Debug.Log("OnClientDisconnect");

        base.OnClientDisconnect(conn);

        //if (serverError != null)                // Event call chez les clients pour afficher un message
        //{
        //    serverError(conn, errorCode);
        //}
    }

  

    public override void OnClientSceneChanged(NetworkConnection conn)
    {
        Debug.Log("OnClientSceneChanged");

        base.OnClientSceneChanged(conn);

        PlayerController pc = conn.playerControllers[0];   // pc = le 1er player sur cette connection 

        if (!pc.unetView.isLocalPlayer)
        {
            return;
        }


        string sceneName = SceneManager.GetActiveScene().name;

        if (sceneName == "MapRomainScene")
        {
            //state = NetworkState.InGame;

            // Tell all network players that they're in the game scene
            for (int i = 0; i < connectedPlayers.Count; ++i)
            {
                PlayerConnection np = connectedPlayers[i];
                if (np != null)
                {
                    np.OnEnterGameScene();
                }
            }
        }
        if (sceneChanged != null)
        {
            sceneChanged(false, sceneName);
        }

    }



    public override void OnServerRemovePlayer(NetworkConnection conn, PlayerController player)
    {
        Debug.Log("OnServerRemovePlayer");
        base.OnServerRemovePlayer(conn, player);

        PlayerConnection connectedPlayer = GetPlayerForConnection(conn);
        if (connectedPlayer != null)
        {
            Destroy(connectedPlayer);
            connectedPlayers.Remove(connectedPlayer);
        }
    }


    public override void OnServerReady(NetworkConnection conn)
    {
        Debug.Log("OnServerReady");
        base.OnServerReady(conn);
    }


    public override void OnServerConnect(NetworkConnection conn)
    {
        Debug.LogFormat("OnServerConnect\nID {0}\nAddress {1}\nHostID {2}", conn.connectionId, conn.address, conn.hostId);

        if (numPlayers >= m_MultiplayerMaxPlayers)
        {
            conn.Disconnect();
        }

        base.OnServerConnect(conn);


    }

    public override void OnServerDisconnect(NetworkConnection conn)
    {
        Debug.Log("OnServerDisconnect");
        base.OnServerDisconnect(conn);
    }

    public override void OnMatchCreate(bool success, string extendedInfo, MatchInfo matchInfo)
    {
        base.OnMatchCreate(success, extendedInfo, matchInfo);
        Debug.Log("OnMatchCreate");
    }

    public override void OnMatchJoined(bool success, string extendedInfo, MatchInfo matchInfo)
    {
        base.OnMatchJoined(success, extendedInfo, matchInfo);
        Debug.Log("OnMatchJoined");
    }

    public override void OnDropConnection(bool success, string extendedInfo)
    {
        base.OnDropConnection(success, extendedInfo);
        Debug.Log("OnDropConnection");

        //if (matchDropped != null)                // Event call chez les clients pour afficher un message
        //{
        //    matchDropped();
        //}
    }

    /// <summary>
    /// Server resets networkSceneName
    /// </summary>
    public override void OnStartServer()
    {
        base.OnStartServer();
        networkSceneName = string.Empty;
    }

    /// <summary>
    /// Server destroys NetworkPlayer objects
    /// </summary>
    public override void OnStopServer()
    {
        base.OnStopServer();
        Debug.Log("OnStopServer");

        for (int i = 0; i < connectedPlayers.Count; ++i)
        {
            PlayerConnection player = connectedPlayers[i];
            if (player != null)
            {
                NetworkServer.Destroy(player.gameObject);
            }
        }

        connectedPlayers.Clear();

        // Reset this
        networkSceneName = string.Empty;
    }


    /// <summary>
    /// Clients also destroy their copies of NetworkPlayer
    /// </summary>
    public override void OnStopClient()
    {
        Debug.Log("OnStopClient");
        base.OnStopClient();

        for (int i = 0; i < connectedPlayers.Count; ++i)
        {
            PlayerConnection player = connectedPlayers[i];
            if (player != null)
            {
                Destroy(player.gameObject);
            }
        }

        connectedPlayers.Clear();
    }

    /// <summary>
    /// Fire host started messages
    /// </summary>
    public override void OnStartHost()
    {
        Debug.Log("OnStartHost");
        base.OnStartHost();

        //if (m_NextHostStartedCallback != null)
        //{
        //    m_NextHostStartedCallback();
        //    m_NextHostStartedCallback = null;
        //}
        //if (hostStarted != null)
        //{
        //    hostStarted();
        //}
    }

    /// <summary>
    /// Called on the server when a player is set to ready
    /// </summary>
    //public virtual void OnPlayerSetReady(NetworkPlayer player)
    //{
    //    if (AllPlayersReady() && serverPlayersReadied != null)
    //    {
    //        serverPlayersReadied();
    //    }
    //}

    #endregion



}

