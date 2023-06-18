using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBall : MonoBehaviour
{
    private Ball ballAttachedToPlayer;

    public KeyCode keyShoot;
    public float force;
    public bool isShoot = false;

    // Start is called before the first frame update
    void Start()
    {
        ballAttachedToPlayer = GameObject.FindGameObjectWithTag("Ball").GetComponent<Ball>();
    }

    // Update is called once per frame  
    void Update()
    {
        if (ballAttachedToPlayer.getStickToPlayer() && Input.GetKeyDown(keyShoot))
        {
            Debug.Log("Have kicked");
            isShoot = true;
            ballAttachedToPlayer.setStickToPlayer(false);
            Rigidbody rigidbody = ballAttachedToPlayer.transform.gameObject.GetComponent<Rigidbody>();
            Vector3 shootdirection = transform.forward;
            //shootdirection.y += 0.5f;

            rigidbody.AddForce(shootdirection * force, ForceMode.Impulse);
        }
    }
}
