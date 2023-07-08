using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallMP : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip fenceHit;
    public AudioClip poleHit;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Fence")
        {
            audioSource.PlayOneShot(fenceHit);
        }

        if (collision.gameObject.tag == "Pole")
        {
            audioSource.PlayOneShot(poleHit);
        }
    }
}