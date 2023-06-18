using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PhaseInfo
{
    public int IntegerPeriod;
    public float FloatCD;
    public float MoveSpeed;
}

public class FirstStageBoss : Hittable
{
    public GameObject VomitZone;
    [SerializeField] private List<PhaseInfo>  phasesStats = new List<PhaseInfo>();
    [SerializeField] private ParticleSystem vomitParticles;
    [SerializeField] private int maxHealth;
    [SerializeField] private float rotSpeed;

    private PhaseInfo stat => phasesStats[phase];
    private int phase = 0;
    private Hittable player;
    private Rigidbody rb;
    private Vector3 dir;
    private float jumpTime;
    private int jumps;
    private bool move = false;

    public void SetMoving(float speed)
    {
        move = speed > 0;
    }

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Hittable>();
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        health = maxHealth;
    }
    
    public void VomitAttack()
    {
        move = false;
        animator.SetTrigger("Vomit");
    }

    public void StartVomitParticles()
    {
        vomitParticles.Play();
        VomitZone.SetActive(true);
    }

    public void StopVomitParticles()
    {
        vomitParticles.Stop();
        VomitZone.SetActive(false);
    }

    private void Update()
    {
        jumpTime -= Time.deltaTime;

        if (jumpTime < 0)
        {
            animator.SetTrigger("Jump");
            jumpTime = stat.FloatCD;
            jumps++;
        }
        
        if (jumps >= stat.IntegerPeriod)
        {
            VomitAttack();
            jumps = 0;
        }

        if (!move)
        {
            if (!vomitParticles.isPlaying)
            {
                dir = (player.transform.position - transform.position).normalized;
                dir.y = 0;
                transform.forward = dir;
            }
        }

        if (health / maxHealth < 0.5f && phase != 1)
        {
            phase = 1;
        }
    }

    private void FixedUpdate()
    {
        var rbvel = rb.velocity;
        rbvel.x = 0;
        rbvel.z = 0;
        rb.velocity = rbvel;

        if (move)
        {
            rb.velocity = stat.MoveSpeed * dir;
            transform.position += stat.MoveSpeed * dir * Time.deltaTime;
        }

        if (phase == 1)
        {
            if (vomitParticles.isPlaying)
            {
                var rot = Quaternion.Euler(0, rotSpeed * Time.fixedDeltaTime, 0);
                transform.rotation = rot * transform.rotation;
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        var go = collision.gameObject;
        
        if (go.CompareTag("Player"))
        {
            if (health > 0)
            {
                player.GetHit(1, collision.GetContact(0).point);
            }
        }
    }

    public override void Die(Vector3 pos)
    {
        base.Die(pos);
        //OnDie?.Invoke();
        //StopVomitParticles();
        //animator?.SetTrigger("Die");
        //PlayDieParticles(pos);
        
        move = false;
    }
}
