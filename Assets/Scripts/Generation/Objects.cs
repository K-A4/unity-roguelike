using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Objects")]
public class Objects : ScriptableObject
{
    public AnimationCurve DistributionCurve;
    public List<GameObject> ObjectList = new List<GameObject>();

    public GameObject GetSpawnObject()
    {
        var r = Random.value;
        var distR = Mathf.Clamp01(DistributionCurve.Evaluate(r));
        var rInd = (int)(distR * ObjectList.Count);
        return ObjectList[rInd];
    }
}
