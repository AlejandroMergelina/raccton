using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;

public class Move : MonoBehaviour
{
    [SerializeField]
    private InputManager inputManager;

    [SerializeField]
    private CharacterController controller;
    [SerializeField]
    private Transform cam;

    [SerializeField]
    private float speed;

    [SerializeField]
    private float gravity;
    private Vector3 velocity;

    [SerializeField]
    private Collider _collider;
    [SerializeField]
    private float groundDistance;
    [SerializeField]
    private LayerMask groundMask;

    [SerializeField]
    private float turnSmoothTime;

    private float turnSmoothVelocity;

    private Vector3 movementDirection;
    public Vector3 MovementDirection { get => movementDirection;}
    //private Vector3 lastDirection;

    public event Action OnDirectionChanged;

    Vector3 puntoPies;


    //private bool prueba;
    //private float pruebatIMER;

    private void OnEnable()
    {
        inputManager.OnMoveAction += OnMoveChanged;
        groundDistance = _collider.bounds.extents.x;
        //print(groundDistance);
    }

    private void OnMoveChanged(Vector2 obj)
    {
        movementDirection = new Vector3(obj.x, 0, obj.y);
        //Debug.Log(Vector3.Dot(movementDirection, lastDirection));
        //if (Vector3.Dot(movementDirection, lastDirection) <= 0.75f)
        //{
        //    pruebatIMER = 0;
        //    OnDirectionChanged?.Invoke();
        //}
        //lastDirection= movementDirection;
    }

    void Update()
    {
        //OnMove(inputManager.GetMoveValue());
        OnMove();
        //UpdateTimers();

    }

    //private void UpdateTimers()
    //{
        
    //    pruebatIMER += Time.deltaTime;
    //    if(pruebatIMER >= 0.5f)
    //    {

    //        lastDirection = movementDirection;
    //        pruebatIMER = 0;
    //        //prueba = false;
    //    }
        
    //}
    private void OnMove()
    {
     
        float lng = _collider.bounds.extents.y;
        puntoPies = transform.position - new Vector3(0, lng, 0);

        

        bool isGrounded = Physics.CheckSphere(puntoPies, groundDistance, groundMask);
        if (isGrounded && velocity.y < 0)
        {
            
            velocity.y = -2;

        }

        //Vector3 direction = new Vector3(movementDirection.x, 0, movementDirection.y);

        if (movementDirection.magnitude >= 0.1f)
        {

            float targetAngle = Mathf.Atan2(movementDirection.x, movementDirection.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0, angle, 0);

            Vector3 moveDir = Quaternion.Euler(0, targetAngle, 0) * Vector3.forward;
            controller.Move(moveDir.normalized * speed * Time.deltaTime);

        }

        


        velocity.y += gravity * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);
    }


    private void OnDrawGizmos()
    {

        Gizmos.DrawSphere(puntoPies, groundDistance);

    }
}
