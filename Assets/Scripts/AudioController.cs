using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            Debug.LogError("AudioSource is missing on AudioController!");
        }
    }
    public void PopSound(AudioClip clip)
    {
        if (clip != null)
        {
            audioSource.PlayOneShot(clip);
        }
        else
        {
            Debug.LogWarning("AudioClip is missing! Cannot play sound.");
        }
    }

    public void PlayBackgroundMusic(AudioClip clip, bool loop = true)
    {
        if (clip != null)
        {
            audioSource.clip = clip;
            audioSource.loop = loop;
            audioSource.Play();
        }
        else
        {
            Debug.LogWarning("AudioClip is null! Cannot play music.");
        }
    }

    public void StopBackgroundMusic()
    {
        audioSource.Stop();
    }
}
