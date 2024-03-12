using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine.Editor;
using Cinemachine.PostFX;
using Cinemachine.Utility;
using Unity.VisualScripting;

public class CameraRotate : MonoBehaviour
{
    [SerializeField]
    private Transform target;

    [SerializeField]
    private InputManager inputManager;

    private bool canRotate = true;

    private void OnEnable()
    {

        inputManager.OnRotateCameraAction += RotateCamera;

    }

    private void RotateCamera()
    {
        if (canRotate)
        {
            StartCoroutine(InterpolarRotacion());
        }
    }

    private void Update()
    {


        
        //fL.ForceCameraPosition(transform.position, Quaternion.Euler(Vector3.up * 10 * Time.deltaTime));
    }

    private void LateUpdate()
    {
        transform.position = target.position;
    }



    IEnumerator InterpolarRotacion()
    {
        //Vector3 initialPos = transform.position;
        //Vector3 finalPos = transform.position + new Vector3(1f,0f,0f);
        canRotate = false;

        Quaternion rotacionInicial = transform.rotation;
        Quaternion rotacionFinal = Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y + (45 * inputManager.GetCameraRotateValue()), transform.eulerAngles.z);
        float timer = 0;
        float tiempo = 0.5f;
        while (timer < tiempo)
        {
            //Vector3 currentPos = Vector3.Lerp(initialPos, finalPos, timer/tiempo);
            Quaternion rotacionActual = Quaternion.Slerp(rotacionInicial, rotacionFinal, timer / tiempo);
            //transform.position = currentPos;
            transform.rotation = rotacionActual;
            timer += Time.deltaTime;
            yield return null;
        }
        //transform.position = finalPos;
        transform.rotation = rotacionFinal;
        canRotate= true;
    }
}
