using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mmpaddleController : MonoBehaviour
{
    public float ms = 15;
    public float targety;

    public GameObject ball;

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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Ball")
        {
            float yvel = Random.Range(-200, 200);
            collision.GetComponent<Rigidbody2D>().velocity = new Vector2(collision.GetComponent<Rigidbody2D>().velocity.x, yvel);
        }
    }
}
