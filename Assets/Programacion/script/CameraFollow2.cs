using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Prueva
{
    public class CameraFollow2 : MonoBehaviour
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
        private Transform mainTransform;
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

        public Vector3 borrar;
        private void OnEnable()
        {
            inputManager.OnRotateCameraAction += ChangeAngle;
        }

        private void Start()
        {

            focusArea = new FocusArea(mainTransform, focusAreaSize, this);


        }

        private void LateUpdate()
        {

            focusArea.Update(mainTransform, this);
            borrar = focusArea.Velocity;
            Vector3 focusPosition = focusArea.Center + Vector3.forward * verticalOffset;
            //print(focusArea.Velocity);
            Vector3 currentInputDirection = main.transform.rotation * main.MovementDirection;
            if (focusArea.Velocity.x != 0)
            {

                lookAheadDirX = Mathf.Sign(focusArea.Velocity.x);
                //print(Mathf.Sign(main.transform.forward.x) +"/"+ Mathf.Sign(focusArea.Velocity.x));
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



            //print(lookAheadDirX + "/" + lookAheadDirZ);
            currentLookAheadX = Mathf.SmoothDamp(currentLookAheadX, targetLookAheadX, ref smoothLoockVelocityX, loockSmoothTimeX);
            currentLookAheadZ = Mathf.SmoothDamp(currentLookAheadZ, targetLookAheadZ, ref smoothLoockVelocityZ, loockSmoothTimeZ);

            //print(new Vector3(targetLookAheadX, 0, targetLookAheadZ));

            focusPosition += Vector3.forward * currentLookAheadZ;
            focusPosition += Vector3.right * currentLookAheadX;
            target = focusPosition;
            transform.position = Orbit(target, angle, height,radio);
            LookAtTheTarget();
        }

        //private void OnDrawGizmos()
        //{
        //    Gizmos.color = new Color(1, 0, 0, 1);
        //    Gizmos.DrawLine(focusArea.Corners[0], focusArea.Corners[1]);
        //    Gizmos.DrawLine(focusArea.Corners[1], focusArea.Corners[2]);
        //    Gizmos.DrawLine(focusArea.Corners[2], focusArea.Corners[3]);
        //    Gizmos.DrawLine(focusArea.Corners[3], focusArea.Corners[0]);
        //    Gizmos.color = Color.white;
        //    Gizmos.DrawSphere(focusArea.Corners[0], 0.1f);
        //    Gizmos.color = Color.red;
        //    Gizmos.DrawSphere(focusArea.Corners[1], 0.1f);
        //    Gizmos.color = Color.yellow;
        //    Gizmos.DrawSphere(focusArea.Corners[2], 0.1f);
        //    Gizmos.color = Color.black;
        //    Gizmos.DrawSphere(focusArea.Corners[3], 0.1f);
        //    //Gizmos.DrawCube(focusArea.Center, focusAreaSize);
        //    //Vector3 shift = Vector3.zero;
        //    //Vector3 q;
        //    //focusArea.Distacias(focusArea.Corners[0], focusArea.Corners[1], mainTransform.position, out shift, out q);
        //    //print(mainTransform.position + " / " + q);
        //    //Gizmos.DrawLine(q, mainTransform.position);
        //    //focusArea.Distacias(focusArea.Corners[1], focusArea.Corners[2], mainTransform.position, out shift, out q);
        //    //print(mainTransform.position + " / " + q);
        //    //Gizmos.DrawLine(q, mainTransform.position);
        //    //focusArea.Distacias(focusArea.Corners[2], focusArea.Corners[3], mainTransform.position, out shift, out q);
        //    //print(mainTransform.position + " / " + q);
        //    //Gizmos.DrawLine(q, mainTransform.position);
        //    //focusArea.Distacias(focusArea.Corners[3], focusArea.Corners[0], mainTransform.position, out shift, out q);
        //    //print(mainTransform.position + " / " + q);
        //    //Gizmos.DrawLine(q, mainTransform.position);
        //    Gizmos.color = Color.blue;
        //    Gizmos.DrawSphere(target, 0.2f);

        //}

        struct FocusArea
        {
            CameraFollow2 camera;

            [SerializeField]
            private Vector3 center;
            public Vector3 Center { get => center;}

            private Vector3[] corners;
            public Vector3[] Corners { get => corners;}

            private float[] cornersAngle;
            public float[] CornersAngle { get => cornersAngle; set => cornersAngle = value; }

            //private float currentAngle;
            //public float CornersAngle { get => currentAngle; set => currentAngle = value; }

            [SerializeField]
            private Vector3 velocity;
            public Vector3 Velocity { get => velocity;}

            private float left, right;
            private float top, bottom;

            private Vector3 size;

            public FocusArea(Transform target, Vector3 size, CameraFollow2 camera)
            {
                this.camera = camera;

                this.size = size;
                left = target.position.x - size.x / 2;
                right = target.position.x + size.x / 2;
                bottom = target.position.z - size.z / 2;
                top = target.position.z + size.z / 2;
                //print(top + "/" + left + "/" + right + "/" + bottom);
                corners = new Vector3[4];

                velocity = Vector3.zero;
                center = new Vector3((left + right) / 2, 0, (top + bottom) / 2);

                corners[0] = new Vector3(right, 0, top);
                corners[1] = new Vector3(right, 0, bottom);
                corners[2] = new Vector3(left, 0, bottom);
                corners[3] = new Vector3(left, 0, top);
                cornersAngle = new float[4];
                cornersAngle[0] = 45;
                cornersAngle[1] = 135;
                cornersAngle[2] = 225;
                cornersAngle[3] = 315;
                //corners[0] = new vector3(mathf.sqrt(2), 0, 0) + target.position;
                //corners[1] = new vector3(0, 0, -mathf.sqrt(2)) + target.position;
                //corners[2] = new vector3(-mathf.sqrt(2), 0, 0) + target.position;
                //corners[3] = new vector3(0, 0, mathf.sqrt(2)) + target.position;

                //currentAngle = 0;


            }


            public void Update(Transform target, CameraFollow2 camera)
            {
                //this.camera = camera;


                //float shiftAngle = Mathf.Deg2Rad * (-camera.angle + currentAngle);
                //if(shiftAngle != 0)
                //{
                //    //print(0 + " antes for: " + corners[0].x + "/" + corners[0].z);
                //    for (int i = 0; i < corners.Length; i++)
                //    {

                //        //corners[i].x = corners[i].x/* + corners[i].x * Mathf.Cos(Mathf.Deg2Rad * shiftAngle) + corners[i].z * Mathf.Sin(Mathf.Deg2Rad * shiftAngle)*/;
                //        //corners[i].z = corners[i].z/* - corners[i].x * Mathf.Sin(Mathf.Deg2Rad * shiftAngle) + corners[i].z * Mathf.Cos(Mathf.Deg2Rad * shiftAngle)*/;
                //        //double newX = center.X + (pointToOrbit.X - center.X) * Math.Cos(angleRadians) - (pointToOrbit.Y - center.Y) * Math.Sin(angleRadians);
                //        //double newY = center.Y + (pointToOrbit.X - center.X) * Math.Sin(angleRadians) + (pointToOrbit.Y - center.Y) * Math.Cos(angleRadians);

                //        float newX = center.x + (corners[i].x - center.x) * Mathf.Cos(shiftAngle) - (corners[i].z - center.z) * Mathf.Sin(shiftAngle);
                //        float newZ = center.z + (corners[i].x - center.x) * Mathf.Sin(shiftAngle) + (corners[i].z - center.z) * Mathf.Cos(shiftAngle);
                //        corners[i].x = newX;
                //        corners[i].z = newZ;

                //    }

                //        //print(0 + "despues del for: " + corners[0].x + "/" + corners[0].z);
                //        //print(shiftAngle + " = " + camera.angle + " - " + currentAngle);
                //}


                //currentAngle = camera.angle;

                for (int i = 0; i < cornersAngle.Length; i++)
                {

                    corners[i] = camera.Orbit(center, cornersAngle[i], 0, Mathf.Sqrt(Mathf.Pow(left,2)+ Mathf.Pow(left, 2)));

                }

                float shiftX = 0;
                float shiftZ = 0;

                float HorizontalDistanceLeft = Distacias(corners[1], corners[0], target.position);
                float HorizontalDistanceRight = Distacias(corners[2], corners[3], target.position);
                
                float HorizontalDistances = HorizontalDistanceLeft + HorizontalDistanceRight;

                if( HorizontalDistances > size.x * 1.001f)//solve the rounding error
                {

                    if(HorizontalDistanceRight < HorizontalDistanceLeft)
                    {

                        Vector3 directorVector = (corners[3] - corners[0]).normalized * HorizontalDistanceRight;
                        print(directorVector + "x");
                        shiftX = directorVector.x;
                        shiftZ = directorVector.z;

                    }
                    else if (HorizontalDistanceRight > HorizontalDistanceLeft)
                    {

                        Vector3 directorVector = (corners[0] - corners[3]).normalized * HorizontalDistanceLeft;
                        print(directorVector + "x");
                        shiftX = directorVector.x;
                        shiftZ = directorVector.z;

                    }


                }

                float VerticalDistancesTop = Distacias(corners[0], corners[3], target.position);
                float VerticalDistancesBottom = Distacias(corners[1], corners[2], target.position);

                float VerticalDistances = VerticalDistancesTop + VerticalDistancesBottom;
                
                if (VerticalDistances >size.z * 1.001f)//solve the rounding error
                {

                    if (VerticalDistancesTop < VerticalDistancesBottom)
                    {

                        Vector3 directorVector = (corners[3] - corners[2]).normalized * VerticalDistancesTop;
                        print(directorVector + "z");
                        shiftX += directorVector.x;
                        shiftZ += directorVector.z;

                    }
                    else if (VerticalDistancesTop > VerticalDistancesBottom)
                    {

                        Vector3 directorVector = (corners[2] - corners[3]).normalized * VerticalDistancesBottom;
                        print(directorVector + "z");
                        shiftX += directorVector.x;
                        shiftZ += directorVector.z;

                    }


                }

                //print("distancias = " + HorizontalDistanceLeft + " + " + HorizontalDistanceRight + " = "+ HorizontalDistances + " / " + VerticalDistancesTop + " + " + VerticalDistancesBottom + " = " + VerticalDistances);

                //if (targetBounds.center.x < left)
                //{

                //    shiftX = targetBounds.min.x - left;

                //}
                //else if (targetBounds.max.x > right)
                //{

                //    shiftX = targetBounds.max.x - right;

                //}
                //left += shiftX;
                //right += shiftX;

                //if (targetBounds.min.z < bottom)
                //{

                //    shiftZ = targetBounds.min.z - bottom;

                //}
                //else if (targetBounds.max.z > top)
                //{

                //    shiftZ = targetBounds.max.z - top;

                //}
                //top += shiftZ;
                //bottom += shiftZ;
                //center = new Vector3((left + right) / 2, 0, (top + bottom) / 2);

                //print(shiftX + "/" + shiftZ);

                corners[0] += new Vector3(shiftX, 0, shiftZ);
                corners[1] += new Vector3(shiftX, 0, shiftZ);
                corners[2] += new Vector3(shiftX, 0, shiftZ);
                corners[3] += new Vector3(shiftX, 0, shiftZ);

                //foreach (Vector3 corner in corners)
                //{

                //    print(corner);

                //}

                center = (corners[0] - corners[2]) / 2 + corners[2];

                velocity = new Vector3(shiftX , 0, shiftZ );
                //print("Alf2 x" + shiftX + " z" + shiftZ + " v" + velocity);

            }
            
            public float Distacias(Vector3 corner1, Vector3 corner2, Vector3 main/*, out Vector3 shift,out Vector3 q*/)
            {
                //print("esquina : " + corner1 + " / " + corner2);
                //float a = (corner2.z - corner1.z)/ (corner2.x - corner1.x);
                //float b = -1;
                //float c = (corner1.z - a * corner1.x )* (corner2.x - corner1.x);
                ///*Vector3*/ q = Vector3.zero;
                //print(q.x);
                ////Vector3 Uc = Vector3.zero;
                ////Vector3 U = new Vector3(main.x - (-c), 0, main.x);
                ////Vector3 V = new Vector3(a, 0, b);

                ////Uc = U - ((U.x * V.x - U.z * V.z) / V.magnitude) * V;

                ////q = main + Uc;

                ////shift = main - q;

                //shift = main - q;
                //print(corner1 + " / " + corner2);

                float a = 0;
                float b = 0;
                float c = 0;

                if ((corner2.x - corner1.x) == 0)
                {
                    a = -1;
                    b = (corner2.x - corner1.x) / (corner2.z - corner1.z);
                    c = (((corner2.x - corner1.x)  / (corner2.z - corner1.z)) * (-corner1.z)) + corner1.x;
                }
                else
                {

                    a = (corner2.z - corner1.z) / (corner2.x - corner1.x);
                    b = -1;
                    c = (((corner2.z - corner1.z) / (corner2.x - corner1.x)) * (-corner1.x)) + corner1.z;

                }
                //q.x = main.x - (a * (a * main.x + b * main.z + c)) / (a * a + b * b);
                //q.z = main.z -(b * (a * main.x + b * main.z + c)) / (a * a + b * b);

                //print(a + " / " + b + " / " + c);

                return Mathf.Abs(a * main.x + b * main.z + c) / Mathf.Sqrt(Mathf.Pow(a, 2) + Mathf.Pow(b, 2));

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


        private Vector3 Orbit(Vector3 target, float angle, float height, float radio)
        {

            Vector3 currentLocalPosition = new Vector3();

            currentLocalPosition.x = radio * Mathf.Sin(angle * Mathf.Deg2Rad);
            currentLocalPosition.z = radio * Mathf.Cos(angle * Mathf.Deg2Rad);
            currentLocalPosition.y = height;

            return  target + currentLocalPosition;

        }

        private void ChangeAngle()
        {
            if (canRotate)
            {
                //print("hola");
                angle %= 360;
                StartCoroutine(InterpolarRotacion());
                for (int i = 0; i < focusArea.CornersAngle.Length; i++)
                {

                    StartCoroutine(InterpolarRotacionCorners(i));

                }
            }

        }

        IEnumerator InterpolarRotacion()
        {

            canRotate = false;



            float rotacionInicial = angle;
            float rotacionFinal = angle + 45 * inputManager.GetCameraRotateValue();
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

        IEnumerator InterpolarRotacionCorners(int i)
        {

            canRotate = false;



            float rotacionInicial = focusArea.CornersAngle[i];
            float rotacionFinal = focusArea.CornersAngle[i] + 45 * inputManager.GetCameraRotateValue();
            float timer = 0;
            float tiempo = 0.5f;
            while (timer < tiempo)
            {

                float rotacionActual = Mathf.Lerp(rotacionInicial, rotacionFinal, timer / tiempo);

                focusArea.CornersAngle[i] = rotacionActual;
                timer += Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }

            focusArea.CornersAngle[i] = rotacionFinal;
            canRotate = true;
        }

    }
}