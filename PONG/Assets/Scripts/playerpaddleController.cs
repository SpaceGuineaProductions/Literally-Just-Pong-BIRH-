using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerpaddleController : MonoBehaviour
{
    public AudioSource ballhit;

    public float hi;
    public float vi;
    public float ms = 5f;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = new Vector3(transform.position.x, worldPosition.y, transform.position.z);

        vi = Input.GetAxis("Vertical");

        transform.Translate(Vector3.up * vi * ms * Time.deltaTime);
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
