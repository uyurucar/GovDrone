using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_agent : MonoBehaviour
{
    private float time;
    private Touch touch;
    public Animator anim;
    private int feed;
    public AudioSource source;
    public AudioClip clip;
    bool isGameOver = false;
    // Start is called before the first frame update
    void Start()
    {
        time = 0;
        feed = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isGameOver)
        {
            time += Time.deltaTime;
            if (Input.touchCount > 0 && time >= 0.4)
            {
                touch = Input.GetTouch(0);
                if (touch.phase == TouchPhase.Began)
                {
                    feed = 1;
                    //Debug.Log("began");
                    anim.SetInteger("feed", feed);
                    time = Time.deltaTime;
                    GameObject gameManager = GameObject.Find("gameManager");
                    gameManager.GetComponent<SCR_game>().feeded();
                    source.PlayOneShot(clip);
                }
                if (touch.phase == TouchPhase.Ended)
                {

                    //Debug.Log("finished");
                }

            }
            if (feed == 1 && time >= 0.4)
            {
                feed = 0;
                anim.SetInteger("feed", feed);
            }
        }
        else
        {

        }
    }

    public void gameOver()
    {
        isGameOver = true;
    }
}
