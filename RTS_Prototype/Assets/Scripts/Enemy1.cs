using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy1 : MonoBehaviour, Moveable
{
    #region header
    //components
    public NavMeshAgent enemy1NavMeshAgent = null;
    [HideInInspector] public Animator anim;
    [HideInInspector] public Selectable selected;

    //variables
    [HideInInspector] public bool isDestSet = false;
    [HideInInspector] public Vector3 dest;
    [HideInInspector] public List<Collider> unitsInRange = new List<Collider>();
    [HideInInspector] public Collider closestEnemy = null;
    public float detectionRadius = 5f;
    public float stoppingDistance = 0.15f;
    public float health = 100f;

    //state machine
    [HideInInspector] public StateMachine enemy1Machine = new StateMachine();
    [HideInInspector] public Enemy1Idle idleState;
    [HideInInspector] public Enemy1Walk walkState;
    [HideInInspector] public Enemy1Attack attackState;
    #endregion header

    void Start()
    {
        //create states
        idleState = new Enemy1Idle(this);
        walkState = new Enemy1Walk(this);
        attackState = new Enemy1Attack(this);

        //get components
        anim = GetComponent<Animator>();
        selected = GetComponent<Selectable>();
        selected.unitType = Selectable.unitTypes.Dinosaur;
        selected.health = health;

        //draw red circle, start in idle state
        Color color = new Color(255, 0, 0);
        selected.DrawCircle(this.gameObject, 1.2f, 0.09f, color);
        enemy1Machine.ChangeState(idleState);
    }

    void Update()
    {
        enemy1Machine.Update();
    }

    public void GoTo(Vector3 destination)
    {
        dest = destination;
        isDestSet = true;
    }
    public void Die()
    {
        Destroy(this.gameObject);
    }
}
