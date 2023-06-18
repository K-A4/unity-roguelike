using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpItem : MonoBehaviour
{
    public virtual void Buff(PlayerBuffer player)
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            var buffedPlayer = collision.transform.GetComponent<PlayerBuffer>();
            Buff(buffedPlayer);    
            Destroy(gameObject);
        }
    }
}
