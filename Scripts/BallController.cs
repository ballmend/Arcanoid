using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.ExceptionServices;
using UnityEngine;

public class BallController : MonoBehaviour
{
    public Transform playerPaddle;
    public Vector3 velocity;
    public float maxX;
    public float maxZ;
    public bool godmode;
    // Start is called before the first frame update
    void Start()
    {
        godmode = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.x < -20 || transform.position.x > 20 || transform.position.z < -5 || transform.position.z > 25)
            Destroy(gameObject);
        else transform.position += velocity * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        switch (other.tag)
        {
            case "Player":
                float paddleCenterlinks = playerPaddle.transform.position.x-0.1f;
                float paddleCenterrechts = playerPaddle.transform.position.x + 0.1f;
                float paddleCenter = playerPaddle.transform.position.x;
                float hitPoint = this.gameObject.transform.position.x;
                float difference = paddleCenter - hitPoint;
                if (hitPoint < paddleCenterlinks)
                {
                    velocity = new Vector3(-difference * maxX, 0, velocity.z);
                }
                else if (hitPoint > paddleCenterrechts) { 
                    velocity = new Vector3(-difference * maxX, 0, velocity.z);
                } else 
                    velocity = new Vector3(0, 0, velocity.z);


                velocity.z *= -1;
                break;

            case "Wand":
                 // ton anfügen
                if(!godmode)
                    velocity.x *= -1;
                break;
            case "Decke":
                // ton anfügen
                if (!godmode)
                    velocity.z *= -1;
                break;
            case "RealWand":
                velocity.x *= -1;
                break;
            case "RealDecke":
                velocity.z *= -1;
                break;
        }
        
    }
}
