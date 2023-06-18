using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireWand : Wand
{
    [SerializeField] GameObject fireBall;

    public override void WandZap(Player player)
    {
        Instantiate(fireBall, player.transform.position /*+ player.transform.forward*/, player.transform.rotation, null);
    }
}
