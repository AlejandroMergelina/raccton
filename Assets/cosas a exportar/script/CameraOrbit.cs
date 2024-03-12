using Inventory.Model;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class CameraOrbit : MonoBehaviour
{

    [SerializeField]
    private Move main;

    [SerializeField]
    private float finalTarget;
    private Vector3 target;
    private Vector3 lastPoint;
    private bool canTraslate;
    Coroutine call;

    [SerializeField]
    private float radio, height;

    [SerializeField]
    private InputManager inputManager;
    private float angle;
    private bool canRotate = true;


    bool inProcess = false;
    Coroutine call2;
    private void OnEnable()
    {
        inputManager.OnRotateCameraAction += ChangeAngle;
        inputManager.OnMoveAction += OnCharacterMove;
        main.OnDirectionChanged += OnDirectionChanged;
        lastPoint = target = main.transform.forward * finalTarget + main.transform.position;
    }

    private void OnDirectionChanged()
    {
        if(!inProcess)
        {
            Debug.Log("Cambio de dirección brusca");
            StartCoroutine(PositionInterpolate());
        }
    }

    private void OnCharacterMove(Vector2 obj)
    {
        
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            print("hola");
            MoveSpring();

        }
    }

    void LateUpdate()
    {
        //controller = main.forward * finalTarget + main.position;
        Orbit();
        LookAtTheTarget();
        //MoveSpring();
        //Debug.Log(main.GetComponent<Move>().LastDirection);
        //if(Vector3.Dot(main.GetComponent<Move>().MovementDirection, main.GetComponent<Move>().LastDirection) <= 0.5f && main.GetComponent<Move>().LastDirection != Vector3.zero)
        //{
        //    if(!once)
        //    {

        //        StartCoroutine(PositionInterpolate());
        //        once = true;
        //    }
        //}
        //else
        //{
            target = main.transform.forward * finalTarget + main.transform.position;
        //}
    }

    private void LookAtTheTarget()
    {

        Vector3 direction = (target - transform.position).normalized;

        CalculateAndSetAngleY(direction.x, direction.z);

        CalculateAndSetAngleX(Mathf.Sqrt(Mathf.Pow(direction.x, 2) + Mathf.Pow(direction.z, 2)), -direction.y);

        //float angely = Mathf.Asin(direction.x / (Mathf.Sqrt(Mathf.Pow(direction.x, 2) + Mathf.Pow(direction.z, 2))));

        //if (Mathf.Sign(direction.z) >= 0)
        //{

        //    transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, Mathf.Rad2Deg * angely, transform.rotation.eulerAngles.z);

        //}
        //else if(Mathf.Sign(direction.z) < 0 && Mathf.Sign(direction.x) >= 0)
        //{

        //    transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, 180 - Mathf.Rad2Deg * angely, transform.rotation.eulerAngles.z);

        //}
        //else
        //{

        //    transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, -180 - Mathf.Rad2Deg * angely, transform.rotation.eulerAngles.z);

        //}

    }

    private void CalculateAndSetAngleY(float c1, float c2)
    {

        float angely = Mathf.Asin(c1 / (Mathf.Sqrt(Mathf.Pow(c1, 2) + Mathf.Pow(c2, 2))));

        if (Mathf.Sign(c2) >= 0)
        {

            transform.rotation = Quaternion.Euler(transform.eulerAngles.x, Mathf.Rad2Deg * angely, transform.eulerAngles.z);

        }
        else if (Mathf.Sign(c2) < 0 && Mathf.Sign(c1) >= 0)
        {

            transform.rotation = Quaternion.Euler(transform.eulerAngles.x, 180 - Mathf.Rad2Deg * angely, transform.eulerAngles.z);

        }
        else
        {

            transform.rotation = Quaternion.Euler(transform.eulerAngles.x, -180 - Mathf.Rad2Deg * angely, transform.eulerAngles.z);

        }

    }
    private void CalculateAndSetAngleX(float c1, float c2)
    {

        float angely = Mathf.Acos(c1 / (Mathf.Sqrt(Mathf.Pow(c1, 2) + Mathf.Pow(c2, 2))));

        //print(Mathf.Rad2Deg * angely + "/" + c1 + "/" + c2);
        transform.rotation = Quaternion.Euler(Mathf.Rad2Deg * angely, transform.eulerAngles.y, transform.eulerAngles.z);

        //if (Mathf.Sign(c2) >= 0)
        //{

        //    transform.rotation = Quaternion.Euler(Mathf.Rad2Deg * angely, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);

        //}
        //else if (Mathf.Sign(c2) < 0 && Mathf.Sign(c1) >= 0)
        //{

        //    transform.rotation = Quaternion.Euler(180 - Mathf.Rad2Deg * angely, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);

        //}
        //else
        //{

        //    transform.rotation = Quaternion.Euler(-180 - Mathf.Rad2Deg * angely, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);

        //}

    }


    private void Orbit()
    {

        Vector3 currentLocalPosition = new Vector3();

        currentLocalPosition.x = radio * Mathf.Sin(angle * Mathf.Deg2Rad);
        currentLocalPosition.z = radio * Mathf.Cos(angle * Mathf.Deg2Rad);
        currentLocalPosition.y = height;

        transform.position = target + currentLocalPosition;

    }

    private void ChangeAngle()
    {
        print("hola");
        if (canRotate)
        {
            //if (angle >= 360 || angle <= -360)
            //{

            //    angle = 0;

            //}
            angle %= 360;
            StartCoroutine(InterpolarRotacion());
        }

    }

    IEnumerator InterpolarRotacion()
    {
        
        canRotate = false;

        

        float rotacionInicial = angle;
        float rotacionFinal = angle + (45 * inputManager.GetCameraRotateValue());
        float timer = 0;
        float tiempo = 0.5f;
        while (timer < tiempo)
        {
            
            float rotacionActual = Mathf.Lerp(rotacionInicial, rotacionFinal, timer / tiempo);
            
            angle = rotacionActual;
            timer += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        
        angle = rotacionFinal;
        canRotate = true;
    }

    private void MoveSpring()
    {


        //Vector3 direction = (controller - main.transform.position).normalized;

        //float resultado = Vector3.Dot(direction, main.forward);
        //print(resultado + "/" + direction + "/"+ main.transform.forward);
        ////Vector3 finalPosition = main.forward * finalTarget + main.position;
        //call = StartCoroutine(PositionInterpolate());
        //Si resultado da positivo los dos vectores están apuntando a dir. similares.
        //1-: Ifualwa
        //-1: Opuestas
        //0: Perpendicular
        //if (resultado <= 0.5f)
        //{
            
        //    if (call != null)
        //    {

        //        call = StartCoroutine(PositionInterpolate(finalPosition));

        //    }
        //    //else
        //    //{
        //    //    StopCoroutine(call);
        //    //    call = StartCoroutine(PositionInterpolate(finalPosition));

        //    //}

        //}
        //else if(resultado > 0.5f && call == null)
        //{

        //    lastPoint = controller = main.forward * finalTarget + main.position;

        //}
        




    }

    IEnumerator PositionInterpolate()
    {
        print("Hola!");
        lastPoint = main.transform.forward * finalTarget + main.transform.position;
        inProcess = true;
        float timer = 0;
        float tiempo = 0.5f;
        Vector3 finalPosition = Vector3.zero;
        while (timer < tiempo)
        {
            finalPosition = main.transform.forward * finalTarget + main.transform.position;
            Vector3 curretPosition = Vector3.Lerp(lastPoint, finalPosition, timer / tiempo);
            
            target = curretPosition ;
            //lastPoint= curretPosition;
            timer += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        inProcess= false;
        lastPoint = finalPosition;
        call = null;

    }

    //private void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.red;
    //    Gizmos.DrawSphere(main.transform.forward * finalTarget + main.transform.position, 0.2f);
    //    Gizmos.color = Color.blue;
    //    Gizmos.DrawSphere(target, 0.2f);
    //}

    private void OnDisable()
    {
        inputManager.OnRotateCameraAction -= ChangeAngle;
    }

}
