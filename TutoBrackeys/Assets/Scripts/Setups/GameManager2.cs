using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class GameManager2 : NetworkBehaviour {

    #region Singleton
    public static GameManager2 Instance;

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

    private void OnDisable()
    {
        Instance = null;
    }

    #endregion

    public GameObject gameOverUI;
    public GameObject completeLevelUI;

    public static bool GameIsOver;

    [SyncVar(hook = "InitPool")]
    public int playerInGameScene;

    private void Start()
    {
        GameIsOver = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown("e"))
            GameOver();
    }

    public void GameOver()   
    {
        if (GameIsOver)
            return;

        gameOverUI.SetActive(true);
        GameIsOver = true;
    }

    public void WinLevel()
    {
        GameIsOver = true;
        completeLevelUI.SetActive(true);
    }


    private void InitPool(int value)
    {
        if (value >= 2)
        {
            foreach (NetworkPlayer player in NetworkManager.Instance.connectedPlayers)
            {
                if (player.hasAuthority)
                {
                    player.CmdAllPlayerInGameScene();
                }
            }
        }
       

        StartCoroutine(WaitForAMoment());

        // != all poolSize are now setup

        Debug.Log("all poolSize are now setup");

        foreach (KeyValuePair<string, NetworkedPool> pool in PoolManager.Instance.poolDictionnary)
        {
            if (pool.Value.gameObject.activeSelf)
                pool.Value.Init();
        }
        
    }


    IEnumerator WaitForAMoment()
    {
        yield return new WaitForSeconds(5f);
    }
}
