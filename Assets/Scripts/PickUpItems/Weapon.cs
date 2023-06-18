using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : UsableItem
{
    [SerializeField] private int damage = 1;
    public override void TriggerUse(Player player)
    {
        player.Attacker.TriggerAttack(damage);
    }

    public override void Consume(Player player)
    {
    }

    public override void OnDestroyItem()
    {
        
    }
}
