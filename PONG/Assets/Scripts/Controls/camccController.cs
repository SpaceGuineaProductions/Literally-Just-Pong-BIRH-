using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class camccController : MonoBehaviour
{
    public Camera cam;

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        cam.backgroundColor = Color.Lerp(cam.backgroundColor, Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f), .007f);
    }
}
