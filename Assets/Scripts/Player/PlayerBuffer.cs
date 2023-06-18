using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBuffer : MonoBehaviour
{
    public PlayerMovement playerMovement;
    public Attacker Attacker;

    private void Start()
    {
        
    }

    private void Update()
    {
        
        //playerMovement.MoveSpeed = Mathf.Sin(Time.time) + 1.0f;
    }

    public void DoubleDamage()
    {

    }

    public void BuffSpeed(float value)
    {
        if (value > 0)
        {
            playerMovement.MoveSpeed += value;
        }
    }
}
