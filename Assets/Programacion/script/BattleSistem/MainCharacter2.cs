using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCharacter2 : MainCharacter
{

    //funcion de elejier tecla aleatoria



    [SerializeField]
    private KeyCode[] key2Select;
    private string currentRandomKey;

    [SerializeField]
    private GameObject objects2Throw;

    [SerializeField]
    private float attackTime;

    private void OnEnable()
    {
        inputManager.OnAttackDocAction += Try2Throw;
        inputManager.OnDogeMain1Action += OnDoge;

    }


    private void StaratAttack()
    {
        inputManager.SwichActionMap(ActionMaps.DocAtack);
        print("attack");
        StartCoroutine(StartAtackTime());
    }

    private void RandomKey()
    {

        currentRandomKey = key2Select[Random.Range(0, key2Select.Length)].ToString(); ;

    }

    private void Try2Throw(string key)
    {
        if (key == currentRandomKey)
        {

            //Throw
            
        }
        else
        {

            //Fail

        }

        currentRandomKey = "";

    }

    private IEnumerator StartAtackTime()
    {


        yield return new WaitForSeconds(attackTime);
        inputManager.SwichActionMap(ActionMaps.CombatMode);
        print("seacobo");

    }

}
