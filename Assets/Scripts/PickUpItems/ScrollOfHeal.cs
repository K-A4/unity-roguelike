using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollOfHeal : Scroll
{
    [SerializeField] private int heal;

    public override void Consume(Player player)
    {
        base.Consume(player);
        player.GetHit(-heal, player.transform.position);
        UIText.Instance.SendText("YOU WERE BEEN HEALED");
    }
}
