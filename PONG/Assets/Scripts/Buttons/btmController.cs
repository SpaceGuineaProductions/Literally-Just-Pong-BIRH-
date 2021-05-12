using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class btmController : MonoBehaviour
{

    public Button mybuttlol;

    // Start is called before the first frame update
    void Start()
    {
        mybuttlol.onClick.AddListener(btm);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void btm()
    {
        SceneManager.LoadScene("Main Menu");
    }
}
