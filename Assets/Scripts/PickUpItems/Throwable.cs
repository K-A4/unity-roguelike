using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Throwable : MonoBehaviour
{
    private LayerMask layer;
    private Rigidbody rb;

    private void Start()
    {
        layer = gameObject.layer;
        rb = GetComponent<Rigidbody>();
        gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
    }

    private void OnCollisionEnter(Collision collision)
    {
        gameObject.layer = layer;
        if (collision.gameObject.TryGetComponent(out Hittable hitter))
        {
            hitter.GetHit(1, collision.GetContact(0).point);
        }
        Destroy(this);
    }
}
