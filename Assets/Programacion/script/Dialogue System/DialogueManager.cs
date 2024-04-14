using Ink.Runtime;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Inventory.Model;

[CreateAssetMenu(menuName = "Dialogue Manager")]
public class DialogueManager : ScriptableObject
{
    [SerializeField]
    private TextAsset loadGlobalsJSON;
    [SerializeField]
    private InventorySO inventory;

    [SerializeField]
    private ItemSO[] items;

    public event Action<Story> OnEnterDialogueMode;
    public event Action OnContinuedialog;

    [SerializeField]
    private InputManager inputManager;

    private DialogueVariables dialogueVariables;

    private void OnEnable()
    {
        inputManager.OnNextLineAction += ContinueDialogue;
        
        dialogueVariables = new DialogueVariables(loadGlobalsJSON);

    }


    public void EnterDialogueMode(TextAsset inkJSON)
    {
        Story currentStory = new Story(inkJSON.text);
        dialogueVariables.StartListening(currentStory);

        inputManager.SwichActionMap(ActionMaps.DialogueMode);
        currentStory.BindExternalFunction("PickUpItem", (string Item, int quantity) => PickUpItem(Item, quantity));
        currentStory.BindExternalFunction("SaveGame", () => SaveGame());
        OnEnterDialogueMode?.Invoke(currentStory);

    }

    public void ContinueDialogue()
    {
        
        OnContinuedialog?.Invoke();
    }

    public void ExitDialogueMode(Story story)
    {

        dialogueVariables.StopListening(story);

        inputManager.SwichActionMap(ActionMaps.MoveOut);

    }

    public Ink.Runtime.Object GetVariableState(string variableName)
    {

        Ink.Runtime.Object variableValue = null;
        dialogueVariables.Variables.TryGetValue(variableName, out variableValue);

        return variableValue;
    }

    private void PickUpItem(string itemName, int cuantity)
    {

        foreach (ItemSO item in items)
        {

            if(item.Name == itemName)
            {

                inventory.AddItem(item,cuantity);

            }

        }

    }

    private void SaveGame()
    {



    }

}
