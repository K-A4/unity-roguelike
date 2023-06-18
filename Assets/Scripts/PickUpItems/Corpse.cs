using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Corpse : UsableItem
{
    public int value = 3;
    [SerializeField] private float acidRate;
    public override void Consume(Player player)
    {
        var r = Random.value - acidRate;
        int healValue = - value;
        if (r < 0)
        {
            healValue = 1;
        }
        player.GetHit(healValue, player.transform.position);
    }

    public override void OnDestroyItem()
    {
        Inventory.RemoveItem(this);
        Destroy(gameObject);
    }
}
