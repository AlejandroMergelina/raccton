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
    protected bool canAttack, canMove, canDodge;
    //private bool goToEnemy;

    [SerializeField]
    protected Animator animator;

    private Collider colr;

    [SerializeField]
    protected InputManager inputManager;
       

    protected void OnAction()
    {

        animator.SetBool("attack", true);
    }

    protected void OnDoge()
    {

        if (currentCooldDownDodge <= 0)
        {
            animator.SetTrigger("dodge");
            return;
        }

    }

    public void SetCanDodge(bool canDodge)
    {

        this.canDodge = canDodge;

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

        //if (Input.GetKeyDown(k) && currentCooldDownDodge <= 0 && canDodge)
        //{
        //    animator.SetTrigger("dodge");
        //}

        if (canMove)
        {

            Move(start, end);

        }
    }

    public override void Attack(Character it)
    {

        targetPosition = end = it.transform.position - distanceToEnemy;
        start = initialPosition;

        canMove = true;
        animator.SetBool("move", canMove);

    }

    void Move(Vector3 start, Vector3 end)
    {

        Vector3 direction = end - start;

        transform.position += direction.normalized * Time.deltaTime * 3;

        if (transform.position.x >= end.x && Mathf.Sign(direction.x) == 1)
        {

            canMove = false;
            animator.SetBool("move", canMove);
            canAttack = true;
            /*if (transform.position.x >= targetPosition.x)
            {

                

            }*/

        }
        else if (transform.position.x <= end.x && Mathf.Sign(direction.x) == -1)
        {

            canMove = false;
            animator.SetBool("move", canMove);



        }


    }

    protected void Fall()
    {

        canAttack = false;

        canMove = true;
        animator.SetBool("move", canMove);
        animator.SetBool("attack", canAttack);

        end = initialPosition;
        start = transform.position;

    }

    protected override void FinishAnimationAtack()
    {

        BattleSistem.Instance.CheckLive("Enemy");

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
