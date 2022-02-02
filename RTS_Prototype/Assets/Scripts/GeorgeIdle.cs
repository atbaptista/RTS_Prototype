using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeorgeIdle : IState
{
    George george;

    public GeorgeIdle(George george) 
    { 
        this.george = george; 
    }

    public void Enter()
    {
        george.playerNavMeshAgent.isStopped = true;
        george.anim.SetBool("isWalking", false);
        george.isDestSet = false;
    }

    public void Execute()
    {
        if (george.isDestSet)
        {
            george.georgeMachine.ChangeState(george.walkState);
        }
    }

    public void Exit()
    {
        
    }
}