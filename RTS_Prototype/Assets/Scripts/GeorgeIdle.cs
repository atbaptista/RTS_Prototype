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

        ChooseAction();
    }

    public void Exit()
    {
        
    }

    private void ChooseAction()
    {
        if (george.isDestSet) //check if new destination is set
        {
            george.georgeMachine.ChangeState(george.walkState);
        }
        else //check if enemies are near
        {
            //update the unitsInRange list
            george.getEnemiesInRange(george.unitsInRange);

            //change state to attack if enemies are detected
            foreach (Collider i in george.unitsInRange)
            {
                if (i.GetComponent<Selectable>().unitType.Equals
                    (Selectable.unitTypes.Dinosaur))
                {
                    george.georgeMachine.ChangeState(george.attackState);
                }
            }
        }
    }

}