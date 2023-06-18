using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBobbing : MonoBehaviour
{
    Vector2 prevPos;
    private float bobbingValue;
    private float timeElapsed;
    [SerializeField] private float bobbingAmplitude = 1;
    [SerializeField] private float bobbingSpeed = 1;
    [SerializeField] private PlayerMovement player;

    private void Start()
    {
        var tr = transform.position;

        prevPos = new Vector2(tr.x, tr.z);
    }

    private void LateUpdate()
    {
        var tr = transform.position;
        var deltaPos = new Vector2(tr.x, tr.z) - prevPos;
        timeElapsed += Time.deltaTime * (deltaPos.magnitude / Time.deltaTime / player.MoveSpeed) * bobbingSpeed;

        bobbingValue = (Mathf.PingPong(timeElapsed, 1) + 0.5f) / 2 * bobbingAmplitude * (deltaPos.magnitude / Time.deltaTime / player.MoveSpeed);
        //var bobbingvalue = (deltaPos.sqrMagnitude) * bobbingValue;
        print(deltaPos.magnitude / Time.deltaTime / player.MoveSpeed);
        transform.position = new Vector3(tr.x, player.transform.position.y + bobbingValue, tr.z);
        prevPos = new Vector2(tr.x, tr.z);
    }
}
