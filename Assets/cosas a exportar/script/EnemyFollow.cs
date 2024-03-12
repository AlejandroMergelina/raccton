using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyFollow : MonoBehaviour
{
    [SerializeField]
    private NavMeshAgent agent;
    [SerializeField]
    private Transform target;
    [SerializeField]
    private GameManager gameManager;

    bool follow = false;

    private void Update()
    {
        
        if(follow) 
        {

            agent.SetDestination(target.position);


        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {

            follow = true;

        }
        

    }
    private void OnTriggerExit(Collider other)
    {

        if (other.CompareTag("Player"))
        {

            follow = false;

        }

    }

   

}
