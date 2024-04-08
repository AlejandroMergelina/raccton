using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MainCharacter2 : MainCharacter
{

    //funcion de elejier tecla aleatoria



    [SerializeField]
    private string[] key2Select;
    private string currentRandomKey;

    [SerializeField]
    private GameObject objects2Throw;

    [SerializeField]
    private float attackTime;
    [SerializeField]
    private float attackCoulddown;

    [SerializeField]
    private TMP_Text keyText;

    private Coroutine call;

    private void OnEnable()
    {
        inputManager.OnAttackDocAction += Try2Throw;
        inputManager.OnDogeMain1Action += OnDoge;

    }


    private void RandomKey()
    {

        currentRandomKey = key2Select[Random.Range(0, key2Select.Length)];
        keyText.text = currentRandomKey;

    }

    private void Try2Throw(string key)
    {
        if (key == currentRandomKey)
        {
            print("tirar");
            animator.SetTrigger("Attack");
            //Throw
            
        }
        else
        {
            print("fallar");
            //Fail

        }
        
        currentRandomKey = "";
        keyText.text = currentRandomKey;
        StartAttackCoulddown();

    }

    private IEnumerator StartAtackTime()
    {
        inputManager.SwichActionMap(ActionMaps.DocAtack);
        print("attack");
        StartAttackCoulddown();
        yield return new WaitForSeconds(attackTime);
        inputManager.SwichActionMap(ActionMaps.CombatMode);
        StopCoroutine(call);
        currentRandomKey = "";
        keyText.text = currentRandomKey;
        animator.SetTrigger("EndAttack");
        print(initialPosition);
        StartCoroutine(Move(transform.position, initialPosition));
        print("seacobo");

    }

    private void StartAttackCoulddown()
    {
        if(call != null)
        {

            StopCoroutine(call);

        }
        
        call = StartCoroutine(AttackCoulddown());

    }

    private IEnumerator AttackCoulddown()
    {

        yield return new WaitForSeconds(attackCoulddown);
        RandomKey();

    }

    private void Throw()
    {

        GameObject clon = Instantiate(objects2Throw, transform.position, transform.rotation);
        clon.GetComponent<Rigidbody>().AddForce(Vector3.up * 10, ForceMode.Impulse);

    }

}
