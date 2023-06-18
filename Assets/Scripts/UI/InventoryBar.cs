using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class InventoryBar : MonoBehaviour
{
    public UnityEvent<int> OnSelect;
    public int Count => inventorySlots.Count;
    [SerializeField] private Color selectionColor;
    [SerializeField] private float selectionScale;
    private List<Image> inventorySlots = new List<Image>();
    private Image selectedSlot;
    private int selectedIndex = 0;

    public void Redraw(List<Item> items)
    {
        for (int i = 0; i < inventorySlots.Count; i++)
        {
            if (i < items.Count)
            {
                items[i].gameObject.SetActive(false);
                SetItemImage(i, items[i].sprite);
            }
            else
            {
                ClearItemImage(i);
            }
        }
        SelectSlot(selectedIndex);
    }

    private void Awake()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            inventorySlots.Add(transform.GetChild(i).GetComponent<Image>());
        }

        SelectSlot(selectedIndex);
    }

    private void Update()
    {
        Selection();
    }

    private void ClearItemImage(int i)
    {
        Image slotImage = inventorySlots[i].transform.GetChild(0).GetComponent<Image>();
        slotImage.enabled = false;
        slotImage.sprite = null;
    }

    private void SetItemImage(int i, Sprite image)
    {
        Image slotImage = inventorySlots[i].transform.GetChild(0).GetComponent<Image>();
        slotImage.sprite = image;
        slotImage.enabled = true;
    }

    private void Selection()
    {
        var wheelDelta = Input.mouseScrollDelta.y;
        if (int.TryParse(Input.inputString, out int i))
        {
            
            if (i >= 0 && inventorySlots.Count >= i)
            {
                SelectSlot(i - 1);
            }
        }
        if (wheelDelta != 0)
        {
            selectedIndex = (selectedIndex - (int)(wheelDelta)) % inventorySlots.Count;
            if (selectedIndex < 0)
            {
                selectedIndex += inventorySlots.Count;
            }
            SelectSlot(selectedIndex);
        }
    }

    private void SelectSlot(int i)
    {
        selectedIndex = i;
        if (selectedSlot)
        {
            selectedSlot.color = Color.white;
            selectedSlot.transform.localScale = Vector3.one;
        }
        selectedSlot = inventorySlots[i];
        selectedSlot.color = selectionColor;
        selectedSlot.transform.localScale = Vector3.one * selectionScale;
        OnSelect?.Invoke(i);
    }
}
