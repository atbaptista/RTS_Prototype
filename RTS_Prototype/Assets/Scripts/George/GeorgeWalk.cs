using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeorgeWalk : IState
{
    George george;

    public GeorgeWalk(George george) 
    { 
        this.george = george; 
    }

    public void Enter()
    {
        //enable navagent movement, set destination, animation
        george.playerNavMeshAgent.isStopped = false;
        george.playerNavMeshAgent.SetDestination(george.dest);
        george.isDestSet = true;
        george.anim.SetBool("isWalking", true);
    }

    public void Execute()
    {
        //enable or disable the selection circle
        //george.drawSelectionCircle();

        george.playerNavMeshAgent.SetDestination(george.dest);

        //calculate vector from pos to destination
        Vector3 distanceToDest = george.dest - george.transform.position;

        if (george.selected.health <= 0)
        {
            george.georgeMachine.ChangeState(george.dieState);
        }
        //within stopping distance
        else if (distanceToDest.magnitude < george.stoppingDistance)
        {
            george.georgeMachine.ChangeState(george.idleState);
        }
    }

    public void Exit()
    {
        george.playerNavMeshAgent.isStopped = true;
        george.anim.SetBool("isWalking", false);
    }
}
