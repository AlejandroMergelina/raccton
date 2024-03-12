using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionSystem : MonoBehaviour
{
    private bool canInteract = false;

    //public event Action OnInteraction;

    [SerializeField]
    private InputManager inputManager;


    Interactuable currentInteractable;

    private void OnEnable()
    {

        inputManager.OnInteractAction += OnInteract;

    }

    private void OnInteract()
    {
        if (canInteract)
        {

            currentInteractable.Interaction();

        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Interactuable"))
        {
            currentInteractable = other.GetComponent<Interactuable>();
            canInteract= true;

        }
    }

    private void OnTriggerExit(Collider other)
    {

        if (other.CompareTag("Interactuable"))
        {
            canInteract = false;
            currentInteractable = null;

        }

    }

}
