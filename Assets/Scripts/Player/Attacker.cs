using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attacker : MonoBehaviour
{
    private RaycastHit[] collides;
    private Animator animator;
    [SerializeField] private float attackDistance;
    [SerializeField] private Vector3 attackExtends;
    [SerializeField] private float damage;
    [SerializeField] private float attackKnockback;
    [SerializeField] private LayerMask attackLayer;

    public bool readyToAttack { get; private set; }

    private void Start()
    {
        readyToAttack = true;
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        
    }

    public void TriggerAttack(int damage)
    {
        this.damage = damage;
        if (readyToAttack)
        {
            animator?.SetTrigger("Attack");
            readyToAttack = false;
        }
    }

    public void Attack()
    {
        var vecOff = new Vector3(0, - 0.1f, 0);
        //print("Attack");
        collides = Physics.BoxCastAll(transform.position + vecOff, attackExtends * 0.5f, transform.forward, transform.rotation, attackDistance, attackLayer);
        
        foreach (var item in collides)
        {
            var rb = item.transform.GetComponent<Rigidbody>();
            var hitInfo = item.transform.GetComponent<Hittable>();
            if (hitInfo)
            {
                hitInfo.GetHit(damage, item.transform.position);
            }
            if (rb)
            {
                rb.AddForce(((item.transform.position - transform.position).normalized + Vector3.up * 0.5f) * attackKnockback, ForceMode.VelocityChange);
            }
        }
    }

    public void OnReadyToAttack()
    {
        //print("readyToAttack");
        readyToAttack = true;
    }

    private void OnDrawGizmos()
    {
        if (collides!=null)
        {
            foreach (var item in collides)
            {
                Gizmos.DrawSphere(item.point, 0.125f);
            }
        }
    }
}
