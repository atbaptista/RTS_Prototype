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
        //update navmesh, animation, isdestset
        george.playerNavMeshAgent.isStopped = true;
        george.anim.SetBool("isWalking", false);
        george.isDestSet = false;
    }

    public void Execute()
    {
        //enable or disable the selection circle
        if (george.selected.isSelected)
        {
            george.GetComponent<LineRenderer>().enabled = true;
        }
        else
        {
            george.GetComponent<LineRenderer>().enabled = false;
        }

        if (george.isDestSet)
        {
            george.georgeMachine.ChangeState(george.walkState);
        }
    }

    public void Exit()
    {
        
    }
}