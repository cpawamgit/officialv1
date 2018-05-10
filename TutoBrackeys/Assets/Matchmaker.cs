using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Matchmaker : NetworkManager
{
    public void StartHostButton()
    {
       StartHost();  
    }


    public void StartClientButton()
    {
        StartClient();   
    }

}
