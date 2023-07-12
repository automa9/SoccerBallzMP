using UnityEngine;

public class PowerUpScript : MonoBehaviour
{
    public float speedMultiplier = 2f; // Adjust this value to control the speed boost

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerMovementMP player = other.GetComponent<PlayerMovementMP>();
            if (player != null)
            {
                player.ActivatePowerUp(); // Trigger the power-up effect on the player
            }

            Destroy(gameObject); // Destroy the power-up prefab
        }
    }
}
