using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeorgeAttack : IState
{
    George george;
    private float lastAttackedAt = -9999f;

    public GeorgeAttack(George george)
    {
        this.george = george;
    }

    public void Enter()
    {
        george.anim.SetBool("isAttacking", true);
        FindClosestEnemy();
    }

    public void Execute()
    {
        FindClosestEnemy();
        //enable or disable the selection circle
        if (george.selected.isSelected)
        {
            george.GetComponent<LineRenderer>().enabled = true;
        }
        else
        {
            george.GetComponent<LineRenderer>().enabled = false;
        }

        if (Time.time > lastAttackedAt + george.attackSpeed)
        {
            Attack();
            lastAttackedAt = Time.time;
        }

        
        if (george.isDestSet) //change state if a new destination is input
        {
            george.georgeMachine.ChangeState(george.walkState);
        }
        else if((george.transform.position - 
            george.closestEnemy.transform.position).magnitude > 
            george.detectionRadius){ //if out range

            //change to chase state later
            george.georgeMachine.ChangeState(george.idleState);
        }
        else if (george.closestEnemy.Equals(null)) //if thing died
        {
            george.georgeMachine.ChangeState(george.idleState);
        }
    }

    public void Exit()
    {
        george.anim.SetBool("isAttacking", false);
    }

    private void FindClosestEnemy()
    {
        //detection radius is the max distance objects will be, add 10 to get edge cases
        float closestDistance = george.detectionRadius + 10f;

        foreach (Collider i in george.unitsInRange)
        {
            //check type of unit
            if (i.GetComponent<Selectable>().unitType == Selectable.unitTypes.Dinosaur)
            {
                //find closest dinosaur
                Vector3 distanceBetwixt = i.transform.position - george.transform.position;
                if (distanceBetwixt.magnitude < closestDistance)
                {
                    george.closestEnemy = i;
                    closestDistance = distanceBetwixt.magnitude;
                }
            }
        }
    }

    private void Attack()
    {
        //mighjt need a null check if the enemy dies
        if (george.closestEnemy.GetComponent<Selectable>().health > 0)
        {
            Debug.Log("pew");
            //look at target
            george.transform.LookAt(george.closestEnemy.transform);

            //shoot target
            
            george.closestEnemy.GetComponent<Selectable>().health -= george.basicAttackDmg;

            //make the vector higher up
            Vector3 newS = new Vector3(george.transform.position.x, george.transform.position.y + 2f,
                george.transform.position.z);
            Vector3 newE = new Vector3(george.closestEnemy.transform.position.x,
                george.closestEnemy.transform.position.y + 2f, george.closestEnemy.transform.position.z);
           
            DrawLine(newS, newE);
            /*george.Shoot(george.closestEnemy.gameObject);*/
        }
    }

    private void DrawLine(Vector3 start, Vector3 end, float duration = 0.08f)
    {
        //spawn a point light at the location of the enemy
        GameObject lightObject = new GameObject();
        Light lightComp = lightObject.AddComponent<Light>();
        lightObject.transform.position = george.closestEnemy.transform.position;
        lightComp.type = LightType.Point;
        lightComp.intensity = 60f;
        lightComp.range = 10f;

        //make a blue line
        GameObject myLine = new GameObject();
        myLine.transform.position = start;
        myLine.AddComponent<LineRenderer>();
        LineRenderer lr = myLine.GetComponent<LineRenderer>();
        lr.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        lr.material = george.laserMat;
        lr.startWidth = 0.1f;
        lr.endWidth = 0.1f;
        lr.SetPosition(0, start);
        lr.SetPosition(1, end);
        GameObject.Destroy(myLine, duration);
        GameObject.Destroy(lightObject, duration);
    }
}
