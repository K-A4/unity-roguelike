using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wand : UsableItem
{
    [SerializeField] private int charges = 5;
    public override void Consume(Player player)
    {
        charges--;
        WandZap(player);
        UIText.Instance.SendText("CHARGES LEFT " + charges);
    }

    public override void OnDestroyItem()
    {
        if (charges <= 0)
        {
            Inventory.RemoveItem(this);
            Destroy(gameObject);
        }
    }

    public virtual void WandZap(Player player) { }
}
