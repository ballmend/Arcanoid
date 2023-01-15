using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController : MonoBehaviour
{
    public Transform ball;
    public Transform playField;
    public float speed;

    private float dir;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        bool inRange;
        if(transform.position.x - 1.3 < ball.transform.position.x && ball.transform.position.x < transform.position.x + 1.3)
            inRange = true;
        else inRange = false;

        if (ball.transform.position.z > 7 && !inRange)
        {
            float dir2 = transform.position.x - ball.transform.position.x;
            dir = Mathf.Clamp(dir2, -1, 1);
            movePlayer();

        }
        else moveToMid();


        
        
    }

    void movePlayer()
    {
        float newX = transform.position.x + speed * Time.deltaTime * -dir;
        float maxX = playField.localScale.x * 0.5f * 10 - transform.localScale.x * 0.5f;
        float clampedX = Mathf.Clamp(newX, -maxX, maxX);

        transform.position = new Vector3(clampedX, transform.position.y, transform.position.z);
    }

    void moveToMid()
    {
        if (transform.position.x > 0.5)
        {
            dir = 1;
            movePlayer();
        }
        else if(transform.position.x < - 0.5)
            dir = -1;
            movePlayer();
    }
}
