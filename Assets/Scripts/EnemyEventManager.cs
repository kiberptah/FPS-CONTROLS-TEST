using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyEventManager : MonoBehaviour
{
    [Header("Attack Visualize")]
    public GameObject attack_visualizer;

    [Header("Pre-Attack Visualize")]
    public GameObject preattack_visualizer;


    [Header("NPC Get Hit")]
    public float pushStrength = 5f;
    public GameObject hitFXPrefab;
    public float FXTimeLength = 5f;
    Rigidbody rb;



    private void OnEnable()
    {
        PlayerShooting.playerHitEnemy += GetHit;
        AttackState.enemyAttack += Attack;
        AttackState.enemyPrepAttack += PrepAttack;
    }

    private void Awake()
    {
        rb = transform.GetComponent<Rigidbody>();
    }

    void GetHit(Transform _hit, Vector3 _hitPoint, float _damage, Vector3 _attackerPosition)
    {
        if (_hit == gameObject.transform)
        {
            // Decrease HP
            gameObject.GetComponent<Health>().TakeDamage(_damage);

            // Push
            //gameObject.GetComponent<EnemyGotHitImpact>().GetPushedFromHit(_attackerPosition);
            rb.AddForce((transform.position - _attackerPosition).normalized * pushStrength, ForceMode.VelocityChange);

            // Impact FX
            //gameObject.GetComponent<EnemyGotHitImpact>().SpawnParticlesOnHit(_attackerPosition, _hitPoint);
            GameObject hitFX = Instantiate(hitFXPrefab, _hitPoint, Quaternion.identity);
            hitFX.transform.LookAt(_attackerPosition);
            Destroy(hitFX, FXTimeLength);
        }
    }

    void Attack(Transform npc, Transform _attackTarget, float _attackDamage)
    {
        if (npc == gameObject.transform)
        {
            // DO DAMAGE!
            _attackTarget.GetComponent<Health>()?.TakeDamage(_attackDamage);


            // Visualize!!
            GameObject attackLine = Object.Instantiate(attack_visualizer, transform);
            attackLine.GetComponent<DrawLineToTarget>().target = _attackTarget;
            Destroy(attackLine, 0.35f);
        }
    }

    void PrepAttack(Transform npc, Transform _attackTarget, float _t)
    {
        if (npc == gameObject.transform)
        {
            GameObject attackLine = Object.Instantiate(preattack_visualizer, transform);
            attackLine.GetComponent<DrawLineToTarget>().target = _attackTarget;
            Destroy(attackLine, 0.35f);
        }
    }












    private void OnDisable()
    {
        PlayerShooting.playerHitEnemy -= GetHit;
        AttackState.enemyAttack -= Attack;
        AttackState.enemyPrepAttack -= PrepAttack;

    }
}
