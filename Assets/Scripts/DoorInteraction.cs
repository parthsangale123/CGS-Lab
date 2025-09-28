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
        if (isOpen)
        {
            animator.SetBool("isOpen", false);
        }
        else
        {
            animator.SetBool("isOpen", true);
        }
    }
}
