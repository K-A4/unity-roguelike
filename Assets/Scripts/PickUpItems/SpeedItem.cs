using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedItem : PickUpItem
{
    [SerializeField] private float speedBonus;

    public override void Buff(PlayerBuffer player)
    {
        player.BuffSpeed(speedBonus);
    }
}
