using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HelloScreen : MonoBehaviour
{
    public int clicktimes = 2;
    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            clicktimes--;
        }

        if (clicktimes < 0)
        {
            Destroy(gameObject);
        }
    }
}
