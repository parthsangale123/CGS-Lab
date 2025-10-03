// DialogueManager.cs (Updated for Synced Audio)
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;

[RequireComponent(typeof(AudioSource))] // Ensures an AudioSource is on this GameObject
public class DialogueManager : MonoBehaviour
{
    [Header("FPS Controller")]
    [SerializeField] private GameObject crosshair;
    public bool istalking = false;

    [Header("UI Elements")]
    public TextMeshProUGUI npcSentenceText;
    public GameObject dialoguePanel; 

    [Header("Dynamic Options")]
    public GameObject optionButtonPrefab;
    public Transform optionsContainer;
    [SerializeField] private string endDialogueText = "Leave";
    
    [Header("Typing Speed")]
    [Tooltip("The default speed for typing when no audio is present.")]
    [SerializeField] private float defaultTypingSpeed = 0.05f;

    // --- Private variables ---
    private DialogueNode currentNode;
    private AudioSource audioSource; // NEW: To play the voice lines

    void Awake()
    {
        // NEW: Get the AudioSource component
        audioSource = GetComponent<AudioSource>();
        
        dialoguePanel.SetActive(false);
        
        if (optionButtonPrefab == null) Debug.LogError("Option Button Prefab not assigned!");
        if (optionsContainer == null) Debug.LogError("Options Container not assigned!");
    }

    public void StartDialogue(DialogueNode startingNode)
    {
        istalking = true;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        if (crosshair != null) crosshair.SetActive(false);

        dialoguePanel.SetActive(true);
        DisplayNode(startingNode);
    }

    private void DisplayNode(DialogueNode node)
    {
        currentNode = node;
        npcSentenceText.text = "";
        ClearOptions();
        
        // NEW: Stop any previously playing audio
        audioSource.Stop();

        StopAllCoroutines();
        StartCoroutine(TypeSentence(node));
    }

    // --- MAJOR CHANGE: This coroutine now handles audio sync ---
    IEnumerator TypeSentence(DialogueNode node)
    {
        float delayPerCharacter;

        // Check if a voice line is attached and the sentence isn't empty
        if (node.voiceLine != null && node.npcSentence.Length > 0)
        {
            // Play the audio clip
            audioSource.PlayOneShot(node.voiceLine);
            // Calculate the typing speed to match the audio length
            delayPerCharacter = node.voiceLine.length / node.npcSentence.Length /2f;
        }
        else
        {
            // Fallback to default speed if there's no audio
            delayPerCharacter = defaultTypingSpeed;
        }

        // Type out the sentence using the calculated or default delay
        foreach (char letter in node.npcSentence.ToCharArray())
        {
            npcSentenceText.text += letter;
            yield return new WaitForSeconds(delayPerCharacter);
        }

        // After typing/audio is complete, create the option buttons
        if (node.playerOptions.Length > 0)
        {
            for (int i = 0; i < node.playerOptions.Length; i++)
            {
                CreateOptionButton(node.playerOptions[i], i);
            }
        }
        else
        {
            CreateEndButton();
        }
    }

    // ... (The rest of the script from CreateOptionButton downwards is unchanged) ...

    private void CreateOptionButton(PlayerOption option, int index)
    {
        GameObject buttonGO = Instantiate(optionButtonPrefab, optionsContainer);
        TextMeshProUGUI buttonText = buttonGO.GetComponentInChildren<TextMeshProUGUI>();
        Button button = buttonGO.GetComponent<Button>();

        buttonText.text = $"{index + 1}. {option.optionText}";
        button.onClick.AddListener(() => ChooseOption(index));
    }
    
    private void CreateEndButton()
    {
        GameObject buttonGO = Instantiate(optionButtonPrefab, optionsContainer);
        TextMeshProUGUI buttonText = buttonGO.GetComponentInChildren<TextMeshProUGUI>();
        Button button = buttonGO.GetComponent<Button>();

        buttonText.text = endDialogueText;
        button.onClick.AddListener(EndDialogue);
    }

    private void ClearOptions()
    {
        foreach (Transform child in optionsContainer)
        {
            Destroy(child.gameObject);
        }
    }

    private void ChooseOption(int optionIndex)
    {
        ClearOptions();

        if (optionIndex < currentNode.playerOptions.Length)
        {
            DialogueNode nextNode = currentNode.playerOptions[optionIndex].nextNode;
            if (nextNode != null)
            {
                DisplayNode(nextNode);
            }
            else
            {
                EndDialogue();
            }
        }
    }

    public void EndDialogue()
    {
        dialoguePanel.SetActive(false);
        audioSource.Stop(); // Stop audio when ending dialogue
        ClearOptions();
        istalking = false;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        if (crosshair != null) crosshair.SetActive(true);
    }
}