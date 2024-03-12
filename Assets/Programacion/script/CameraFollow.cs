using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Fiable
{
    public class CameraFollow : MonoBehaviour
    {
        //camera orbit
        private Vector3 target;

        [SerializeField]
        private float radio, height;

        [SerializeField]
        private InputManager inputManager;
        [SerializeField]
        private float angle;
        private bool canRotate = true;

        //camera follow

        [SerializeField]
        private CharacterController controller;
        [SerializeField]
        private float verticalOffset;
        [SerializeField]
        private float lookAheadDstX;
        [SerializeField]
        private float lookAheadDstZ;
        [SerializeField]
        private float loockSmoothTimeX;
        [SerializeField]
        private float loockSmoothTimeZ;
        [SerializeField]
        private Vector3 focusAreaSize;
        [SerializeField]
        private Move main;

        private FocusArea focusArea;

        private float currentLookAheadX;
        private float targetLookAheadX;
        private float lookAheadDirX;
        private float smoothLoockVelocityX;

        private float currentLookAheadZ;
        private float targetLookAheadZ;
        private float lookAheadDirZ;
        private float smoothLoockVelocityZ;

        private bool lookAheadStoppedX;
        private bool lookAheadStoppedZ;

        private void OnEnable()
        {
            inputManager.OnRotateCameraAction += ChangeAngle;
        }

        private void Start()
        {

            focusArea = new FocusArea(controller.bounds, focusAreaSize);


        }

        private void LateUpdate()
        {
            focusArea.Update(controller.bounds);
            Vector3 focusPosition = focusArea.Center + Vector3.forward * verticalOffset;
            //print("input: " + focusArea.Velocity + " / " + main.transform.forward);
            Vector3 currentInputDirection = main.transform.rotation * main.MovementDirection;
            if (focusArea.Velocity.x != 0)
            {

                lookAheadDirX = Mathf.Sign(focusArea.Velocity.x);

                if (Mathf.Sign(main.transform.forward.x) == Mathf.Sign(focusArea.Velocity.x) && currentInputDirection.x != 0)
                {
                    lookAheadStoppedX = false;
                    targetLookAheadX = lookAheadDirX * lookAheadDstX;

                }

            }
            else if (!lookAheadStoppedX)
            {


                lookAheadStoppedX = true;
                targetLookAheadX = currentLookAheadX + (lookAheadDirX * lookAheadDstX - currentLookAheadX) / 4f;




            }

            if (focusArea.Velocity.z != 0)
            {

                lookAheadDirZ = Mathf.Sign(focusArea.Velocity.z);

                if (Mathf.Sign(main.transform.forward.z) == Mathf.Sign(focusArea.Velocity.z) && currentInputDirection.z != 0)
                {
                    lookAheadStoppedZ = false;
                    targetLookAheadZ = lookAheadDirZ * lookAheadDstZ;

                }

            }
            else if (!lookAheadStoppedZ)
            {


                lookAheadStoppedZ = true;
                targetLookAheadZ = currentLookAheadZ + (lookAheadDirZ * lookAheadDstZ - currentLookAheadZ) / 4f;




            }

            currentLookAheadX = Mathf.SmoothDamp(currentLookAheadX, targetLookAheadX, ref smoothLoockVelocityX, loockSmoothTimeX);
            currentLookAheadZ = Mathf.SmoothDamp(currentLookAheadZ, targetLookAheadZ, ref smoothLoockVelocityZ, loockSmoothTimeZ);

            focusPosition += Vector3.forward * currentLookAheadZ;
            focusPosition += Vector3.right * currentLookAheadX;
            target = focusPosition;
            Orbit();
            LookAtTheTarget();
        }

        //private void OnDrawGizmos()
        //{

        //    Gizmos.color = new Color(1, 0, 0, .5f);
        //    Gizmos.DrawCube(focusArea.Center, focusAreaSize);
        //    Gizmos.color = Color.blue;
        //    Gizmos.DrawSphere(target, 0.2f);

        //}

        struct FocusArea
        {
            [SerializeField]
            private Vector3 center;
            public Vector3 Center { get => center; set => center = value; }
            [SerializeField]
            private Vector3 velocity;
            public Vector3 Velocity { get => velocity; set => velocity = value; }

            private float left, right;
            private float top, bottom;

            public FocusArea(Bounds targetBounds, Vector3 size)
            {

                left = targetBounds.center.x - size.x / 2;
                right = targetBounds.center.x + size.x / 2;
                bottom = targetBounds.min.z - size.z / 2;
                top = targetBounds.min.z + size.z / 2;

                velocity = Vector3.zero;
                center = new Vector3((left + right) / 2, 0, (top + bottom) / 2);

            }

            public void Update(Bounds targetBounds)
            {

                float shiftX = 0;
                if (targetBounds.min.x < left)
                {

                    shiftX = targetBounds.min.x - left;

                }
                else if (targetBounds.max.x > right)
                {

                    shiftX = targetBounds.max.x - right;

                }
                left += shiftX;
                right += shiftX;

                float shiftZ = 0;
                if (targetBounds.min.z < bottom)
                {

                    shiftZ = targetBounds.min.z - bottom;

                }
                else if (targetBounds.max.z > top)
                {

                    shiftZ = targetBounds.max.z - top;

                }
                top += shiftZ;
                bottom += shiftZ;
                center = new Vector3((left + right) / 2, 0, (top + bottom) / 2);
                velocity = new Vector3(shiftX, 0, shiftZ);

            }

        }

        // camera orbit

        private void LookAtTheTarget()
        {

            Vector3 direction = (target - transform.position).normalized;

            CalculateAndSetAngleY(direction.x, direction.z);

            CalculateAndSetAngleX(Mathf.Sqrt(Mathf.Pow(direction.x, 2) + Mathf.Pow(direction.z, 2)), -direction.y);

        }

        private void CalculateAndSetAngleY(float c1, float c2)
        {

            float angely = Mathf.Asin(c1 / Mathf.Sqrt(Mathf.Pow(c1, 2) + Mathf.Pow(c2, 2)));

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

            float angely = Mathf.Acos(c1 / Mathf.Sqrt(Mathf.Pow(c1, 2) + Mathf.Pow(c2, 2)));

            transform.rotation = Quaternion.Euler(Mathf.Rad2Deg * angely, transform.eulerAngles.y, transform.eulerAngles.z);

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
            //print("hola");
            if (canRotate)
            {
                angle %= 360;

                StartCoroutine(InterpolarRotacion());
            }

        }

        IEnumerator InterpolarRotacion()
        {

            canRotate = false;



            float rotacionInicial = angle;
            float rotacionFinal = angle + 90 * inputManager.GetCameraRotateValue();
            float timer = 0;
            float tiempo = 1f;
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

    }
}