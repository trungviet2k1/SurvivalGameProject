using UnityEngine;

public class AIMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 0.2f;
    public float walkCounter;
    public float waitCounter;

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

            Vector3 movement = Vector3.zero;
            float yAdjustment = 0f;

            switch (walkDirection)
            {
                case 0:
                    transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
                    movement += moveSpeed * Time.deltaTime * transform.forward;
                    break;
                case 1:
                    transform.localRotation = Quaternion.Euler(0f, 90f, 0f);
                    movement += moveSpeed * Time.deltaTime * transform.forward;
                    break;
                case 2:
                    transform.localRotation = Quaternion.Euler(0f, -90f, 0f);
                    movement += moveSpeed * Time.deltaTime * transform.forward;
                    break;
                case 3:
                    transform.localRotation = Quaternion.Euler(0f, 180f, 0f);
                    movement += moveSpeed * Time.deltaTime * transform.forward;
                    break;
            }

            if (Physics.Raycast(transform.position, -Vector3.up, out RaycastHit hit, 1.0f))
            {
                yAdjustment = hit.point.y - transform.position.y;
            }

            MoveCharacter(movement, yAdjustment);

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

    void MoveCharacter(Vector3 movement, float yAdjustment)
    {
        Vector3 adjustedMovement = moveSpeed * Time.deltaTime * new Vector3(movement.x, 0f, movement.z).normalized;
        transform.position += new Vector3(adjustedMovement.x, yAdjustment, adjustedMovement.z);
    }

    public void ChooseDirection()
    {
        walkDirection = Random.Range(0, 4);
        isWalking = true;
        walkCounter = walkTime;
    }
}