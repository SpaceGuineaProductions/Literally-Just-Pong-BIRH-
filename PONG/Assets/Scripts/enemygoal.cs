using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemygoal : MonoBehaviour
{
    public AudioSource goal;

    public GameObject gamemanager;
    // Start is called before the first frame update
    void Start()
    {
        gamemanager = GameObject.FindGameObjectWithTag("gm");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Ball")
        {
            gamemanager.GetComponent<GameManager>().enemygoal();
            goal.Play();
        }
    }
}
