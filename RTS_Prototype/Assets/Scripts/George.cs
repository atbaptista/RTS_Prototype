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
    public float stoppingDistance = 0.15f;

    //state machine
    [HideInInspector] public StateMachine georgeMachine = new StateMachine();
    [HideInInspector] public GeorgeIdle idleState;
    [HideInInspector] public GeorgeWalk walkState;
    #endregion header

    void Start()
    {
        //create states
        idleState = new GeorgeIdle(this);
        walkState = new GeorgeWalk(this);

        //get components
        anim = GetComponent<Animator>();
        selected = GetComponent<Selectable>();

        //draw circle, start in idle state
        selected.DrawCircle(this.gameObject, 1.2f, 0.09f);
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

}
