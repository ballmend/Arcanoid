using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using Random = UnityEngine.Random;

public class Blocks : MonoBehaviour
{
    public int hp;
    [SerializeField] private Renderer myObject;
    [SerializeField] private GameObject PowerUp;
    [SerializeField] private GameObject PowerUpLargePaddle;
    [SerializeField] private GameObject PowerUpSmallPaddle;
    [SerializeField] private GameObject PowerUpGodBall;
    [SerializeField] private GameObject PowerUpPlusHp;
    //[SerializeField] private GameObject PowerUpSpeed;
    [SerializeField] private GameObject PowerUpMultiBalls;
    public GameObject animationPrefab;
    public GameObject ballPrefab;
    private GameObject powerups;
    
    


    void Awake()
    {
        UpdateColor();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ball"))
        {
            if (other.gameObject.GetComponent<BallController>().godmode)
            {
                this.hp = 0;
            }   
            else this.hp -= 1;
            
            UpdateColor();
        }
        else if (other.CompareTag("Stuck"))
        {
            Debug.Log("!!StuckInsideBugPrevented!!");
            Destroy(gameObject);
        }

    }

    public void SpawnPowerup()
    {
        GameObject go;
        GameObject usePowerUp = PowerUp;
        int rand = Random.Range(1,20);

        switch (rand)
        {
            case <4:
                usePowerUp = PowerUpPlusHp;
                break;
            case <10:
                usePowerUp = PowerUpMultiBalls;
                break;
            case <13:
                usePowerUp = PowerUpLargePaddle;
                break;
            case <15:
                usePowerUp = PowerUpSmallPaddle;
                break;
            case < 19:
                usePowerUp = PowerUpGodBall;
                break;
            case <= 20:
                usePowerUp = PowerUpGodBall;
                break;
        }
        go = Instantiate(usePowerUp, transform.position, transform.rotation);
        go.transform.parent = GameObject.Find("PowerUps").transform;
        
    }

    public void UpdateColor()
    {   
        switch (hp)
        {
            case 0:
                if(Random.Range(1, 10) < 2) //Hier kann man die Spawn Rate der powerups ändern
                {
                    SpawnPowerup();
                }
                GameManager.Instance.score += 30;
                GameManager.Instance.UpdateScore();
                Instantiate(animationPrefab, transform.position, transform.rotation);
                Destroy(gameObject);
                break;

            case 1:
                myObject.material.color = new Color32(255, 0 ,240, 255 );
                break;

            case 2:
                myObject.material.color = new Color32(255,40,100,255);
                break;
            case 3:
                myObject.material.color = new Color32(255, 46, 35, 255);
                break;
            case 4:
                myObject.material.color = new Color32(255, 147, 50, 255);
                break;
            case 5:
                myObject.material.color = new Color32(234, 255, 93, 255);
                break;
            case 6:
                myObject.material.color = new Color32(229, 255, 170, 255);
                break;
            case 7:
                myObject.material.color = new Color32(214, 255, 187, 255);
                break;
            case 8:
                myObject.material.color = new Color32(255, 255, 255, 255);
                break;
            case 9:
                myObject.material.color = new Color32(176, 176, 176, 255);
                break;
            case 10:
                myObject.material.color = new Color32(103, 103, 103, 255);
                break;
            case 11:
                myObject.material.color = new Color32(0, 0, 0, 255);
                break;
            
            default:
                Console.WriteLine("HP nicht initialisiert!");
                break;
        }
    }
}
