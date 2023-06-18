using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lightning : MonoBehaviour
{
    [SerializeField] private float zapTime;
    [SerializeField] private Transform lighnObject;

    private void Start()
    {
        StartCoroutine(ZapCoroutine());
    }

    private IEnumerator ZapCoroutine()
    {
        var timeElapse = 0.0f;
        while (timeElapse < zapTime)
        {
            timeElapse += Time.deltaTime;
            var r = ((Random.value - 0.5f) * 2.0f);

            if (Mathf.Abs(r) > 0.5f)
            {
                lighnObject.localRotation = Quaternion.Euler(0, 0, r * 30.0f);
                var scale = lighnObject.localScale;
                scale.x = r ;
            }
            yield return null;
        }
        Destroy(gameObject);
        yield break;
    }
}
