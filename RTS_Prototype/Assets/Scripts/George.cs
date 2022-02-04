using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;

public class George : MonoBehaviour, Moveable 
{
    #region header
    //components
    public Camera cam;
    public NavMeshAgent playerNavMeshAgent = null;
    [HideInInspector] public Animator anim;
    [HideInInspector] public Selectable selected;

    //variables
    [HideInInspector] public bool isDestSet = false;
    [HideInInspector] public Vector3 dest;
    [HideInInspector] public List<Collider> unitsInRange = new List<Collider>();
    [HideInInspector] public Collider closestEnemy = null;
    public Material laserMat;
    public float detectionRadius = 5f;
    public float stoppingDistance = 0.15f;
    public float attackSpeed = 0.5f;
    public float basicAttackDmg = 20f;
    public float health = 100f;

    //state machine
    [HideInInspector] public StateMachine georgeMachine = new StateMachine();
    [HideInInspector] public GeorgeIdle idleState;
    [HideInInspector] public GeorgeWalk walkState;
    [HideInInspector] public GeorgeAttack attackState;
    #endregion header

    void Start()
    {
        //create states
        idleState = new GeorgeIdle(this);
        walkState = new GeorgeWalk(this);
        attackState = new GeorgeAttack(this);

        //get components
        anim = GetComponent<Animator>();
        selected = GetComponent<Selectable>();
        selected.unitType = Selectable.unitTypes.Robot;
        selected.health = health;

        //draw green circle, start in idle state
        Color color = new Color(0, 255, 0);
        selected.DrawCircle(this.gameObject, 1.2f, 0.09f, color);
        georgeMachine.ChangeState(idleState);
    }

    void Update()
    {
        georgeMachine.Update();
    }

    public void GoTo(Vector3 destination)
    {
        dest = destination;
        isDestSet = true;
    }

    public void getEnemiesInRange(List<Collider> enemiesList)
    {
        Collider[] newList;

        //clear old list
        enemiesList.Clear();

        //Selectable layermask (7)
        int layerMask = 1 << 7;

        //get all objects near 
        newList = Physics.OverlapSphere(transform.position, detectionRadius, layerMask);

        foreach(Collider i in newList)
        {
            enemiesList.Add(i);
        }
    }

/*    public void Shoot(GameObject target)
    {
        Instantiate(laser, transform.position, Quaternion.identity);
        laser.GetComponent<Projectile>().target = target;
        laser.GetComponent<Projectile>().projectileSpeed = projectileSpeed;
    }*/

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, closestEnemy.transform.position);
    }
}
