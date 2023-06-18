using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class PlayerMovementMP : MonoBehaviour
{
    private CharacterController _controller;
    PhotonView view;

    [SerializeField]
    private float _playerSpeed = 5f;

    [SerializeField]
    private float _rotationSpeed = 10f;

    private Vector3 _playerVelocity;
    private bool _groundedPlayer;
    private Animator animator;

    [SerializeField]
    private float _gravityValue = -9.81f;

    [SerializeField]
    private float _jumpHeight = 1.0f;

    private void Start()
    {
        view = GetComponent<PhotonView>();
        animator = GetComponent<Animator>();
        _controller = GetComponent<CharacterController>();
    }

    IEnumerator JumpAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        _playerVelocity.y += Mathf.Sqrt(_jumpHeight * -3.0f * _gravityValue);
    }

    void Update()
    {
        Debug.Log("Player not poses controller");
        if (view.IsMine)
        {
            Debug.Log("Player Successfully Poses controller");
            _groundedPlayer = _controller.isGrounded;
            if (_groundedPlayer && _playerVelocity.y < 0)
            {
                _playerVelocity.y = 0f;
            }

            float horizontalInput = Input.GetAxis("Horizontal");
            float verticalInput = Input.GetAxis("Vertical");

            Vector3 movementInput = new Vector3(verticalInput, 0, -horizontalInput);
            Vector3 movementDirection = movementInput.normalized;

            _controller.Move(movementDirection * _playerSpeed * Time.deltaTime);

            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                // Release the ball and apply the kick force
                Debug.Log("Have kicked");
                animator.SetBool("isShoot", true);
            }else{animator.SetBool("isShoot", false);}

            if (movementDirection != Vector3.zero)
            {
                animator.SetBool("Dribble", true);
                Quaternion desiredRotation = Quaternion.LookRotation(movementDirection, Vector3.up);
                transform.rotation = Quaternion.Slerp(transform.rotation, desiredRotation, _rotationSpeed * Time.deltaTime);
            }
            else
            {
                animator.SetBool("Dribble", false);
            }

            if (_groundedPlayer && Input.GetKeyDown(KeyCode.Space))
            {
                animator.SetBool("isJump", true);
                StartCoroutine(JumpAfterDelay(0.4f));
            }
            else
            {
                animator.SetBool("isJump", false);
            }

            _playerVelocity.y += _gravityValue * Time.deltaTime;
            _controller.Move(_playerVelocity * Time.deltaTime);
        }
    }

/*
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            // Write data to the stream (e.g., synchronize position, rotation, or custom properties)

            // Serialize the position
            stream.SendNext(transform.position);

            // Serialize the rotation
            stream.SendNext(transform.rotation);

            // Serialize the grounded state
            stream.SendNext(_groundedPlayer);

            // Serialize the player velocity
            stream.SendNext(_playerVelocity);

            // Serialize the animator parameters
            stream.SendNext(animator.GetBool("Dribble"));
            stream.SendNext(animator.GetBool("isJump"));
        }
        else
        {
            // Read data from the stream (e.g., update position, rotation, or custom properties)

            // Deserialize the position
            transform.position = (Vector3)stream.ReceiveNext();

            // Deserialize the rotation
            transform.rotation = (Quaternion)stream.ReceiveNext();

            // Deserialize the grounded state
            _groundedPlayer = (bool)stream.ReceiveNext();

            // Deserialize the player velocity
            _playerVelocity = (Vector3)stream.ReceiveNext();

            // Deserialize the animator parameters
            animator.SetBool("Dribble", (bool)stream.ReceiveNext());
            animator.SetBool("isJump", (bool)stream.ReceiveNext());
        }
    }*/
}
