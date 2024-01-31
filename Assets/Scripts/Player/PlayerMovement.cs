using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public static PlayerMovement Instance { get; set; }

    [Header("Run Speed")]
    public CharacterController controller;
    public float speedInGround = 12f;
    public float speedInWater = 8f;

    [Header("Gravity")]
    public float gravity;
    public float swimmingGravity = -0.5f;
    public float walkingGravity = -9.81f * 2;

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
    [HideInInspector] public bool isSwimming;
    [HideInInspector] public bool isUnderWater;

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
            Movement();
        }
    }

    public void Movement()
    {
        if (isSwimming)
        {
            if (isUnderWater)
            {
                gravity = swimmingGravity;
            }
            else
            {
                velocity.y = 0;
            }
        }
        else
        {
            gravity = walkingGravity;
        }

        int layerMask = 1 << gameObject.layer;

        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;

        if (gameObject.layer != LayerMask.NameToLayer("Water"))
        {
            controller.Move(speedInGround * Time.deltaTime * move);
        }
        else
        {
            controller.Move(speedInWater * Time.deltaTime * move);
        }

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            if (isMoving)
            {
                velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
                controller.Move(jumpForwardForce * Time.deltaTime * transform.forward);
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

            if (gameObject.layer != LayerMask.NameToLayer("Water"))
            {
                SoundManager.Instance.PlaySound(SoundManager.Instance.grassWalkSound);
            }
        }
        else
        {
            isMoving = false;
            SoundManager.Instance.grassWalkSound.Stop();
        }

        lastPosition = gameObject.transform.position;
    }
}