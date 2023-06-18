using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteRotator : MonoBehaviour
{
    private Camera mainCamera;

    private void Start()
    {
        mainCamera = Camera.main;
    }

    private void Update()
    {
        RotateToLook();
    }

    private void RotateToLook()
    {
        var camRot = mainCamera.transform.rotation;
        var rot = transform.rotation.eulerAngles;
        transform.rotation = Quaternion.Euler(0, camRot.eulerAngles.y, 0);
    }
}
