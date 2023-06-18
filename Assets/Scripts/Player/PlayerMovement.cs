using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Camera PlayerCamera;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float sensetivity;
    [SerializeField] private float stepPeriod;
    [SerializeField] private AudioSource stepSound;
    private float steps = 0.4f;
    public float MoveSpeed
    {
        get { return moveSpeed; }
        set
        {
            if (moveSpeed > 0)
            {
                moveSpeed = value;
            } 
        }
    }

    private float angle;
    private Vector3 deltaMouse  => new Vector3(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"), 0);
    private Vector3 inputVec;
    private Rigidbody rb;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        angle -= deltaMouse.y * sensetivity;
        angle = Mathf.Clamp(angle, -90, 90);
        transform.rotation = Quaternion.Euler(0, deltaMouse.x * sensetivity, 0) * transform.rotation;

        PlayerCamera.transform.rotation = transform.rotation;
        var localRot = PlayerCamera.transform.localRotation;
        PlayerCamera.transform.localRotation = Quaternion.Euler(angle, localRot.eulerAngles.y, 0);

        inputVec = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        if (inputVec != Vector3.zero)
        {
            steps += /*Mathf.Sign(steps) **/ Time.deltaTime;

            if (steps > stepPeriod)
            {
                steps = 0;
                stepSound.Play();
            }
        }
        else
        {
            steps = 0.4f;
        }
        inputVec = Vector3.ClampMagnitude(inputVec, 1.0f);
        
    }

    private void FixedUpdate()
    {
        CameraMovement();
        var velocity = rb.velocity;
        var moveVec = transform.forward * inputVec.z + transform.right * inputVec.x;
        velocity = Vector3.MoveTowards(velocity, moveVec * moveSpeed, 100 * Time.deltaTime);
        
        //velocity = moveVec * moveSpeed;
        //Physics.SphereCast
        rb.velocity = velocity;
    }

    private void CameraMovement()
    {
        PlayerCamera.transform.position = Vector3.Lerp(PlayerCamera.transform.position, transform.position, Time.deltaTime * 20);
    }
}
