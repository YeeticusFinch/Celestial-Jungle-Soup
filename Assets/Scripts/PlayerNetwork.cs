using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerNetwork : NetworkBehaviour {

    public GameObject Character;

    //this method only runs on the player GameObject that belongs to that particular client
	public override void OnStartLocalPlayer()
    {
        Character.SetActive(true);

        //Character.GetComponent<Camera>()
    }
}
