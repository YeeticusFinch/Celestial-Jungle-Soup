using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class start : NetworkBehaviour {

    
    // Use this for initialization
    void Start () {
        
            
        GameObject charSelect = Instantiate(Resources.Load("CharSelect"), Vector3.zero, transform.rotation) as GameObject;
        charSelect.transform.GetChild(0).gameObject.SetActive(false);
        
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
