using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy1 : MonoBehaviour
{
    #region header
    //components
    public NavMeshAgent enemy1NavMeshAgent = null;
    [HideInInspector] public Animator anim;
    [HideInInspector] public Selectable selected;

    //variables
    [HideInInspector] public Vector3 dest;
    [HideInInspector] public List<Collider> unitsInRange = new List<Collider>();
    [HideInInspector] public Collider closestEnemy = null;
    [HideInInspector] public bool going = true;
    public GameObject patrolStart;
    public GameObject patrolEnd = null;
    public float deathDeletionTime = 1.2f;
    public float attackRadius = 2f;
    public float detectionRadius = 5f;
    public float stoppingDistance = 0.15f;
    public float basicAttackDmg = 40f;
    public float attackSpeed = 1f;
    [SerializeField] private float health = 100f;



    //state machine
    [HideInInspector] public StateMachine enemy1Machine = new StateMachine();
    [HideInInspector] public Enemy1Idle idleState;
    [HideInInspector] public Enemy1Chase chaseState;
    [HideInInspector] public Enemy1Attack attackState;
    [HideInInspector] public Enemy1Die dieState;
    [HideInInspector] public Enemy1Patrol patrolState;
    #endregion header

    void Start()
    {
        //create states
        idleState = new Enemy1Idle(this);
        chaseState = new Enemy1Chase(this);
        attackState = new Enemy1Attack(this);
        dieState = new Enemy1Die(this);
        patrolState = new Enemy1Patrol(this);

        //get components and initialize stuff
        anim = GetComponent<Animator>();
        selected = GetComponent<Selectable>();
        selected.unitType = Selectable.unitTypes.Dinosaur;
        selected.health = health;
        if (patrolEnd == null)
        {
            going = false;
        }
        
        //make an empty gameobject and set it's location to where the dino spawns
        patrolStart = new GameObject("patrolStart for " + name);
        patrolStart.transform.position = transform.position;

        //draw red circle, disable it until selection is decided upon
        Color color = new Color(255, 0, 0);
        selected.DrawCircle(this.gameObject, 1.2f, 0.09f, color);
        GetComponent<LineRenderer>().enabled = false;

        //set state
        enemy1Machine.ChangeState(idleState);
    }

    void Update()
    {
        enemy1Machine.Update();
    }

    //can put these two methods within the selectable class
    public void Die()
    {
        Destroy(this.gameObject, deathDeletionTime);
    }

    public void getUnitsInRange(List<Collider> unitsList)
    {
        Collider[] newList;

        //clear old list
        unitsList.Clear();

        //Selectable layermask (7)
        int layerMask = 1 << 7;

        //get all objects near 
        newList = Physics.OverlapSphere(transform.position, detectionRadius, layerMask);

        foreach (Collider i in newList)
        {
            unitsList.Add(i);
        }
    }
    public void drawSelectionCircle()
    {
        //enable or disable the selection circle
        if (selected.isSelected)
        {
            GetComponent<LineRenderer>().enabled = true;
        }
        else
        {
            GetComponent<LineRenderer>().enabled = false;
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRadius);

        Gizmos.color = Color.green;
        if (closestEnemy != null)
        {
            Gizmos.DrawLine(transform.position, closestEnemy.transform.position);
        }
    }
}
