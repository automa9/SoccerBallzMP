using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveAroundObject : MonoBehaviour
{
    [SerializeField]
    private float _mouseSensitivity;

    private float _rotationY;

    [SerializeField]
    private Transform _target;

    private Vector3 _currentRotation;
    private Vector3 _smoothVelocity = Vector3.zero;

    [SerializeField]
    private float _smoothTime = 0.2f;

    private float _initialDistance;

    private void Start()
    {
        // Set initial rotation to the rotation in the inspector
        _currentRotation = transform.rotation.eulerAngles;

        // Calculate distance between camera and target
        _initialDistance = Vector3.Distance(transform.position, _target.position);
    }

    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * _mouseSensitivity;

        _rotationY += mouseX;

        Vector3 nextRotation = new Vector3(_currentRotation.x, _rotationY, _currentRotation.z);

        // Apply damping between rotation changes
        _currentRotation = Vector3.SmoothDamp(_currentRotation, nextRotation, ref _smoothVelocity, _smoothTime);
        transform.localEulerAngles = _currentRotation;

        // Calculate distance between camera and target
        float distance = Vector3.Distance(transform.position, _target.position);

        // Set the desired distance between the camera and the target
        float desiredDistance = _initialDistance;

        // Check if the player is moving backwards
        if (Input.GetAxis("Vertical") < 0)
        {
            desiredDistance = Mathf.Max(distance, _initialDistance);
        }

        // Move the camera
        transform.position = _target.position - transform.forward * desiredDistance;
    }
}
