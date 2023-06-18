using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InventorySystem : MonoBehaviour
{
    public Item SelectedItem;
    public int SelectedIndex;
    public List<Item> Items = new List<Item>();
    public UnityEvent<Item> OnSelect;
    [SerializeField] private InventoryBar inventorySlots;

    private void Start()
    {
        foreach (var item in Items)
        {
            item.Inventory = this;
        }
        inventorySlots.OnSelect.AddListener(SelectItem);
        inventorySlots.Redraw(Items);
    }

    public void SelectItem(int i)
    {
        SelectedIndex = i;

        if (i < Items.Count)
        {
            SelectedItem = Items[i];
        }
        else
        {
            SelectedItem = null;
        }
        OnSelect?.Invoke(SelectedItem);
    }

    public void AddItem(Item item)
    {
        if (Items.Count < inventorySlots.Count)
        {
            item.gameObject.SetActive(false);

            Items.Add(item);
            item.Inventory = this;
            //item.OnItemDestroy.AddListener(()=>RemoveItem)
        }
        inventorySlots.Redraw(Items);
    }

    public Item Switch(Item item, int switchIndex)
    {
        item?.gameObject.SetActive(false);
        Item retItem = null;
        if (switchIndex >= Items.Count)
        {
            Items.Add(item);
        }
        else
        {
            retItem = Items[switchIndex];
            Items[switchIndex] = item;
        }
        
        inventorySlots.Redraw(Items);
        return retItem;
    }

    public void RemoveItem(int i)
    {
        Items[i].gameObject.SetActive(true);
        Items.RemoveAt(i);
        inventorySlots.Redraw(Items);
    }

    public void RemoveItem(Item item)
    {
        item.gameObject.SetActive(true);
        Items.Remove(item);
        inventorySlots.Redraw(Items);
    }
}
