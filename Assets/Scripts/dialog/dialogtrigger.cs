// DialogueTrigger.cs
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    // This is where you'll build your conversation tree in the Inspector
    public DialogueNode startingNode;
    private DialogueManager dialogueManager;

    void Start()
    {
        // Find the DialogueManager in the scene
        dialogueManager = FindObjectOfType<DialogueManager>();
    }

    // You can call this method from another script, or a button, or when the player enters a trigger zone
    public void TriggerDialogue()
    {
        dialogueManager.StartDialogue(startingNode);
    }

    
}