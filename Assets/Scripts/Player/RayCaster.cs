using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public interface IItemHoverEnter
{
    public void OnHoverBegin(RayCaster e);
}

public interface IItemHoverExit
{
    public void OnHoverEnd(RayCaster e);
}

public class RayCaster : MonoBehaviour
{
    public LayerMask InteractLayers;
    public Transform HoverObject { get { return hoverObject; } protected set { } }
    public RaycastHit HoverHit;
    public Vector3 HoverPosition;
    //public RaycastHit HoverObject
    //[SerializeField] private UnityEvent<bool, Transform> onHover;
    private UnityEvent<RayCaster> onHoverBegin = new UnityEvent<RayCaster>();
    private UnityEvent<RayCaster> onHoverEnd = new UnityEvent<RayCaster>();

    public Transform hoverObject = null;
    private void Start()
    {
        var hoverEnterObjects = FindInterfaces.Find<IItemHoverEnter>();
        //var hoverEnterObjects = FindObjectsOfType<MonoBehaviour>().OfType<IItemHoverEnter>();
        foreach (IItemHoverEnter hoverable in hoverEnterObjects)
        {
            onHoverBegin.AddListener(hoverable.OnHoverBegin);
        }

        var hoverExitObjects = FindInterfaces.Find<IItemHoverExit>();
        foreach (IItemHoverExit hoverable in hoverExitObjects)
        {
            onHoverEnd.AddListener(hoverable.OnHoverEnd);
        }
    }

    private void Update()
    {
        RaycastHit hit;

        if (Physics.Raycast(transform.position, transform.forward, out hit, 1.5f, InteractLayers) /*&& hit.transform.gameObject.layer == InteractLayers*/)
        {
            if (hoverObject == null || hoverObject != hit.transform)
            {
                HoverHit = hit;
                if (hoverObject != null)
                    onHoverEnd?.Invoke(this);

                HoverObject = hoverObject = hit.transform;
                onHoverBegin?.Invoke(this);
            }
            HoverPosition = hit.point;
        }
        else
        {
            if (hoverObject != null)
                onHoverEnd?.Invoke(this);

            HoverObject = hoverObject = null;
        }

        //onHover?.Invoke(hoverObject != null, hit.transform ?? null);
    }
}
