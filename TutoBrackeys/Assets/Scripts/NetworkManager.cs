using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine;


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


    public string sceneToLoad;
    [SerializeField]
    public PlayerConnection m_NetworkPlayerPrefab;

    public int sfsdqsddsdsdsddsqdsqsqqqsqsdsqdsqdqsfsd;


    public List<PlayerConnection> playersConnected
    { get; private set; }


    void Start()
    {

        Debug.Log("Start dans NetworkManager");

        playersConnected = new List<PlayerConnection>();
    }

    public void RegisterNetworkPlayer(PlayerConnection playerConnection)
    {
        playersConnected.Add(playerConnection);

        if (NetworkServer.active)
        {
            UpdatePlayersIDs();
        }
    }



    private void UpdatePlayersIDs()
    {
        for (int i = 0; i < playersConnected.Count; i++)
        {
            playersConnected[i].SetPlayerID(i);
        }

        if (playersConnected.Count == 2)
        {
            ServerChangeScene(sceneToLoad);
        }
    }

    public override void OnServerSceneChanged(string sceneName)
    {
        base.OnServerSceneChanged(sceneName);

        if (sceneToLoad == sceneName)
        {
            //GameMaster.Instance.RpcSetPlayersConnectedList(playersConnected);
        }
    }

    public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId)
    {
        // Intentionally not calling base here - we want to control the spawning of prefabs
        Debug.Log("OnServerAddPlayer");

        if (true)
        {
            PlayerConnection newPlayer = Instantiate<PlayerConnection>(m_NetworkPlayerPrefab);
            DontDestroyOnLoad(newPlayer);
            NetworkServer.AddPlayerForConnection(conn, newPlayer.gameObject, playerControllerId); //associate prefab with his connection

        }
    }
}

