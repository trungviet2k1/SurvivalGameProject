using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; set; }

    //Sound Effect
    public AudioSource dropItemSound;
    public AudioSource craftingItemSound;
    public AudioSource toolSwingSound;
    public AudioSource chopSound;
    public AudioSource pickUpItemSound;
    public AudioSource grassWalkSound;

    //Music
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