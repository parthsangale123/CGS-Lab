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
        if (Physics.Raycast(InteractorSource.position, InteractorSource.forward, out RaycastHit hitInfo, InteractRange))
        {
            if (hitInfo.collider.gameObject.TryGetComponent(out IInteractable interactObj))
            {
                currentTarget = interactObj;

                //Swap crosshairs
                crosshairNormal.SetActive(false);
                crosshairE.SetActive(true);
                return;
            }
        }

        //No interactable → default crosshair
        currentTarget = null;
        crosshairNormal.SetActive(true);
        crosshairE.SetActive(false);
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        if (!context.performed) return;

        if (currentTarget != null)
        {
            currentTarget.Interact();
        }
    }

}
