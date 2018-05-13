using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class playerPFC : NetworkBehaviour {

    private string playerName = "empty";
    public Text UiPlayerName;

	// Use this for initialization
	void Start () {
        playerName = "player number :" + Random.Range(1, 1000);
        UiPlayerName.text = playerName;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
