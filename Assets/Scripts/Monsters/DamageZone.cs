using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageZone : MonoBehaviour
{
    [SerializeField] private int damage;
    [SerializeField] private float cd;
    private float lastHitted;

    private void Update()
    {
        var pos = transform.position;
        pos.y = 0.1f;
        transform.position = pos;
    }

    private void OnTriggerStay(Collider other)
    {
        if (Time.time > lastHitted)
        {
            lastHitted = Time.time + cd;

            var go = other.gameObject;
            var hittable = go.GetComponent<Hittable>();
            print(hittable);
            if (hittable)
            {
                hittable.GetHit(damage, go.transform.position);
            }
        }
    }
}
