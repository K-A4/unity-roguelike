using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scroll : UsableItem
{
    [SerializeField] private int charges = 1;

    public override void Consume(Player player)
    {
        charges--;
        ReadScroll(player);
        UIText.Instance.SendText("SCROLL OF NOTHING");
    }

    public override void OnDestroyItem()
    {
        if (charges <= 0)
        {
            Inventory.RemoveItem(this);
            Destroy(gameObject);
        }
    }
    public virtual void ReadScroll(Player player) { }
}
