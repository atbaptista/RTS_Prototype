using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy1Walk : IState
{
    Enemy1 enemy1;

    public Enemy1Walk(Enemy1 enemy1)
    {
        this.enemy1 = enemy1;
    }

    public void Enter()
    {
        //enable navagent movement, set destination, animation
        enemy1.enemy1NavMeshAgent.isStopped = false;
        enemy1.enemy1NavMeshAgent.SetDestination(enemy1.dest);
        enemy1.isDestSet = true;
        enemy1.anim.SetBool("isWalking", true);
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

        enemy1.enemy1NavMeshAgent.SetDestination(enemy1.dest);

        //calculate vector from pos to destination
        Vector3 distanceToDest = enemy1.dest - enemy1.transform.position;

        //within stopping distance
        if (enemy1.selected.health <= 0)
        {
            //die code
            enemy1.Die();
        }
        else if (distanceToDest.magnitude < enemy1.stoppingDistance)
        {
            enemy1.enemy1Machine.ChangeState(enemy1.idleState);
        }
    }

    public void Exit()
    {
        
    }
}
