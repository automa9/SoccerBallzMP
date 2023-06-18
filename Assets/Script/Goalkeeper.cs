using UnityEngine;

public class PongObject : MonoBehaviour
{
    public float speed = 5f; // Adjust this to control the speed of the object
    public float distance = 10f; // Adjust this to control the distance the object will move
    private Vector3 originalPosition;
    private float xBoundary;

    void Start()
    {
        originalPosition = transform.position;
        xBoundary = originalPosition.x + distance;
    }

    void Update()
    {
        // Move the object back and forth between original position and xBoundary
        transform.Translate(Vector3.right * speed * Time.deltaTime);
        if (transform.position.x >= xBoundary)
        {
            transform.position = new Vector3(xBoundary, transform.position.y, transform.position.z);
            speed *= -1;
        }
        else if (transform.position.x <= originalPosition.x)
        {
            transform.position = new Vector3(originalPosition.x, transform.position.y, transform.position.z);
            speed *= -1;
        }
    }
}
