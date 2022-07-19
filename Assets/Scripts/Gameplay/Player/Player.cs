using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // Singleton

    public static Player Instance;


    // Objects & Components

    [Header("Objects & Components", order = 0)]

    [SerializeField]
    private PlayerController PlayerController;

    public Animator Animator;

    public MoneyFlow MoneyFlow;
    public Transform MoneyFlowPoint;

    public AudioSource AudioSource;


    // Values

    [Header("Values", order = 0)]

    private bool isNavigating;


    // Unity Functions

    private void Awake()
    {
        Instance = this;


    }

    private void Start()
    {

    }

    private void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 8)        // Interactable
        {
            other.GetComponent<Interactable>().StartInteraction();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == 8)        // Interactable
        {
            other.GetComponent<Interactable>().ExitInteraction();
        }
    }


    // Methods

    


}
