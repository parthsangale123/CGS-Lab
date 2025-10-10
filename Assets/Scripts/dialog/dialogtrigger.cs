// DialogueTrigger.cs
using UnityEngine;
using System.Collections;
public class DialogueTrigger : MonoBehaviour, IInteractable
{
    // This is where you'll build your conversation tree in the Inspector
    public DialogueNode startingNode;
    private DialogueManager dialogueManager;
    private Animator anim;
    private Animator anim2;
    public Vector3 To;
    public Vector3 angle;
    public Vector3 cameraangle;
    void Start()
    {
        // Find the DialogueManager in the scene
        dialogueManager = FindObjectOfType<DialogueManager>();
        anim2=dialogueManager.GetComponent<Animator>();
        if (gameObject.GetComponent<Animator>()!=null){
            anim=gameObject.GetComponent<Animator>();
        }
    }

    // You can call this method from another script, or a button, or when the player enters a trigger zone
    public void TriggerDialogue()
    {
        dialogueManager.StartDialogue(startingNode, anim, To, angle, cameraangle);
        
    }

    public void Interact()
    {
        Debug.Log("yes");
        
        StartCoroutine(Fade());


    }
    IEnumerator Fade()
    {
        anim2.SetTrigger("sfade");
        dialogueManager.istalking = true;
        yield return new WaitForSeconds(1f);
        
        TriggerDialogue();
    }

}