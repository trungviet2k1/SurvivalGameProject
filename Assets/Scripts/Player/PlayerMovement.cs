using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public static PlayerMovement Instance { get; set; }

    [Header("Run Speed")]
    public CharacterController controller;
    public float speed = 12f;

    [Header("Gravity")]
    public float gravity = -9.81f * 2;

    [Header("Jump Activity")]
    public float jumpHeight = 2.5f;
    public float jumpForwardForce = 4f;
    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    private Vector3 lastPosition = new(0f, 0f, 0f);
    Vector3 velocity;
    bool isMoving;

    [HideInInspector] public bool isGrounded;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    void Update()
    {
        if (MenuManager.Instance.isMenuOpen == false && StorageSystem.Instance.storageUIOpen == false
            && CampFireUIManager.Instance.isUIOpen == false)
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
                if (isMoving)
                {
                    velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
                    controller.Move(transform.forward * jumpForwardForce * Time.deltaTime);
                }
                else
                {
                    velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
                }
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