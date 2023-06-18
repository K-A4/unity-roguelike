using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollOfLocking : Scroll
{
    public override void Consume(Player player)
    {
        base.Consume(player);
        UIText.Instance.SendText("NOW CHESTS IS LOCKED");
    }

    public override void ReadScroll(Player player)
    {
        var containersCount = ItemContaner.LevelContainers.Count;
        for (int i = 0; i < (int)containersCount * 0.2f; i++)
        {
            var r = Random.value * containersCount;

            ItemContaner.LevelContainers[(int)r].isLocked = true;
        }
    }
}
