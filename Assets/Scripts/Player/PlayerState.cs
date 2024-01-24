using System.Collections;
using UnityEngine;

public class PlayerState : MonoBehaviour
{
    public static PlayerState Instance { get; set; }

    [Header("Player")]
    public GameObject playerBody;

    [Header("Health")]
    public float maxHealth;
    public float maxCalories;
    public float maxHydrationPercent;

    [HideInInspector] public float currentHealth;
    [HideInInspector] public float currentCalories;
    [HideInInspector] public float currentHydrationPercent;

    private float distanceTravelled = 0;
    private Vector3 lastPosition;

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

    void Start()
    {
        currentHealth = maxHealth;
        currentCalories = maxCalories;
        currentHydrationPercent = maxHydrationPercent;
        StartCoroutine(DecreaseHydration());
    }

    private IEnumerator DecreaseHydration()
    {
        WaitForSeconds waitTime = new WaitForSeconds(10);
        while (true)
        {
            currentHydrationPercent -= 1;
            yield return waitTime;
        }
    }

    void Update()
    {
        distanceTravelled += Vector3.Distance(playerBody.transform.position, lastPosition);
        lastPosition = playerBody.transform.position;

        if (distanceTravelled >= 5)
        {
            distanceTravelled = 0;
            currentCalories -= 1;
        }

        if (Input.GetKeyDown(KeyCode.N))
        {
            currentHealth -= 10;
            currentCalories -= 10;
        }
    }

    public void SetHealth(float newHealth)
    {
        currentHealth = newHealth;
    }

    public void SetCalories(float newCalories)
    {
        currentCalories = newCalories;
    }

    public void SetHydration(float newHydration)
    {
        currentHydrationPercent = newHydration;
    }
}