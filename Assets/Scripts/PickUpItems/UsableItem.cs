using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UsableItem : Item
{
    public virtual void TriggerUse(Player player) 
    {
        player.UseAnimation(this);
        player.OnItemUsed.AddListener(OnDestroyItem);
    }
    public abstract void Consume(Player player);
    public abstract void OnDestroyItem();
}
