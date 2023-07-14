using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private CharacterController _controller;

    [SerializeField]
    private float _playerSpeed = 5f;

    private float _basePlayerSpeed; // Store the base player speed

    [SerializeField]
    private float _rotationSpeed = 10f;

    [SerializeField]
    private Camera _followCamera;

    private Vector3 _playerVelocity;
    private bool _groundedPlayer;

    [SerializeField]
    private float _gravityValue = -9.81f;

    private bool isPoweredUp = false; // Flag to track power-up state

    private void Start()
    {
        _controller = GetComponent<CharacterController>();
        _basePlayerSpeed = _playerSpeed; // Store the base player speed at the start
    }

    private void Update()
    {
        Movement();
    }

    void Movement()
    {
        _groundedPlayer = _controller.isGrounded;
        if (_groundedPlayer && _playerVelocity.y < 0)
        {
            _playerVelocity.y = 0f;
        }

        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 movementInput = Quaternion.Euler(0, _followCamera.transform.eulerAngles.y, 0) * new Vector3(horizontalInput, 0, verticalInput);
        Vector3 movementDirection = movementInput.normalized;

        _controller.Move(movementDirection * GetPlayerSpeed() * Time.deltaTime);

        if (movementDirection != Vector3.zero)
        {
            Quaternion desiredRotation = Quaternion.LookRotation(movementDirection, Vector3.up);

            transform.rotation = Quaternion.Slerp(transform.rotation, desiredRotation, _rotationSpeed * Time.deltaTime);
        }

        _playerVelocity.y += _gravityValue * Time.deltaTime;
        _controller.Move(_playerVelocity * Time.deltaTime);
    }

    private float GetPlayerSpeed()
    {
        if (isPoweredUp)
        {
            return _playerSpeed * 2f; // Double the player speed during power-up
        }
        else
        {
            return _playerSpeed;
        }
    }

    // Call this method to activate the power-up
    public void ActivatePowerUp()
    {
        isPoweredUp = true;
        StartCoroutine(DeactivatePowerUpAfterDelay());
    }

    // Coroutine to deactivate the power-up after a certain duration
    private IEnumerator DeactivatePowerUpAfterDelay()
    {
        yield return new WaitForSeconds(10f); // Adjust the duration as needed
        isPoweredUp = false;
    }
}
