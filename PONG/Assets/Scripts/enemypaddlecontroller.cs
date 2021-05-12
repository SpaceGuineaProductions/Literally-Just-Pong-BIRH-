using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemypaddlecontroller : MonoBehaviour
{
    public AudioSource ballhit;

    public float ms = 15;
    public float targety;

    public GameObject ball;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        ball = GameObject.FindGameObjectWithTag("Ball");
        targety = ball.transform.position.y;

        if (transform.position.y < targety)
        {
            transform.Translate(transform.up * ms * Time.deltaTime);
        }
        if (transform.position.y > targety)
        {
            transform.Translate(-transform.up * ms * Time.deltaTime);
        }
    }

    public void asspeed(bool b)
    {
        if (b)
        {
            ms += 1;
        }
        else
        {
            ms -= 1;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Ball")
        {
            float yvel = Random.Range(-20, 20);
            collision.GetComponent<Rigidbody2D>().velocity = new Vector2(collision.GetComponent<Rigidbody2D>().velocity.x, yvel);
            ballhit.Play();
        }
    }
}
