using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnFXFromAttack : MonoBehaviour
{
    public GameObject hitFXPrefab;
    public float FXTimeLength = 5f;

    private void OnEnable()
    {
        EventDirector.someAttack += SpawnFX;
    }
    private void OnDisable()
    {
        EventDirector.someAttack -= SpawnFX;

    }

    void SpawnFX(Transform _attacker, Transform _whom, Vector3 _hitPoint, float _amount)
    {
        if (_whom == transform)
        {
            GameObject hitFX = Instantiate(hitFXPrefab, _hitPoint, Quaternion.identity);
            hitFX.transform.LookAt(_attacker.position);
            Destroy(hitFX, FXTimeLength);
        }
    }
}
