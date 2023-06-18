using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIHearts : MonoBehaviour
{
    public GameObject HeartPrefab;
    private int heartNumber => transform.childCount;

    public void SetHearts(int n)
    {
        var diff = heartNumber - n;
        
        if (diff > 0)
        {
            DecriaseHearts(diff);
        }
        else
        {
            IncreaseHearts(-diff);
        }
    }

    private void DecriaseHearts(int n)
    {
        var hearts = heartNumber;
        for (int i = hearts - 1; i > (hearts - 1) - n; i--)
        {
            Destroy(transform.GetChild(i).gameObject);
        }
    }

    private void IncreaseHearts(int n)
    {
        for (int i = 0; i < n; i++)
        {
            Instantiate(HeartPrefab, transform);
        }
    }
}
