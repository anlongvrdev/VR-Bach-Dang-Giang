using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Audio : MonoBehaviour
{
    public AudioSource audioSource;

    void Start()
    {
        // Check if the AudioSource is assigned
        if (audioSource != null)
        {
            audioSource.Play();
        }
        else
        {
            Debug.LogError("AudioSource is not assigned!");
        }
    }
}
