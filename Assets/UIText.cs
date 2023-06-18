using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIText : MonoBehaviour
{
    public static UIText Instance;
    [SerializeField] private TextMeshProUGUI text1;
    [SerializeField] private float showTime;

    private void Awake()
    {
        Instance = this;
    }

    public void SendText(string text)
    {
        StartCoroutine(ShowMassageCor(text));
    }

    private IEnumerator ShowMassageCor(string massage)
    {
        var timeELapse = 0.0f;
        text1.enabled = true;
        while (timeELapse < showTime)
        {
            timeELapse += Time.deltaTime;
            text1.text = massage;
            yield return null;
        }
        text1.text = "";
        text1.enabled = false;
        yield break;
    }
}
