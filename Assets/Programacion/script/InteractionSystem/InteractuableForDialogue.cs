using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class InteractuableForDialogue : MonoBehaviour, Interactuable
{

    [SerializeField]
    private InputManager inputManager;

    [SerializeField]
    private Dialogue dialogue;

    [SerializeField]
    private GameObject dialogueGO;
    [SerializeField]
    private TMP_Text dialogueText;

    [SerializeField]
    private Button buttonContinue;

    public void Interaction()
    {
        //inputManager.
        dialogueGO.SetActive(true);
        dialogueText.text = dialogue.CurrentDialogue;

        buttonContinue.onClick.AddListener(() => Next());
        Time.timeScale = 0f;
    }

    public void Next()
    {
        Debug.Log("fdsfdsfds");
        if(dialogue.CurrentDialogueIndex >= dialogue.AllDialogue.Length-1)
        {
            dialogueGO.SetActive(false);
            dialogue.ResetDialogue();
            Time.timeScale = 1f;
        }
        else
        {
            dialogue.NextLine();
            dialogueText.text = dialogue.CurrentDialogue;
        }
    }

}
