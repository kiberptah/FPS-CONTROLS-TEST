using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionDetection : MonoBehaviour
{
    public bool isColliding = false;
    public Collider collision = new Collider();

    private void OnTriggerEnter(Collider other)
    {
        isColliding = true;
        collision = other;
    }

    private void OnTriggerExit(Collider other)
    {
        isColliding = false;
        collision = other;
    }
}
