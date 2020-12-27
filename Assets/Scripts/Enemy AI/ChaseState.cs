using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseState : INPCState
{
    bool isStateEntered = false;
    bool isLastReachedCombatPosition = false;

    public INPCState DoState(EnemyStateMachine npc)
    {


        if (npc.canSeePlayer)
        {
            isStateEntered = false;
            return npc.attackState;
        }
        MoveToLastCombatPosition(npc);
        MoveToKnownPlayerPosition(npc);


        Vector3 arrivalDistance = npc.transform.position - npc.lastKnownPlayerPosition;
        if (arrivalDistance.magnitude < 1f)
        {
            isStateEntered = false;
            return npc.patrolState;
        }

        isStateEntered = true;
        return npc.chaseState;
    }

    private void OnStateEnter(EnemyStateMachine npc)
    {
        if (isStateEntered == false)
        {

            
        }
        isStateEntered = true;
    }

    void MoveToLastCombatPosition(EnemyStateMachine npc)
    {
        if (isLastReachedCombatPosition == false)
        {
            if ((npc.transform.position - npc.lastCombatPosition).magnitude > 1f)
            {
                isLastReachedCombatPosition = false;

                Vector3 _direction;
                _direction = (npc.lastCombatPosition - npc.transform.position).normalized;

                npc.transform.Translate(_direction * npc.moveSpeed * Time.deltaTime, Space.World);
            }
            else
            {
                isLastReachedCombatPosition = true;
            }
        }
    }

    void MoveToKnownPlayerPosition(EnemyStateMachine npc)
    {
        if (isLastReachedCombatPosition == true)
        {
            if (npc.lastKnownPlayerPosition != Vector3.zero)
            {
                Vector3 _direction;
                _direction = (npc.lastKnownPlayerPosition - npc.transform.position).normalized;

                npc.transform.Translate(_direction * npc.moveSpeed * Time.deltaTime, Space.World);

            }

            npc.transform.LookAt(npc.lastKnownPlayerPosition);
        }
    }


}
