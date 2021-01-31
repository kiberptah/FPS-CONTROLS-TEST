using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackVisualizer : MonoBehaviour
{
    [Header("Attack Visualize")]
    public GameObject attack_visualizer;

    [Header("Pre-Attack Visualize")]
    public GameObject preattack_visualizer;

    private void OnEnable()
    {
        EventDirector.someAttack += Attack;
        EventDirector.somePrepAttack += PrepAttack;
    }
    private void OnDisable()
    {
        EventDirector.someAttack -= Attack;
        EventDirector.somePrepAttack -= PrepAttack;

    }

    void Attack(Transform _attacker, Transform _whom, Vector3 _hitPoint, float _amount)
    {
        if (_attacker == gameObject.transform)
        {
            // DO DAMAGE!
            //_attackTarget.GetComponent<Health>()?.TakeDamage(_attackDamage);


            // Visualize!!
            GameObject attackLine = Object.Instantiate(attack_visualizer, transform);
            attackLine.GetComponent<DrawLineToTarget>().target = _whom;
            Destroy(attackLine, 0.35f);
        }
    }

    void PrepAttack(Transform _attacker, Transform _whom, Vector3 _hitPoint, float _amount)
    {
        if (_attacker == gameObject.transform)
        {
            GameObject attackLine = Object.Instantiate(preattack_visualizer, transform);
            attackLine.GetComponent<DrawLineToTarget>().target = _whom;
            Destroy(attackLine, 0.35f);
        }
    }
}
