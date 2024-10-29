using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_bird : MonoBehaviour
{
    public Animator anim;
    enum Direction {Left, Right};  
    enum State {Fly, Walk, Eat, FlyAway, WalkieTalkie};
    private State state; //18
    private Direction direction; //22
    private int feed;    //17
    Vector3 landingPlace; //33
    private float time;
    private float eatingtime;
    private float walktime;
    private float LEFT_MOST_X = -1.1f;
    private float RIGHT_MOST_X = 2.6F;
    private Vector3 rightout;
    private Vector3 leftout;
    GameObject gameManager;
    bool isGameOver = false;

    [SerializeField] private float FLY_SPEED;
    float t;
    // Start is called before the first frame update
    void Start()
    {
        feed = Random.Range(1, 5);
        state = State.Fly;  //first state when instantiate
        anim.SetInteger("state", (int)state); 
        if(transform.position.x >= 3.1f)   //on the right side 
        {
            direction = Direction.Left;
            transform.localScale = new Vector3(-1, 1, 1);
        }
        else
        {
            direction = Direction.Right;
            transform.localScale = new Vector3(1, 1, 1);
        }
        float randx = Random.Range(LEFT_MOST_X, RIGHT_MOST_X);
        float randy = Random.Range(-1.55f, -3.3f);
        randy += Mathf.Abs(randx-LEFT_MOST_X) * 10 * 0.005f;
        landingPlace = new Vector3(randx, randy, -1.3f);
        time = 0f;
        eatingtime = Random.Range(3f, 13f);
        walktime = 0f;
        leftout = new Vector3(Random.Range(-4.5f, -3.6f), Random.Range(0.7f, 5f), -1.3f);
        rightout = new Vector3(Random.Range(3.6f,4.5f), Random.Range(0.7f,5f), -1.3f);
        gameManager = GameObject.Find("gameManager");
        FLY_SPEED = 1f;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isGameOver)
        {
            if (state == State.Fly)
            {
                t += Time.deltaTime;
                transform.position = Vector3.Lerp(transform.position, landingPlace, (FLY_SPEED * 2)*Time.deltaTime);   //FLY
                if (Mathf.Abs(transform.position.x - landingPlace.x) < 0.1)
                {
                    state = State.Walk;
                    anim.SetInteger("state", (int)State.Walk);
                    t = 0;
                }

            }
            else if (state == State.Walk)
            {
                time += Time.deltaTime;
                walktime += Time.deltaTime;
                if (walktime >= 0.5)
                {
                    if (direction == Direction.Left)
                    {
                        if (transform.localScale.x > 0) transform.localScale = new Vector3(-1, 1, 1);
                        transform.position = new Vector3(transform.position.x - 0.1f, transform.position.y - 0.005f, transform.position.z);
                        if (transform.position.x <= -1.1f)
                        {
                            direction = Direction.Right;
                        }
                    }
                    else if (direction == Direction.Right)
                    {
                        if (transform.localScale.x < 0) transform.localScale = new Vector3(1, 1, 1);
                        transform.position = new Vector3(transform.position.x + 0.1f, transform.position.y + 0.005f, transform.position.z);
                        if (transform.position.x >= 2.6f)
                        {
                            direction = Direction.Left;
                        }
                    }
                    walktime = 0;
                }
                if (time >= eatingtime)
                {
                    if (feed <= 0)
                    {
                        state = State.FlyAway;
                        anim.SetInteger("state", (int)state);
                        t = 0;
                    }
                    else
                    {
                        state = State.Eat;
                        anim.SetInteger("state", (int)state);
                        eatingtime = Random.Range(3f, 13f);
                        time = 0;
                    }
                }
            }
            else if (state == State.Eat)
            {
                time += Time.deltaTime;
                if (time >= 0.7f)
                {
                    feed--;
                    gameManager.GetComponent<SCR_game>().ate();
                    state = State.Walk;
                    anim.SetInteger("state", (int)state);
                    time = 0;
                }
            }
            else if (state == State.FlyAway)
            {
                anim.SetInteger("state", 3);
                if (direction == Direction.Left)
                {
                    if (transform.localScale.x > 0) transform.localScale = new Vector3(-1, 1, 1);
                    t += Time.deltaTime;
                    transform.position = Vector3.Lerp(transform.position, leftout, (FLY_SPEED * 2) * Time.deltaTime);    //FLY
                    if (transform.position.x < -3.5f) Destroy(this.gameObject, 0f);
                }
                else if (direction == Direction.Right)
                {
                    if (transform.localScale.x < 0) transform.localScale = new Vector3(1, 1, 1);
                    t += Time.deltaTime;
                    transform.position = Vector3.Lerp(transform.position, rightout, (FLY_SPEED * 2) * Time.deltaTime);    //FLY
                    if (transform.position.x > 3.5f) Destroy(this.gameObject, 0f);
                }
            }
            else if (state == State.WalkieTalkie)
            {

                if (direction == Direction.Right)
                {
                    direction = Direction.Left;
                    if (transform.localScale.x > 0) transform.localScale = new Vector3(-1, 1, 1);

                }
                t += Time.deltaTime;
                transform.position = Vector3.Lerp(transform.position, new Vector3(-1.74f, -1.97f, -3f), (FLY_SPEED * 2) * Time.deltaTime); //walkie talkie'ye gidiyor.
                if (Mathf.Abs(transform.position.x - (-1.74f)) < 0.2f)
                {
                    anim.SetInteger("state", 2);
                    gameManager.GetComponent<SCR_game>().walkie();
                }
            }
        }
        else
        {

        }
    }

    public void busted()
    {
        state = State.FlyAway;
    }

    public void noFeed()
    {
        state = State.WalkieTalkie;
        t = 0;
        transform.position = new Vector3(transform.position.x, transform.position.y, -3f);  //-3f agentin üstünde görünmesi için
        anim.SetInteger("state", 3);
    }
    public void gameOver()
    {
        isGameOver = true;
    }
}
