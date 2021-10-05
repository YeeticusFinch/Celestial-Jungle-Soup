using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class wall : NetworkBehaviour {

    [SyncVar(hook = "OnRedChange")] public byte r = 0;
    [SyncVar(hook = "OnGreenChange")] public byte g = 0;
    [SyncVar(hook = "OnBlueChange")] public byte b = 0;

    void OnRedChange(byte x)
    {
        r = x;
        this.GetComponent<SpriteRenderer>().color = new Color32(r, g, b, (byte)255);
    }

    void OnGreenChange(byte x)
    {
        g = x;
        this.GetComponent<SpriteRenderer>().color = new Color32(r, g, b, (byte)255);
    }

    void OnBlueChange(byte x)
    {
        b = x;
        this.GetComponent<SpriteRenderer>().color = new Color32(r, g, b, (byte)255);
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        //transform.SetAsFirstSibling();
	}

    // Fixed update is called 50 times per second
    void FixedUpdate()
    {
        if (this.gameObject.GetComponent<Rigidbody2D>().bodyType == RigidbodyType2D.Dynamic)
        {
            StartCoroutine(Die());
        }
    }

    IEnumerator Die()
    {
        yield return new WaitForSecondsRealtime(1.5f);
        GameObject.Destroy(this.gameObject);
    }

}
