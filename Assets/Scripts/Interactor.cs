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
    [SerializeField] TextMeshProUGUI promptText;


    public void OnInteract(InputAction.CallbackContext context)
    {
        if (!context.performed) return;

        Ray r = new Ray(InteractorSource.position, InteractorSource.forward);

        if (Physics.Raycast(r, out RaycastHit hitInfo, InteractRange))
        {
            if (hitInfo.collider.gameObject.TryGetComponent(out IInteractable interactObj))
            {
                interactObj.Interact();
            }
        }
    }
}
