using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EStates
{
    Chase,
    Attack,
    Idle,
    Returning,
    Died
}

public class Enemy : Hittable
{
    public Attacker PlayerAttacker;
    [SerializeField] private float rotateSpeed;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float attackRadius = 1f;
    [SerializeField] private float lookRadius = 1.5f;
    [SerializeField] private float chaseRadius = 4.5f;
    [SerializeField] private float attackCD = 1.5f;
    private float attackTime;

    public EStates state;
    private Transform player;
    private Rigidbody rb;
    private Vector3 moveVec;
    private Vector3 toPlayer;
    private Vector3 startPos;
    private Quaternion rotate;
    private float distanceToPlayer;
    private bool attacking;


    public override void Die(Vector3 pos)
    {
        base.Die(pos);
        Destroy(PlayerAttacker);
        
        state = EStates.Died;
    }

    private void Start()
    {
        animator = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        rb = GetComponent<Rigidbody>();
        rotate = Quaternion.identity;
        state = EStates.Idle;
        startPos = transform.position;
    }

    private void Update()
    {
        switch (state)
        {
            case EStates.Chase: Chase(); break;
            case EStates.Attack: Attack(); break;
            case EStates.Idle: Idle(); break;
            case EStates.Returning: Return(); break;
            case EStates.Died: moveVec = Vector3.zero; break;
            default:
                break;
        }
        animator.SetBool("Move", moveVec != Vector3.zero);
    }

    private void FixedUpdate()
    {
        toPlayer = (player.position - transform.position).normalized;

        distanceToPlayer = Vector3.Distance(transform.position, player.position);
        transform.rotation = rotate * transform.rotation;
        rb.velocity = moveVec;
    }

    private void Idle()
    {
        if (distanceToPlayer < lookRadius)
        {
            LookAtPos(player.position);

            state = EStates.Chase;
            //if playervisible Check

            //if (Vector3.Dot(transform.forward, toPlayer) >= 0.67f)
            //{
            //    LookAtPlayer();
            //    if (Vector3.Dot(transform.forward, toPlayer) >= 0.9f)
            //    {
            //        state = EStates.Chase;
            //    }
            //}
        }
        
        Debug.DrawRay(transform.position, transform.forward, Color.blue);

        rotate = Quaternion.identity;
        moveVec = Vector3.zero;
    }

    private void Attack()
    {
        attackTime -= Time.deltaTime;
        //
        //print("ready"+ PlayerAttacker.readyToAttack +" atime"+attackTime);
        if (attackTime < 0)
        {
            PlayerAttacker.TriggerAttack(1);
            attackTime = attackCD;
        }

        if (distanceToPlayer > lookRadius)
        {
            state = EStates.Chase;
        }

        if (distanceToPlayer > attackRadius && PlayerAttacker.readyToAttack)
        {
            state = EStates.Chase;
        }
        rotate = Quaternion.identity;
    }

    private void Chase()
    {
        LookAtPos(player.position);
        //moveVec = toPlayer * moveSpeed;
        Move(toPlayer);
        if (distanceToPlayer <= attackRadius)
        {
            moveVec = Vector3.zero;
            state = EStates.Attack;
        }

        if (Vector3.Distance(startPos, transform.position) > chaseRadius)
        {
            state = EStates.Returning;
        }
    }

    private void Move(Vector3 dir)
    {
        moveVec = dir * moveSpeed;
    }

    private void Return()
    {
        if (!startPos.Equals(transform.position, 0.1f))
        {
            LookAtPos(startPos);
            var toStartPos = (startPos - transform.position).normalized;
            Move(toStartPos);

            if (distanceToPlayer < lookRadius && (startPos-player.position).magnitude < chaseRadius)
            {
                state = EStates.Chase;
            }
            //moveVec = toStartPos.normalized * moveSpeed;
        }
        else
        {
            state = EStates.Idle;
        }
    }

    private void LookAtPos(Vector3 pos)
    {
        var lookDir = (pos - transform.position).normalized;
        var lookRot = Quaternion.LookRotation(lookDir, Vector3.up);
        var deltaRot = lookRot * Quaternion.Inverse(transform.rotation);
        rotate = deltaRot;// Quaternion.RotateTowards(rotate, deltaRot, rotateSpeed);
        Debug.DrawRay(transform.position, lookDir);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRadius);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, lookRadius);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(startPos, chaseRadius);
    }
}
