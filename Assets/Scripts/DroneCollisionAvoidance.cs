using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneCollisionAvoidance : MonoBehaviour
{
    public EnemyStateMachine host;
    public CollisionDetection collisionPrevention;

    Rigidbody rb;

    private void Awake()
    {
        rb = gameObject.GetComponent<Rigidbody>();

    }

    private void Update()
    {

        CollisionPrevention();
    }

    /*
    // Collision behaviour
    private void OnCollisionEnter(Collision collision)
    {
        Vector3 _directionection = -(collision.transform.position - transform.position).normalized;

        rb.AddForce(_directionection * host.moveSpeed, ForceMode.VelocityChange);
    }
    private void OnCollisionStay(Collision collision)
    {
        Vector3 _directionection = -(collision.transform.position - transform.position).normalized;
        transform.Translate(_directionection * Time.deltaTime * host.moveSpeed, Space.World);
    }
    */
    // Collision prevention
    void CollisionPrevention()
    {
        if (collisionPrevention.isColliding == true)
        {
            //Debug.Log("collision prevention: " + collisionPrevention.collision.name);


            Vector3 _directionection = -(collisionPrevention.collision.transform.position - transform.position).normalized;

            /*
            rb.AddForce(_directionection * host.moveSpeed, ForceMode.VelocityChange);
            transform.Translate(_directionection * Time.deltaTime * host.moveSpeed, Space.World);
            */
            Vector3 _current = transform.position;
            Vector3 _offset = transform.position - (collisionPrevention.collision.transform.position - transform.position).normalized;

            float _mod = 0.1f;
            transform.position = new Vector3(Mathf.SmoothStep(_current.x, _offset.x, _mod),
                Mathf.SmoothStep(_current.y, _offset.y, _mod),
                Mathf.SmoothStep(_current.z, _offset.z, _mod));
        }
    }

}
