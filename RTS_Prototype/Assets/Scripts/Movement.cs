using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] private Camera cam;
    [SerializeField] private NavMeshAgent playerNavMeshAgent = null;
    public bool isSelected = false;
    private bool isDestSet = false;
    private Animator anim;

    private Vector3? dest;
    public float stoppingDistance = 0.25f;

    //public GameObject thingy;
    private void Awake()
    {
        
    }
    void Start()
    {
        anim = GetComponent<Animator>();

    }
    void Update()
    {
        if (isSelected)
        {
            //this.GetComponent<Renderer>().material.color = Color.blue;
        }
        else
        {
            //this.GetComponent<Renderer>().material.color = Color.black;
        }

        //Bit shift the index of the ground layer (8) to get a bit mask
        int layerMask = 1 << 8;

        //idk need a maxdistance to input layermask
        float maxDistance = 100f;

        
        if (isDestSet)
        {
            Debug.Log("hi jameslolw");
            Vector3 distanceToDest = (Vector3)(dest - transform.position);
            
            if (distanceToDest.magnitude < stoppingDistance)
            {
                playerNavMeshAgent.isStopped = true;
                anim.SetBool("isWalking", false);
                isDestSet = false;
                Debug.Log("we here");
                
            }
        }


        if (Input.GetMouseButtonDown(1) && isSelected)
        {
            RaycastHit hit;

            //point in game where i click
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            
            //raycast and only hit things in layer 8 (ground)
            
            if (Physics.Raycast(ray, out hit, maxDistance ,layerMask))
            {
                playerNavMeshAgent.isStopped = false;

                playerNavMeshAgent.SetDestination(hit.point);
                anim.SetBool("isWalking", true);
                dest = hit.point;
                isDestSet = true;
                Debug.Log("setDestinatiounknpow");



                //spawn object where you click
                //Instantiate(thingy, hit.point, Quaternion.identity);
            }
        }
    }
}
