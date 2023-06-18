using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpritesAnimator : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    [SerializeField] private bool flipOnWest;
    private Animator animator;
    private Camera mainCamera;

    private void Start()
    {
        mainCamera = Camera.main;
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        //Debug.DrawRay(transform.position, SpriteEntity.forward,Color.blue);
        var forward = new Vector3(mainCamera.transform.forward.x, 0, mainCamera.transform.forward.z);
        //Debug.DrawRay(transform.position, forward * 2,Color.blue / 2);
        var angle = (mainCamera.transform.rotation * Quaternion.Inverse(transform.rotation)).eulerAngles.y;
        var agnIndx = (int)((angle / 90 + 0.5) % 4);
        animator.SetFloat("Side", agnIndx);

        if (flipOnWest)
        {
            spriteRenderer.flipX = agnIndx == 1;
        }
    }
}
