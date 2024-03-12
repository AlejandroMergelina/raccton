using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public enum ActionMaps { MoveOut, CombatMode, Menu , Doge, DialogueMode}

[CreateAssetMenu(menuName = "InputManager")]
public class InputManager : ScriptableObject
{
    Controles controls;

    public event Action OnRotateCameraAction;
    public event Action<Vector2> OnMoveAction;
    public event Action OnInteractAction;


    public event Action OnActionP1Action;
    public event Action OnActionP2Action;

    public event Action OnDogeMain1Action;
    public event Action OnDogeMain2Action;

    public event Action OnNextLineAction;

    private void OnEnable()
    {
        //Constructor.
        controls = new Controles();

        controls.MoveOut.Enable();

        controls.MoveOut.RotateCamera.started += OnRotateCamera;
        controls.MoveOut.Move.performed += OnMoveFer;
        controls.MoveOut.Move.canceled += OnCancelledMove;
        controls.MoveOut.Inreract.started += OnInteract;

        //Suscripciones a eventos del ActionPnº.
        controls.CombatMode.ActionP1.started += OnActionP1;
        controls.CombatMode.ActionP2.started += OnActionP2;

        controls.Doge.Main1doge.started += OnDogeMain1;

        controls.DialogueMode.NextLine.started += OnNextLine;

        
    }

    private void OnNextLine(InputAction.CallbackContext obj)
    {
        
        OnNextLineAction?.Invoke();
    }

    private void OnDogeMain1(InputAction.CallbackContext obj)
    {

        OnDogeMain1Action?.Invoke();

    }

    private void OnInteract(InputAction.CallbackContext obj)
    {
        OnInteractAction?.Invoke();
    }

    private void OnCancelledMove(InputAction.CallbackContext obj)
    {
        OnMoveAction?.Invoke(Vector2.zero);
    }

    private void OnMoveFer(InputAction.CallbackContext obj)
    {
        OnMoveAction?.Invoke(obj.ReadValue<Vector2>());
    }

    private void OnRotateCamera(InputAction.CallbackContext obj)
    {
        OnRotateCameraAction?.Invoke();
    }

    private void OnActionP2(InputAction.CallbackContext obj)
    {
        OnActionP2Action?.Invoke();

    }

    private void OnActionP1(InputAction.CallbackContext obj)
    {
        OnActionP1Action?.Invoke();
    }

    //public Vector2 GetMoveValue()
    //{
    //    //OnActionP1Action?.Invoke();
    //    //return controls.MoveOut.Move.ReadValue<Vector2>();

    //}

    public float GetCameraRotateValue()
    {

        return controls.MoveOut.RotateCamera.ReadValue<float>();

    }

    public void SwichActionMap(ActionMaps actionMap)
    {
        if (actionMap == ActionMaps.MoveOut)
        {
            controls.MoveOut.Enable();

            controls.DialogueMode.Disable();
            controls.CombatMode.Disable();
            controls.Menu.Disable();
            controls.Doge.Disable();
            

        }
        else if (actionMap == ActionMaps.CombatMode)
        {

            controls.CombatMode.Enable();

            controls.DialogueMode.Disable();
            controls.MoveOut.Disable();
            controls.Menu.Disable();
            controls.Doge.Disable();

        }
        else if (actionMap == ActionMaps.Menu)
        {
            controls.Menu.Enable();

            controls.DialogueMode.Disable();
            controls.MoveOut.Disable();
            controls.CombatMode.Disable();
            controls.Doge.Disable();

        }
        else if(actionMap == ActionMaps.Doge)
        {

            controls.Doge.Enable();

            controls.DialogueMode.Disable();
            controls.MoveOut.Disable();
            controls.CombatMode.Disable();
            controls.Menu.Disable();

        }
        else if(actionMap == ActionMaps.DialogueMode)
        {

            controls.DialogueMode.Enable();

            controls.MoveOut.Disable();
            controls.CombatMode.Disable();
            controls.Menu.Disable();
            controls.Doge.Disable();
        }


    }
}

