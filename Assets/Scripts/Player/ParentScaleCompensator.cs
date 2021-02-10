using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParentScaleCompensator : MonoBehaviour
{
    public Vector3 desiredScale = Vector3.one;

    private void LateUpdate()
    {
        // it is essential to keep such stuff in late update!
        transform.localScale = new Vector3(
            desiredScale.x / transform.parent.transform.localScale.x, 
            desiredScale.y / transform.parent.transform.localScale.y, 
            desiredScale.z / transform.parent.transform.localScale.z);
    }
}
