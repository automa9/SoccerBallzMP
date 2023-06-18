using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class Goal : MonoBehaviour
{
    public Text scoredText;
    public TextMeshProUGUI scoreText;
    public int score = 0;
    public Transform spawnPoint;
    public AudioSource audioSource;
    public AudioClip audioClip;
    public bool isGoal = false;

    private IEnumerator ShowScoredText()
    {
        scoredText.gameObject.SetActive(true);
        yield return new WaitForSeconds(2f); // wait for 2 seconds
        scoredText.gameObject.SetActive(false); // set the game object to inactive
    }

    private IEnumerator Delay()
    {
        yield return new WaitForSeconds(1f);
        isGoal = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Ball") && isGoal == false)
        {
            isGoal = true;
            StartCoroutine(Delay());
            score ++;
            scoreText.text = score.ToString();
            StartCoroutine(ShowScoredText());
            audioSource.PlayOneShot(audioClip);

            Rigidbody ballRb = other.GetComponent<Rigidbody>();
            ballRb.velocity = Vector3.zero;
            ballRb.angularVelocity = Vector3.zero;
            other.gameObject.transform.position = spawnPoint.position;            
        }
    }
}
