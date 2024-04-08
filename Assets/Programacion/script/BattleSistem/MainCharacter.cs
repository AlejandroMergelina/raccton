using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCharacter : Character
{
    [SerializeField]
    private Vector3 distanceToEnemy;
    protected Vector3 initialPosition;
    private Vector3 targetPosition;
    protected Vector3 start, end;

    [SerializeField]
    protected LayerMask enemyMask;
    [SerializeField]
    protected Transform counterattack;
    [SerializeField]
    protected float counterattackRadius;
    [SerializeField]
    protected float cooldDownDodge;
    private float currentCooldDownDodge;
    protected bool canAttack;//borrar
    //private bool goToEnemy;

    [SerializeField]
    protected Animator animator;

    private Collider colr;

    [SerializeField]
    protected InputManager inputManager;
       



    protected void OnDoge()
    {

        if (currentCooldDownDodge <= 0)
        {
            animator.SetTrigger("dodge");
            return;
        }

    }


    protected override void Start()
    {
        base.Start();
        initialPosition = transform.position;
        colr = GetComponent<BoxCollider>();
    }

    protected virtual void Update()
    {

        if (currentCooldDownDodge >= 0)
        {

            currentCooldDownDodge -= Time.deltaTime;

        }
    }

    public override void Attack(Character it)
    {

        targetPosition = end = it.transform.position - distanceToEnemy;

        StartCoroutine(Move(transform.position, targetPosition = end = it.transform.position - distanceToEnemy));
        animator.SetBool("IsWallkig", true);

    }

    protected IEnumerator Move(Vector3 start, Vector3 end)
    {

        Vector3 direction = end - start;
        bool canMove= true;
        while (canMove)
        {

            transform.position += direction.normalized * Time.deltaTime;

            if (transform.position.x >= end.x && Mathf.Sign(direction.x) == 1)
            {
                print("entro");
                canMove = false;
                animator.SetBool("IsWallkig", canMove);
                canAttack = true;
                /*if (transform.position.x >= targetPosition.x)
                {



                }*/

            }
            else if (transform.position.x <= end.x && Mathf.Sign(direction.x) == -1)
            {

                canMove = false;
                animator.SetBool("IsWallkig", canMove);



            }

            yield return null;

        }
        


    }



    protected override void FinishAnimationAtack()
    {

        battleSistem.CheckLive("Enemy");

    }

    protected void StartDodge()
    {

        colr.enabled = false;
        currentCooldDownDodge = cooldDownDodge;

    }

    protected void FinishDodge()
    {

        colr.enabled = true;


        Collider[] enemy = Physics.OverlapSphere(counterattack.position, counterattackRadius, enemyMask);

        foreach (Collider _enemy in enemy)
        {

            _enemy.GetComponent<EnemyCharacter>().TakeDamage(CharacterData.Power.GetValue());
            print(_enemy.GetComponent<EnemyCharacter>().GetHP());
        }

    }
}
