using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class PlayerMovementMP : MonoBehaviourPunCallbacks, IPunObservable
{
    private CharacterController _controller;
    PhotonView view;
    public Vector3 moveDirection;

    public const float maxDashTime = 2.0f;
    public float dashDistance = 20;
    public float dashStoppingSpeed = 0.1f;
    public float dashSpeed = 5f;
    float currentDashTime = maxDashTime;

    [SerializeField]
    private float _playerSpeed = 5f;

    private float _basePlayerSpeed; //to Store the base player speed

    [SerializeField]
    private float _rotationSpeed = 10f;

    private Vector3 _playerVelocity;
    private bool _groundedPlayer;
    private Animator animator;

    [SerializeField]
    private float _gravityValue = -9.81f;

    [SerializeField]
    private float _jumpHeight = 1.0f;

    private Photon.Realtime.Player kickingPlayer;

    Rigidbody _rigidbody;

    private bool isPoweredUp = false;
    private float powerUpDuration = 10f;
    private float powerUpTimer = 0f;


    private void Start()
    {
        view = GetComponent<PhotonView>();
        animator = GetComponent<Animator>();
        _controller = GetComponent<CharacterController>();
        _rigidbody = GetComponent<Rigidbody>();
        _basePlayerSpeed = _playerSpeed;
    }
    
    void FixedUpdate()
    {
        Debug.Log("Player not poses controller");
        
        if (view.IsMine){
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
                kickingPlayer = PhotonNetwork.LocalPlayer;
            }else {
                animator.SetBool("isShoot", false);
            }

            if (movementDirection != Vector3.zero){
                animator.SetBool("Dribble", true);
                Quaternion desiredRotation = Quaternion.LookRotation(movementDirection, Vector3.up);
                transform.rotation = Quaternion.Slerp(transform.rotation, desiredRotation, _rotationSpeed * Time.deltaTime);
            }else {animator.SetBool("Dribble", false);}

            //dash
            if (_groundedPlayer && Input.GetKeyDown(KeyCode.Mouse1)){

               //animator.SetBool("isJump", true);
                //StartCoroutine(JumpAfterDelay(0.4f));
            }else {
                animator.SetBool("isJump", false);
            }

            //Dash
            if (Input.GetButtonDown("Fire2")) //Right mouse button
            {
                currentDashTime = 0;  }
                
            if(currentDashTime < maxDashTime)
            {
                animator.SetBool("Dribble", true);
                moveDirection = transform.forward * dashDistance;
                currentDashTime += dashStoppingSpeed;}
            else{
                moveDirection = Vector3.zero;
                 //animator.SetBool("Dribble", false);
            }  _controller.Move(moveDirection * Time.deltaTime * dashSpeed);

            _playerVelocity.y += _gravityValue * Time.deltaTime;
            _controller.Move(_playerVelocity * Time.deltaTime);

            if (isPoweredUp)
            {
                powerUpTimer -= Time.deltaTime;
                if(powerUpTimer <=0)
                {
                    isPoweredUp = false;
                    _playerSpeed = _basePlayerSpeed;
                }
            }
        }
    }

     IEnumerator JumpAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        _playerVelocity.y += Mathf.Sqrt(_jumpHeight * -3.0f * _gravityValue);
    }

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

            // Serialize the kicking player
            stream.SendNext(kickingPlayer);
        }else{
            // Read data from the stream (e.g., update position, rotation, or custom properties)

            // Deserialize the position
            transform.position = (Vector3)stream.ReceiveNext();

            // Deserialize the rotation
            transform.rotation = (Quaternion)stream.ReceiveNext();

            // Deserialize the grounded state
            _groundedPlayer = (bool)stream.ReceiveNext();

            // Deserialize the player velocity
            _playerVelocity = (Vector3)stream.ReceiveNext();

            // Deserialize the kicking player
            kickingPlayer = (Photon.Realtime.Player)stream.ReceiveNext();
        }
    }

    private float GetPlayerSpeed()
    {
        if (isPoweredUp)
        {
            return _playerSpeed * 2f;

        }
        else
        {
            return _playerSpeed;
        }
    }

    public void ActivatePowerUp()
    {
        isPoweredUp = true;
        powerUpTimer = powerUpDuration;
        _playerSpeed = _basePlayerSpeed * 2f;
    }
}
