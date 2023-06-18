using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningWand : Wand
{
    [SerializeField] GameObject lightning;
    [SerializeField] private Color flashColor;
    [SerializeField] private float zapDistance;
    [SerializeField] private LayerMask zapLayer;
    [SerializeField] private float zapDamage;

    public override void WandZap(Player player)
    {
        Flash(player);
        ZapMonsters(player);
        Instantiate(lightning, player.transform.position, player.transform.rotation, null);
    }

    private void ZapMonsters(Player player)
    {
        //player.
        var hits = Physics.RaycastAll(player.transform.position, player.transform.forward, zapDistance, zapLayer);
        foreach (var hit in hits)
        {
            if (hit.transform.gameObject.TryGetComponent(out Hittable hitter))
            {
                hitter.GetHit(zapDamage, hit.point);
            }
        }
    }


    private void Flash(Player player)
    {
        var col = flashColor;
        player.HitScreen.color = col;
    }
}
