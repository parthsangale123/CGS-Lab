using UnityEngine;
using UnityEngine.InputSystem;

public class Sit : MonoBehaviour, IInteractable
{
    [SerializeField] private Transform seatPoint; // where the player will sit
    private bool isSitting = false;
    private GameObject player;
    private PlayerMovement movement;

    public void Interact()
    {
        if (!isSitting)
        {
            SitDown();
        }
        else
        {
            StandUp();
        }
    }

    public string GetPrompt()
    {
        return isSitting ? "Press E to stand" : "Press E to sit";
    }
    
    private void SitDown()
    {
        Debug.Log("Sitting on seat...");

        // Find the player
        player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            // Move player to the seat position
            player.transform.position = seatPoint.position;
            player.transform.rotation = seatPoint.rotation;

            // Disable movement script while sitting
            movement = player.GetComponent<PlayerMovement>();
            if (movement != null)
                movement.enabled = false;

            isSitting = true;
        }
    }

    private void StandUp()
    {
        Debug.Log("Standing up...");

        if (player != null)
        {
            // Enable movement
            if (movement != null)
                movement.enabled = true;

            // Move player slightly in front of the seat so they don't get stuck
            player.transform.position = seatPoint.position + seatPoint.forward * 1f;

            isSitting = false;
        }
    }
}
