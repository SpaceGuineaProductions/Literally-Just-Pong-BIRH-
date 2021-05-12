using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheatingEnemyPaddle : MonoBehaviour
{
    public AudioSource ballhit;

    public float ms = 5f;
    public float hms = 5f;
    public float targety;
    public float targetx;

    public float wmin = 0f;
    public float bwmin = 0f;
    public float c2min = 0f;

    public bool canmake = true;
    public bool bcanmake = true;
    public bool cantp = false;
    public bool xlocked = true;

    public GameObject ball;
    public GameObject gm;
    public GameObject wall;
    public GameObject BIGWALL;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        ball = GameObject.FindGameObjectWithTag("Ball");
        targety = ball.transform.position.y;

        switch (gm.GetComponent<GameManager>().playerscore)
        {
            case 3:
                ms = 8f;
                break;
            case 5:
                ms = 12f;
                break;
            case 20:
                xlocked = false;
                break;
        }

        if (!xlocked)
        {
            targetx = ball.transform.position.x;

            if(targetx < transform.position.x && transform.position.x > 2f)
            {
                transform.Translate(-transform.right * hms * Time.deltaTime);
            }
            if (targetx > transform.position.x && transform.position.x < 7.95f)
            {
                transform.Translate(transform.right * hms * Time.deltaTime);
            }
            if (gm.GetComponent<GameManager>().goalgot)
            {
                targetx = 7.95f;
            }
        }

        if (gm.GetComponent<GameManager>().playerscore >= 7)
        {
            CHEAT1();
        }

        if (gm.GetComponent<GameManager>().playerscore >= 10)
        {
            CHEAT2();
        }

        if (gm.GetComponent<GameManager>().playerscore >= 15)
        {
            CHEAT3();
        }

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

    void CHEAT1()
    {
        if (canmake)
        {
            Instantiate(wall, new Vector3(transform.position.x, Random.Range(-4.20f, 4.20f), 0f), Quaternion.Euler(0f, 0f, 0f));
            Instantiate(wall, new Vector3(transform.position.x, Random.Range(-4.20f, 4.20f), 0f), Quaternion.Euler(0f, 0f, 0f));
            canmake = false;
        }
        else
        {
            if (wmin < 5f)
            {
                wmin += 1f * Time.deltaTime;
            }
            if (wmin >= 5f)
            {
                wmin = 0.0f;
                canmake = true;
            }
        }
    }

    void CHEAT2()
    {
        if (cantp)
        {
            transform.position = new Vector3(transform.position.x, targety, transform.position.z);
            cantp = false;
        }
        else
        {
            if (c2min < 5f)
            {
                c2min += 1f * Time.deltaTime;
            }
            if (c2min >= 5f)
            {
                c2min = 0f;
                cantp = true;
            }
        }
    }

    void CHEAT3()
    {
        if (bcanmake)
        {
            Instantiate(BIGWALL, new Vector3(ball.transform.position.x, 0f, 0f), Quaternion.Euler(0f, 0f, 0f));
            bcanmake = false;
        }
        else
        {
            if (bwmin < 10f)
            {
                bwmin += 1f * Time.deltaTime;
            }
            if (bwmin >= 10f)
            {
                bwmin = 0.0f;
                bcanmake = true;
            }
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