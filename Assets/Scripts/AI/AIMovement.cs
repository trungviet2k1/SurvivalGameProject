using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIMovement : MonoBehaviour
{
    Animator anim;
    public float moveSpeed = 0.2f;

    Vector3 stopPosition;
    float walkTime;
    public float walkCounter;
    float waitTime;
    public float waitCounter;
    int walkDirection;
    public bool isWalking;

    void Start()
    {
        anim = GetComponent<Animator>();

        walkTime = Random.Range(3, 6);
        waitTime = Random.Range(5, 7);

        waitCounter = waitTime;
        walkCounter = waitTime;

        ChooseDirection();
    }

    // Update is called once per frame
    void Update()
    {
        if (isWalking)
        {
            anim.SetBool("isRunning", true);
            walkCounter -= Time.deltaTime;

            switch (walkDirection)
            {
                case 0:
                    transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
                    transform.position += transform.forward * moveSpeed * Time.deltaTime;
                    break;
                case 1:
                    transform.localRotation = Quaternion.Euler(0f, 90f, 0f);
                    transform.position += transform.forward * moveSpeed * Time.deltaTime;
                    break;
                case 2:
                    transform.localRotation = Quaternion.Euler(0f, -90f, 0f);
                    transform.position += transform.forward * moveSpeed * Time.deltaTime;
                    break;
                case 3:
                    transform.localRotation = Quaternion.Euler(0f, 180f, 0f);
                    transform.position += transform.forward * moveSpeed * Time.deltaTime;
                    break;
            }

            if (walkCounter <= 0)
            {
                stopPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z);
                isWalking = false;

                transform.position = stopPosition;
                anim.SetBool("isRunning", false);
                waitCounter = waitTime;
            }
        }
        else
        {
            waitCounter -= Time.deltaTime;
            if (waitCounter <= 0)
            {
                ChooseDirection();
            }
        }
    }

    public void ChooseDirection()
    {
        walkDirection = Random.Range(0, 4);
        isWalking = true;
        walkCounter = walkTime;
    }
}
