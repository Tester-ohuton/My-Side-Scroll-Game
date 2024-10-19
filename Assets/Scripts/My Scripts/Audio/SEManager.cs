using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SEManager : MonoBehaviour
{
    public AudioClip[] SE = new AudioClip[2];

    AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }


    public void PlaySE(int No)
    {
        audioSource.PlayOneShot(SE[No]);
    }
}
