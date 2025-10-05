// DialogueTrigger.cs
using UnityEngine;

public class DialogueTrigger : MonoBehaviour, IInteractable
{
    // This is where you'll build your conversation tree in the Inspector
    public DialogueNode startingNode;
    private DialogueManager dialogueManager;
    private Animator anim;

    void Start()
    {
        // Find the DialogueManager in the scene
        dialogueManager = FindObjectOfType<DialogueManager>();
        if(gameObject.GetComponent<Animator>()!=null){
            anim=gameObject.GetComponent<Animator>();
        }
    }

    // You can call this method from another script, or a button, or when the player enters a trigger zone
    public void TriggerDialogue()
    {
        dialogueManager.StartDialogue(startingNode, anim);
    }

    public void Interact()
    {
        Debug.Log("yes");
          TriggerDialogue();
        
       
        
    }

}