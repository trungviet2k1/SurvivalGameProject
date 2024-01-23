using System.Collections;
using UnityEngine;

public class Animal : MonoBehaviour
{
    public string animalName;
    public bool playerInRange;

    [SerializeField] int currentHealth;
    [SerializeField] int maxHealth;

    [Header("Sound")]
    [SerializeField] AudioSource soundChannel;
    [SerializeField] AudioClip rabbitHitAndScream;
    [SerializeField] AudioClip rabbitHitAndDie;

    [Header("Particle System")]
    [SerializeField] ParticleSystem bloodSplashParticalSystem;
    public GameObject bloodPuddle;

    private Animator anim;
    [HideInInspector] public bool isDead;

    enum AnimalType
    {
        Rabbit,
        Lion,
        Snake
    }

    [Header("Animals")]
    [SerializeField] AnimalType thisAnimalType;

    void Start()
    {
        currentHealth = maxHealth;
        anim = GetComponent<Animator>();
    }

    public void TakeDamage(int damage)
    {
        if (isDead == false)
        {
            currentHealth -= damage;

            bloodSplashParticalSystem.Play();

            if (currentHealth <= 0)
            {
                PlayDyingSound();
                anim.SetTrigger("isDead");
                GetComponent<AIMovement>().enabled = false;

                StartCoroutine(PuddleDelay());
                isDead = true;
            }
            else
            {
                PlayHitSound();
            } 
        }
    }

    IEnumerator PuddleDelay()
    {
        yield return new WaitForSeconds(1f);
        bloodPuddle.SetActive(true);
    }

    private void PlayDyingSound()
    {
        switch (thisAnimalType)
        {
            case AnimalType.Rabbit:
                soundChannel.PlayOneShot(rabbitHitAndDie);
                break;
            case AnimalType.Lion:
                //soundChannel.PlayOneShot(); //Lion Sound Clip
                break;
            case AnimalType.Snake:
                //soundChannel.PlayOneShot(); //Snake Sound Clip
                break;
            default:
                break;
        }
    }

    private void PlayHitSound()
    {
        switch (thisAnimalType)
        {
            case AnimalType.Rabbit:
                soundChannel.PlayOneShot(rabbitHitAndScream);
                break;
            case AnimalType.Lion:
                //soundChannel.PlayOneShot(); //Lion scream Sound Clip
                break;
            case AnimalType.Snake:
                //soundChannel.PlayOneShot(); //Snake scream Sound Clip
                break;
            default:
                break;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }
}