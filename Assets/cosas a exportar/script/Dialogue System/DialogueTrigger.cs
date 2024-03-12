using Inventory.Model;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using static UnityEditor.Progress;

public class DialogueTrigger : MonoBehaviour
{
    [SerializeField]
    private InputManager input;

    [SerializeField]
    protected DialogueManager dialogueManager;

    protected bool playerInRange = false;

    [SerializeField]
    protected TextAsset inkJSON;


    private void OnEnable()
    {

        input.OnInteractAction += Interact;
        
    }

    protected virtual void Interact()
    {
        if (playerInRange)
        {
            
            dialogueManager.EnterDialogueMode(inkJSON);

        }
        
    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.CompareTag("Player"))
        {

            playerInRange = true;

        }
        
    }

    private void OnTriggerExit(Collider other)
    {

        if (other.gameObject.CompareTag("Player"))
        {

            playerInRange = false;

        }

    }


}
