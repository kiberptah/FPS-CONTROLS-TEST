using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceImpulse : MonoBehaviour
{
    public float strengh = 50f;
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMouseDown()
    {
        gameObject.GetComponent<Rigidbody>().AddRelativeForce(Vector3.forward * strengh, ForceMode.Impulse);

    }
}
