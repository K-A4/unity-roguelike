using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Item : MonoBehaviour
{
    public Sprite sprite;

    public InventorySystem Inventory { get; set; }

    private void Awake()
    {
        sprite = GetComponent<SpriteRenderer>().sprite;
    }
}
