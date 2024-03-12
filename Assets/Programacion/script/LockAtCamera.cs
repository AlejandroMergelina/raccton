using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockAtCamera : MonoBehaviour
{
    [SerializeField]
    private Transform target;

    // Update is called once per frame
    void LateUpdate()
    {
        LookAtTheTarget();
    }

    private void LookAtTheTarget()
    {

        Vector3 direction = (target.position - transform.position).normalized;

        CalculateAndSetAngleY(direction.x, direction.z);



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

}
