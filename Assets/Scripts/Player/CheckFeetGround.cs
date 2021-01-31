using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckFeetGround : MonoBehaviour
{
    public bool isColliding = false;
    public Transform surface;

    private void OnTriggerStay(Collider other)
    {
        if (other.transform.tag != "Player")
        {
            isColliding = true;
            surface = other.transform;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform.tag != "Player")
        {
            isColliding = false;
            surface = null;
        }
    }
}
