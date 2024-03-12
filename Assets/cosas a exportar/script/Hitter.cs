using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hitter : MonoBehaviour
{
    [SerializeField]
    private GameObject superiorParent;
    [SerializeField]
    private string targetToDamage;

    [SerializeField]
    private GameManager gameManager;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag(targetToDamage))
        {
            print("te pille");
            gameManager.Change2CombatMode(superiorParent);
        }
    }
}
