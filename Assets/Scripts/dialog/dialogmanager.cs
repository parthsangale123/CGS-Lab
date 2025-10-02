// DialogueManager.cs (Updated)
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;
public class DialogueManager : MonoBehaviour
{
    // --- UI Elements ---
    public TextMeshProUGUI npcSentenceText;
    public Button option1Button;
    public Button option2Button;
    public Button endButton; // <<< NEW: Add a reference for the End button
    public GameObject dialoguePanel; 
    public bool istalking=false;
    // --- Private variables ---
    private DialogueNode currentNode;
    private TextMeshProUGUI option1ButtonText;
    private TextMeshProUGUI option2ButtonText;

    void Awake()
    {
        option1ButtonText = option1Button.GetComponentInChildren<TextMeshProUGUI>();
        option2ButtonText = option2Button.GetComponentInChildren<TextMeshProUGUI>();
        
        // --- NEW: Set up the end button ---
        // Make sure the end button calls the EndDialogue method when clicked.
        if (endButton != null)
        {
            endButton.onClick.AddListener(EndDialogue);
        }
        
        dialoguePanel.SetActive(false);
    }

    public void StartDialogue(DialogueNode startingNode)
    {
        istalking=true;
        dialoguePanel.SetActive(true);
        currentNode = startingNode;
        DisplayNode(currentNode);
    }

    private void DisplayNode(DialogueNode node)
    {
        StopAllCoroutines();
        StartCoroutine(TypeSentence(node.npcSentence));

        // Check if there are player options to display
        if (node.playerOptions.Length >= 2)
        {
            // --- Show option buttons, hide end button ---
            option1Button.gameObject.SetActive(true);
            option2Button.gameObject.SetActive(true);
            if(endButton != null) endButton.gameObject.SetActive(false); // NEW

            option1ButtonText.text = node.playerOptions[0].optionText;
            option2ButtonText.text = node.playerOptions[1].optionText;

            option1Button.onClick.RemoveAllListeners();
            option2Button.onClick.RemoveAllListeners();

            option1Button.onClick.AddListener(() => ChooseOption(0));
            option2Button.onClick.AddListener(() => ChooseOption(1));
        }
        else
        {
            // --- This is the end of a branch ---
            // --- Hide option buttons, show end button ---
            option1Button.gameObject.SetActive(false);
            option2Button.gameObject.SetActive(false);
            if(endButton != null) endButton.gameObject.SetActive(true); // NEW
        }
    }

    private void ChooseOption(int optionIndex)
    {
        if (optionIndex < currentNode.playerOptions.Length)
        {
            DialogueNode nextNode = currentNode.playerOptions[optionIndex].nextNode;
            if (nextNode != null)
            {
                currentNode = nextNode;
                DisplayNode(currentNode);
            }
            else
            {
                EndDialogue();
            }
        }
    }

    // This method is now also called by the endButton's onClick event
    private void EndDialogue()
    {
        dialoguePanel.SetActive(false);
        Debug.Log("Dialogue ended.");
        istalking=false;
        
    }
    
    IEnumerator TypeSentence(string sentence)
    {
        npcSentenceText.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            npcSentenceText.text += letter;
            yield return new WaitForSeconds(0.05f);
        }
    }
}