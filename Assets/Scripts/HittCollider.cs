using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HittCollider : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        var go = collision.gameObject;

        if (go.CompareTag("Player"))
        {
            var player = go.GetComponent<Hittable>();
            player.GetHit(1, collision.GetContact(0).point);
        }
    }
}
