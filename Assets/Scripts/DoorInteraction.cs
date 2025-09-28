using UnityEngine;

public class DoorInteraction : MonoBehaviour, IInteractable
{

    private Animator animator;
    private bool isOpen = false;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void Interact()
    {
        isOpen = !isOpen; // toggle state for door
        animator.SetBool("isOpen", isOpen);
    }
}
