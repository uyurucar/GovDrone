using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class background_loop : MonoBehaviour
{
    public float time;
    public Text highscoreText;
    // Start is called before the first frame update
    void Start()
    {
        int highscore = PlayerPrefs.GetInt("highscore", 0);
        if (highscore == 0) highscoreText.enabled = false;
        else
        {
            highscoreText.enabled = true;
            highscoreText.text = "HIGHSCORE: " + highscore;
        }
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        if (time>0.1)
        {
            transform.position = new Vector3(transform.position.x - 0.1f, transform.position.y, transform.position.z);
            if(transform.position.x <= -16.2f)
            {
                transform.position = new Vector3(16.2f, transform.position.y, transform.position.z);
            }
            time = Time.deltaTime;
        }
    }
    public void onClick()
    {
        SceneManager.LoadScene("GameScreen");
    }
}
