using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SecondStageBoss : Hittable
{
    //public Hittable SecondBossLimbs;
    [SerializeField] private List<PhaseInfo> phasesStats = new List<PhaseInfo>();
    [SerializeField] private int maxHealth;
    [SerializeField] private float phasesTimer = 20.0f;
    [SerializeField] private float TimeBeetweenPhases;
    [SerializeField] private ParticleSystem wormParticles;
    private float phaseTime;

    private Hittable player;
    private Rigidbody rb;
    private PhaseInfo stat => phasesStats[phase];
    private int phase = 0;
    private int wormAttack = 0;
    private float wormAtckTime;
    private bool holding = true;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Hittable>();
        rb = GetComponent<Rigidbody>();
        health = maxHealth;
        phasesStats[0].IntegerPeriod = 1;
    }

    private void Update()
    {
        phaseTime += Time.deltaTime;

        if (holding)
        {
            if (phaseTime > TimeBeetweenPhases)
            {
                phaseTime = 0;
                holding = false;
                phasesStats[0].IntegerPeriod = 1;
                phasesStats[1].IntegerPeriod = 1;
            }
        }
        else
        {
            if (phaseTime > phasesTimer)
            {
                phase = (phase + 1) % phasesStats.Count;
                phaseTime = 0;
                holding = true;
                ReturnMove();
            }

            if (!holding)
            {
                switch (phase % phasesStats.Count)
                {
                    case 0: FirstPhaseAttacks();  break;
                    case 1: SecondPhaseAttacks(); break;
                    default: break;
                }
            }
        }
    }

    private void SecondPhaseAttacks()
    {
        wormAtckTime += Time.deltaTime;
        if (wormAtckTime > stat.FloatCD)
        {
            if (stat.IntegerPeriod > 0)
            {
                stat.IntegerPeriod--;
                ReturnMove();
            }
            else
            {
                FlyAttack();
            }
        }
    }

    private void FirstPhaseAttacks()
    {
        wormAtckTime += Time.deltaTime;
        if (wormAtckTime > stat.FloatCD)
        {
            if (stat.IntegerPeriod > 0)
            {
                stat.IntegerPeriod--;
                StartCoroutine(MoveFromTo(transform.position, transform.position + Vector3.up * 5.0f, stat.MoveSpeed));
            }
            else
            {
                WormAttack();
            }
            wormAtckTime = 0;
        }
    }

    private void WormAttack()
    {
        wormAttack++;
        var r = Random.value;
        var bossSpace = BossRoom.OpenSpace;
        var cellPos = bossSpace[(int)(r * bossSpace.Count)].position;
        var blink = Mathf.Pow(-1, wormAttack);
        var pos = new Vector3(cellPos.x, 0.0f, cellPos.y) + new Vector3(0.5f, 0, 0.5f);

        if (wormParticles)
        {
            var particles = Instantiate(wormParticles);
            particles.transform.position = pos;
        }
        transform.forward = new Vector3(0, - blink, 0).normalized;
        //var pos = new Vector3(cellPos.x, Mathf.Pow(-1, worms), cellPos.y);
        StartCoroutine(MoveFromTo(pos - Vector3.up * blink * 5.0f, pos + Vector3.up * blink * 5.0f, stat.MoveSpeed));
    }

    public void ReturnMove()
    {
        StopAllCoroutines();
        var altarPosition = Altar.Instance.transform.position;
        StartCoroutine(MoveFromTo(altarPosition - (Vector3.up * 5.0f), altarPosition + (Vector3.up * 0.25f), 4));
    }

    private IEnumerator MoveFromTo(Vector3 from, Vector3 to, float speed)
    {
        var TimeElapsed = 0.0f;
        var toFrom = to - from;
        var dist = toFrom.magnitude;
        var medianTime = (dist / speed);
        transform.forward = toFrom.normalized;

        while (true)
        {
            TimeElapsed += Time.deltaTime;
            var t = TimeElapsed / medianTime;
            var pos = Vector3.Lerp(from, to, t);
            transform.position = pos;
            if (t >= 1)
            {
                break;
            }
            yield return null;
        }

        var newForward = player.transform.position - transform.position;
        transform.forward = newForward.normalized;
        yield break;
    }

    private IEnumerator ReturnCoroutine()
    {
        yield return null;
        yield return null;
        yield return null;
        ReturnMove();
        yield break;
    }

    private void FlyAttack()
    {
        var forward = transform.forward;
        forward.y = 0;
        transform.position += forward.normalized * Time.deltaTime * stat.MoveSpeed;
    }

    public override void Die(Vector3 pos)
    {
        base.Die(pos);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag != "Player")
        {
            var newForward = Vector3.Reflect(transform.forward, collision.GetContact(0).normal);
            newForward.y = 0;
            transform.forward = newForward.normalized + Vector3.one * 0.01f;
        }
    }
}
