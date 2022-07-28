using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Customer : MonoBehaviour
{
    [HideInInspector]
    public int ID;

    [HideInInspector]
    public CustomerStates CurrentState;

    [SerializeField]
    private NavMeshAgent Agent;

    [SerializeField]
    private float StopDistance;

    private Vector3 destinationPoint;

    [SerializeField]
    private Animator customerAnimator;


}
