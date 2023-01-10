using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioClip watering;
    public AudioClip money;
    public AudioClip press;
    public AudioClip select;

    private AudioSource audio_source;

    public static SoundManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        audio_source = GetComponent<AudioSource>();
    }

    public void Watering()
    {
        audio_source.clip = watering;
        audio_source.Play();
    }

    public void GetMoney()
    {
        audio_source.clip = money;
        audio_source.Play();
    }

    public void Press()
    {
        audio_source.clip = press;
        audio_source.Play();
    }

    public void Selected()
    {
        audio_source.clip = select;
        audio_source.Play();
    }
}
