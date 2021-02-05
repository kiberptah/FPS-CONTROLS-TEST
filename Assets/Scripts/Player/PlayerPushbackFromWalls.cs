using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPushbackFromWalls : MonoBehaviour
{
    public float pushbackStrength = 1f;
    [SerializeField] CollisionDetection collisionPrevention;
    [SerializeField] Rigidbody rb;


    Vector3 horizontalVelocity = Vector3.zero;

    void Awake()
    {
        //TryGetComponent<Rigidbody>(out rb);
        //rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Pushback();
    }

    void Pushback()
    {
        if (collisionPrevention != null)
        {
            if (collisionPrevention.isColliding == false)
            {
                horizontalVelocity = rb.velocity;
                horizontalVelocity.y = 0;
            }
            if (collisionPrevention.isColliding)
            {
                
                rb.AddForce(-horizontalVelocity * pushbackStrength, ForceMode.Acceleration);
/*
                Ray rrr = new Ray(transform.position, transform.position);
                if (rb.velocity != Vector3.zero)
                {
                    rrr = new Ray(transform.position, rb.velocity.normalized);
                }

                if (Physics.Raycast(rrr, 5f, ~LayerMask.GetMask("Player")))
                {
                    
                }*/

            }
        }
        else
        {
            Debug.Log("COLLISION PREVENTION MISSING");
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        /*Vector3 _direction = collision.GetContact(0).normal;
        rb.AddRelativeForce(_direction * pushbackStrength, ForceMode.VelocityChange);*/
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, -horizontalVelocity);
    }
}
