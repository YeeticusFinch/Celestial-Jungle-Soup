using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.NetworkSystem;
using UnityEngine.UI;
using Random = UnityEngine.Random;
using System.IO;
using System;

public class map : NetworkBehaviour {

    Color32 c0 = new Color32();
    Color32 c1 = new Color32();

    //List<List<GameObject>> tiles = new List<List<GameObject>>();
    List<GameObject> tiles = new List<GameObject>();

    public int spacing;

    // Use this for initialization
    void Start () {
        

    }

	// Update is called once per frame
	void Update () {
		
	}

    void FixedUpdate()
    {
        
    }

    

    public void ReadFile()
    {
        string path = "Assets/Resources/maps/";

        var info = new DirectoryInfo(path);
        int length = info.GetFiles().Length;
        
        //Read the text from directly from the test.txt file
        StreamReader reader = new StreamReader(path + "m" + Random.Range(0, length-1) + ".txt");
        
        string m = reader.ReadToEnd();
        reader.Close();

        byte r = (byte)(Int32.Parse(m.Substring(0, m.IndexOf(':'))));
        m = m.Substring(m.IndexOf(':') + 1);
        byte g = (byte)(Int32.Parse(m.Substring(0, m.IndexOf(':'))));
        m = m.Substring(m.IndexOf(':') + 1);
        byte b = (byte)(Int32.Parse(m.Substring(0, m.IndexOf(':'))));
        m = m.Substring(m.IndexOf(':') + 1);

        Debug.Log("RGB = " + r + ", " + g + ", " + b);
        c0 = new Color32( r, g, b, (byte)255 );

        r = (byte)(Int32.Parse(m.Substring(0, m.IndexOf(':'))));
        m = m.Substring(m.IndexOf(':') + 1);
        g = (byte)(Int32.Parse(m.Substring(0, m.IndexOf(':'))));
        m = m.Substring(m.IndexOf(':') + 1);
        b = (byte)(Int32.Parse(m.Substring(0, m.IndexOf(':'))));
        m = m.Substring(m.IndexOf(':') + 2);

        Debug.Log("RGB = " + r + ", " + g + ", " + b);
        c1 = new Color32(r, g, b, (byte)255);

        int x = 0;
        int y = 0;
        float z = 0;
        //tiles.Add(new List<GameObject>());
        for (int i = 0; i < m.Length; i++)
        {
            GameObject yeet = null;
            switch (m[i])
            {
                case '0':
                    yeet = Instantiate(Resources.Load("floor"), transform.position, transform.rotation) as GameObject;
                    yeet.GetComponent<SpriteRenderer>().color = c0;
                    yeet.transform.SetAsFirstSibling();
                    break;
                case '1':
                    yeet = Instantiate(Resources.Load("wall"), transform.position, transform.rotation) as GameObject;
                    yeet.GetComponent<SpriteRenderer>().color = c1;
                    z = -0.2f;
                    break;
                case '\n':
                    y++;
                    x = 0;
                    //tiles.Add(new List<GameObject>());
                    break;
            }
            
            if (yeet != null)
            {
                yeet.transform.parent = transform;
                yeet.transform.SetParent(transform);
                yeet.transform.localPosition += new Vector3(x*spacing, -y*spacing, z);
                //yeet.GetComponent<wall>().r = c0.r;
                //yeet.GetComponent<wall>().g = c0.g;
                //yeet.GetComponent<wall>().b = c0.b;
                //NetworkServer.Spawn(yeet);
                tiles.Add(yeet);
                
                /*
                if (x > 1)
                {
                    //tiles[tiles.Count - 1].GetComponent<FixedJoint2D>().connectedBody = tiles[tiles.Count - 2].GetComponent<Rigidbody2D>();

                    tiles[y][x].GetComponent<FixedJoint2D>().connectedBody = tiles[y][x-1].GetComponent<Rigidbody2D>();
                    if (y > 1)
                    {
                        //tiles[y][x].GetComponents<FixedJoint2D>()[1].connectedBody = tiles[y-1][x].GetComponent<Rigidbody2D>();
                    }
                }*/
                x++;
            }
            
            
        }

    }

    void YeetBlock(GameObject block)
    {
        StartCoroutine(KillDebris(block));
    }

    IEnumerator KillDebris(GameObject block)
    {
        yield return new WaitForSecondsRealtime(1f);
        GameObject.Destroy(block);
        tiles.Remove(block);
    }
}
