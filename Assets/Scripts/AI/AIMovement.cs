using UnityEngine;

public class AIMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 0.2f;

    [HideInInspector] public float walkCounter;
    [HideInInspector] public float waitCounter;

    Animator anim;
    Vector3 stopPosition;
    bool isWalking;
    float walkTime;
    float waitTime;
    int walkDirection;

    void Start()
    {
        anim = GetComponent<Animator>();

        walkTime = Random.Range(3, 6);
        waitTime = Random.Range(5, 7);

        waitCounter = waitTime;
        walkCounter = walkTime;

        ChooseDirection();
    }

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
                    transform.localPosition += moveSpeed * Time.deltaTime * transform.forward;
                    break;
                case 1:
                    transform.localRotation = Quaternion.Euler(0f, 90f, 0f);
                    transform.localPosition += moveSpeed * Time.deltaTime * transform.forward;
                    break;
                case 2:
                    transform.localRotation = Quaternion.Euler(0f, -90f, 0f);
                    transform.localPosition += moveSpeed * Time.deltaTime * transform.forward;
                    break;
                case 3:
                    transform.localRotation = Quaternion.Euler(0f, 180f, 0f);
                    transform.localPosition += moveSpeed * Time.deltaTime * transform.forward;
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