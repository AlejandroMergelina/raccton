using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCharacter1 : MainCharacter
{
    [SerializeField]
    private Transform centerOfPunch;
    [SerializeField]
    private float radius;

    //[SerializeField] 
    //private InputManager inputManager;


    private void OnEnable()
    {
        inputManager.OnActionP1Action += OnAction;
        inputManager.OnDogeMain1Action += OnDoge;
    }

    //protected override void Update()
    //{
    //    base.Update();

    //    //if (Input.GetKeyDown(KeyCode.E) && canAttack)
    //    //{
    //    //    print("entro");
    //    //    animator.SetBool("attack", true);
    //    //    canAttack = false;
            
    //    //}
        
    //}

    //Ejecutado desde evento de animación.
    void ComfirmAtack()
    {

        Collider[] enemy = Physics.OverlapSphere(centerOfPunch.position, radius, enemyMask);
        
        foreach(Collider _enemy in enemy)
        {
            
            _enemy.GetComponent<EnemyCharacter>().TakeDamage(CharacterData.Power.GetValue());
            print(_enemy.GetComponent<EnemyCharacter>().GetHP());
        }

        canMove = true;
        animator.SetBool("move", canMove);
        animator.SetBool("attack", canAttack);

        end = initialPosition;
        start = transform.position;

    }
    
}
