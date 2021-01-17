using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    public bool isGrounded = false;
    public float sphereCastRadius = 1;


    void FixedUpdate()
    {
        if (Physics.CheckSphere(transform.position, sphereCastRadius, ~LayerMask.GetMask("Player")))
        {
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }

    }

    private void OnDrawGizmos()
    {
        //Gizmos.DrawSphere(transform.position, sphereCastRadius);
    }




}
