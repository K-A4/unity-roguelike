using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollOfMonster : Scroll
{
    public override void Consume(Player player)
    {
        base.Consume(player);
        UIText.Instance.SendText("LOOK BEHIND");
    }

    public override void ReadScroll(Player player)
    {
        var mons = DungeonGeneration.Monsters.GetSpawnObject();
        var pos = Vector3.zero;
        var distTomonster = 3.0f;
        if (Physics.Raycast(player.transform.position, - player.transform.forward, out RaycastHit hit, distTomonster))
        {
            pos = hit.point;
        }
        else
        {
            pos = player.transform.position - (player.transform.forward * distTomonster);
        }
        pos.y = 0.5f;

        Instantiate(mons, pos, Quaternion.identity);
    }
}
