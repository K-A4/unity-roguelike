using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandSwitch : MonoBehaviour
{
    public InventorySystem InventorySystem;
    public Grabber SecondHand;
    public MainHand MainHand;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            var selected = InventorySystem.SelectedIndex;
            Switch(selected);
        }
    }

    private void Switch(int index)
    {
        Item item = null;
        if (SecondHand.GrabbedItem)
        {
            if (SecondHand.GrabbedItem.TryGetComponent(out item))
            {
                SecondHand.UnGrab();
                var invItem = InventorySystem.Switch(item, index);
                if (invItem)
                {
                    SecondHand.SetGrabItem(invItem.transform);
                }
            }
        }
        else if (InventorySystem.SelectedItem)
        {
            SecondHand.SetGrabItem(InventorySystem.SelectedItem.transform);
            InventorySystem.RemoveItem(InventorySystem.SelectedItem);
        }
    }
}
