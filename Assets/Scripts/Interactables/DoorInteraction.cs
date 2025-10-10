using UnityEngine;

public class DoorInteraction : MonoBehaviour, IInteractable
{

    private AudioSource audioSource;
    private Animator animator;
    private bool isOpen = false;

    [SerializeField] private AudioClip doorOpenSound;
    [SerializeField] private AudioClip doorCloseSound;


    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
    }

    public void Interact()
    {
        isOpen = !isOpen; // toggle state for door
        animator.SetBool("isOpen", isOpen);
        PlayDoorSound();
    }

    public void PlayDoorSound()
    {
        if (isOpen)
        {
            audioSource.PlayOneShot(doorOpenSound);
        }
        else
        {
             audioSource.PlayOneShot(doorCloseSound);
        }
    }
}
