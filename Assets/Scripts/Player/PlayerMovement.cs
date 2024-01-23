using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Run Speed")]
    public CharacterController controller;
    public float speed = 12f;

    [Header("Gravity")]
    public float gravity = -9.81f * 2;

    [Header("Jump Activity")]
    public float jumpHeight = 3f;
    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    private Vector3 lastPosition = new(0f, 0f, 0f);
    Vector3 velocity;
    bool isGrounded;
    bool isMoving;

    void Update()
    {
        if (MenuManager.Instance.isMenuOpen == false)
        {
            isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

            if (isGrounded && velocity.y < 0)
            {
                velocity.y = -2f;
            }

            float x = Input.GetAxis("Horizontal");
            float z = Input.GetAxis("Vertical");

            Vector3 move = transform.right * x + transform.forward * z;

            controller.Move(speed * Time.deltaTime * move);

            if (Input.GetButtonDown("Jump") && isGrounded)
            {
                velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            }

            velocity.y += gravity * Time.deltaTime;
            controller.Move(velocity * Time.deltaTime);

            isMoving = (lastPosition != gameObject.transform.position && isGrounded);
            if (isMoving)
            {
                isMoving = true;
                SoundManager.Instance.PlaySound(SoundManager.Instance.grassWalkSound);
            }
            else
            {
                isMoving = false;
                SoundManager.Instance.grassWalkSound.Stop();
            }

            lastPosition = gameObject.transform.position; 
        }
    }
}