using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Random = UnityEngine.Random;

public class button : NetworkBehaviour {

	// Use this for initialization
	void Start () {
        PlayerStuff.newChar = Random.Range(0, 32);
    }
	
	// Update is called once per frame
	void Update () {
        int i = -1;
        if (Int32.TryParse(this.name.Substring(3), out i) && i != -1)
        {
            if (i == PlayerStuff.newChar)
            {
                this.GetComponent<SpriteRenderer>().color = new Color32((byte)Random.Range(100, 255), (byte)Random.Range(100, 255), (byte)Random.Range(100, 255), (byte)255);

            }
            else
                this.GetComponent<SpriteRenderer>().color = new Color32((byte)255, (byte)255, (byte)255, (byte)255);

            if (Input.GetMouseButtonDown(0))
            {
                Vector3 mousePos = Vector3.zero;
                try
                {
                    mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                }
                catch (Exception e)
                {
                    foreach (GameObject f in GameObject.FindGameObjectsWithTag("player"))
                    {
                        if (f.GetComponent<PlayerStuff>().localPlayer)
                        {
                            mousePos = new Vector3(f.GetComponent<PlayerStuff>().mouseX, f.GetComponent<PlayerStuff>().mouseY);
                            Debug.Log(mousePos);
                            break;
                        }
                    }

                }

                Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);

                RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero);
                if (hit.collider != null)
                {
                    int j = -1;
                    if (Int32.TryParse(hit.collider.gameObject.name.Substring(3), out j))
                    {
                        //Debug.Log("Click" + hit.collider.gameObject.name.Substring(3));
                        Debug.Log("newChar = " + Int32.Parse(hit.collider.gameObject.name.Substring(3)));
                        PlayerStuff.newChar = Int32.Parse(hit.collider.gameObject.name.Substring(3));
                    } else
                    {
                        foreach (GameObject f in GameObject.FindGameObjectsWithTag("player"))
                        {
                            if (f.GetComponent<PlayerStuff>().localPlayer)
                            {
                                f.GetComponent<PlayerStuff>().respawn();
                                break;
                            }
                        }
                    }
                }
            }
        }
	}
}
