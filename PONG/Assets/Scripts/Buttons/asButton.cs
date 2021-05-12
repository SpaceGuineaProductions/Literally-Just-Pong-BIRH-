using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class asButton : MonoBehaviour
{
    public GameObject enemypaddle;

    public bool isAdding;
    public Button mybutton;

    // Start is called before the first frame update
    void Start()
    {
        mybutton.onClick.AddListener(addorsub);
    }

    // Update is called once per frame
    void Update()
    {

    }

    void addorsub()
    {
        if (isAdding)
        {
            enemypaddle.GetComponent<enemypaddlecontroller>().asspeed(isAdding);
        }
        else
        {
            enemypaddle.GetComponent<enemypaddlecontroller>().asspeed(isAdding);
        }
    }
}
