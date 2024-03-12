using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyCharacter : Character
{
    [SerializeField]
    private Transform centerOfPunch;
    [SerializeField]
    private float radius;
    [SerializeField]
    private LayerMask enemyMask;

    [SerializeField]
    private Vector3 distanceToEnemy;
    protected Vector3 initialPosition;
    
    protected Vector3 start, end;

    private bool canMove;
    [SerializeField]
    Animator animator;

    protected override void Start()
    {
        base.Start();
        initialPosition = transform.position;
    }

    private void Update()
    {

        if (canMove)
        {

            Move(start, end);

        }

    }

    public override void Attack(Character charaterWhoAttacked)
    {

        end = charaterWhoAttacked.transform.position - distanceToEnemy;
        start = initialPosition;


        canMove = true;
        animator.SetBool("move", canMove);

    }

    void Move(Vector3 start, Vector3 end)
    {

        Vector3 direction = end - start;

        transform.position += direction.normalized * Time.deltaTime*3;

        if (transform.position.x >= end.x && Mathf.Sign(direction.x) == 1)
        {

            canMove = false;
            animator.SetBool("move", canMove);
  
        }
        else if (transform.position.x <= end.x && Mathf.Sign(direction.x) == -1)
        {

            canMove = false;
            animator.SetBool("move", canMove);



        }


    }

    void ComfirmAtack()
    {

        Collider[] enemy = Physics.OverlapSphere(centerOfPunch.position, radius, enemyMask);
        
        foreach (Collider _enemy in enemy)
        {
            
            _enemy.GetComponent<MainCharacter>().TakeDamage(characterData.Power.GetValue());
            print(_enemy.GetComponent<MainCharacter>().GetHP());
        }

        canMove = true;
        animator.SetBool("move", canMove);

        end = initialPosition;
        start = transform.position;
        
    }

    protected override void FinishAnimationAtack()
    {

        BattleSistem.Instance.CheckLive("Main");


    }

    
}