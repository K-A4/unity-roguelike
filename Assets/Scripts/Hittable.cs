using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Hittable : MonoBehaviour
{
    public UnityEvent<int> OnGetHit;
    public UnityEvent OnDie;

    public AudioSource HitSound { get { return hitSound; } set { hitSound = value; } }
    
    public ParticleSystem Particles { get { return hitParticles; } set { hitParticles = value; } }

    protected Animator animator;

    [SerializeField] protected float health;
    [SerializeField] protected ParticleSystem hitParticles;
    [SerializeField] protected GameObject corpseItem;
    [SerializeField] protected AudioSource hitSound;

    public virtual void GetHit(float damage, Vector3 pos)
    {
        OnGetHit?.Invoke(Mathf.RoundToInt(health));
        health -= damage;
        if (health <= 0)
        {
            Die(pos);
        }

        if (hitSound)
        {
            hitSound.Play();
        }
        PlayDieParticles(pos);
    }

    protected void PlayDieParticles(Vector3 pos)
    {
        if (hitParticles)
        {
            hitParticles.transform.position = pos;
            hitParticles.Play();
        }
    }

    public virtual void Die(Vector3 pos)
    {
        if (gameObject.layer == LayerMask.NameToLayer("Corpse"))
        {
            //hitParticles.transform.SetParent(null);
            //var ps = hitParticles.main;
            //ps.stopAction = ParticleSystemStopAction.Destroy;
            if (corpseItem)
            {
                Instantiate(corpseItem, transform.position , Quaternion.identity);
            }
            OnDie?.Invoke();
            Destroy(gameObject);
        }
        PlayDieParticles(pos);
        gameObject.layer = LayerMask.NameToLayer("Corpse");
        animator?.SetTrigger("Die");
    }

    private void OnDestroy()
    {
        if (!gameObject.scene.isLoaded) return;
        var particles = Instantiate(hitParticles, transform.position, Quaternion.identity);
        var hitsound = particles.gameObject.AddComponent<AudioSource>();
        hitsound.clip = hitSound.clip;
        var ps = particles.main;
        ps.stopAction = ParticleSystemStopAction.Destroy;
        particles.Play();
        hitsound.Play();
    }
}
