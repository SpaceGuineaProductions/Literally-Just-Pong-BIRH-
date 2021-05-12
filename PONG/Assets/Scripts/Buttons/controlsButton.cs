using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class controlsButton : MonoBehaviour
{
    public Button mybutt;

    public string scene;

    // Start is called before the first frame update
    void Start()
    {
        mybutt.onClick.AddListener(dostuff);
    }

    void dostuff()
    {
        SceneManager.LoadScene(scene);
    }
}
