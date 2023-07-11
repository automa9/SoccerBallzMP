using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UIElements;

public class GetBallMP : MonoBehaviourPunCallbacks, IPunObservable
{
    public Transform ball_pos;
    public KeyCode keyShoot;
    public float force = 10f;

    public Animator animator;
    private AudioSource audioSource;
    public AudioClip audioClip;
    public bool isShoot = false;
    public Transform spawnPoint;

    PhotonView view;
    private bool isStickToPlayer;
    private GameObject ball;
    private Vector3 previousLocation;

    void Start()
    {
        view = GetComponent<PhotonView>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ball"))
        {
            // Move the ball to the player's position
            isStickToPlayer = true;
            ball = other.gameObject;
            //other.transform.position = ball_pos.position;
        }
    }

    /*IEnumerator ShootAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        isShoot = true;
        isStickToPlayer = false;
        Rigidbody ballRigidbody = ball.GetComponent<Rigidbody>();
        Vector3 shootdirection = transform.forward;
        shootdirection.y += 0.1f;
        //audioSource.PlayOneShot(audioClip);
        Debug.Log("I have kick the ball");

        ballRigidbody.AddForce(shootdirection * force, ForceMode.Impulse);
    }*/

    void Update()
    {
        if (isStickToPlayer)
        {
            Vector2 currentLocation = new Vector2(transform.position.x, transform.position.z);
            float speed = Vector2.Distance(currentLocation, previousLocation) / Time.deltaTime;
            ball.transform.position = ball_pos.position;
            ball.transform.Rotate(new Vector3(transform.right.x, 0, transform.right.z), speed, Space.World);
            previousLocation = currentLocation;

            if (photonView.IsMine && Input.GetKeyDown(keyShoot))
            {
                photonView.RPC("KickBall", RpcTarget.All);
            }
            else
            {
                animator.SetBool("isShoot", false);
            }
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            // Write data to the stream (e.g., synchronize position, rotation, or custom properties)

            // Serialize the "isStickToPlayer" flag
            stream.SendNext(isStickToPlayer);

            // Serialize the animator parameters
            stream.SendNext(animator.GetBool("isShoot"));
        }
        else
        {
            // Read data from the stream (e.g., update position, rotation, or custom properties)

            // Deserialize the "isStickToPlayer" flag
            isStickToPlayer = (bool)stream.ReceiveNext();

            // Deserialize the animator parameters 
            animator.SetBool("isShoot", (bool)stream.ReceiveNext());
        }
    }

    [PunRPC]
    void KickBall()
    {
            Debug.Log("Have kicked");
            animator.SetBool("isShoot", true);
            /*StartCoroutine(ShootAfterDelay(delay));*/
            isShoot = true;
            isStickToPlayer = false;
            Rigidbody ballRigidbody = ball.GetComponent<Rigidbody>();

            Debug.Log("Rigid Body not null: " + ballRigidbody.position);

            Vector3 shootdirection = transform.forward;
            shootdirection.y += 0.1f;
            //audioSource.PlayOneShot(audioClip);

            ballRigidbody.AddForce(shootdirection * force, ForceMode.Impulse);
    }
}
