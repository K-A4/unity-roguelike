using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float projectileSpeed;
    [SerializeField] private float flightDistance;
    [SerializeField] private GameObject dropitem;

    private void Start()
    {
        StartCoroutine(FlightCoroutine());
    }

    private IEnumerator FlightCoroutine()
    {
        var distanceElapse = 0.0f;
        var lastDistance = 0.15f * flightDistance;
        while (distanceElapse < flightDistance)
        {
            distanceElapse += projectileSpeed * Time.deltaTime;
            if (distanceElapse > flightDistance - lastDistance)
            {
                var scale = Vector3.Lerp(Vector3.one, Vector3.zero, distanceElapse - (flightDistance - lastDistance) / lastDistance);
                transform.localScale = scale;
            }
            transform.position += transform.forward * projectileSpeed * Time.deltaTime;
            yield return null;
        }
        Destroy(gameObject);
        yield break;
    }

    private void OnCollisionEnter(Collision collision)
    {
        var newForward = Vector3.Reflect(transform.forward, collision.GetContact(0).normal);
        transform.forward = newForward;

        if (collision.gameObject.TryGetComponent(out Hittable hitter))
        {
            hitter.GetHit(1, collision.GetContact(0).point);
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        if (dropitem)
        {
            Instantiate(dropitem, transform.position, transform.rotation);
        }
    }
}
