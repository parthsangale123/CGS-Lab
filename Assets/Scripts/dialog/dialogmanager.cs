// DialogueManager.cs (Updated for Synced Audio)
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.Rendering.Universal.Internal;

[RequireComponent(typeof(AudioSource))] // Ensures an AudioSource is on this GameObject
public class DialogueManager : MonoBehaviour
{
    [Header("FPS Controller")]
    [SerializeField] private GameObject crosshair;
    public bool istalking = false;
    private bool isdone = false;
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

    [Header("Environment Audio Control")]
    public AudioSource tvAudio;
    public AudioSource speakerAudio;
    [Range(0f, 1f)] public float reducedVolume = 0.2f;

    // --- Private variables ---
    private DialogueNode currentNode;
    private AudioSource audioSource;
    private Animator anim;// NEW: To play the voice lines
    private Transform tr;
    private Vector3 from;
    private Vector3 currentangle;
    private DialogueNode dd;
    private Transform tr2;
    private Vector3 currentcamera;
    private Animator anim2;
    private float originalTVVolume;
    private float originalSpeakerVolume;
    void Awake()
    {
        // NEW: Get the AudioSource component
        audioSource = GetComponent<AudioSource>();

        dialoguePanel.SetActive(false);

        if (optionButtonPrefab == null) Debug.LogError("Option Button Prefab not assigned!");
        if (optionsContainer == null) Debug.LogError("Options Container not assigned!");
    }
    private void Start()
    {
        tr = FindObjectOfType<PlayerMovement>().transform;
        tr2 = FindObjectOfType<MouseLook>().transform;
        anim2 = GetComponent<Animator>();

        if (tvAudio != null) originalTVVolume = tvAudio.volume;
        if (speakerAudio != null) originalSpeakerVolume = speakerAudio.volume;
    }
    public void StartDialogue(DialogueNode startingNode, Animator j, Vector3 To, Vector3 angle, Vector3 CameraAngles)
    {
        from = tr.position;
        istalking = true;
        anim = j;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        if (crosshair != null) crosshair.SetActive(false);
        dd = startingNode;
        currentangle = tr.eulerAngles;
        currentcamera = tr2.eulerAngles;
        isdone = false;

        if (tvAudio != null) tvAudio.volume = reducedVolume;
        if (speakerAudio != null) speakerAudio.volume = reducedVolume;

        StartCoroutine(why(To, angle, CameraAngles + angle));
    }

    private void DisplayNode(DialogueNode node)
    {
        anim.SetBool("istalking", true);
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
            Debug.Log(node.voiceLine.length);
            Debug.Log(node.npcSentence.Length);
            // Calculate the typing speed to match the audio length
            delayPerCharacter = node.voiceLine.length / node.npcSentence.Length / 1.25f;
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
        anim.SetBool("istalking", false);
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
        isdone = true;

        Go(from, currentangle, currentcamera + currentangle);
        StartCoroutine(Fade());


    }
    private void Go(Vector3 final, Vector3 finalangle, Vector3 finalcamera)
    {

        tr.position = final;
        tr.eulerAngles = finalangle;
        tr2.eulerAngles = finalcamera;

        if (istalking && !isdone)
        {
            dialoguePanel.SetActive(true);
            DisplayNode(dd);
        }
    }
    IEnumerator Fade()
    {
        dialoguePanel.SetActive(false);
        audioSource.Stop(); // Stop audio when ending dialogue
        ClearOptions();
        anim2.SetTrigger("sfade");
        yield return new WaitForSeconds(1f);


        istalking = false;

        if (tvAudio != null) tvAudio.volume = originalTVVolume;
        if (speakerAudio != null) speakerAudio.volume = originalSpeakerVolume;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        if (crosshair != null) crosshair.SetActive(true);
    }
    IEnumerator why(Vector3 t, Vector3 a, Vector3 c)
    {
        yield return new WaitForSeconds(0.1f);
        Go(t, a, c);
    }
}