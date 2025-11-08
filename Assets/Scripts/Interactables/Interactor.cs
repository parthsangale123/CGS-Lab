using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

interface IInteractable
{
    public void Interact();
}

public class Interactor : MonoBehaviour
{
    [SerializeField] Transform InteractorSource;
    [SerializeField] float InteractRange = 5f;

    [SerializeField] GameObject crosshairNormal;// normal crosshair image
    [SerializeField] GameObject crosshairE;// E crosshair image

    private IInteractable currentTarget;

    private void Update()
    {
        // --- EDITED CODE STARTS HERE ---

        // Cast a ray that goes through all objects and stores them in an array
        RaycastHit[] hits = Physics.RaycastAll(InteractorSource.position, InteractorSource.forward, InteractRange);

        // Sort the array by distance to make sure we check the closest objects first
        System.Array.Sort(hits, (x, y) => x.distance.CompareTo(y.distance));

        // Loop through every object that was hit
        foreach (RaycastHit hitInfo in hits)
        {
            // Check if this specific object has an interactable component
            if (hitInfo.collider.gameObject.TryGetComponent(out IInteractable interactObj))
            {
                // If it does, we've found our target
                currentTarget = interactObj;

                // Swap crosshairs
                crosshairNormal.SetActive(false);
                crosshairE.SetActive(true);

                // Exit the function immediately since we found the closest interactable
                return;
            }
        }

        // If the loop finishes and finds no interactable objects, then reset to default
        currentTarget = null;
        crosshairNormal.SetActive(true);
        crosshairE.SetActive(false);

        // --- EDITED CODE ENDS HERE ---
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        if(FindObjectOfType<DialogueManager>().istalking) return;
        if (currentTarget != null)
        {
            currentTarget.Interact();
        }
    }

}
