using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class NetworkHUD : MonoBehaviour
{
    private NetworkManager manager;

	void Start ()
    {
        manager = GetComponent<NetworkManager>();
    }
	
    public void HostButton()
    {
        manager.StartHost();
    }

    public void JoinButton()
    {
        manager.StartClient();
    }

  
}
