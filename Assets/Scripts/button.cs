using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Random = UnityEngine.Random;

public class button : NetworkBehaviour {

    byte r, g, b;
    Color32 c = new Color32(0, 0, 0, 255);
    int d = 0;

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
                rainbow((byte)(d % 255));
                d+=4;
                this.GetComponent<SpriteRenderer>().color = c;

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

    void rainbow(int p)
    {
        if (p < 85)
        {
            c.r = (byte)(p * 3);
            c.g = (byte)(255 - p * 3);
            c.b = 0;
        }
        else if (p < 170)
        {
            p -= 85;
            c.r = (byte)(255 - p * 3);
            c.g = 0;
            c.b = (byte)(p * 3);
        }
        else
        {
            p -= 170;
            c.r = 0;
            c.g = (byte)(p * 3);
            c.b = (byte)(255 - p * 3);
        }
        c.r = (byte)(85 + c.r * 2 / 3);
        c.g = (byte)(85 + c.g * 2 / 3);
        c.b = (byte)(85 + c.b * 2 / 3);
    }
    
}
