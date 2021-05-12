using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class panningcambutton : MonoBehaviour
{
    public Button mybutt;

    public float movespeed;
    public float moveamount;
    float newposx;
    public float min = 0.0f;
    public float max = 1.5f;
    public float dmin = 0f;

    public bool right;
    public bool lerping = false;
    public bool set = false;

    public Camera cam;

    public Vector3 pos;

    void Awake()
    {
        lerping = false;
    }

// Start is called before the first frame update
    void Start()
    {
        pos = cam.transform.position;
        cam = Camera.main;
        mybutt.onClick.AddListener(pan);
        lerping = false;
        min = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        Time.timeScale = 1f;

        if (lerping)
        {
            //Debug.Log("<color=red>PANNING </color>");

            if (min < max)
            {
                min += 1f * Time.deltaTime;
            }
            if (min >= max)
            {
                min = 0f;
                lerping = false;
            }
            cam.transform.position = pos;
            if (!set)
            {
                if (right)
                {
                    newposx = pos.x + moveamount;
                }
                else
                {
                    newposx = pos.x - moveamount;
                }
                set = true;
            }

            if (right)
            {
                pos = Vector3.Lerp(pos, new Vector3(newposx, pos.y, pos.z), movespeed);
            }
            if (!right)
            {
                pos = Vector3.Lerp(pos, new Vector3(newposx, pos.y, pos.z), movespeed);
            }
        }

    }

    void pan()
    {
        Debug.Log("<color = red> PANNING </color>");
        pos = cam.transform.position;
        lerping = true;
    }
}
