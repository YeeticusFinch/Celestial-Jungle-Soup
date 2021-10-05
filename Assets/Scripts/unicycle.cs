using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class unicycle : MonoBehaviour
{

    int c = 0;
    float vx = 0, vy = 0;
    public float damage = 0;
    public int owner = -1;
    public int id = 3;
    float ox;
    float oy;

    // Use this for initialization
    void Start()
    {

    }

    public void yeet(float moveX, float moveY, int o)
    {
        vx = moveX;
        vy = moveY;
        c = 0;
        damage = 10 * Mathf.Sqrt(moveX * moveX + moveY * moveY);
        owner = o;
    }

    void FixedUpdate()
    {
        if (id == 3)
            GetComponent<SpriteRenderer>().sprite = Resources.Load("images/uc" + c % 4, typeof(Sprite)) as Sprite;
        else if (id == 12)
        {
            GetComponent<SpriteRenderer>().flipX = transform.position.x < ox;
            GetComponent<SpriteRenderer>().sprite = Resources.Load("images/donkey" + (c / 5) % 6, typeof(Sprite)) as Sprite;
        }
        //transform.position += new Vector3(vx, vy, 0) * Time.fixedDeltaTime * 6;
        c++;
        vx = (ox - transform.position.x) / Time.fixedDeltaTime;
        vy = (oy - transform.position.y) / Time.fixedDeltaTime;
        ox = transform.position.x;
        oy = transform.position.y;
        //if ((vx != 0 || vy != 0) && c > 50)
        //    GameObject.Destroy(this.gameObject);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "wall" && other.gameObject.GetComponent<Rigidbody2D>().bodyType != RigidbodyType2D.Dynamic)
        {
                GameObject.Destroy(this.gameObject);
                other.gameObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
                other.gameObject.GetComponent<Rigidbody2D>().drag = 0.5f;
                other.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0.2f * vx, 0.2f * vy);
        }
    }
}
