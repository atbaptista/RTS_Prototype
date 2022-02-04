using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy1Idle : IState 
{
    Enemy1 enemy1;

    public Enemy1Idle(Enemy1 enemy1)
    {
        this.enemy1 = enemy1;
    }

    public void Enter()
    {
        //update navmesh, animation, isdestset
        enemy1.enemy1NavMeshAgent.isStopped = true;
        enemy1.anim.SetBool("isWalking", false);
        enemy1.isDestSet = false;
    }

    public void Execute()
    {
        //enable or disable the selection circle
        if (enemy1.selected.isSelected)
        {
            enemy1.GetComponent<LineRenderer>().enabled = true;
        }
        else
        {
            enemy1.GetComponent<LineRenderer>().enabled = false;
        }

        checkIsDist();
    }

    public void Exit()
    {
        
    }

    private void checkIsDist()
    {
        //enable or disable the selection circle
        if (enemy1.selected.isSelected)
        {
            enemy1.GetComponent<LineRenderer>().enabled = true;
        }
        else
        {
            enemy1.GetComponent<LineRenderer>().enabled = false;
        }

        if (enemy1.selected.health <= 0)
        {
            //when dying change layermask to dead things layermask
            enemy1.Die();
        }
        else if (enemy1.isDestSet)
        {
            enemy1.enemy1Machine.ChangeState(enemy1.walkState);
        }
    }
}
