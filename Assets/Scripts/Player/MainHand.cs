using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MainHand : MonoBehaviour, IItemHoverEnter, IItemHoverExit
{
    //public UnityEvent<Transform> OnSwitch;
    public UnityEvent<Item> OnPickUpItem;
    [SerializeField] private SpriteRenderer itemSprite;
    [SerializeField] private RayCaster ray;
    private Item selectedItem;

    public void UseSelectedItem(Player player)
    {
        var usableItem = selectedItem as UsableItem;
        if (usableItem)
        {
            usableItem.TriggerUse(player);
        }
    }

    public void OnItemChangeed(Item item)
    {
        itemSprite.sprite = item ? item.sprite : null;
        selectedItem = item;
    }

    private void Update()
    {
        var hoverObject = ray.hoverObject;
        if (Input.GetKeyDown(KeyCode.T))
        {
            if (hoverObject)
            {
                if (hoverObject.transform.TryGetComponent<Item>(out Item item))
                {
                    OnPickUpItem?.Invoke(item);
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (selectedItem)
            {
                selectedItem.transform.position = transform.position;
                selectedItem.Inventory.RemoveItem(selectedItem);
            }
        }
        //if (Input.GetKeyDown(KeyCode.F))
        //{
        //    if (selectedItem)
        //    {
        //        OnSwitch?.Invoke(selectedItem.transform);
        //    }
        //}
    }


    public void OnHoverBegin(RayCaster e)
    {
    }

    public void OnHoverEnd(RayCaster e)
    {
    }
}
