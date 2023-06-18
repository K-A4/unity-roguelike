using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Interactor : MonoBehaviour, IItemHoverEnter, IItemHoverExit
{
    public RayCaster ray;
    
    //[SerializeField] private UnityEvent<bool> onHover;
    [SerializeField] private UnityEvent onHoverBegin;
    [SerializeField] private UnityEvent onHoverEnd;

    //private Transform hoverObject;
    //private RaycastHit hoverObject;


    private void Update()
    {
        Interact();
    }

    public void Interact()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Transform hoverObject = ray.HoverObject;
            Vector3 point = ray.HoverHit.point;
            if (hoverObject)
            {
                //if (hoverObject?.transform.gameObject.layer == LayerMask.NameToLayer("Interact"))
                //{
                if (hoverObject.transform.TryGetComponent(out Iinteractable interactItem))
                {
                    //if (interactItem.IsReady())
                    //{
                    InteractWith(interactItem, point);
                    //}
                    //}
                }
            }
        }
    }
    
    private void InteractWith(Iinteractable interactItem, Vector3 pos)
    {
        interactItem.Interact(pos);
    }

    public void OnHoverBegin(RayCaster e)
    {
        var isValidLayer = e.HoverHit.transform ? e.HoverHit.transform.gameObject.layer == LayerMask.NameToLayer("Interact") : false;
        //var isValidLayer = tr ? tr.gameObject.layer == LayerMask.NameToLayer("Interact") : false;
        
        if (isValidLayer)
        {
            onHoverBegin?.Invoke();
        }
    }

    public void OnHoverEnd(RayCaster e)
    {
        onHoverEnd?.Invoke();
    }

    //public void OnHover(bool isHover, Transform tr)
    //{
    //    var isValidLayer = tr ? tr.gameObject.layer == LayerMask.NameToLayer("Interact") : false;
    //    onHover?.Invoke(isHover && isValidLayer);
    //    onHoverEnd?.Invoke();
    //}
    //private void Update()
    //{
    //    RaycastHit hit;

    //    if (Physics.Raycast(transform.position, transform.forward, out hit, 2, InteractLayers) /*&& hit.transform.gameObject.layer == InteractLayers*/)
    //    {
    //        if (hoverObject == null || hoverObject != hit.transform)
    //        {

    //            if (hoverObject != null)
    //                onHoverEnd.Invoke(hoverObject);

    //            hoverObject = hit.transform;
    //            onHoverBegin.Invoke(hoverObject);
    //        }

    //        var hitLayer = hit.transform?.gameObject.layer;

    //        if (Input.GetKeyDown(KeyCode.E))
    //        {
    //            if (hitLayer == LayerMask.NameToLayer("Interact"))
    //            {
    //                if (hit.transform.TryGetComponent(out Iinteractable interactItem))
    //                {
    //                    //if (interactItem.IsReady())
    //                    //{
    //                    InteractWith(interactItem, hit.point);
    //                    //}
    //                }
    //            }
    //        }
    //    }
    //    else
    //    {
    //        if (hoverObject != null)
    //            onHoverEnd.Invoke(hoverObject);

    //        hoverObject = null;
    //    }

    //    onHover?.Invoke(hoverObject != null);
    //}

    //private void InteractWith(Iinteractable interactItem, Vector3 pos)
    //{
    //    interactItem.Interact(pos);
    //}
}
