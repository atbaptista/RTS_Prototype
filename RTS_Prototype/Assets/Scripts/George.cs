using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;

public class George : MonoBehaviour, Moveable 
{
    public Camera cam;
    public NavMeshAgent playerNavMeshAgent = null;
    [HideInInspector] public Selectable selected;
    [HideInInspector] public bool isDestSet = false;
    [HideInInspector] public Animator anim;

    [HideInInspector] public Vector3 dest;
    public float stoppingDistance = 0.15f;

    [HideInInspector] public StateMachine georgeMachine = new StateMachine();

    [HideInInspector] public GeorgeIdle idleState;
    [HideInInspector] public GeorgeWalk walkState;

    void Start()
    {
        idleState = new GeorgeIdle(this);
        walkState = new GeorgeWalk(this);

        anim = GetComponent<Animator>();
        selected = GetComponent<Selectable>();
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
