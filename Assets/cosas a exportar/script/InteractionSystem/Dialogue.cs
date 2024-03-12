using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "New dialogue")]
public class Dialogue : ScriptableObject
{
    [SerializeField, TextArea(minLines: 1, maxLines: 10)]
    private string[] allDialogue;
    public string[] AllDialogue { get => allDialogue; set => allDialogue = value; }
    [SerializeField]
    private InputManager inputManager;

    private string currentDialogue;
    public string CurrentDialogue { get => currentDialogue;}

    private int currentDialogueIndex;
    public int CurrentDialogueIndex { get => currentDialogueIndex;}

    private void OnEnable()
    {
        ResetDialogue();
    }

    public void NextLine()
    {

        currentDialogueIndex++;
        currentDialogue = allDialogue[currentDialogueIndex];

    }

    public void ResetDialogue()
    {
        currentDialogueIndex = 0;
        currentDialogue = allDialogue[currentDialogueIndex];
    }

}
