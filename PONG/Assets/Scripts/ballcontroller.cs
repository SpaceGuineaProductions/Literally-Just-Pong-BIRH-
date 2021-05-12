using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ballcontroller : MonoBehaviour
{
    public AudioClip hit;
    public AudioSource hitsound;

    public Camera cam;

    public Color color;

    public TextMeshProUGUI speedBOI;
    public Rigidbody2D rb;
    public Vector2 vel;

    public float maxspeed = 200f;
    
    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0f;
        rb.AddForce(-transform.right * 100f);
        float yvel = Random.Range(-50f, 50f);
        //rb.AddForce(transform.up * yvel);
        
    }

    // Update is called once per frame
    void Update()
    {
        cam.backgroundColor = Color.Lerp(cam.backgroundColor, Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f), .007f);

        vel = rb.velocity;

        if (transform.position.y > 5.5f)
        {
            transform.position = new Vector3(transform.position.x, 4.5f, transform.position.z);
        }

        if (transform.position.y < -5.5f)
        {
            transform.position = new Vector3(transform.position.x, -4.5f, transform.position.z);
        }

        if (rb.velocity.x > -5 && rb.velocity.x < 5)
        {
            Vector2 bvel = new Vector2(rb.velocity.x, rb.velocity.y);
            bvel.x *= 4.4f;
            rb.velocity = bvel;
        }

        if (rb.velocity.y > -5 && rb.velocity.y < 5)
        {
           // Vector2 bvel = new Vector2(rb.velocity.x, rb.velocity.y);
           // bvel.x *= 2.4f;
            //rb.velocity = bvel;
        }

        if (rb.velocity.x > 35)
        {
            rb.velocity = new Vector2(35, rb.velocity.y);
        }

        if (rb.velocity.x < -35)
        {
            rb.velocity = new Vector2(-35, rb.velocity.y);
        }

        if (rb.velocity.y < -25)
        {
            rb.velocity = new Vector2(rb.velocity.x, -25);
        }

        if (rb.velocity.y > 25)
        {
            rb.velocity = new Vector2(rb.velocity.x, 25);
        }
    }

    void OnTriggerEnter2D()
    {
        hitsound.Play();
    }
}
