using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SCR_game : MonoBehaviour
{
    private Vector3 leftout;
    private Vector3 rightout;
    public GameObject bird;
    int totalfeed;
    float time;
    float inTime;
    public AudioSource source;
    public AudioClip clip;
    bool walki;
    bool gameOver;
    int FEED_AMOUNT;
    int highscore;
    enum State {Begin,InGame,TooFed, NoFeed, Restart};
    State state;
    public UnityEngine.UI.Text text;
    // Start is called before the first frame update
    void Start()
    {
        totalfeed = 0;
        time = 0;
        inTime = 0;
        state = State.Begin;
        walki = true;
        gameOver = false;
        FEED_AMOUNT = 5;
        highscore = PlayerPrefs.GetInt("highscore", 0);
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(totalfeed);
        if (!gameOver) time += Time.deltaTime;
        if(!gameOver) text.text = "Time: " + (int)time;
        if(state == State.Begin)
        {
            inTime += Time.deltaTime;
            if(inTime>1)
            {
                //instantiate
                leftout = new Vector3(Random.Range(-4.5f, -3.5f), Random.Range(0.7f, 5f), -1.3f);
                rightout = new Vector3(Random.Range(3.3f, 4.5f), Random.Range(0.7f, 5f), -1.3f);
                if (Random.Range(1, 11) >= 5) Instantiate(bird, leftout, Quaternion.Euler(0, 0, 0));
                else Instantiate(bird, rightout, Quaternion.Euler(0, 0, 0));
                inTime = 0;
            }
            if(time>10)
            {
                state = State.InGame;
                inTime = 0;
            }
        }
        else if(state == State.InGame && !gameOver)
        {
            inTime += Time.deltaTime;
            GameObject[] objects = GameObject.FindGameObjectsWithTag("bird");
            if(objects.Length < Random.Range(10,20) )
            {
                if(inTime>1)
                {
                    //instantiate
                    leftout = new Vector3(Random.Range(-4.5f, -3.5f), Random.Range(0.7f, 5f), -1.3f);
                    rightout = new Vector3(Random.Range(3.3f, 4.5f), Random.Range(0.7f, 5f), -1.3f);
                    if (Random.Range(1, 11) >= 5) Instantiate(bird, leftout, Quaternion.Euler(0, 0, 0));
                    else Instantiate(bird, rightout, Quaternion.Euler(0, 0, 0));
                    inTime = 0;
                }

            }
            if(totalfeed < 4)
            {
                state = State.NoFeed;
                inTime = 0;
                gameOver = true;
                objects[0].GetComponent<SCR_bird>().noFeed();
            }
            if(totalfeed > (objects.Length*3)/2)
            {
                state = State.TooFed;
                gameOver = true;
                inTime = 0;
                busted(objects);
            }
        }
        else if(state == State.TooFed)
        {
            if(highscore < (int)time) { PlayerPrefs.SetInt("highscore", (int)time); PlayerPrefs.Save();  if((int)time>30 ) text.text = "NEW HIGHSCORE: " + (int)time; }
            else text.text = "DON'T FEED TOO MUCH. GAME OVER, SCORE: " + (int)time;
            GameObject.Find("agent").GetComponent<SCR_agent>().gameOver();
            state = State.Restart;
            inTime = 0;
        }
        else if(state== State.NoFeed)
        {
            if (highscore < (int)time) { PlayerPrefs.SetInt("highscore", (int)time); PlayerPrefs.Save(); if ((int)time > 30) text.text = "NEW HIGHSCORE: " + (int)time; }
            else text.text = "YOU DID'NT FEED ENOUGH. GAME OVER, SCORE: " + (int)time;
            inTime += Time.deltaTime;
            if(!walki)
            {
                GameObject[] objects = GameObject.FindGameObjectsWithTag("bird");
                gameOverBirds(objects);
                GameObject.Find("agent").GetComponent<SCR_agent>().gameOver();
                if (inTime > 6)
                {
                    state = State.Restart;
                    inTime = 0;
                }
            }
        }
        else if(state == State.Restart)
        {
            inTime += Time.deltaTime;
            if(inTime>4)
            {
                SceneManager.LoadScene("FirstScreen");
            }
        }
        
    }

    void busted(GameObject[] objects)
    {
        foreach (GameObject obje in objects)
        {
            obje.GetComponent<SCR_bird>().busted();
        }
    }
    void gameOverBirds(GameObject[] objects)
    {
        foreach (GameObject obje in objects)
        {
            obje.GetComponent<SCR_bird>().gameOver();
        }
    }

    public void feeded()
    {
        totalfeed += FEED_AMOUNT;
        
    }
    public void ate()
    {
        totalfeed -= 1;
    }

    public void walkie()
    {
        if (walki == false) return;
        if (walki == true)
        {
            source.PlayOneShot(clip);
            walki = false;
        }
    }
}

