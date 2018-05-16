using UnityEngine;
using UnityEngine.UI;
using HeroNetworkManager = NetworkManager;
using HeroNetworkPlayer = NetworkPlayer;


public class PFCPlayer : MonoBehaviour
{
    [HideInInspector]
    public HeroNetworkPlayer m_NetPlayer;
    public Text nameText;
    public Text scoreText;

    public void Init(HeroNetworkPlayer netPlayer)
    {
        Debug.Log("Init PFCPlayer ma gueule");
        this.m_NetPlayer = netPlayer;

        nameText.text = "Player " + (netPlayer.playerID + 1).ToString();
        scoreText.text = "0";


        //if (netPlayer != null)
        //{
        //    netPlayer.syncVarsChanged += OnNetworkPlayerSyncVarChanged;
        //}

        UIPFCController.Instance.AddPlayer(this);
    }


    private void OnNetworkPlayerSyncVarChanged(HeroNetworkPlayer player)
    {

    }

}