using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public int playerHP;
    public float speed;
    public Transform playField;
    public GameObject bälle;
    public GameObject ballPrefab;
    public bool ballstarted;
    public float ballSpeed;
    private GameObject echterBall;
    public GameObject hearth;
    private GameObject playerStats;
    //window for heath anzeige

    // Start is called before the first frame update
    void Awake()
    {
        playerStats = GameObject.Find("PlayerStats");
        Initialize();
    }

    public void Initialize()
    {
        transform.position = new Vector3(0, 0, 2.5f);
        transform.localScale = new Vector3(0.5f, 1.5f, 0.5f);
        int b = bälle.transform.childCount;                 //delete existing balls
        for (int i = 0; i < b; i++)
            Destroy(bälle.transform.GetChild(i).gameObject);

        int p = GameObject.Find("PowerUps").transform.childCount;//delete existing PowerUps
        for (int i = 0; i < p; i++)
            Destroy(GameObject.Find("PowerUps").transform.GetChild(i).gameObject);

        echterBall = Instantiate(ballPrefab, new Vector3(transform.position.x, transform.position.y, transform.position.z + 1), Quaternion.identity);
        echterBall.GetComponent<BallController>().playerPaddle = transform;
        echterBall.transform.parent = bälle.transform;
        ballstarted = false;
    }

    public void GameOver()
    {
        playerHP = 0;
        updateStats();
    }

    public void lostHP()
    {
        playerHP -= 1;
        echterBall = Instantiate(ballPrefab, new Vector3(transform.position.x, transform.position.y, transform.position.z + 1), Quaternion.identity);
        echterBall.GetComponent<BallController>().playerPaddle = transform;
        echterBall.transform.parent = bälle.transform;
        ballstarted = false;
        updateStats();
    }


    // Update is called once per frame
    void Update()
    {
        if (ballstarted == false)
        {
            echterBall.transform.position = new Vector3(transform.position.x, echterBall.transform.position.y, echterBall.transform.position.z);
            if (Input.GetKeyDown(KeyCode.Space))
            {
                echterBall.GetComponent<BallController>().velocity = new Vector3(0, 0, ballSpeed);
                ballstarted = true;
            }
        }
        movePlayer();
    }

    void movePlayer() {
        float dir = Input.GetAxis("Horizontal");
        float newX = transform.position.x + speed * dir * Time.deltaTime;
        float maxX = playField.localScale.x * 0.5f * 10 - transform.localScale.y * 1f;
        float clampedX = Mathf.Clamp(newX, -maxX, maxX);
        transform.position = new Vector3(clampedX, transform.position.y, transform.position.z);
    }

    public void updateStats()  //Player HP, 
    {
        GameObject healthBar;
        int currenthp = playerStats.transform.childCount;
        if (currenthp < playerHP) {
            for (int i = currenthp; i < playerHP; i++) {
                healthBar = Instantiate(hearth, new Vector3(hearth.transform.position.x, hearth.transform.position.y, hearth.transform.position.z + 2.5f * i), hearth.transform.rotation);
                healthBar.transform.parent = playerStats.transform;
            }
        }else if(currenthp > playerHP)
        {
            Destroy(playerStats.transform.GetChild(currenthp-1).gameObject);
            GetComponent<AudioSource>().Play();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        switch (other.tag)
        {
            case "Enemy":
                playerHP -= 1;
                updateStats();
                break;
            case "PowerUpSpeed":
                break;
            case "PowerUpMultiBall"://copy ball & instanciate
                GameObject ballChild = bälle.transform.GetChild(0).gameObject;

                Vector3 position = new Vector3(ballChild.transform.position.x - 1, ballChild.transform.position.y, ballChild.transform.position.z);
                Vector3 position2 = new Vector3(ballChild.transform.position.x + 1, ballChild.transform.position.y, ballChild.transform.position.z);

                GameObject ball1 = Instantiate(ballChild, position, Quaternion.identity);
                ball1.transform.rotation = ballChild.transform.rotation;
                ball1.GetComponent<BallController>().velocity = ballChild.GetComponent<BallController>().velocity;
                GameObject ball2 = Instantiate(ballChild, position2, Quaternion.identity);
                ball2.transform.rotation = ballChild.transform.rotation;
                ball2.GetComponent<BallController>().velocity = ballChild.GetComponent<BallController>().velocity;
                ball1.transform.parent = bälle.transform;
                ball2.transform.parent = bälle.transform;
                break;
            case "PowerUpLargePaddle":
                transform.localScale = new Vector3(transform.localScale.x, 2f, transform.localScale.z);
                break;
            case "PowerUpSmallPaddle":
                transform.localScale = new Vector3(transform.localScale.x, 1f, transform.localScale.z);
                break;
            case "PowerUpPlusHp":
                playerHP += 1;
                updateStats();
                break;
            case "PowerUpGodBall":
                int b = bälle.transform.childCount;
                for (int i = 0; i < b; i++)
                {
                    bälle.transform.GetChild(i).gameObject.GetComponent<BallController>().godmode = true;
                    bälle.transform.GetChild(i).gameObject.transform.GetChild(0).gameObject.SetActive(true);
                }
                break;
        }


    }

    
}
