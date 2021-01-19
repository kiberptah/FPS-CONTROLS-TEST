using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetPushedFromAttack : MonoBehaviour
{
    Rigidbody rb;
    public float pushStrength;
    private void OnEnable()
    {
        EventDirector.someAttack += GetPushed;
    }
    private void OnDisable()
    {
        EventDirector.someAttack -= GetPushed;

    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    void GetPushed(Transform _attacker, Transform _whom, Vector3 _point, float _amount)
    {
        if (_whom == transform)
        {
            rb.AddForce((transform.position - _attacker.position).normalized * pushStrength, ForceMode.VelocityChange);
        }
    }
}
