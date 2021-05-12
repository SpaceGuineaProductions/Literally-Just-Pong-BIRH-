using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mmballController : MonoBehaviour
{
    public Camera cam;

    public Color color;

    public Rigidbody2D rb;
    public Vector2 vel;

    public float maxspeed = 200f;

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0f;
        rb.AddForce(-transform.right * 1000f);
        float yvel = Random.Range(-200f, 200f);
        //rb.AddForce(transform.up * yvel);

    }

    // Update is called once per frame
    void Update()
    {
        cam.backgroundColor = Color.Lerp(cam.backgroundColor, Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f), .007f);

        vel = rb.velocity;

        if (transform.position.y > 604)
        {
            transform.position = new Vector3(transform.position.x, 4.5f, transform.position.z);
        }

        if (transform.position.y < 451)
        {
            transform.position = new Vector3(transform.position.x, -4.5f, transform.position.z);
        }

        if (rb.velocity.x > -5 && rb.velocity.x < 5)
        {
            Vector2 bvel = new Vector2(rb.velocity.x, rb.velocity.y);
            bvel.x *= 40.4f;
            rb.velocity = bvel;
        }

        if (rb.velocity.y > -5 && rb.velocity.y < 5)
        {
            // Vector2 bvel = new Vector2(rb.velocity.x, rb.velocity.y);
            // bvel.x *= 2.4f;
            //rb.velocity = bvel;
        }

        if (rb.velocity.x > maxspeed)
        {
            rb.velocity = new Vector2(maxspeed, rb.velocity.y);
        }

        if (rb.velocity.x < -maxspeed)
        {
            rb.velocity = new Vector2(-maxspeed, rb.velocity.y);
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
}
