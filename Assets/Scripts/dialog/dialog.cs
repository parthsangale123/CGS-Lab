using UnityEngine;

[System.Serializable]
public class DialogueNode
{
    [TextArea(3, 10)]
    public string npcSentence;
    public PlayerOption[] playerOptions;
}

[System.Serializable]
public class PlayerOption
{
    public string optionText;
    public DialogueNode nextNode;
}