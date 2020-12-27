using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : INPCState
{
    INPCState nextState;
    IEnumerator dodging;
    bool isStateEntered = false;
    public INPCState DoState(EnemyStateMachine npc)
    {
        OnStateEnter(npc);

        if (npc.canSeePlayer == false)
        {
            isStateEntered = false;
            return npc.chaseState;
        }

        Attack(npc);
        KeepDistance(npc);
        
        

        isStateEntered = true;
        return npc.attackState;
    }


    private void OnStateEnter(EnemyStateMachine npc)
    {
        if (isStateEntered == false)
        {
            dodging = WeirdDodging(npc);
            CoRunner.instance.StartCoroutine(dodging);

        }

        isStateEntered = true;
    }

    void Attack(EnemyStateMachine npc)
    {
        npc.transform.LookAt(npc.attackTarget);

        if (npc.attackCurrentCooldown >= npc.attackCooldown)
        {
            npc.attackTarget.GetComponent<Health>()?.TakeDamage(npc.attackDamage);


            GameObject attackLine = Object.Instantiate(npc.attackVisualizer, npc.transform);
            attackLine.GetComponent<DrawLineToTarget>().target = npc.attackTarget;
            Object.Destroy(attackLine, 0.2f);

            npc.attackCurrentCooldown = 0;
        }
        else
        {
            npc.attackCurrentCooldown += Time.deltaTime;
        }
    }

    void KeepDistance(EnemyStateMachine npc)
    {
        Vector3 _direction;
        _direction = (npc.attackTarget.position - npc.transform.position).normalized;


        float _distance;
        _distance = Vector3.Distance(npc.transform.position, npc.attackTarget.position);

        if (_distance < npc.attackDistance * 0.5f)
        {
            // retreat
            npc.transform.Translate(-_direction * npc.moveSpeed * Time.deltaTime, Space.World);
        }
        if (_distance > npc.attackDistance * 0.75f)
        {
            // chase
            npc.transform.Translate(_direction * npc.moveSpeed * Time.deltaTime, Space.World);
        }

    }

    IEnumerator WeirdDodging(EnemyStateMachine npc)
    {
        // this shit sucks 
        // ok it's better now
        while (npc.currentStateName == "AttackState" && npc != null)
        {
            Vector3 _direction = Vector3.zero;
            _direction.x = 1 - 2 * Random.value;
            //_direction.y = Random.Range(-0.25f, 0.25f);
            _direction = _direction.normalized;

            RaycastHit _hit;
            if (Physics.Raycast(npc.transform.position, npc.transform.position + _direction, out _hit, 2f) == false)
            {
                npc.rb.AddRelativeForce(_direction * 1, ForceMode.VelocityChange);
            }
            else
            {

            }


            yield return new WaitForSeconds(0.1f);
        }
        yield return null;
    }

}
