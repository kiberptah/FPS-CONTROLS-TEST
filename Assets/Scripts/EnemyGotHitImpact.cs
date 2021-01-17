using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGotHitImpact : MonoBehaviour
{
    [SerializeField] private float pushStrength = 5f;
    [SerializeField] private GameObject hitFXPrefab;
    [SerializeField] private float FXTimeLength = 5f;
    Rigidbody rb;

    private void Awake()
    {
        rb = transform.GetComponent<Rigidbody>();
    }
    public void SpawnParticlesOnHit(Vector3 _attackerPosition, Vector3 _hitPoint)
    {
        GameObject hitFX = Instantiate(hitFXPrefab, _hitPoint, Quaternion.identity);
        hitFX.transform.LookAt(_attackerPosition);
        Destroy(hitFX, FXTimeLength);
    }


    public void GetPushedFromHit(Vector3 _attackerPosition)
    {
        rb.AddForce((transform.position - _attackerPosition).normalized * pushStrength, ForceMode.VelocityChange);
    }
}
