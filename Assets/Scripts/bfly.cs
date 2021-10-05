using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Random = UnityEngine.Random;

public class bfly : NetworkBehaviour {

    public int speed;
    public int damage;
    public float xVel;
    public float yVel;
    public int seed;
    public int lifetime;
    public Rigidbody2D rb;
    public int randomness = 0;
    public int owner;
    public bool explosive = false;
    public int speedRand = 0;
    public int lifeRand = 0;
    public int id = 0;
    bool first = true;
    public float offset = 1;
    public bool penetration = false;
    public bool destruction = false;
    public bool detach = false;
    public bool dupli = false;
    public bool trail = false;
    public int trailLength = 10;
    public string trailGameObject;
    public bool shrinking = false;
    public bool fading = false;
    public bool melee = false;
    public int meleeDegrees = 60;
    public bool noDmg = false;
    public bool noDie = false;
    public bool swordHider = false;
    public bool particle = false;
    public GameObject owner_obj = null;
    public bool dead = false;
    public float knockback = 0;
    public int poison = 0;
    public bool homing = false;
    public bool ownerImmune = false;
    public int slow = 0;
    public int stun = 0;
    public int weakness = 0;
    public int freeze = 0;
    public int sleep = 0;
    public int glowing = 0;
    public int marked = 0;
    public bool reveal = false;
    bool invulImmune = false;
    public bool stat = false;
    Vector3 og_scale;

    GameObject[] trails;
    int trailIndex = 0;

    public int grace = 3;

    public int c = 0;

    // Use this for initialization
    void Start () {
        //rb.MovePosition(rb.position + (Vector2)transform.up * 50);
        if (melee)
            this.transform.Rotate(new Vector3(0, 0, meleeDegrees / 2));
        this.transform.Translate(new Vector3(0, 1, 0) * offset);
        if (!noDie && swordHider && !particle)
            owner_obj.GetComponent<PlayerStuff>().hideSwordCooldown = lifetime;
        trails = new GameObject[trailLength];
        og_scale = this.transform.localScale;
        if (id == 621 || id == 111)
            this.transform.localScale *= 2f;
        else if (id == 222)
            this.transform.localScale *= 18f;
        if (owner_obj.GetComponent<PlayerStuff>().slow > 0)
        {
            speed /= 2;
            lifetime *= 2;
        }
        if (id == -9)
        {
            owner_obj.GetComponent<PlayerStuff>().moonbeam = true;
            rb.MovePosition(new Vector2(owner_obj.GetComponent<PlayerStuff>().mouseX, owner_obj.GetComponent<PlayerStuff>().mouseY));
            rb.MoveRotation(0);
        }

        /*if (id == 31)
        {
            rb.velocity = (Vector2)transform.up * speed + new Vector2(xVel, yVel);
        }*/
    }
	
	// Update is called once per frame
	void Update () {
        
    }

    public Vector2 getNetVel()
    {
        if (melee)
            return (Vector2)transform.right * speed + new Vector2(xVel, yVel);
        else
            return (Vector2)transform.up * speed + new Vector2(xVel, yVel);
    }

    float FancyMin(float a, float b)
    {
        if (Mathf.Abs(a) < Mathf.Abs(b))
            return a;
        return b;
    }

    int sc = 0;
    void FixedUpdate()
    {
        if (owner_obj.GetComponent<PlayerStuff>().frozen > 0)
            return;
        if (!stat)
        {
            switch (owner_obj.GetComponent<PlayerStuff>().crimsonrite)
            {
                case 0:
                    break;
                case 1:
                    if (!trail)
                    {
                        damage = (int)(damage*1.5f);
                    }
                    trail = true;
                    trailGameObject = "fire0";
                    break;
                case 2:
                    if (!trail)
                    {
                        damage = (int)(damage * 1.5f);
                        weakness = 1;
                    }
                    trail = true;
                    trailGameObject = "spark2";
                    break;
                case 3:
                    if (!trail)
                    {
                        damage = (int)(damage * 1.4f);
                        reveal = true;
                        glowing = 2;
                    }
                    trail = true;
                    trailGameObject = "spark0";
                    break;
            }
        }
        if (c == 0 && lifeRand > 0)
            c = Mathf.Abs(seed) * lifeRand / 100;
        if (speedRand != 0)
        {
            speed += Random.Range(-speedRand, speedRand);
            speedRand = 0;
        }

        if (id == 4)
        {
            this.GetComponent<SpriteRenderer>().sprite = Resources.Load("images/s" + Random.Range(0, 7), typeof(Sprite)) as Sprite;
        } else if (id == 5)
        {
            id = -1;
            this.GetComponent<SpriteRenderer>().sprite = Resources.Load("images/cp" + Random.Range(0, 18), typeof(Sprite)) as Sprite;
        } else if (id == 6)
        {
            id = -1;
            this.GetComponent<SpriteRenderer>().sprite = Resources.Load("images/vm" + (int)(0.01f*Mathf.Abs(seed)*7), typeof(Sprite)) as Sprite;
        } else if (id == 17 && c == 0)
        {
            this.GetComponent<SpriteRenderer>().sprite = Resources.Load("images/lightning" + Random.Range(0, 4), typeof(Sprite)) as Sprite;
        }
        else if (id == -31)
        {
            this.GetComponent<SpriteRenderer>().sprite = Resources.Load("images/bolt" + Random.Range(0, 7), typeof(Sprite)) as Sprite;
        } else if (id == -8)
        {
            this.GetComponent<SpriteRenderer>().sprite = Resources.Load("images/flame" + (c/2)%7, typeof(Sprite)) as Sprite;
        } else if (id == 80)
        {
            this.GetComponent<SpriteRenderer>().sprite = Resources.Load("images/wind" + (c / 2) % 3, typeof(Sprite)) as Sprite;
        }
        if (fading)
        {
            this.GetComponent<SpriteRenderer>().color = new Color32(255, 255, 255, (byte)(255*((float)(lifetime-c)/lifetime)));
        }

        if (homing)
        {
            float mouseDir = Mathf.Atan2((owner_obj.GetComponent<PlayerStuff>().mouseY - rb.position.y), (owner_obj.GetComponent<PlayerStuff>().mouseX - rb.position.x)) * Mathf.Rad2Deg - 90;
           
            if (id != 90 && Vector2.Distance(new Vector2(owner_obj.GetComponent<PlayerStuff>().mouseX, owner_obj.GetComponent<PlayerStuff>().mouseY), rb.position) < speed * Time.fixedDeltaTime)
                dead = true;
            if (Mathf.Abs(FancyMin(mouseDir - rb.rotation, 360 + mouseDir - rb.rotation)) > 5)
                rb.MoveRotation(rb.rotation + 8 * FancyMin(mouseDir - rb.rotation, 360 + mouseDir - rb.rotation) / Mathf.Abs(FancyMin(mouseDir - rb.rotation, 360 + mouseDir - rb.rotation)));
        }

        //Debug.Log("Flying");
        //rb.transform.position += transform.up * speed * Time.fixedDeltaTime;
        if (melee)
        {
            if (id == 112)
                rb.MovePosition(owner_obj.transform.position);
            else
                rb.MovePosition(rb.position + (Vector2)transform.right * speed * Time.fixedDeltaTime + new Vector2(xVel, yVel) * Time.fixedDeltaTime);
            rb.MoveRotation(rb.rotation - (float)meleeDegrees / lifetime);
        }
        else if (id == 621)
        {
            float mouseDir = Mathf.Atan2((owner_obj.transform.position.y - owner_obj.GetComponent<PlayerStuff>().mouseY), (owner_obj.transform.position.x - owner_obj.GetComponent<PlayerStuff>().mouseX)) * Mathf.Rad2Deg + 180;
            this.GetComponent<SpriteRenderer>().sprite = Resources.Load("images/shield/" + sc / 3, typeof(Sprite)) as Sprite;
            sc++;
            sc %= 42;
            rb.MoveRotation(mouseDir);
            rb.MovePosition(owner_obj.transform.position + 1.5f * transform.right + 0.5f*owner_obj.transform.up);
        } else if (id == -9)
        {
            rb.MovePosition(rb.position + speed * Time.fixedDeltaTime * (new Vector2(owner_obj.GetComponent<PlayerStuff>().mouseX - rb.position.x, offset + owner_obj.GetComponent<PlayerStuff>().mouseY - rb.position.y)));
        }
        else
            rb.MovePosition(rb.position + (Vector2)transform.up * speed * Time.fixedDeltaTime + new Vector2(xVel, yVel) * Time.fixedDeltaTime);
        if (trail && !particle)
        {
            if (id == 8)
            {
                if (noDie)
                    do_trail(rb.position + (Vector2)transform.up * Random.Range(-7, 15) / 10f + new Vector2(xVel, yVel) * Time.fixedDeltaTime, new Vector2(xVel, yVel), trailGameObject);
                else
                    for (int i = 0; i < 3; i++) do_trail(rb.position + (Vector2)transform.up*Random.Range(-7, 15)/10f + new Vector2(xVel, yVel) * Time.fixedDeltaTime, new Vector2(xVel, yVel), trailGameObject);
            } else if (id == 11)
               do_trail(rb.position + (Vector2)transform.up*1.5f + new Vector2(xVel, yVel) * Time.fixedDeltaTime, new Vector2(xVel, yVel), trailGameObject);
            else
                do_trail(rb.position + new Vector2(xVel, yVel) * Time.fixedDeltaTime, new Vector2(xVel, yVel), trailGameObject);
        }
        if (shrinking)
        {
            this.transform.localScale = new Vector3(this.transform.localScale.x * (lifetime-c-1f) / System.Math.Max(lifetime-c, 1), this.transform.localScale.y * (lifetime - c - 1f) / System.Math.Max(lifetime - c, 1), this.transform.localScale.z * (lifetime - c - 1f) / System.Math.Max(lifetime - c, 1));
        } else if (id == 29)
        {
            this.transform.localScale = new Vector3(this.transform.localScale.x * (lifetime - c - 1f) / System.Math.Max(lifetime - c, 1), this.transform.localScale.y, this.transform.localScale.z * (lifetime - c - 1f) / System.Math.Max(lifetime - c, 1));
        } else if (id == 30 && c == 0)
            do_trail(rb.position + new Vector2(xVel, yVel) * Time.fixedDeltaTime - 0.5f * (Vector2)transform.up * speed * Time.fixedDeltaTime, new Vector2(xVel + Random.Range(-10, 10) * 0.05f, yVel + Random.Range(-10, 10) * 0.05f), trailGameObject);
        else if ((id == -14 && c > 2) || (id == 31 && c > 7))
        {
            if (trailIndex == 0)
            {
                trailIndex = 1;
                trails[0] = owner_obj.GetComponent<PlayerStuff>().FireParticle2(owner_obj.GetComponent<PlayerStuff>().Character.transform.position, new Vector2(xVel, yVel) * Time.fixedDeltaTime, Quaternion.Euler(0, 0, rb.rotation), trailGameObject, owner_obj, true, false);
                if (owner_obj.GetComponent<PlayerStuff>().chara == 13)
                    trails[0].GetComponent<bfly>().damage = 3;
                else
                    trails[0].GetComponent<bfly>().damage = damage;
            }
            trails[0].GetComponent<bfly>().transform.rotation = Quaternion.Euler(0, 0, Mathf.Atan2((rb.position.y - owner_obj.GetComponent<PlayerStuff>().Character.transform.position.y), (rb.position.x - owner_obj.GetComponent<PlayerStuff>().Character.transform.position.x)) * Mathf.Rad2Deg - 90);
            float dist = Vector2.Distance(owner_obj.GetComponent<PlayerStuff>().Character.transform.position, rb.position);
            trails[0].GetComponent<bfly>().transform.position = owner_obj.GetComponent<PlayerStuff>().Character.transform.position + (0.5f * dist + 0.5f) * trails[0].GetComponent<bfly>().transform.up;
            trails[0].GetComponent<bfly>().transform.localScale = new Vector3(Random.Range(10, 40)*0.1f, 0.2f * dist, 1);
            if (c > 10)
            {
                xVel = 0;
                yVel = 0;
                speed = 0;
            }
            if (trails[0].GetComponent<bfly>().dead || dead)
            {
                Debug.Log('a');
                GameObject.Destroy(trails[0].GetComponent<bfly>().gameObject);
                GameObject.Destroy(this.gameObject);
            }
        }

        if (c < lifetime)
            c++;
        if (grace > 0)
            grace--;

        if (c >= lifetime)
            dead = true;
        if (dead && id == 26)
            owner_obj.GetComponent<PlayerStuff>().firewave(rb.position.x, rb.position.y);
        if (dead && (id == 621 || id == 111 || id == -9 || id == 90 || id == 112 || id == 60 || id == -8))
            GameObject.Destroy(this.gameObject);
        if (dead && !noDie)
            GameObject.Destroy(this.gameObject);
    }

    public void do_trail(Vector2 pos, Vector2 vel, String GameObjectName)
    {
        if (trailIndex < trailLength)
        {
            trails[trailIndex] = owner_obj.GetComponent<PlayerStuff>().FireParticle2(pos, vel, Quaternion.Euler(0, 0, Random.Range(0, 360)), GameObjectName, owner_obj, true);
            trailIndex++;
        }
        for (int i = 0; i < trailIndex; i++)
        {
            if (trails[i].GetComponent<bfly>().dead)
            {
                trails[i].GetComponent<bfly>().reset(pos, vel);
            }
        }
    }

    public void reset(Vector2 pos, Vector2 vel)
    {
        c = 0;
        dead = false;
        this.transform.localScale = og_scale;
        this.transform.position = pos;
        xVel = vel.x;
        yVel = vel.y;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        //Debug.Log("Hit");
        if (other.tag == "wall")
        {
            if (id == -14 || id == 31)
            {
                rb.MovePosition(rb.position - (Vector2)transform.up * speed * Time.fixedDeltaTime - new Vector2(xVel, yVel) * Time.fixedDeltaTime);
                xVel = 0;
                yVel = 0;
                speed = 0;
            }
            if (!penetration)
                GameObject.Destroy(this.gameObject);
            if (destruction)
                GameObject.Destroy(other.gameObject);
            if (detach)
            {
                //GameObject.Destroy(other.gameObject);
                other.gameObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
                other.gameObject.GetComponent<Rigidbody2D>().drag = 0.5f;
                other.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(1.5f * speed * Mathf.Cos((90+rb.rotation) * Mathf.Deg2Rad), 1.5f * speed * Mathf.Sin((90+rb.rotation) * Mathf.Deg2Rad));
            }
            if (id == 4)
            {
                lifetime+=Random.Range(0,3);
                c = 0;
                if (!dupli)
                    owner_obj.GetComponent<PlayerStuff>().addSparks(transform.position.x, transform.position.y);
            }
            if (id == 1)
            {
                owner_obj.GetComponent<PlayerStuff>().explode(rb.position.x, rb.position.y);
            }

        } else if (other.tag == "floor")
        {
            if (destruction && Random.Range(0,3) == 1)
                GameObject.Destroy(other.gameObject);
            if (detach && Random.Range(0, 3) == 1)
            {
                other.gameObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
                other.gameObject.GetComponent<Rigidbody2D>().drag = 0.5f;
                other.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(1.5f * speed * Mathf.Cos((90 + rb.rotation) * Mathf.Deg2Rad), 1.5f * speed * Mathf.Sin((90 + rb.rotation) * Mathf.Deg2Rad));
                if (!isServer)
                    CmdDestroy(other.gameObject);
                else
                    RpcDestroy(other.gameObject);
               
            }
        } else if (other.tag == "shield" && !(melee && other.GetComponent<bfly>().owner == owner))
        {
            if (id == 29)
                GameObject.Destroy(other.gameObject);
            else if (id == -14 || id == 31)
            {
                rb.MovePosition(rb.position - (Vector2)transform.up * speed * Time.fixedDeltaTime - new Vector2(xVel, yVel) * Time.fixedDeltaTime);
                xVel = 0;
                yVel = 0;
                speed = 0;
            } else if (id != -31 && !noDie)
            {
                if (id == 1)
                {
                    owner_obj.GetComponent<PlayerStuff>().explode(rb.position.x, rb.position.y);
                }
                GameObject.Destroy(this.gameObject);
            }
        } else if (other.tag == "bullet" && other.GetComponent<bfly>().id == 112)
        {
            if (c < 1 + Mathf.Abs(seed) * lifeRand / 100)
                invulImmune = true;
            if (!invulImmune)
            {
                if (id == -14 || id == 31)
                {
                    rb.MovePosition(rb.position - (Vector2)transform.up * speed * Time.fixedDeltaTime - new Vector2(xVel, yVel) * Time.fixedDeltaTime);
                    xVel = 0;
                    yVel = 0;
                    speed = 0;
                }
                else if (id != -31)
                {
                    GameObject.Destroy(this.gameObject);
                }
            }
        }
    }

    [Command]
    void CmdDestroy(GameObject block)
    {
        StartCoroutine(KillDebris(block));
    }

    [ClientRpc]
    void RpcDestroy(GameObject block)
    {
        StartCoroutine(KillDebris(block));
    }

    IEnumerator KillDebris(GameObject block)
    {
        yield return new WaitForSecondsRealtime(1f);
        GameObject.Destroy(block);
    }

    private void OnDestroy()
    {
        if (id == -9)
            owner_obj.GetComponent<PlayerStuff>().moonbeam = false;
        if (trailIndex > 0)
            for (int i = 0; i < trailIndex; i++)
                GameObject.Destroy(trails[i]);
    }

}
