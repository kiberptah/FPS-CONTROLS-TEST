using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;

public class AttackState : INPCState
{
    INPCState nextState;
    IEnumerator dodging;
    bool isStateEntered = false;



    //public static event Action<Transform, Transform, float> enemyAttack;
    //public static event Action<Transform, Transform, float> enemyPrepAttack;


    public INPCState DoState(EnemyStateMachine npc)
    {
        OnStateEnter(npc);

        if (npc.doesSeePlayer == false)
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
            npc.attackCharge = 0;
            npc.isAttackReady = false;

            dodging = WeirdDodging(npc);
            CoRunner.instance.StartCoroutine(dodging);

        }

        isStateEntered = true;
    }

    void Attack(EnemyStateMachine npc)
    {
        npc.transform.LookAt(npc.attackTarget);

        if (npc.isAttackReady == false)
        {
            if (npc.attackCurrentCooldown >= npc.attackCooldown)
            {
                //npc.attackTarget.GetComponent<Health>()?.TakeDamage(npc.attackDamage);


                /*GameObject attackLine = Object.Instantiate(npc.attackVisualizer, npc.transform);
                attackLine.GetComponent<DrawLineToTarget>().target = npc.attackTarget;
                Object.Destroy(attackLine, 0.2f);*/

                //enemyPrepAttack?.Invoke(npc.transform, npc.attackTarget, npc.attackChargeRequired);
                EventDirector.somePrepAttack?.Invoke(npc.transform, npc.attackTarget, npc.attackTarget.position, npc.attackDamage);

                npc.isAttackReady = true;
            }
            else
            {
                npc.attackCurrentCooldown += Time.deltaTime;
            }
        }

        if (npc.isAttackReady == true)
        {
            if (npc.attackCharge >= npc.attackChargeRequired)
            {
                //enemyAttack?.Invoke(npc.transform, npc.attackTarget, npc.attackDamage);
                EventDirector.someAttack?.Invoke(npc.transform, npc.attackTarget, npc.attackTarget.position, npc.attackDamage);

                npc.isAttackReady = false;
                npc.attackCharge = 0;
                npc.attackCurrentCooldown = 0;
            }
            else
            {
                npc.attackCharge += Time.deltaTime;
            }
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
            _direction.x = 1 - 2 * UnityEngine.Random.value;
            //_direction.y = Random.Range(-0.25f, 0.25f);
            _direction = _direction.normalized;
            _direction = npc.transform.TransformDirection(_direction);
            
            RaycastHit _hit;
            
            if (Physics.Raycast(npc.transform.position, _direction, out _hit, 2f) == false)
            {
                npc.rb.AddRelativeForce(_direction * 1, ForceMode.VelocityChange);
                //Debug.DrawRay(npc.transform.position, _direction, Color.green, 0.5f);
                //Debug.Log(npc.transform.position + " — " + _direction);

            }
            else
            {

            }


            yield return new WaitForSeconds(0.1f);
        }
        yield return null;
    }

}
