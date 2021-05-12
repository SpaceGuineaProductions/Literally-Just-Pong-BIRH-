using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
    public AudioSource sb;

    public Camera cam;

    public TMP_InputField botspeed;

    public TextMeshProUGUI playerscoret;
    public TextMeshProUGUI enemyscoret;
    public TextMeshProUGUI endingtext;
    public TextMeshProUGUI btmtxt;
    public TextMeshProUGUI pbtmtxt;
    public TextMeshProUGUI presstobegin;

    public Button btm;
    public Button pbtm;

    public bool goalgot = false;
    public bool started = false;
    public bool paused = false;
    public bool canpause = true;

    public int times = 0;
    public int etimes = 0;

    public int playerscore = 0;
    public int enemyscore = 0;
    public int targetscore = 10;

    public float min = 0.0f;
    public float dur;

    public string winningoponent;

    public GameObject rball;
    public GameObject ball;
    public GameObject camshake;

    public GameObject enempad;

    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1f;
        cam = Camera.main;
        btm.onClick.AddListener(menu);
        endingtext.enabled = false;
        btm.GetComponent<Image>().enabled = false;
        btm.GetComponent<Button>().enabled = false;
        pbtm.GetComponent<Image>().enabled = false;
        pbtm.GetComponent<Button>().enabled = false;
        pbtmtxt.enabled = false;
        btmtxt.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!started)
        {
            Time.timeScale = 0f;
            presstobegin.enabled = true;
            canpause = false;

            //enabled
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                started = true;
                canpause = true;
                presstobegin.enabled = false;
                Time.timeScale = 1f;
            }
        }

        if (canpause && Input.GetKeyDown(KeyCode.Escape))
        {
            paused = !paused;
            btm.GetComponent<Image>().enabled = paused;
            btm.GetComponent<Button>().enabled = paused;
        }
        if (paused)
        {
            Time.timeScale = 0f;
            pbtm.GetComponent<Image>().enabled = true;
            pbtm.GetComponent<Button>().enabled = true;
            pbtmtxt.enabled = true;
        }
        if (!paused && started)
        {
            Time.timeScale = 1f;
            pbtm.GetComponent<Image>().enabled = false;
            pbtm.GetComponent<Button>().enabled = false;
            pbtmtxt.enabled = false;
        }

        playerscoret.text = playerscore.ToString();
        enemyscoret.text = enemyscore.ToString();

        if (playerscore == targetscore)
        {
            winningoponent = "Player";
            Quit();
        }
        if (enemyscore == targetscore)
        {
            winningoponent = "Bot";
            Quit();
        }

        ball = GameObject.FindGameObjectWithTag("Ball");
        if (goalgot)
        {
            if (min < 2f)
            {
                min += 1 * Time.deltaTime;
            }

            if (min >= 2f)
            {
                goalgot = false;
                min = 0f;
                respawnball();
            }
        }
    }

    public void respawnball()
    {
        Instantiate(rball, new Vector3(0f, 0f, 0f), Quaternion.Euler(0f, 0f, 0f));
    }

    public void playergoal()
    {
        //camshake.GetComponent<CameraController>().shakeDuration = dur;
        Destroy(ball);
        if (times < 2)
        {
            times += 1;
        }
        if (times == 2)
        {
            playerscore += 1;
            times = 0;
        }
        goalgot = true;
    }

    public void enemygoal()
    {
        //camshake.GetComponent<CameraController>().shakeDuration = dur;
        Destroy(ball);
        if (etimes < 2)
        {
            etimes += 1;
        }
        if (etimes == 2)
        {
            enemyscore += 1;
            etimes = 0;
        }
        goalgot = true;
    }

    void Quit()
    {
        canpause = false;
        btm.GetComponent<Image>().enabled = true;
        btm.GetComponent<Button>().enabled = true;
        endingtext.enabled = true;
        btmtxt.enabled = true;
        endingtext.text = "The winner was: " + winningoponent;
        endingtext.color = new Color(0, 255, 0);
        Time.timeScale = 0f;
    }

    void menu()
    {
        SceneManager.LoadScene("Main Menu");
    }
}
