using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;
using Photon.Pun;

public class Goal : MonoBehaviourPunCallbacks
{
    public Text scoredText;
    public TextMeshProUGUI scoreText;
    public int score = 0;
    public Transform spawnPoint;
    public AudioSource audioSource;
    public AudioClip audioClip;
    public bool isGoal = false;
    public string goalName;

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
        if (other.gameObject.CompareTag("Ball") && isGoal == false )
        {
            isGoal = true;
            StartCoroutine(Delay());
            score ++;
            if(photonView == null)
            {
                scoreText.text = score.ToString();
                scoredText.text = "SCORED";
                StartCoroutine(ShowScoredText());
                audioSource.PlayOneShot(audioClip);
            }
            else
            {
                if (photonView.IsMine)
                {
                    scoreText.text = score.ToString();

                    photonView.RPC("UpdatePlayerScore", RpcTarget.OthersBuffered, score);
                    scoredText.text = goalName + " SCORED";
                    StartCoroutine(ShowScoredText());
                    audioSource.PlayOneShot(audioClip);
                }
            }

            Rigidbody ballRb = other.GetComponent<Rigidbody>();
            ballRb.velocity = Vector3.zero;
            ballRb.angularVelocity = Vector3.zero;
            other.gameObject.transform.position = spawnPoint.position;            
        }
    }

    [PunRPC]
    private void UpdatePlayerScore(int score)
    {
        Debug.Log(score);
        scoreText.text = score.ToString();
        scoredText.text = goalName + " SCORED";
        StartCoroutine(ShowScoredText());
    }
}
