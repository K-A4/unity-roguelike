using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class Grabber : MonoBehaviour, IItemHoverEnter, IItemHoverExit
{
    public RayCaster ray;

    public Vector3 Graboffset;
    public Vector3 GrabScale;
    public float ThrowForceMin;
    public float ThrowForceMax;
    public bool grabbing { get; private set; }
    public Transform GrabbedItem { get { return grabbedObject; } private set { } }
    //[SerializeField] private UnityEvent<bool> onHover;
    [SerializeField] private UnityEvent onHoverBegin;
    [SerializeField] private UnityEvent onHoverEnd;
    [SerializeField] private Transform grabbHand;
    [SerializeField] private float throwMaxTime;

    private LayerMask grabbedLayer;
    public Transform grabbedObject;
    private float throwTime;

    public void SetGrabItem(Transform item)
    {
        if (grabbing)
        {
            UnGrab();
        }

        grabbing = true;

        item.gameObject.SetActive(true);
        Grab(item.transform);
    }

    //public void OnHover(bool isHover, Transform tr)
    //{
    //    onHover?.Invoke(tr && tr?.tag == "Grabbable" && isHover);
    //}

    private void Update()
    {
        if (Input.GetMouseButton(1))
        {
            throwTime += Time.deltaTime;
        }

        //var hitTag = ray.HoverObject?.tag;// gameObject.layer;
        //if (hitTag == "Grabbable")
        //{
        //    if (Input.GetKeyDown(KeyCode.G))
        //    {
        //        //if (grabbing)
        //        //{
        //        //    UnGrab();
        //        //}

        //        //grabbing = true;
        //        //if (grabbing)
        //        //{
        //        //    Grab(ray.HoverObject);
        //        //}
        //        SetGrabItem(ray.HoverObject.transform);
        //    }
        //}

        if (grabbing)
        {
            if (!grabbedObject)
            {
                grabbing = false;
            }
            grabbedObject.transform.position = grabbHand.transform.position;
            grabbedObject.transform.rotation = grabbHand.transform.rotation;
            grabbedObject.localScale = grabbHand.transform.localScale;
            grabbedObject.gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
            //grabbedObject.collider.enabled = false;

            if (Input.GetMouseButtonUp(1))
            {
                Throw(Mathf.Clamp01(throwTime / throwMaxTime));
                throwTime = 0;
                grabbing = false;
            }
        }
    }

    private void Grab(Transform grabItem)
    {
        grabbedObject = grabItem;
        grabbedLayer = grabbedObject.transform.gameObject.layer;
    }

    private void Throw(float alpha)
    {
        var throwForce = ThrowForceMin + alpha * (ThrowForceMax - ThrowForceMin);

        if (grabbedObject.TryGetComponent(out Rigidbody rb))
        {
            rb.AddForce((transform.forward + (Vector3.up * 0.5f)) * throwForce , ForceMode.VelocityChange);
        }
        grabbedObject.gameObject.AddComponent<Throwable>();
        UnGrab();
    }

    public void UnGrab()
    {
        grabbing = false;
        grabbedObject.transform.localScale = Vector3.one;
        grabbedObject.transform.rotation = Quaternion.identity;
        grabbedObject.gameObject.layer = grabbedLayer;
        grabbedObject = null;
    }

    public void OnHoverBegin(RayCaster e)
    {
        var isGrabbable = e.HoverHit.transform ? e.HoverHit.transform?.tag == "Grabbable" : false;
        if (isGrabbable)
        {
            onHoverBegin.Invoke();
        }
    }

    public void OnHoverEnd(RayCaster e)
    {
        onHoverEnd.Invoke();
    }
}
