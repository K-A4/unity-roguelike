using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSegment : MonoBehaviour
{
    public Vector3 PreviousPosition;
    public BossSegment SegmentPrefab;
    public int SegmentCount;
    public void SetParentSegment(BossSegment ps) { parentSegment = ps; }
    [SerializeField] private BossSegment parentSegment;
    [SerializeField] bool isMain;
    private float distanceToParent;

    private void Start()
    {
        var myhitt = GetComponent<Hittable>();
        if (SegmentCount > 0)
        {
            var s = Instantiate(SegmentPrefab);
            s.SetParentSegment(this);
            s.SegmentPrefab = SegmentPrefab;
            s.transform.position = transform.position - (Vector3.up * 0.5f);
            s.SegmentCount = SegmentCount - 1;
            var hittable = s.GetComponent<Hittable>();
            hittable.HitSound = myhitt.HitSound;
            hittable.Particles = myhitt.Particles;

            //hittable.OnGetHit.AddListener(
            //    (s) =>
            //        {
            //            var myhitt = GetComponent<Hittable>();
            //            myhitt.GetHitSound.transform.position = hittable.transform.position;
            //            myhitt.GetHitSound.Play();
            //            myhitt.GetParticles.transform.position = hittable.transform.position;
            //            myhitt.GetParticles.Play();
            //        }
            //);
        }

        PreviousPosition = transform.position;
        if (parentSegment)
        {
            distanceToParent = (parentSegment.transform.position - transform.position).magnitude;
        }
    }

    private void Update()
    {
        var currentPosistion = transform.position;
        var deltaPosiotion = currentPosistion - PreviousPosition;
        
        if (!isMain)
        {
            var distanceToParent = (parentSegment.PreviousPosition - transform.position).magnitude;
            var newdelta = distanceToParent - this.distanceToParent;

            transform.position += newdelta * (parentSegment.PreviousPosition - transform.position).normalized;
        }
        if (!isMain && !parentSegment)
        {
            Destroy(gameObject);
        }

        PreviousPosition = currentPosistion;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        if (parentSegment)
        {
            Gizmos.DrawLine(transform.position, parentSegment.transform.position);
        }
    }
}
