using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.NetworkSystem;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class PlayerStuff : NetworkBehaviour {
    int i = 0;
    public static int newChar;
    [SyncVar(hook = "OnCharaChange")] public int chara = -1;
    public GameObject Character;
    public GameObject Cam;
    public GameObject Head;
    public GameObject sword;
    public GameObject Back;
    public GameObject Front;
    public GameObject Mouse;
    public int hideSwordCooldown = 0;
    public SpriteRenderer sR;
    public SpriteRenderer shed;
    public Rigidbody2D rb;
    [SyncVar(hook = "OnMoveXChange")] float moveX;
    [SyncVar(hook = "OnMoveYChange")] float moveY;
    [SyncVar(hook = "OnMouseXChange")] public float mouseX;
    [SyncVar(hook = "OnMouseYChange")] public float mouseY;
    Vector3 camPos = new Vector3(0, 0, -12);
    float camAcc = 0;
    Vector2 drift = new Vector2(0, 0);
    public static int playerCount;
    [SyncVar(hook = "OnNumChange")] public int playerNum = -1;
    int tempX, tempY;
    bool canFire = true;
    public NetworkClient m_client;
    bool mouseDown;
    public float HP = 69420;
    [SyncVar(hook = "OnDeadChange")] public bool dead = false;
    [SyncVar(hook = "OnFlippedChange")] public bool flipped = false;
    GameObject Text;
    GameObject Text2;
    GameObject canvas;
    bool canSpace = true;
    bool canShift = true;
    public bool localPlayer = false;
    [SyncVar(hook = "OnUnicycleChange")] bool unicycle = false;
    [SyncVar(hook = "OnInvisibleChange")] bool invisible = false;
    private GameObject spawnedUnicycle = null;
    int poison = 0;
    public int slow = 0;
    int stun = 0;
    int wingCooldown = 0;
    int weakness = 0;
    int sleep = 0;
    int glowing = 0;
    int marked = 0;
    int windwalk = 0;
    public int frozen = 0;
    public bool moonbeam = false;
    Color32 c0 = new Color32(0, 0, 0, (byte)255);

    static List<GameObject> arrows = new List<GameObject>();
    static List<GameObject> arrowsText = new List<GameObject>();

    [SyncVar(hook = "OnRedChange")] public byte r = 255;
    [SyncVar(hook = "OnGreenChange")] public byte g = 255;
    [SyncVar(hook = "OnBlueChange")] public byte b = 255;

    [SyncVar(hook = "OnRagingChange")] int raging = 0;

    GameObject charSelector;

    float[] saveState0 = { 0, 0, 0, 0, 0, 0, 0, 0, 0}; //HP, x, y, poison, slow, stun, weakness, glowing, marked
    float[] saveState1 = { 0, 0, 0, 0, 0, 0, 0, 0, 0}; //HP, x, y, poison, slow, stun, weakness, glowing, marked

    void OnRedChange(byte x)
    {
        r = x;
        c0.r = x;
    }

    void OnGreenChange(byte x)
    {
        g = x;
        c0.g = x;
    }

    void OnBlueChange(byte x)
    {
        b = x;
        c0.b = x;
    }

    void OnRagingChange(int r)
    {
        raging = r;
    }

    String[] names = { "Osman", "Anand", "Abraham", "Carl", "Claire", "Anton", "Bad Bitch", "Rudy Barrow", "Fern Hana", "Lilly Moonriver", "Kumari", "Rok Son Hak", "Roscoe Tosscobble", "Ezra", "Tenzo Inukami", "Ollie North", "Xenerich Quorin", "Knope Rollen", "Brad", "Beerstein Brewbeard", "Kohou Tekka", "Tom Nightshade", "Hastur", "Einma Ma'yera", "Basti Theyla", "Puw Llewyn", "MetalMythicMaster","Iosef Stalin","Durrith Vamor","Acoda Va'Suun","Lenin","Pickle Rick","Galon *Bean Man*" };

    String[] abilities =
    {
        "Elbow Connector",
        "",
        "Dash",
        "",

        "Nuke",
        "",
        "Explode Nuke",
        "",

        "YES YES YES",
        "",
        "YOINK",
        "",

        "Fancy Thumb",
        "",
        "Unicycle",
        "",

        "High Voltage",
        "",
        "",
        "",

        "PC Parts Spam",
        "",
        "",
        "",

        "Vicious Mockery",//
        "Ice Breath",//
        "Sleep",//
        "Cure Wounds",//

        "Arrow",//
        "Poison Bolt",//
        "Hunter's Mark",//
        "Goodberry",//

        "Flame Blade",//
        "Sacred Flame",//
        "Might of the Wind",//
        "Cure Wounds",//

        "Spear",//
        "Moonbeam",//
        "Fairy Form",//
        "Healing Spirit",//

        "Rapier",//
        "Bolt",
        "Crimson Rite of Storm",
        "Crimson Rite of Dawn",

        "Greataxe",//
        "Fire Breath",
        "Hellhound Form",
        "Rage",//

        "Shortsword",//
        "Dart",
        "Donkey",//
        "Fade Away",

        "Greatsword",//
        "Healing Hands",//
        "Wings",//
        "Wild Surge Rage",//

        "Unnarmed Strike",//
        "Sun Bolt",
        "Step of the Wind",//
        "Stunning Strike",

        "Big Iron",//
        "-5 + 10",//
        "Boots of Haste",//
        "Pushing Attack",//

        "Sneak Attack",//
        "Bow of the Sentry",//
        "Dash",//
        "Invisibility",//

        "Call Lightning",//
        "Healing Word",//
        "Insect Swarm",
        "Conjure Animals",

        "Might of the Quiet One",//
        "Spiritual Weapon",
        "Spirit Guardians",
        "Cure Wounds",//

        "Charged Warhammer",//
        "Wand of Magic Missiles",//
        "Shatter",//
        "Shield",//

        "Kitsune Claws",//
        "Sun Bolt",
        "Dash",//
        "Stunning Strike",

        "Mouth Sword",//
        "Hand Crossbow",
        "Crimson Rite of Fire",
        "Blood Curse of Binding",

        "Eldritch Blast",//
        "Grasp of the Deep",
        "Misty Step",
        "Darkness",

        "Firebolt",//
        "Magic Missiles",//
        "Pulse Wave",//
        "Slow",//

        "Greatsword",//
        "Firebolt",//
        "Manifest Echo",
        "Swap places with Echo",

        "Vicious Mockery",//
        "Pistol",//
        "Silence",
        "Hidious Laughter",

        "Fireball",//
        "Magic Missiles",//
        "Chronal Shift",//
        "Shield",//

        "Warhammer",//
        "Pistol",//
        "---",
        "Rage",//

        "Corecut Dagger",//
        "Guiding Bolt",//
        "Pyrotechnics",//
        "Cure Wounds",//

        "Disintegrate",//
        "Ray of Enfeeblement",//
        "Globe of Invulnerability",//
        "Time Stop",//

        "Tokarev",//
        "Maskirovka",
        "Sealed Train",
        "Red Terror",

        "Lightning Bolt",//
        "Gravity Fissure",
        "Flame Aura",
        "Paralyzing Aura",

        "Bean",
        "",
        "",
        "",
    };

    float[] dmg2 =
    {
        //Osman
        5,
        0.3f,
        //Anand
        5,
        0.3f,
        //Abe
        5,
        0.3f,
        //Carl
        5,
        0.3f,
        //Claire
        5,
        0.3f,
        //Anton
        5,
        0.3f,
        //Bad Bitch
        2,
        0.1f,
        //Rudy
        4,
        0.5f,
        //Fern
        4,
        0.35f,
        //Lilly
        4,
        0.4f,
        //Kumari
        4,
        0.3f,
        //Rok
        6,
        0.1f,
        //Roscoe
        4,
        0.15f,
        //Ezra
        -2,
        1.5f,
        //Tenzo
        4,
        0.15f,
        //Ollie
        12,
        0.2f,
        //Xenerich
        8,
        0.3f,
        //Knope
        -1,
        0.45f,
        //Brad
        4,
        0.3f,
        //Beerstein
        2,
        1.2f,
        //Kohou
        4,
        0.15f,
        //Tom Nightshade
        4,
        0.3f,
        //Hastur
        5,
        0.3f,
        //Einma
        2,
        0.3f,
        //Basti
        4,
        0.3f,
        //Puw
        6,
        0.5f,
        //MetalMythic
        2,
        0.5f,
        //Stalin
        6,
        0.2f,
        //Durrith
        7,
        0.4f,
        //Acoda
        0,
        0.3f,
        //Lenin
        0,
        0.3f,
        //Pickle Rick
        18,
        0.4f,
        //Galon (bean man)
        0,
        0.3f
    };

    float[] stats =
    {
        //Osman
        15,     //HP
        8,      //Speed
        7,      //Damage
        0.1f,  //Attack Delay
        1,      //Full Auto

        //Anand
        20,
        3,
        4,
        0.5f,
        0,

        //Abe
        25,
        4,
        10,
        0.3f,
        0,

        //Carl
        15,
        5,
        5,
        0.2f,
        1,

        //Claire
        16,
        6,
        9,
        0.02f,
        1,

        //Anton
        18,
        6,
        6,
        0.2f,
        0,

        //Bad Bitch 
        21,
        5,
        4,
        0.3f,
        1,

        //Rudy
        21,
        6,
        7,
        0.3f,
        0,

        //Fern
        26,
        5,
        6,
        0.3f,
        0,

        //Lilly
        12,
        5,
        5,
        0.3f,
        0,

        //Kumari
        15,
        5,
        4,
        0.15f,
        0,

        //Rok
        30,
        5,
        8,
        0.15f,
        0,

        //Roscoe
        15,
        7,
        5,
        0.1f,
        1,

        //Ezra
        30,
        5,
        8,
        0.15f,
        0,

        //Tenzo
        22,
        9,
        4,
        0.1f,
        1,

        //Ollie
        19,
        5,
        7,
        0.1f,
        1,

        //Xenerich
        24,
        5,
        12,
        0.3f,
        0,

        //Knope
        18,
        6,
        5,
        0.3f,
        0,

        //Brad
        24,
        5,
        6,
        0.3f,
        0,

        //Beerstein
        34,
        5,
        8,
        0.15f,
        0,

        //Kohou
        20,
        10,
        5,
        0.1f,
        1,

        //Tom Nightshade
        14,
        6,
        6,
        0.15f,
        0,

        //Hastur
        16,
        5,
        7,
        0.15f,
        1,

        //Einma
        15,
        5,
        8,
        0.3f,
        1,

        //Basti
        22,
        5,
        7,
        0.15f,
        0,

        //Puw
        18,
        5,
        4,
        0.3f,
        1,

        //MetalMythicMaster
        15,
        5,
        4,
        0.5f,
        0,

        //Stalin
        27,
        7,
        7,
        0.15f,
        0,

        //Durrith
        18,
        5,
        6,
        0.3f,
        0,

        //Acoda
        30,
        6,
        28,
        0.4f,
        0,

        //Lenin
        41,
        6,
        7,
        0.15f,
        1,

        //Pickle Rick
        34,
        5,
        7,
        0.45f,
        0,

        //Galon
        3,
        4,
        2,
        0.3f,
        0
    };

    void OnUnicycleChange(bool u)
    {
        unicycle = u;
    }

    void OnCharaChange(int c)
    {
        
        chara = c;
    }

    void OnNumChange(int n)
    {
        playerNum = n;
    }

    void OnDeadChange(bool n)
    {
        dead = n;
    }

    void OnFlippedChange(bool n)
    {
        flipped = n;
    }

    void OnMoveXChange(float a)
    {
        moveX = a;
    }

    void OnMoveYChange(float a)
    {
        moveY = a;
    }

    void OnMouseXChange(float a)
    {
        mouseX = a;
    }

    void OnMouseYChange(float a)
    {
        mouseY = a;
    }

    void OnInvisibleChange(bool n)
    {
        invisible = n;
    }
    
    [Command]
    void CmdUnicycleSync(bool u)
    {
        unicycle = u;
    }

    [Command]
    void CmdRagingSync(int r)
    {
        raging = r;
    }

    [Command]
    void CmdSyncMove(float x, float y, bool f, bool d, float mX, float mY, bool invis)
    {
        moveX = x;
        moveY = y;
        flipped = f;
        dead = d;
        mouseX = mX;
        mouseY = mY;
        invisible = invis;
    }

    [Command]
    void CmdSyncVarWithServer(int pn, int c)
    {
        if (pn != -1)
            playerNum = pn;
        if (c != -1)
            chara = c;
    }

    [Command]
    void CmdReset(int c)
    {
        Reset(c);
        RpcReset(c);
    }

    [ClientRpc]
    void RpcReset(int c)
    {
        Reset(c);
    }

    void Reset(int c)
    {
        chara = c;
        GameObject.Destroy(sword);
        sword = null;
        HP = 100;
        spawned = false;
        initted = false;
        dead = false;
        StartCoroutine(SetHealth());
        this.gameObject.GetComponent<CapsuleCollider2D>().isTrigger = false;
        Debug.Log("Oh no! Someone just respawned");
    }

    public void respawn()
    {
        HP = 100;
        chara = newChar;
        CmdSyncVarWithServer(playerNum, chara);
        spawned = false;
        initted = false;
        dead = false;
        if (!isServer)
        {
            CmdReset(newChar);
            //Reset(newChar);
        }
        else
            RpcReset(newChar);
        this.gameObject.GetComponent<CapsuleCollider2D>().isTrigger = false;
        Debug.Log("Oh no! You just respawned");
        GameObject.Destroy(this.charSelector);
    }

    // Use this for initialization
    void Start () {

        playerCount = GameObject.FindGameObjectsWithTag("player").Length;

        canvas = GameObject.Find("Canvas");

        if (isLocalPlayer)
        {
            localPlayer = true;
            chara = newChar;
            //m_client = GameObject.FindGameObjectWithTag("network").GetComponent<NetworkManager>().client;
            //NetworkServer.RegisterHandler(1002, OnServerReadyToBeginMessage);
            playerNum = playerCount - 1;
            //SendReadyToBeginMessage("n" + playerNum + ":" + newChar);
            Debug.Log("You just joined as " + chara);
            CmdSyncVarWithServer(playerNum, chara);
            //StartCoroutine(Spawn());
            Text = Instantiate(Resources.Load("text"), transform.position, transform.rotation) as GameObject;
            Text2 = Instantiate(Resources.Load("text"), transform.position, transform.rotation) as GameObject;
            Text.transform.SetParent(canvas.transform);
            Text2.transform.SetParent(canvas.transform);
            Debug.Log("width = " + Screen.width);
            Text.transform.GetComponent<RectTransform>().localPosition = new Vector3(0.37f*Screen.width, 0.26f*Screen.width, 0);
            Text2.transform.GetComponent<RectTransform>().localPosition = new Vector3(0.5f * Screen.width, 0.25f * Screen.width, 0);
            Text.transform.localScale = new Vector3(1, 1, 1);
            Text2.transform.localScale = new Vector3(1, 1, 1);
            Text.GetComponent<Text>().fontSize = (int)(0.02f * Screen.width);
            Text2.GetComponent<Text>().fontSize = (int)(0.01f * Screen.width);
            StartCoroutine(SetHealth());
            //if (isServer)
            //{
                foreach (GameObject e in GameObject.FindGameObjectsWithTag("map")) {
                    e.GetComponent<map>().ReadFile();
                }
            //}
            
        }
        else
        {
            //playerNum = playerCount - 1;
            Debug.Log("Someone else just joined as " + chara);
            localPlayer = false;
            //m_client = GameObject.FindGameObjectWithTag("network").GetComponent<NetworkManager>().client;
            //SendReadyToBeginMessage("n" + playerNum + ":" + newChar);
        }
        //charas[playerCount - 1] = (int)(rb.position.y / 100f + 0.5f);
        Debug.Log("Player Count = " + playerCount);
        Debug.Log("Player Num = " + playerNum);

        sR = this.GetComponent<SpriteRenderer>();
        shed = Head.GetComponent<SpriteRenderer>();
        //shed.sprite = Resources.Load("images/h" + characters[playerNum], typeof(Sprite)) as Sprite;
        //GameObject charSelect = Instantiate(Resources.Load("CharSelect"), Vector3.zero, transform.rotation) as GameObject;
        
    }

    IEnumerator Spawn()
    {
        yield return new WaitForSeconds(2f);
        Debug.Log("Spawning in");
        
    }

    [Command]
    void CmdCureWounds(Quaternion mouseDirection, Vector3 pos, string name, int owner, int seed)
    {
        //CureWounds(mouseDirection, pos, name, owner, seed);
        RpcCureWounds(mouseDirection, pos, name, owner, seed);
    }

    [ClientRpc]
    void RpcCureWounds(Quaternion mouseDirection, Vector3 pos, string name, int owner, int seed)
    {
        CureWounds(mouseDirection, pos, name, owner, seed);
    }

    void CureWounds(Quaternion mouseDirection, Vector3 pos, string name, int owner, int seed)
    {
        GameObject instantiatedProjectile = Instantiate(Resources.Load(name), pos, mouseDirection) as GameObject;
        
        instantiatedProjectile.GetComponent<bfly>().xVel = moveX * stats[5 * chara + 1];
        instantiatedProjectile.GetComponent<bfly>().yVel = moveY * stats[5 * chara + 1];
        if (instantiatedProjectile.GetComponent<bfly>().damage == 0)
            instantiatedProjectile.GetComponent<bfly>().damage = (int)(-3 - 0.05f * Mathf.Abs(seed)) / (weakness > 0 ? 2 : 1);
        instantiatedProjectile.transform.Rotate(0, 0, seed * 0.01f * instantiatedProjectile.GetComponent<bfly>().randomness);
        instantiatedProjectile.GetComponent<bfly>().owner = owner;
        instantiatedProjectile.GetComponent<bfly>().seed = seed;
        instantiatedProjectile.GetComponent<bfly>().owner_obj = this.gameObject;
    }

    [Command]
    void CmdFire(Quaternion mouseDirection, Vector3 pos, string name, int owner, int seed)
    {
        //Fire(mouseDirection, pos, name, owner, seed);
        RpcFire(mouseDirection, pos, name, owner, seed);
    }

    [ClientRpc]
    void RpcFire(Quaternion mouseDirection, Vector3 pos, string name, int owner, int seed)
    {
        Fire(mouseDirection, pos, name, owner, seed);
    }

    void Fire(Quaternion mouseDirection, Vector3 pos, string name, int owner, int seed)
    {
        try
        {
            GameObject instantiatedProjectile = Instantiate(Resources.Load(name), pos, mouseDirection) as GameObject;
            if (name[0] == 'c')
            {
                instantiatedProjectile.GetComponent<bfly>().xVel = moveX * stats[5 * chara + 1];
                instantiatedProjectile.GetComponent<bfly>().yVel = moveY * stats[5 * chara + 1];
                if (instantiatedProjectile.GetComponent<bfly>().damage == 0)
                    instantiatedProjectile.GetComponent<bfly>().damage = (int)dmg2[2 * chara] / (weakness > 0 ? 2 : 1) + (raging > 0 ? 3 : 0);
            }
            else if (name[0] != 'y')
            {
                instantiatedProjectile.GetComponent<bfly>().xVel = moveX * stats[5 * chara + 1];
                instantiatedProjectile.GetComponent<bfly>().yVel = moveY * stats[5 * chara + 1];
                if (instantiatedProjectile.GetComponent<bfly>().damage == 0)
                    instantiatedProjectile.GetComponent<bfly>().damage = (int)stats[5 * chara + 2] / (weakness > 0 ? 2 : 1) + (raging > 0 ? 3 : 0);
            }
            instantiatedProjectile.transform.Rotate(0, 0, seed * 0.01f * instantiatedProjectile.GetComponent<bfly>().randomness);
            instantiatedProjectile.GetComponent<bfly>().owner = owner;
            instantiatedProjectile.GetComponent<bfly>().seed = seed;
            instantiatedProjectile.GetComponent<bfly>().owner_obj = this.gameObject;
        } catch (Exception e)
        {
            Debug.Log("Unable to create Name=" + name);
        }
    }

    [Command]
    void CmdFire2(Vector2 vel, Vector3 pos, string name, int owner, int seed)
    {
        //Fire2(vel, pos, name, owner, seed);
        RpcFire2(vel, pos, name, owner, seed);
    }

    [ClientRpc]
    void RpcFire2(Vector2 vel, Vector3 pos, string name, int owner, int seed)
    {
        Fire2(vel, pos, name, owner, seed);
    }

    void Fire2(Vector2 vel, Vector3 pos, string name, int owner, int seed)
    {
        GameObject instantiatedProjectile = Instantiate(Resources.Load(name), pos, Quaternion.Euler(0, 0, 0)) as GameObject;
        if (name[0] != 'y' && !name.Substring(0,4).Equals("fire"))
        {
            instantiatedProjectile.GetComponent<bfly>().xVel = vel.x*4;
            instantiatedProjectile.GetComponent<bfly>().yVel = vel.y*4;
            if (instantiatedProjectile.GetComponent<bfly>().damage == 0)
                instantiatedProjectile.GetComponent<bfly>().damage = (int)(10 * Mathf.Sqrt(vel.x * vel.x + vel.y * vel.y)) / (weakness > 0 ? 2 : 1) + (raging > 0 ? 3 : 0);
            instantiatedProjectile.transform.localScale = new Vector3(1, 1, 1);
        }
        instantiatedProjectile.transform.Rotate(0, 0, seed * 0.01f * instantiatedProjectile.GetComponent<bfly>().randomness);
        instantiatedProjectile.GetComponent<bfly>().owner = owner;
        instantiatedProjectile.GetComponent<bfly>().seed = seed;
        instantiatedProjectile.GetComponent<bfly>().owner_obj = this.gameObject;
    }

    [Command]
    void CmdFire3(Vector3 pos, string name, int owner, int seed)
    {
        //Fire3(pos, name, owner, seed);
        RpcFire3(pos, name, owner, seed);
    }

    [ClientRpc]
    void RpcFire3(Vector3 pos, string name, int owner, int seed)
    {
        Fire3(pos, name, owner, seed);
    }

    void Fire3(Vector3 pos, string name, int owner, int seed)
    {
        GameObject instantiatedProjectile = Instantiate(Resources.Load(name), pos, Quaternion.Euler(0, 0, 0)) as GameObject;
        if (name[0] == 'c')
        {
            //instantiatedProjectile.GetComponent<bfly>().xVel = moveX * stats[5 * chara + 1];
            //instantiatedProjectile.GetComponent<bfly>().yVel = moveY * stats[5 * chara + 1];
            if (instantiatedProjectile.GetComponent<bfly>().damage == 0)
                instantiatedProjectile.GetComponent<bfly>().damage = (int)dmg2[2*chara] / (weakness > 0 ? 2 : 1) + (raging > 0 ? 3 : 0);
        }
        else if (name[0] != 'y')
        {
            //instantiatedProjectile.GetComponent<bfly>().xVel = moveX * stats[5 * chara + 1];
            //instantiatedProjectile.GetComponent<bfly>().yVel = moveY * stats[5 * chara + 1];
            if (instantiatedProjectile.GetComponent<bfly>().damage == 0)
                instantiatedProjectile.GetComponent<bfly>().damage = (int)stats[5 * chara + 2] / (weakness > 0 ? 2 : 1) + (raging > 0 ? 3 : 0);
        }
        instantiatedProjectile.transform.Rotate(0, 0, seed * 0.01f * instantiatedProjectile.GetComponent<bfly>().randomness);
        instantiatedProjectile.GetComponent<bfly>().owner = owner;
        instantiatedProjectile.GetComponent<bfly>().seed = seed;
        instantiatedProjectile.GetComponent<bfly>().owner_obj = this.gameObject;
    }

    [Command]
    void CmdFireParticle(Quaternion mouseDirection, Vector3 pos, string name, int owner)
    {
        //FireParticle(mouseDirection, pos, name, owner);
        RpcFireParticle(mouseDirection, pos, name, owner);
    }

    [ClientRpc]
    void RpcFireParticle(Quaternion mouseDirection, Vector3 pos, string name, int owner)
    {
        FireParticle(mouseDirection, pos, name, owner);
    }

    void FireParticle(Quaternion mouseDirection, Vector3 pos, string name, int owner)
    {
        GameObject instantiatedProjectile;
        if (name[0] == 'w' && name[1] == 'i')
        {
            instantiatedProjectile = Instantiate(Resources.Load("wind"), pos, mouseDirection) as GameObject;
            instantiatedProjectile.GetComponent<bfly>().c = 2*int.Parse(name[4].ToString());
            instantiatedProjectile.GetComponent<bfly>().lifetime += 2*int.Parse(name[4].ToString());
        } else
            instantiatedProjectile = Instantiate(Resources.Load(name), pos, mouseDirection) as GameObject;
        if (name[0] == 'c')
        {
            instantiatedProjectile.GetComponent<bfly>().xVel = moveX * stats[5 * chara + 1];
            instantiatedProjectile.GetComponent<bfly>().yVel = moveY * stats[5 * chara + 1];
            instantiatedProjectile.GetComponent<bfly>().damage = 0;
            instantiatedProjectile.GetComponent<bfly>().noDmg = true;
        }
        else if (name[0] != 'y')
        {
            instantiatedProjectile.GetComponent<bfly>().xVel = moveX * stats[5 * chara + 1];
            instantiatedProjectile.GetComponent<bfly>().yVel = moveY * stats[5 * chara + 1];
            instantiatedProjectile.GetComponent<bfly>().damage = 0;
            instantiatedProjectile.GetComponent<bfly>().noDmg = true;
        }
        
        instantiatedProjectile.transform.Rotate(0, 0, Random.Range(-instantiatedProjectile.GetComponent<bfly>().randomness, instantiatedProjectile.GetComponent<bfly>().randomness));
        instantiatedProjectile.GetComponent<bfly>().owner = owner;
        instantiatedProjectile.GetComponent<bfly>().seed = Random.Range(-100, 100);
        instantiatedProjectile.GetComponent<bfly>().particle = true;
        instantiatedProjectile.GetComponent<bfly>().owner_obj = this.gameObject;
    }

    public GameObject FireParticle2(Vector3 pos, Vector3 vel, Quaternion direction, string name, GameObject owner_obj, bool noDie = false, bool noDmg = true)
    {
        GameObject instantiatedProjectile = Instantiate(Resources.Load(name), pos, direction) as GameObject;

        instantiatedProjectile.GetComponent<bfly>().xVel = vel.x;
        instantiatedProjectile.GetComponent<bfly>().yVel = vel.y;
        if (noDmg) instantiatedProjectile.GetComponent<bfly>().damage = 0;
        instantiatedProjectile.GetComponent<bfly>().noDmg = noDmg;
        instantiatedProjectile.GetComponent<bfly>().penetration = true;
        instantiatedProjectile.GetComponent<bfly>().noDie = noDie;
        instantiatedProjectile.GetComponent<bfly>().seed = Random.Range(-100, 100);

        instantiatedProjectile.GetComponent<bfly>().owner = owner_obj.GetComponent<PlayerStuff>().playerNum;
        instantiatedProjectile.GetComponent<bfly>().particle = true;
        instantiatedProjectile.GetComponent<bfly>().owner_obj = owner_obj;

        return instantiatedProjectile;
    }

    bool initted = false;
    // Update is called once per frame
    void Update () {

        if (invisible && this.gameObject.GetComponent<SpriteRenderer>().enabled)
        {
            Front.GetComponent<SpriteRenderer>().enabled = false;
            Back.GetComponent<SpriteRenderer>().enabled = false;
            Head.GetComponent<SpriteRenderer>().enabled = false;
            this.gameObject.GetComponent<SpriteRenderer>().enabled = false;
            if (sword)
                sword.GetComponent<SpriteRenderer>().enabled = false;
            if (chara == 9)
                this.gameObject.GetComponent<CapsuleCollider2D>().isTrigger = true;
        } else if (!invisible && !this.gameObject.GetComponent<SpriteRenderer>().enabled)
        {
            Front.GetComponent<SpriteRenderer>().enabled = true;
            Back.GetComponent<SpriteRenderer>().enabled = true;
            Head.GetComponent<SpriteRenderer>().enabled = true;
            this.gameObject.GetComponent<SpriteRenderer>().enabled = true;
            if (sword)
                sword.GetComponent<SpriteRenderer>().enabled = true;
            if (chara == 9)
                this.gameObject.GetComponent<CapsuleCollider2D>().isTrigger = false;
        }

        if (sword != null)
        {
            if (chara == 13)
                sword.transform.position = transform.position + new Vector3(-1.5f, -0.5f, 0);
            else
                sword.transform.position = transform.position + new Vector3(-0.8f, 0.9f, 0);
        }

        /*if (playerNum < charas.Count && charas[playerNum] != -1)
            shed.sprite = Resources.Load("images/h" + charas[playerNum], typeof(Sprite)) as Sprite;*/
        if (!initted && chara != -1)
        {
            shed.sprite = Resources.Load("images/h" + chara, typeof(Sprite)) as Sprite;
            try
            {
                Front.GetComponent<SpriteRenderer>().sortingOrder = 1;
                Front.GetComponent<SpriteRenderer>().sprite = Resources.Load("images/front" + chara, typeof(Sprite)) as Sprite;
                Back.GetComponent<SpriteRenderer>().sprite = Resources.Load("images/back" + chara, typeof(Sprite)) as Sprite;
            } catch (Exception e) {}
            
            initted = true;
        }
        if (dead)
        {
            Front.GetComponent<SpriteRenderer>().sortingOrder = 3;
            Front.GetComponent<SpriteRenderer>().sprite = Resources.Load("images/dead", typeof(Sprite)) as Sprite;
        }
        
        if (unicycle)
        {
            if (spawnedUnicycle == null)
            {
                if (chara == 3)
                    spawnedUnicycle = Instantiate(Resources.Load("unicycle"), transform.position - transform.up * 2f, Quaternion.Euler(0, 0, 0)) as GameObject;
                else if (chara == 13)
                    spawnedUnicycle = Instantiate(Resources.Load("wings"), transform.position + transform.up * 0.5f, Quaternion.Euler(0, 0, 0)) as GameObject;
                if (chara == 12)
                    spawnedUnicycle = Instantiate(Resources.Load("donkey"), transform.position - transform.up * 2f, Quaternion.Euler(0, 0, 0)) as GameObject;
            }
            if (chara == 3)
                spawnedUnicycle.transform.SetPositionAndRotation(transform.position - transform.up, Quaternion.Euler(0, 0, 0));
            else if (chara == 12)
                spawnedUnicycle.transform.SetPositionAndRotation(transform.position - transform.up * 0.5f, Quaternion.Euler(0, 0, 0));
            else if (chara == 13)
                spawnedUnicycle.transform.SetPositionAndRotation(transform.position + transform.up * 0.5f, Quaternion.Euler(0, 0, 0));
        }
        else
        {
            /*
            if (spawnedUnicycle != null)
            {
                if (!isServer)
                {
                    CmdFire2(new Vector2(moveX, moveY), transform.position - transform.up * 2, "uniP", playerNum);
                    Fire2(new Vector2(moveX, moveY), transform.position - transform.up * 2, "uniP", playerNum);
                }
                else
                    RpcFire2(new Vector2(moveX, moveY), transform.position - transform.up * 2, "uniP", playerNum);
            }*/
            if (spawnedUnicycle != null)
            {
                canSpace = false;
                StartCoroutine(ResetSpace(4));
                GameObject.Destroy(spawnedUnicycle);
                spawnedUnicycle = null;
            }
        }

        if (isLocalPlayer)
        {
            //shed.sprite = Resources.Load("images/h" + characters[playerNum], typeof(Sprite)) as Sprite;
            if (unicycle)
            {
                float maxSpeed = 3;
                if (chara == 12)
                    maxSpeed = 2;
                moveX += Input.GetAxisRaw("Horizontal") * 0.05f;
                if (moveX > 0)
                    moveX = Mathf.Min(moveX, maxSpeed);
                else
                    moveX = Mathf.Max(moveX, -maxSpeed);

                moveY += Input.GetAxisRaw("Vertical") * 0.05f;
                if (moveY > 0)
                    moveY = Mathf.Min(moveY, maxSpeed);
                else
                    moveY = Mathf.Max(moveY, -maxSpeed);
            }
            else
            {
                moveX = Input.GetAxisRaw("Horizontal");
                moveY = Input.GetAxisRaw("Vertical");
            }
            if (dead)
            {
                moveX = 0;
                moveY = 0;
                invisible = false;
                this.gameObject.GetComponent<CapsuleCollider2D>().isTrigger = true;
            }
            if (slow > 0)
            {
                moveX /= 2;
                moveY /= 2;
            }
            if (windwalk > 0)
            {
                moveX *= 4;
                moveY *= 4;
            }
            if (stun > 0 || sleep > 0)
            {
                moveX = 0;
                moveY = 0;
            }
            if (chara == 9 && invisible)
            {
                moveX *= 2;
                moveY *= 2;
                trail(rb.position + Time.fixedDeltaTime * (new Vector2(moveX, moveY)), new Vector2(moveX, moveY), "spark4");
            }
            moveX += drift.x;
            moveY += drift.y;
            drift *= 0.6f;
            if (Mathf.Abs(drift.x) < 0.01) drift.x = 0;
            if (Mathf.Abs(drift.y) < 0.01) drift.y = 0;

            canvas.GetComponent<RectTransform>().position = new Vector3(moveX, moveY, 0);
            camPos.z = Cam.GetComponent<Transform>().position.z;

            float dist = Vector2.Distance(camPos, Character.transform.position);
            if (drift.x != 0 || drift.y != 0 || (camAcc > 0))
            {
                Cam.GetComponent<Transform>().position = camPos;
               
                if (dist > 0.1)
                {
                    camPos.x += camAcc * (Character.transform.position.x - camPos.x) / dist;
                    camPos.y += camAcc * (Character.transform.position.y - camPos.y) / dist;
                    if (Vector2.Distance(camPos, Character.transform.position) > dist)
                        camAcc = 0;
                    if (dist > 1)
                        camAcc += 0.002f * (dist-1f);
                }
                else if (camAcc > 0)
                {
                    if (camAcc < 0.01f)
                    {
                        camAcc = 0;
                        camPos.x = Character.GetComponent<Transform>().position.x;
                        camPos.y = Character.GetComponent<Transform>().position.y;
                    }
                    else camAcc -= 0.008f;
                }
            }
            else
            {
                camPos.x = Character.GetComponent<Transform>().position.x;
                camPos.y = Character.GetComponent<Transform>().position.y;
            }

            if (chara != -1)
                Text.GetComponent<Text>().text = "HP: " + (int)(HP+0.5f) + "/" + (int)stats[5 * chara];
            Text2.GetComponent<Text>().text = names[chara] + " #" + playerNum + "\nPrimary: " + (canFire ? abilities[4 * chara] : "---") + "\nSecondary: " + (canFire ? abilities[4 * chara + 1] : "---") + "\nSpacebar: " + (canSpace ? abilities[4 * chara + 2] : "---") + "\nL-Shift: " + (canShift ? abilities[4 * chara + 3] : "---") + (poison > 0 ? "\nPoison: " + poison/50f : "") + (stun > 0 ? "\nStun: " + stun/50f : "") + (slow > 0 ? "\nSlow: " + slow/50f : "") + (weakness > 0 ? "\nWeakness: " + weakness/50f : "") + (marked > 0 ? "\nMarked: " + marked / 50f : "") + (glowing > 0 ? "\nGlowing: " + glowing / 50f : "") + (sleep > 0 ? "\nSleep: " + sleep / 50f : "") + (frozen > 0 ? "\nFrozen in Time: " + frozen / 50f : "") + (raging > 0 ? "\nRaging: " + raging / 50f : "");
            //Debug.Log("HP: " + HP);

        }
        else
        {
            GameObject player = getLocalPlayer();
            while (arrows.Count < Mathf.Max(playerNum+1,playerCount))
            {
                arrows.Add(Instantiate(Resources.Load("arrow"), new Vector3(0, 0, 0), Quaternion.Euler(0, 0, 0)) as GameObject);
                arrows[arrows.Count - 1].transform.SetParent(player.transform);
                arrows[arrows.Count - 1].transform.parent = player.transform;
            }
            while (arrowsText.Count < Mathf.Max(playerNum+1,playerCount))
            {
                arrowsText.Add(Instantiate(Resources.Load("text"), new Vector3(0, 0, 0), Quaternion.Euler(0, 0, 0)) as GameObject);
                arrowsText[arrowsText.Count - 1].transform.SetParent(player.transform);
                arrowsText[arrowsText.Count - 1].transform.parent = player.transform;
                arrowsText[arrowsText.Count - 1].GetComponent<Text>().fontSize = (int)(0.02f * Screen.width);
            }
            try
            {
                Quaternion dir = Quaternion.Euler(0, 0, Mathf.Atan2((transform.position.y - player.transform.position.y), (transform.position.x - player.transform.position.x)) * Mathf.Rad2Deg - 90);
                arrows[playerNum].transform.rotation = dir;
                arrows[playerNum].transform.position = player.transform.position + arrows[playerNum].transform.up * 6;
                if (Mathf.Sqrt(0.564f*Mathf.Pow(player.transform.position.x - transform.position.x, 2) + Mathf.Pow(player.transform.position.y - transform.position.y, 2)) < Mathf.Abs(player.GetComponent<PlayerStuff>().Cam.GetComponent<Camera>().transform.position.z) * (0.642f) || (invisible && chara != 9))
                    arrows[playerNum].transform.position += transform.up * 100000;
                arrows[player.GetComponent<PlayerStuff>().playerNum].transform.position = player.transform.position + transform.up * 1000000;

                arrowsText[playerNum].transform.position = player.transform.position + arrows[playerNum].transform.up * 4;
                arrowsText[playerNum].GetComponent<Text>().text = (int)(Mathf.Sqrt(Mathf.Pow(transform.position.y - player.transform.position.y, 2) + Mathf.Pow(transform.position.x - player.transform.position.x, 2))) + " ft";
            }
            catch (Exception e) { }
        }
        /*
        foreach (GameObject e in GameObject.FindGameObjectsWithTag("player"))
        {
            //e.GetComponent<PlayerStuff>().shed.sprite = Resources.Load("images/h" + characters[e.GetComponent<PlayerStuff>().playerNum], typeof(Sprite)) as Sprite;
            e.GetComponent<PlayerStuff>().shed.sprite = Resources.Load("images/h" + charas[e.GetComponent<PlayerStuff>().playerNum], typeof(Sprite)) as Sprite;
            Debug.Log(e.GetComponent<PlayerStuff>().playerNum);
            Debug.Log(e.GetComponent<PlayerStuff>().playerNum + " -> images/h" + charas[e.GetComponent<PlayerStuff>().playerNum]);
        }*/


    }

    float FancyMin(float a, float b)
    {
        if (Mathf.Abs(a) < Mathf.Abs(b))
            return a;
        return b;
    }

    public GameObject getLocalPlayer()
    {
        if (isLocalPlayer)
            return this.gameObject;
        else
        {
            foreach (GameObject e in GameObject.FindGameObjectsWithTag("player"))
            {
                if (e.GetComponent<PlayerStuff>().localPlayer)
                    return e;
            }
        }
        return null;
    }

    public void trail(Vector2 pos, Vector2 vel, String GameObjectName)
    {
        
            Quaternion mouseDirection = Quaternion.Euler(0, 0, Random.Range(0, 360));
            if (!isServer)
            {
                CmdFireParticle(mouseDirection, (Vector3)pos, GameObjectName, playerNum);
                //FireParticle(mouseDirection, (Vector3)pos, GameObjectName, playerNum);
            }
            else
                RpcFireParticle(mouseDirection, (Vector3)pos, GameObjectName, playerNum);
    }

    public void windtrail(Vector2 pos, Vector2 vel)
    {
        String GameObjectName = "wind" + (windwalk/5) % 3;
        Quaternion mouseDirection = Quaternion.Euler(0, 0, Random.Range(0, 360));
        if (!isServer)
        {
            CmdFireParticle(mouseDirection, (Vector3)pos, GameObjectName, playerNum);
            //FireParticle(mouseDirection, (Vector3)pos, GameObjectName, playerNum);
        }
        else
            RpcFireParticle(mouseDirection, (Vector3)pos, GameObjectName, playerNum);
    }

    public void explode(float x, float y)
    {
        for (int i = 0; i < 20; i++)
        {
            Quaternion mouseDirection = Quaternion.Euler(0, 0, Random.Range(0, 360));
            int seed = Random.Range(-100, 100);
            if (!isServer)
            {
                CmdFire(mouseDirection, new Vector3(x, y, 0), "explode0", playerNum, seed);
                //Fire(mouseDirection, new Vector3(x, y, 0), "explode0", playerNum, seed);
            }
            else
                RpcFire(mouseDirection, new Vector3(x, y, 0), "explode0", playerNum, seed);
        }
    }

    public void shockwave(float x, float y)
    {
        for (int i = 0; i < 5; i++)
        {
            Quaternion mouseDirection = Quaternion.Euler(0, 0, Random.Range(0, 360));
            int seed = Random.Range(-100, 100);
            if (!isServer)
            {
                CmdFire(mouseDirection, new Vector3(x, y, 0), "spark1", playerNum, seed);
                //Fire(mouseDirection, new Vector3(x, y, 0), "spark1", playerNum, seed);
            }
            else
                RpcFire(mouseDirection, new Vector3(x, y, 0), "spark1", playerNum, seed);
        }
    }

    public void healwave(Vector3 pos)
    {
        for (int i = 0; i < 3; i++)
        {
            Quaternion mouseDirection = Quaternion.Euler(0, 0, Random.Range(0, 360));
            int seed = Random.Range(-100, 100);
            if (!isServer)
            {
                CmdFire(mouseDirection, pos, "cheal0", playerNum, seed);
                //Fire(mouseDirection, pos, "cheal0", playerNum, seed);
            }
            else
                RpcFire(mouseDirection, pos, "cheal0", playerNum, seed);
        }
    }

    public void firewave(float x, float y)
    {
        for (int i = 0; i < 15; i++)
        {
            Quaternion mouseDirection = Quaternion.Euler(0, 0, Random.Range(0, 360));
            int seed = Random.Range(-100, 100);
            if (!isServer)
            {
                CmdFire(mouseDirection, new Vector3(x, y, 0), "fire2", playerNum, seed);
                //Fire(mouseDirection, new Vector3(x, y, 0), "fire2", playerNum, seed);
            }
            else
                RpcFire(mouseDirection, new Vector3(x, y, 0), "fire2", playerNum, seed);
        }
    }

    [Command]
    void Cmddelete(int playerNum)
    {
        foreach (GameObject e in GameObject.FindGameObjectsWithTag("bullet"))
        {

            if (e.GetComponent<bfly>().explosive && e.GetComponent<bfly>().owner == playerNum)
            {
                GameObject.Destroy(e.gameObject);

            }
        }
        Rpcdelete(playerNum);
    }

    [ClientRpc]
    void Rpcdelete(int playerNum)
    {
        foreach (GameObject e in GameObject.FindGameObjectsWithTag("bullet"))
        {

            if (e.GetComponent<bfly>().explosive && e.GetComponent<bfly>().owner == playerNum)
            {
                GameObject.Destroy(e.gameObject);

            }
        }
    }

    bool spawned = false;
    int saveElapse = 0;
    bool glowInit = false;
    //Called on a fixed timer (50 times per second by default)
    void FixedUpdate()
    {
        //if (isServer)
        //RpcSyncVarWithClients(playerNum, chara);

        if (frozen > 0)
        {
            frozen--;
            return;
        }

        if (sleep > 0)
        {
            sleep--;
            return;
        }

        shed.flipX = flipped;
        Mouse.transform.position = new Vector3(mouseX, mouseY, 0);

        if (localPlayer)
        {
            if (chara == 13 && unicycle)
            {
                trail(rb.position + 3.1f*Vector2.right + 2.8f*Vector2.up, new Vector2(0, 0), "spark4");
                trail(rb.position - 3.1f*Vector2.right + 2.8f*Vector2.up, new Vector2(0, 0), "spark4");
                wingCooldown--;
                if (wingCooldown <= 0)
                {
                    canSpace = false;
                    StartCoroutine(ResetSpace(4));
                    GameObject.Destroy(spawnedUnicycle);
                    spawnedUnicycle = null;
                    unicycle = false;
                    CmdUnicycleSync(unicycle);
                }
            }

            if (HP <= 0 && !dead)
            {
                dead = true;
                StartCoroutine(Die());
            }

            if (Input.mouseScrollDelta.y > 0 && Cam.GetComponent<Transform>().position.z < -3)
            {
                Cam.GetComponent<Transform>().position = Cam.GetComponent<Transform>().position + new Vector3(0, 0, 1);
            } else if (Input.mouseScrollDelta.y < 0 && Cam.GetComponent<Transform>().position.z > -40)
            {
                Cam.GetComponent<Transform>().position = Cam.GetComponent<Transform>().position + new Vector3(0, 0, -1);
            }
            
            if (chara == 26)
            {
                saveElapse++;
                if (saveElapse % 80 == 0)
                {
                    saveState1[0] = saveState0[0];
                    saveState1[1] = saveState0[1];
                    saveState1[2] = saveState0[2];
                    saveState1[3] = saveState0[3];
                    saveState1[4] = saveState0[4];
                    saveState1[5] = saveState0[5];

                    saveState0[0] = HP;
                    saveState0[1] = rb.position.x;
                    saveState0[2] = rb.position.y;
                    saveState0[3] = poison;
                    saveState0[4] = slow;
                    saveState0[5] = stun;
                }
            }
        }

        if (raging > 0)
        {
            if (raging == 1)
                stats[5 * chara + 1] /= 1.5f;
            raging--;
            if (raging % 5 == 0)
                trail(transform.position, new Vector2(moveX, moveY), "rage");
        }
        
        c0.r = r;
        c0.g = g;
        c0.b = b;
        this.GetComponent<SpriteRenderer>().color = c0;
        Head.GetComponent<SpriteRenderer>().color = c0;
        if (r < 230)
            r += 20;
        else
            r = 255;
        if (g < 230)
            g += 20;
        else
            g = 255;
        if (b < 230)
            b += 20;
        else
            b = 255;
        if (poison > 0)
        {
            if (poison % 50 == 0 && HP > 1)
            {
                HP -= 1;
                r = 0;
                g = 0;
                b = 0;
            }
            poison--;
        }
        if (weakness > 0)
            weakness--;
        if (slow > 0)
            slow--;
        if (stun > 0)
            stun--;
        if (marked > 0)
            marked--;
        if (windwalk > 0)
        {
            windtrail(transform.position + new Vector3(0.05f * Random.Range(-10, 10), 0.05f * Random.Range(-10, 10), 0), Vector2.zero);
            windwalk--;
        }
        if (glowing > 0)
        {
            if (!glowInit)
            {
                glowInit = true;
                this.gameObject.GetComponent<CircleCollider2D>().enabled = true;
            }
            Front.GetComponent<SpriteRenderer>().color = new Color32((byte)Random.Range(0, 255), (byte)Random.Range(0, 255), (byte)Random.Range(0, 255), (byte)255);
            Back.GetComponent<SpriteRenderer>().color = new Color32((byte)Random.Range(0, 255), (byte)Random.Range(0, 255), (byte)Random.Range(0, 255), (byte)255);
            shed.color = new Color32((byte)Random.Range(0, 255), (byte)Random.Range(0, 255), (byte)Random.Range(0, 255), (byte)255);
            glowing--;
        } else if (glowInit)
        {
            glowInit = false;
            this.gameObject.GetComponent<CircleCollider2D>().enabled = false;
            Front.GetComponent<SpriteRenderer>().color = new Color32((byte)255, (byte)255, (byte)255, (byte)255);
            Back.GetComponent<SpriteRenderer>().color = new Color32((byte)255, (byte)255, (byte)255, (byte)255);
            shed.color = new Color32((byte)255, (byte)255, (byte)255, (byte)255);
        }

        int[] swordChars = { 8, 9, 10, 11, 13, 18, 19, 21, 24, 27 };
        if (Array.Exists(swordChars, element => element == chara) && sword == null)
        {
            if (chara == 11)
                sword = Instantiate(Resources.Load("b" + chara), transform.position + new Vector3(-0.2f, -0.8f, 0), Quaternion.Euler(0, 0, 40)) as GameObject;
            else if (chara == 13)
                sword = Instantiate(Resources.Load("b" + chara), transform.position + new Vector3(-1f, -1.8f, 0), Quaternion.Euler(0, 0, 110)) as GameObject;
            else
                sword = Instantiate(Resources.Load("b" + chara), transform.position + new Vector3(-0.2f, -0.8f, 0), Quaternion.Euler(0, 0, 20)) as GameObject;
            sword.GetComponent<bfly>().damage = 0;
            sword.GetComponent<bfly>().speed = 0;
            sword.GetComponent<bfly>().speedRand = 0;
            sword.GetComponent<bfly>().noDmg = true;
            sword.GetComponent<bfly>().noDie = true;
            sword.GetComponent<bfly>().penetration = true;
            sword.GetComponent<bfly>().melee = false;
            sword.GetComponent<bfly>().owner_obj = this.gameObject;
            sword.GetComponent<bfly>().knockback = 0;
            if (chara != 8)
                sword.GetComponent<bfly>().trail = false;
            sword.GetComponent<bfly>().detach = false;
            sword.GetComponent<bfly>().destruction = false;
            if (chara == 9)
                sword.transform.localScale = sword.transform.localScale * 1.4f;
            else if (chara != 8 && chara != 18)
                sword.transform.localScale = sword.transform.localScale * 0.65f;
        }
        else if (sword != null)
        {
            sword.SetActive(hideSwordCooldown == 0);
            if (hideSwordCooldown > 0) hideSwordCooldown--;
        }

        if (!isServer)
            CmdSyncMove(moveX, moveY, flipped, dead, mouseX, mouseY, invisible);
        animate();

        Vector3 mousePosition = Cam.GetComponent<Camera>().ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Input.mousePosition.z - Cam.GetComponent<Camera>().transform.position.z));
        if (isLocalPlayer)
        {
            mouseX = mousePosition.x;
            mouseY = mousePosition.y;
        }

        if (isLocalPlayer && !dead)
        {
            if (Input.GetKeyDown(KeyCode.Space) && canSpace)
            {
                if (chara != 9 && chara != 16)
                    invisible = false;
                canSpace = false;
                int cooldown = 0;
                if (chara == 0 || chara == 16 || chara == 14 || chara == 20)
                {
                    stats[5 * chara + 1] *= 4;
                }
                else if (chara == 1)
                {

                    foreach (GameObject e in GameObject.FindGameObjectsWithTag("bullet"))
                    {

                        if (e.GetComponent<bfly>().explosive && e.GetComponent<bfly>().owner == playerNum)
                        {
                            Debug.Log("boom");
                            explode(e.GetComponent<bfly>().rb.position.x, e.GetComponent<bfly>().rb.position.y);
                            GameObject.Destroy(e.gameObject);

                        }
                    }
                    if (!isServer)
                        Cmddelete(playerNum);
                    else
                        Rpcdelete(playerNum);
                }
                else if (chara == 2)
                {
                    stats[5 * chara + 1] *= 3;
                }
                else if (chara == 3)
                {
                    unicycle = !unicycle;
                    CmdUnicycleSync(unicycle);
                    if (unicycle == false)
                    {
                        int seed = Random.Range(-100, 100);
                        if (!isServer)
                        {
                            CmdFire2(new Vector2(moveX, moveY), transform.position - transform.up * 2, "uniP", playerNum, seed);
                            //Fire2(new Vector2(moveX, moveY), transform.position - transform.up * 2, "uniP", playerNum, seed);
                        }
                        else
                            RpcFire2(new Vector2(moveX, moveY), transform.position - transform.up * 2, "uniP", playerNum, seed);
                        GameObject.Destroy(spawnedUnicycle);
                        spawnedUnicycle = null;
                    }
                }
                else if (chara == 6)
                {
                    canSpace = false;
                    cooldown = 9;
                    if (!isServer)
                    {
                        CmdFire2(Vector2.zero, mousePosition, "sleep", playerNum, 0);
                        //Fire2(Vector2.zero, mousePosition, "sleep", playerNum, 0);
                    }
                    else
                        RpcFire2(Vector2.zero, mousePosition, "sleep", playerNum, 0);
                }
                else if (chara == 7)
                {
                    canSpace = false;
                    cooldown = 5;
                    if (!isServer)
                    {
                        CmdFire2(Vector2.zero, mousePosition, "huntersmark", playerNum, 0);
                        //Fire2(Vector2.zero, mousePosition, "huntersmark", playerNum, 0);
                    }
                    else
                        RpcFire2(Vector2.zero, mousePosition, "huntersmark", playerNum, 0);
                }
                else if (chara == 8)
                {
                    canSpace = false;
                    cooldown = 3;
                    windwalk = 30;
                }
                else if (chara == 9)
                {
                    invisible = !invisible;
                }
                else if (chara == 12)
                {
                    unicycle = !unicycle;
                    CmdUnicycleSync(unicycle);
                    if (unicycle == false)
                    {
                        GameObject.Destroy(spawnedUnicycle);
                        spawnedUnicycle = null;
                    }
                }
                else if (chara == 13)
                {
                    if (wingCooldown <= 0)
                        wingCooldown = 150;
                    unicycle = !unicycle;
                    CmdUnicycleSync(unicycle);
                    if (unicycle == false)
                    {
                        canSpace = false;
                        GameObject.Destroy(spawnedUnicycle);
                        spawnedUnicycle = null;
                    }
                }
                else if (chara == 15)
                {
                    cooldown = 4;
                    canSpace = false;
                    stats[5 * chara + 1] *= 2;
                }
                else if (chara == 19)
                {
                    canSpace = false;
                    cooldown = 4;
                    shockwave(mouseX, mouseY);
                } else if (chara == 23)
                {
                    canSpace = false;
                    float mouseDirection = Mathf.Atan2((mousePosition.y - Character.transform.position.y), (mousePosition.x - Character.transform.position.x)) * Mathf.Rad2Deg - 90;
                    int seed = Random.Range(-100, 100);
                    if (!isServer)
                    {
                        CmdFire(Quaternion.Euler(0, 0, mouseDirection), Character.transform.position, "pulse", playerNum, seed);
                        //Fire(Quaternion.Euler(0, 0, mouseDirection), Character.transform.position, "pulse", playerNum, seed);
                    }
                    else
                        RpcFire(Quaternion.Euler(0, 0, mouseDirection), Character.transform.position, "pulse", playerNum, seed);
                    
                    cooldown = 5;
                }
                else if (chara == 26)
                {
                    canSpace = false;
                    HP = saveState1[0];
                    drift.x = (saveState1[1] - rb.position.x) * 2.3f;
                    drift.y = (saveState1[2] - rb.position.y) * 2.3f;
                    poison = (int)saveState1[3];
                    slow = (int)saveState1[4];
                    stun = (int)saveState1[5];
                    weakness = (int)saveState1[6];
                    cooldown = 5;
                    int seed = Random.Range(-100, 100);
                    Quaternion mouseDirection = Quaternion.Euler(0, 0, 0);
                    if (!isServer)
                    {
                        CmdFire2(drift/2.3f, Character.transform.position, "time0", playerNum, seed);
                        //Fire2(drift / 2.3f, Character.transform.position, "time0", playerNum, seed);
                    }
                    else
                        RpcFire2(drift / 2.3f, Character.transform.position, "time0", playerNum, seed);
                } else if (chara == 28)
                {
                    canSpace = false;
                    for (int i = 0; i < 15; i++)
                    {
                        int seed = Random.Range(-100, 100);
                        if (!isServer)
                        {
                            CmdFire2(Vector2.zero, mousePosition, "smoke", playerNum, seed);
                            //Fire2(Vector2.zero, mousePosition, "smoke", playerNum, seed);
                        }
                        else
                            RpcFire2(Vector2.zero, mousePosition, "smoke", playerNum, seed);
                    }
                    cooldown = 5;
                } else if (chara == 29)
                {
                    canSpace = false;
                    cooldown = 10;
                    if (!isServer)
                    {
                        CmdFire2(Vector2.zero, mousePosition, "invul", playerNum, 0);
                        //Fire2(Vector2.zero, mousePosition, "invul", playerNum, 0);
                    }
                    else
                        RpcFire2(Vector2.zero, mousePosition, "invul", playerNum, 0);
                }
                if (!canSpace)
                    StartCoroutine(ResetSpace(cooldown));
            }

            if ((Input.GetKeyDown(KeyCode.LeftShift) || Input.GetMouseButtonDown(2)) && canShift)
            {
                invisible = false;
                canShift = false;
                int cooldown = 5;
                switch (chara)
                {
                    case 9: // lilly
                        cooldown = 9;
                        if (!isServer)
                        {
                            CmdFire2(Vector2.zero, mousePosition, "mantaray", playerNum, 0);
                            //Fire2(Vector2.zero, mousePosition, "mantaray", playerNum, 0);
                        }
                        else
                            RpcFire2(Vector2.zero, mousePosition, "mantaray", playerNum, 0);
                        break;
                    case 13: // ezra
                        StartCoroutine(WildMagic());
                        raging = 300;
                        stats[5 * chara + 1] *= 1.5f;
                        if (!isServer)
                            CmdRagingSync(raging);
                        cooldown = 10;
                        break;
                    case 16: // xenerich
                        invisible = true;
                        cooldown = 5;
                        break;
                    case 11: // rok
                    case 27: // stalin
                        raging = 300;
                        stats[5 * chara + 1] *= 1.5f;
                        if (!isServer)
                            CmdRagingSync(raging);
                        cooldown = 10;
                        break;
                    case 19: // beerstein
                        {
                            Quaternion mouseDirection = Quaternion.Euler(0, 0, 0);
                            if (!isServer)
                            {
                                CmdFire2(Vector2.zero, Character.transform.position, "shield1", playerNum, 0);
                                //Fire2(Vector2.zero, Character.transform.position, "shield1", playerNum, 0);
                            }
                            else
                                RpcFire2(Vector2.zero, Character.transform.position, "shield1", playerNum, 0);
                            break;
                        }
                    case 23: // einma
                        cooldown = 10;
                        if (!isServer)
                        {
                            CmdFire2(Vector2.zero, mousePosition, "time1", playerNum, 0);
                            //Fire2(Vector2.zero, mousePosition, "time1", playerNum, 0);
                        }
                        else
                            RpcFire2(Vector2.zero, mousePosition, "time1", playerNum, 0);
                        break;
                    case 26: // metalmythicmaster
                        {
                            if (!isServer)
                            {
                                CmdFire2(Vector2.zero, Character.transform.position, "shield", playerNum, 0);
                                //Fire2(Vector2.zero, Character.transform.position, "shield", playerNum, 0);
                            }
                            else
                                RpcFire2(Vector2.zero, Character.transform.position, "shield", playerNum, 0);
                            break;
                        }
                    case 15: // ollie
                        {
                            canSpace = false;
                            float mouseDirection = Mathf.Atan2((mousePosition.y - Character.transform.position.y), (mousePosition.x - Character.transform.position.x)) * Mathf.Rad2Deg - 90;
                            int seed = Random.Range(-100, 100);
                            if (!isServer)
                            {
                                CmdFire(Quaternion.Euler(0, 0, mouseDirection), Character.transform.position, "d15", playerNum, seed);
                                //Fire(Quaternion.Euler(0, 0, mouseDirection), Character.transform.position, "d15", playerNum, seed);
                            }
                            else
                                RpcFire(Quaternion.Euler(0, 0, mouseDirection), Character.transform.position, "d15", playerNum, seed);

                            cooldown = 2;
                            break;
                        }
                    case 6: // bad bitch
                    case 7: // rudy
                    case 8: // fern
                    case 18: // brad
                    case 28: // durrith
                        {
                            Quaternion mouseDirection = Quaternion.Euler(0, 0, Mathf.Atan2((mouseY - Character.transform.position.y), (mouseX - Character.transform.position.x)) * Mathf.Rad2Deg - 90);
                            if (!isServer)
                            {
                                CmdCureWounds(mouseDirection, Character.transform.position, "cheal0", playerNum, 0);
                                //CmdCureWounds(mouseDirection, Character.transform.position, "cheal0", playerNum, 0);
                            }
                            else
                                CmdCureWounds(mouseDirection, Character.transform.position, "cheal0", playerNum, 0);
                            cooldown = 2;
                            break;
                        }
                    case 29: // acoda
                        cooldown = 10;
                        foreach (GameObject e in GameObject.FindGameObjectsWithTag("player"))
                        {
                            if (!isServer)
                            {
                                CmdFire2(Vector2.zero, e.transform.position, "time2", playerNum, 0);
                                //Fire2(Vector2.zero, e.transform.position, "time2", playerNum, 0);
                            }
                            else
                                RpcFire2(Vector2.zero, e.transform.position, "time2", playerNum, 0);
                        }
                        break;
                }
                StartCoroutine(ResetShift(cooldown));
            }

            rb.MovePosition(rb.position + new Vector2(moveX, moveY) * stats[5 * chara + 1] * Time.fixedDeltaTime * 1.4f);
            if (!spawned && chara != -1 && HP != 100)
            {
                rb.MovePosition(new Vector2(Random.Range(-50, 50), Random.Range(-50, 50)));
                spawned = true;
            }

            bool flip = true;
            int[] flipChars = { 0, 1, 4, 5, 7, 8, 9, 11, 12, 15, 16, 19, 23, 24, 26, 27, 28, 29, 30, 31 };
            if (Array.Exists(flipChars, element => element == chara))
                flip = false;

            if ((Input.GetMouseButtonDown(0) || (Input.GetMouseButton(0) && stats[5 * chara + 4] == 1)) && canFire)
            {
                invisible = false;
                canFire = false;
                StartCoroutine(ResetFire());
                mouseDown = true;
                Quaternion mouseDirection = Quaternion.Euler(0, 0, Mathf.Atan2((mousePosition.y - Character.transform.position.y), (mousePosition.x - Character.transform.position.x)) * Mathf.Rad2Deg - 90);

                if (mouseDirection.eulerAngles.z > 180)
                    flipped = flip;
                else
                    flipped = !flip;

                if (chara == 17 || chara == 26)
                {
                    Vector3 targetPos;
                    if (Vector2.Distance(rb.position, new Vector2(mouseX, mouseY)) < 15)
                    {
                        targetPos = mousePosition;
                    } else
                    {
                        Debug.Log(mouseDirection.eulerAngles.z);
                        targetPos = transform.position + new Vector3(15 * Mathf.Cos(Mathf.Deg2Rad*(mouseDirection.eulerAngles.z+90)), 15 * Mathf.Sin(Mathf.Deg2Rad * (mouseDirection.eulerAngles.z+90)), 0);
                    }
                    int seed = Random.Range(-100, 100);
                    if (!isServer)
                    {
                        CmdFire3(targetPos, "b" + chara, playerNum, seed);
                        //Fire3(targetPos, "b" + chara, playerNum, seed);
                    }
                    else
                        RpcFire3(targetPos, "b" + chara, playerNum, seed);
                    if (chara == 17)
                        shockwave(targetPos.x, targetPos.y);
                    
                }
                else
                {
                    int seed = Random.Range(-100, 100);
                    if (!isServer)
                    {
                        CmdFire(mouseDirection, Character.transform.position, "b" + chara, playerNum, seed);
                        //Fire(mouseDirection, Character.transform.position, "b" + chara, playerNum, seed);
                    }
                    else
                        RpcFire(mouseDirection, Character.transform.position, "b" + chara, playerNum, seed);
                }
            }
            if (Input.GetMouseButtonUp(0))
                mouseDown = false;

            if ((Input.GetMouseButtonDown(1) || (Input.GetMouseButton(1) && stats[5 * chara + 4] == 1)) && canFire)
            {
                invisible = false;
                canFire = false;
                StartCoroutine(ResetFire(dmg2[2*chara+1]));
                mouseDown = true;
                if (chara == 17)
                {
                    Quaternion mouseDirection = Quaternion.Euler(0, 0, Mathf.Atan2((mousePosition.y - Character.transform.position.y), (mousePosition.x - Character.transform.position.x)) * Mathf.Rad2Deg - 90);
                    if (mouseDirection.eulerAngles.z > 180)
                        flipped = flip;
                    else
                        flipped = !flip;
                    Vector3 targetPos;
                    if (Vector2.Distance(rb.position, new Vector2(mouseX, mouseY)) < 15)
                    {
                        targetPos = mousePosition;
                    }
                    else
                    {
                        Debug.Log(mouseDirection.eulerAngles.z);
                        targetPos = transform.position + new Vector3(15 * Mathf.Cos(Mathf.Deg2Rad * (mouseDirection.eulerAngles.z + 90)), 15 * Mathf.Sin(Mathf.Deg2Rad * (mouseDirection.eulerAngles.z + 90)), 0);
                    }
                    int seed = Random.Range(-100, 100);
                    if (!isServer)
                    {
                        CmdFire3(targetPos, "c" + chara, playerNum, seed);
                        //Fire3(targetPos, "c" + chara, playerNum, seed);
                    }
                    else
                        RpcFire3(targetPos, "c" + chara, playerNum, seed);
                    if (chara == 17)
                        healwave(targetPos);
                }
                if (chara == 8)
                {
                    Vector3 targetPos;
                    Quaternion mouseDirection = Quaternion.Euler(0, 0, Mathf.Atan2((mousePosition.y - Character.transform.position.y), (mousePosition.x - Character.transform.position.x)) * Mathf.Rad2Deg - 90);
                    if (Vector2.Distance(rb.position, new Vector2(mouseX, mouseY)) < 15)
                    {
                        targetPos = mousePosition;
                    }
                    else
                    {
                        Debug.Log(mouseDirection.eulerAngles.z);
                        targetPos = transform.position + new Vector3(15 * Mathf.Cos(Mathf.Deg2Rad * (mouseDirection.eulerAngles.z + 90)), 15 * Mathf.Sin(Mathf.Deg2Rad * (mouseDirection.eulerAngles.z + 90)), 0);
                    }
                    int seed = Random.Range(-100, 100);
                    if (!isServer)
                    {
                        CmdFire3(targetPos, "c" + chara, playerNum, seed);
                        //Fire3(targetPos, "b" + chara, playerNum, seed);
                    }
                    else
                        RpcFire3(targetPos, "c" + chara, playerNum, seed);

                }
                else if (chara == 13)
                {
                    Quaternion mouseDirection = Quaternion.Euler(0, 0, Mathf.Atan2((mousePosition.y - Character.transform.position.y), (mousePosition.x - Character.transform.position.x)) * Mathf.Rad2Deg - 90);
                    if (!isServer)
                    {
                        CmdCureWounds(mouseDirection, Character.transform.position, "cheal0", playerNum, 0);
                        //CmdCureWounds(mouseDirection, Character.transform.position, "cheal0", playerNum, 0);
                    }
                    else
                        CmdCureWounds(mouseDirection, Character.transform.position, "cheal0", playerNum, 0);
                }
                else if (!(chara == 9 && moonbeam))
                {
                    int rep = 1;
                    if (chara == 6 || chara == 23)
                        rep = 2;
                    else if (chara == 26)
                        rep = 4;
                    else if (chara == 19)
                        rep = 7;
                    for (int i = 0; i < rep; i++)
                    {
                        Quaternion mouseDirection = Quaternion.Euler(0, 0, Mathf.Atan2((mousePosition.y - Character.transform.position.y), (mousePosition.x - Character.transform.position.x)) * Mathf.Rad2Deg - 90);
                        if (mouseDirection.eulerAngles.z > 180)
                            flipped = flip;
                        else
                            flipped = !flip;
                        int seed = Random.Range(-100, 100);
                        if (!isServer)
                        {
                            CmdFire(mouseDirection, Character.transform.position, "c" + chara, playerNum, seed);
                            //Fire(mouseDirection, Character.transform.position, "c" + chara, playerNum, seed);
                        }
                        else
                            RpcFire(mouseDirection, Character.transform.position, "c" + chara, playerNum, seed);
                    }
                }
                
            }
            if (Input.GetMouseButtonUp(1))
                mouseDown = false;

        }
    }

    void animate()
    {
        if (moveY != 0)
        {
            sR.sprite = Resources.Load("images/walk_up_" + i, typeof(Sprite)) as Sprite;
            i++;
            i %= 4;
        }
        else if (moveX != 0)
        {
            sR.sprite = Resources.Load("images/walk_side_" + i, typeof(Sprite)) as Sprite;
            i++;
            i %= 4;
        }
        else
            sR.sprite = Resources.Load("images/2D Rig", typeof(Sprite)) as Sprite;
    }

    void OnTriggerEnter2D (Collider2D other)
    {
        //Debug.Log("Hit");
        if (other.tag == "bullet")
        {
            if (other.GetComponent<bfly>().grace <= 0)
            {
                if (other.GetComponent<bfly>().ownerImmune && other.GetComponent<bfly>().owner == playerNum)
                    return;
                stun = Mathf.Max(other.GetComponent<bfly>().stun * 50, stun);
                slow = Mathf.Max(other.GetComponent<bfly>().slow * 50, slow);
                weakness = Mathf.Max(other.GetComponent<bfly>().weakness * 50, weakness);
                sleep = Mathf.Max(other.GetComponent<bfly>().sleep * 50, sleep);
                glowing = Mathf.Max(other.GetComponent<bfly>().glowing * 50, glowing);
                marked = Mathf.Max(other.GetComponent<bfly>().marked * 50, marked);
                if (other.GetComponent<bfly>().reveal)
                    invisible = false;
                frozen = Mathf.Max(other.GetComponent<bfly>().freeze * 50, frozen);
                if ((chara == 4 || chara == 31 || other.GetComponent<bfly>().melee) && other.GetComponent<bfly>().owner == playerNum)
                    return;
                HP -= other.GetComponent<bfly>().damage * (marked > 0 ? 1.1f : 0.7f);
                HP = Mathf.Min(HP, (int)stats[5*chara]);
                drift += other.GetComponent<bfly>().getNetVel() * other.GetComponent<bfly>().knockback;
                poison = Mathf.Max(other.GetComponent<bfly>().poison*50, poison);
                if (other.GetComponent<bfly>().damage < 0)
                {
                    r = 0;
                    b = 0;
                }
                else if (other.GetComponent<bfly>().damage > 0)
                {
                    g = 0;
                    b = 0;
                    if (raging > 0)
                        HP += other.GetComponent<bfly>().damage*0.7f / 2f + 0.4f;
                    if (other.GetComponent<bfly>().sleep == 0)
                        sleep = 0;
                }
                if (unicycle && chara == 3 && Random.Range(0,3) == 1)
                {
                    unicycle = false;
                    CmdUnicycleSync(unicycle);
                    int seed = Random.Range(-100, 100);
                    if (chara == 3)
                    {
                        if (!isServer)
                        {
                            CmdFire2(new Vector2(moveX, moveY), transform.position - transform.up * 2, "uniP", playerNum, seed);
                            //Fire2(new Vector2(moveX, moveY), transform.position - transform.up * 2, "uniP", playerNum, seed);
                        }
                        else
                            RpcFire2(new Vector2(moveX, moveY), transform.position - transform.up * 2, "uniP", playerNum, seed);
                    }
                    GameObject.Destroy(spawnedUnicycle);
                    spawnedUnicycle = null;
                }
                
                if (!other.GetComponent<bfly>().noDie)
                    GameObject.Destroy(other.gameObject);
            }
            
        } else if (other.tag == "unicycle")
        {
            if (other.GetComponent<unicycle>().damage != 0 && other.GetComponent<unicycle>().owner != playerNum)
            {
                HP -= other.GetComponent<unicycle>().damage * 0.7f;
                if (raging > 0)
                    HP += other.GetComponent<unicycle>().damage * 0.7f / 2f + 0.4f;
                g = 0;
                b = 0;
                if (unicycle && Random.Range(0, 3) == 1)
                {
                    unicycle = false;
                    CmdUnicycleSync(unicycle);
                    int seed = Random.Range(-100, 100);
                    if (!isServer)
                    {
                        CmdFire2(new Vector2(moveX, moveY), transform.position, "uniP", playerNum, seed);
                        //Fire2(new Vector2(moveX, moveY), transform.position, "uniP", playerNum, seed);
                    }
                    else
                        RpcFire2(new Vector2(moveX, moveY), transform.position, "uniP", playerNum, seed);
                    GameObject.Destroy(spawnedUnicycle);
                    spawnedUnicycle = null;
                }
                if (!other.GetComponent<bfly>().noDie)
                    GameObject.Destroy(other.gameObject);
            }
        }
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.collider.tag == "wall")
        {
            if (unicycle)
            {
                if (chara == 13)
                {
                    canSpace = false;
                    StartCoroutine(ResetSpace(4));
                }
                GameObject.Destroy(spawnedUnicycle.gameObject);
                spawnedUnicycle = null;
                unicycle = false;
                CmdUnicycleSync(unicycle);
                col.collider.gameObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
                col.collider.gameObject.GetComponent<Rigidbody2D>().drag = 0.5f;
                col.collider.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(3f * moveX, 3f * moveY);
            }
        }
    }

    IEnumerator SetHealth()
    {
        yield return new WaitForSecondsRealtime(0.6f);
        HP = (int)stats[5 * chara];
    }

    IEnumerator ResetFire(float delay = -1)
    {
        if (chara == 2)
        {
            Vector3 mousePosition = Cam.GetComponent<Camera>().ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Input.mousePosition.z - Cam.GetComponent<Camera>().transform.position.z));
            Quaternion mouseDirection = Quaternion.Euler(0, 0, Mathf.Atan2((mousePosition.y - transform.position.y), (mousePosition.x - transform.position.x)) * Mathf.Rad2Deg - 90);
            yield return new WaitForSecondsRealtime(0.2f * (slow > 0 ? 2 : 1));
            int seed = Random.Range(-100, 100);
            if (!isServer)
            {
                CmdFire(mouseDirection, Character.transform.position, "b" + chara, playerNum, seed);
                //Fire(mouseDirection, Character.transform.position, "b" + chara, playerNum, seed);
            }
            else
                RpcFire(mouseDirection, Character.transform.position, "b" + chara, playerNum, seed);
            yield return new WaitForSecondsRealtime(0.2f * (slow > 0 ? 2 : 1));
            seed = Random.Range(-100, 100);
            if (!isServer)
            {
                CmdFire(mouseDirection, Character.transform.position, "b" + chara, playerNum, seed);
                //Fire(mouseDirection, Character.transform.position, "b" + chara, playerNum, seed);
            }
            else
                RpcFire(mouseDirection, Character.transform.position, "b" + chara, playerNum, seed);

        } else if (chara == 5)
        {
            Vector3 mousePosition = Cam.GetComponent<Camera>().ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Input.mousePosition.z - Cam.GetComponent<Camera>().transform.position.z));
            Quaternion mouseDirection = Quaternion.Euler(0, 0, Mathf.Atan2((mousePosition.y - transform.position.y), (mousePosition.x - transform.position.x)) * Mathf.Rad2Deg - 90);
            int seed = Random.Range(-100, 100);
            for (int i = 0; i < 5; i++)
            {
                if (!isServer)
                {
                    CmdFire(mouseDirection, Character.transform.position, "b" + chara, playerNum, seed);
                    //Fire(mouseDirection, Character.transform.position, "b" + chara, playerNum, seed);
                }
                else
                    RpcFire(mouseDirection, Character.transform.position, "b" + chara, playerNum, seed);
            }
        }
        if (delay == -1)
            yield return new WaitForSecondsRealtime(stats[5 * chara + 3] * (slow > 0 ? 2 : 1));
        else
            yield return new WaitForSecondsRealtime(delay * (slow > 0 ? 2 : 1));
        canFire = true;
    }

    IEnumerator Die()
    {
        yield return new WaitForSeconds(5);
        this.gameObject.GetComponent<CapsuleCollider2D>().isTrigger = true;
        charSelector = Instantiate(Resources.Load("CharSelect"), this.transform.position, transform.rotation) as GameObject;
    }

    IEnumerator ResetShift(float delay = 0f)
    {
        yield return new WaitForSeconds(delay * (slow > 0 ? 2 : 1));
        if (chara == 16)
        {
            invisible = false;
            yield return new WaitForSeconds(delay * (slow > 0 ? 2 : 1));
        }
        canShift = true;
    }

    IEnumerator ResetSpace(float delay = 0f)
    {
        yield return new WaitForSeconds(delay * (slow > 0 ? 2 : 1));

        if (chara == 0 || chara == 16 || chara == 14 || chara == 20)
        {
            if (chara == 14 || chara == 20)
                yield return new WaitForSeconds(0.3f * (slow > 0 ? 2 : 1));
            yield return new WaitForSeconds(0.2f * (slow > 0 ? 2 : 1));
            stats[5 * chara + 1] /= 4;
            yield return new WaitForSeconds(0.3f * (slow > 0 ? 2 : 1));
        }
        else if (chara == 2)
        {
            //Vector3 mousePosition = Cam.GetComponent<Camera>().ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Input.mousePosition.z - Cam.GetComponent<Camera>().transform.position.z));
            //Quaternion mouseDirection = Quaternion.Euler(0, 0, Mathf.Atan2((moveY), (moveX)) * Mathf.Rad2Deg + 90);
            Quaternion mouseDirection = Quaternion.Euler(0, 0, 0);
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    int seed = Random.Range(-100, 100);
                    if (!isServer)
                    {
                        CmdFire(mouseDirection, Character.transform.position + new Vector3(Random.Range(-5, 5)/5f, Random.Range(-1, 1), Random.Range(-1, 1)), "y" + i, playerNum, seed);
                        //Fire(mouseDirection, Character.transform.position + new Vector3(Random.Range(-5, 5) / 5f, Random.Range(-1, 1), Random.Range(-1, 1)), "y" + i, playerNum, seed);
                    }
                    else
                        RpcFire(mouseDirection, Character.transform.position + new Vector3(Random.Range(-5, 5) / 5f, Random.Range(-1, 1), Random.Range(-1, 1)), "y" + i, playerNum, seed);
                    yield return new WaitForSecondsRealtime(0.07f * (slow > 0 ? 2 : 1));
                }
            }
            yield return new WaitForSeconds(0.2f * (slow > 0 ? 2 : 1));
            stats[5 * chara + 1] /= 3;
            yield return new WaitForSeconds(1.2f * (slow > 0 ? 2 : 1));
        } else if (chara == 15)
        {
            stats[5 * chara + 1] /= 2;
            stun = Mathf.Max(30, stun);
            slow = Mathf.Max(100, stun);
            yield return new WaitForSeconds(delay * (slow > 0 ? 2 : 1));
        }

        canSpace = true;
    }

    public void addSparks(float x, float y)
    {
        Quaternion mouseDirection = Quaternion.Euler(0, 0, Random.Range(-180, 180));
        if (!isServer)
        {
            CmdSpark(mouseDirection, new Vector3(x, y, 0), "b" + chara, playerNum);
            //Spark(mouseDirection, new Vector3(x, y, 0), "b" + chara, playerNum);
        }
        else
            RpcSpark(mouseDirection, new Vector3(x, y, 0), "b" + chara, playerNum);
    }

    [Command]
    void CmdSpark(Quaternion mouseDirection, Vector3 pos, string name, int owner)
    {
        //Spark(mouseDirection, pos, name, owner);
        RpcSpark(mouseDirection, pos, name, owner);
    }

    [ClientRpc]
    void RpcSpark(Quaternion mouseDirection, Vector3 pos, string name, int owner)
    {
        Spark(mouseDirection, pos, name, owner);
    }

    void Spark(Quaternion mouseDirection, Vector3 pos, string name, int owner)
    {
        int m = Random.Range(1, 5);
        for (int i = 0; i < m; i++)
        {
            GameObject instantiatedProjectile = Instantiate(Resources.Load(name), pos, mouseDirection) as GameObject;
            if (name[0] != 'y')
            {
                instantiatedProjectile.GetComponent<bfly>().xVel = moveX * stats[5 * chara + 1];
                instantiatedProjectile.GetComponent<bfly>().yVel = moveY * stats[5 * chara + 1];
                instantiatedProjectile.GetComponent<bfly>().damage = (int)stats[5 * chara + 2] / (weakness > 0 ? 2 : 1) + (raging > 0 ? 3 : 0);
            }
            instantiatedProjectile.transform.Rotate(0, 0, Random.Range(-instantiatedProjectile.GetComponent<bfly>().randomness, instantiatedProjectile.GetComponent<bfly>().randomness));
            instantiatedProjectile.GetComponent<bfly>().owner = owner;
            instantiatedProjectile.GetComponent<bfly>().dupli = true;
        }
    }

    IEnumerator WildMagic()
    {
        yield return new WaitForSeconds(0);
        bool init = false;
        int effect = Random.Range(0, 9);
        while (raging > 0)
        {
            switch (effect)
            {
                case 0: // lightning
                    {
                        Quaternion mouseDirection = Quaternion.Euler(0, 0, Mathf.Atan2((mouseY - Character.transform.position.y), (mouseX - Character.transform.position.x)) * Mathf.Rad2Deg - 90);
                        int seed = Random.Range(-100, 100);
                        if (!isServer)
                        {
                            CmdFire(mouseDirection, Character.transform.position, "b31", playerNum, seed);
                            //Fire(mouseDirection, Character.transform.position, "b31", playerNum, seed);
                        }
                        else
                            RpcFire(mouseDirection, Character.transform.position, "b31", playerNum, seed);
                        yield return new WaitForSeconds(0.2f * (slow > 0 ? 2 : 1));
                        break;
                    }
                case 1: // teleporting
                    {
                        Quaternion mouseDirection = Quaternion.Euler(0, 0, Mathf.Atan2((mouseY - Character.transform.position.y), (mouseX - Character.transform.position.x)) * Mathf.Rad2Deg - 90);
                        if (init == false || Input.GetKeyDown(KeyCode.LeftShift) || Random.Range(0, 80) == 0)
                        {
                            init = true;
                            shockwave(mouseX, mouseY);
                            drift.x = (mouseX - transform.position.x) * 2.3f;
                            drift.y = (mouseY - transform.position.y) * 2.3f;
                            yield return new WaitForSeconds(0.2f * (slow > 0 ? 2 : 1));
                        }
                        yield return new WaitForSeconds(0.001f);
                        break;
                    }
                case 2: // explosions
                    {
                        //Quaternion mouseDirection = Quaternion.Euler(0, 0, Mathf.Atan2((mouseY - Character.transform.position.y), (mouseX - Character.transform.position.x)) * Mathf.Rad2Deg - 90);
                        explode(transform.position.x, transform.position.y);
                        yield return new WaitForSeconds(0.7f * (slow > 0 ? 2 : 1));
                        break;
                    }
                case 3: // speed
                    {
                        if (!init && raging > 200)
                        {
                            init = true;
                            stats[5 * chara + 1] *= 2;
                        } else if (init && raging < 60)
                        {
                            init = false;
                            stats[5 * chara + 1] /= 2;
                        }
                        yield return new WaitForSeconds(0.5f * (slow > 0 ? 2 : 1));
                        break;
                    }
                case 4: // fire
                    {
                        //Quaternion mouseDirection = Quaternion.Euler(0, 0, Mathf.Atan2((mouseY - Character.transform.position.y), (mouseX - Character.transform.position.x)) * Mathf.Rad2Deg - 90);
                        firewave(transform.position.x, transform.position.y);
                        yield return new WaitForSeconds(0.25f * (slow > 0 ? 2 : 1));
                        break;
                    }
                case 5: // magic missiles
                    {
                        Quaternion mouseDirection = Quaternion.Euler(0, 0, Mathf.Atan2((mouseY - Character.transform.position.y), (mouseX - Character.transform.position.x)) * Mathf.Rad2Deg - 90);
                        int seed = Random.Range(-100, 100);
                        if (!isServer)
                        {
                            CmdFire(mouseDirection, Character.transform.position, "c23", playerNum, seed);
                            //Fire(mouseDirection, Character.transform.position, "c23", playerNum, seed);
                        }
                        else
                            RpcFire(mouseDirection, Character.transform.position, "c23", playerNum, seed);
                        yield return new WaitForSeconds(0.3f * (slow > 0 ? 2 : 1));
                        break;
                    }
                case 6: // healing
                    if (HP < stats[5 * chara])
                        HP++;
                    yield return new WaitForSeconds(0.5f * (slow > 0 ? 2 : 1));
                    break;
                case 7: // shield
                    {
                        
                        Quaternion mouseDirection = Quaternion.Euler(0, 0, 0);
                        if (!isServer)
                        {
                            CmdFire2(Vector2.zero, Character.transform.position, "shield1", playerNum, 0);
                            //Fire2(Vector2.zero, Character.transform.position, "shield1", playerNum, 0);
                        }
                        else
                            RpcFire2(Vector2.zero, Character.transform.position, "shield1", playerNum, 0);
                        yield return new WaitForSeconds(1.6f * (slow > 0 ? 2 : 1));
                        break;
                    }
                case 8: // empty
                    yield return new WaitForSeconds(0.5f);
                    break;
            }
        }
    }
}
