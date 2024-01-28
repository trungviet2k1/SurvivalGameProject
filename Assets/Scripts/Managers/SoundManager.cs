using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; set; }

    [Header("Sound Effect")]
    public AudioSource dropItemSound;
    public AudioSource craftingItemSound;
    public AudioSource toolSwingSound;
    public AudioSource chopSound;
    public AudioSource pickUpItemSound;
    public AudioSource grassWalkSound;
    public AudioSource wateringChanel;
    public AudioClip wateringCanSound;

    [Header("Game Music")]
    public AudioSource startingZoneBGMusic;

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

    public void PlaySound(AudioSource soundToPlay)
    {
        if (!soundToPlay.isPlaying)
        {
            soundToPlay.Play();
        }
    }
}