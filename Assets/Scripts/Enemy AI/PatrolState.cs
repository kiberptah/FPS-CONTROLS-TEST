using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolState : INPCState
{
    bool isStateEntered = false;
    public INPCState DoState(EnemyStateMachine npc)
    {
        OnStateEnter(npc);

        JustRotate(npc);

        if (npc.doesSeePlayer)
        {
            isStateEntered = false;
            return npc.attackState;
        }


        isStateEntered = true;
        return npc.patrolState;        
    }

    private void OnStateEnter(EnemyStateMachine npc)
    {
        if (isStateEntered == false)
        {
            npc.transform.rotation = Quaternion.identity;

            
        }
        isStateEntered = true;
    }

    private void JustRotate(EnemyStateMachine npc)
    {
        
        //npc.gameObject.transform.Rotate(Vector3.up, 100f * Time.deltaTime);
    }
}