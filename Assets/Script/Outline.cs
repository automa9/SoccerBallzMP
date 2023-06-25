using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;


public class Outline : MonoBehaviour
{
    
    public Transform spawnPoint;
    public AudioSource audioSource;
    public AudioClip audioClip;

  
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Ball") )
        {        
            audioSource.PlayOneShot(audioClip);

            Rigidbody ballRb = other.GetComponent<Rigidbody>();
            ballRb.velocity = Vector3.zero;
            ballRb.angularVelocity = Vector3.zero;
            other.gameObject.transform.position = spawnPoint.position;
        }
    }
}
