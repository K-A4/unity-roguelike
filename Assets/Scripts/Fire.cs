using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : DamageZone
{
    [SerializeField] private float lifeTime;
    [SerializeField] ParticleSystem fireParticles;

    private void Start()
    {
        var ps = fireParticles.main;
        ps.duration = lifeTime;
    }
}
