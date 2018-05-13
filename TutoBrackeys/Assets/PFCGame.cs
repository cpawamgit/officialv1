using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PFCGame : NetworkBehaviour {

    public GameObject playerUnitPrefab;

    // Use this for initialization
    void Start () {
        CmdSpawnMyUnit();

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    [Command]
    void CmdSpawnMyUnit()
    {
        GameObject go = Instantiate(playerUnitPrefab);

        //possible
        //go.GetComponent<NetworkIdentity>().AssignClientAuthority(connectionToClient);

        NetworkServer.SpawnWithClientAuthority(go, connectionToClient);
    }
}
