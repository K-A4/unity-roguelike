using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    public GameObject ItemPrefab;
    [SerializeField] private float spawnRate = 1;
    [SerializeField] private float spawnForce = 10;
    private float tmieelapse;

    private void Update()
    {
        tmieelapse += Time.deltaTime;

        if (tmieelapse >= spawnRate)
        {
            tmieelapse = 0;
            var rb = Instantiate(ItemPrefab, transform.position, Quaternion.identity).GetComponent<Rigidbody>();
            rb.AddForce(transform.up * spawnForce + new Vector3(Random.value, 0, Random.value) );
        }
    }
}
