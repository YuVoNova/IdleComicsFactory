using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Customer : MonoBehaviour
{
    [HideInInspector]
    public CustomerStates CurrentState;

    [SerializeField]
    private NavMeshAgent Agent;

    [SerializeField]
    private float StopDistance;

    [SerializeField]
    private Animator Animator;

    [SerializeField]
    private int MoneyAmount;

    [SerializeField]
    private GameObject ComicObject;

    private int shelfID;
    private Transform targetShelf;
    private int waitingLineIndex;
    private int waitingRegisterLineIndex;
    private int exitIndex;

    private Vector3 targetAngles;
    private Vector3 destinationPoint;

    [SerializeField]
    private float TakeComicDuration;
    private float takeComicTimer;

    [SerializeField]
    private float PurchaseDuration;
    private float purchaseTimer;

    private void Awake()
    {
        targetAngles = Vector3.zero;

        purchaseTimer = PurchaseDuration;
    }

    private void Update()
    {
        switch (CurrentState)
        {
            case CustomerStates.Waiting_Line:



                break;

            case CustomerStates.Walking_NextLine:

                if (waitingLineIndex >= 0)
                {
                    if (Vector3.Distance(transform.position, destinationPoint) < StopDistance)
                    {
                        Agent.enabled = false;

                        Animator.SetBool("isRunning", false);

                        CurrentState = CustomerStates.Waiting_Line;
                    }
                }
                else
                {

                }

                break;

            case CustomerStates.Walking_Comic:

                if (Vector3.Distance(transform.position, destinationPoint) < StopDistance)
                {
                    if (Agent.enabled)
                    {
                        Agent.enabled = false;

                        Animator.SetBool("isRunning", false);

                        transform.eulerAngles = Vector3.zero;
                    }

                    if (takeComicTimer <= 0f)
                    {
                        GameManager.Instance.Shelves[shelfID].GiveComic();
                        ComicObject.SetActive(true);

                        Agent.enabled = true;

                        Animator.SetBool("isRunning", true);

                        waitingRegisterLineIndex = GameManager.Instance.GetRegisterLine();

                        destinationPoint = GameManager.Instance.CustomerRegisterLinePoints[waitingRegisterLineIndex].position;
                        Agent.SetDestination(destinationPoint);

                        CurrentState = CustomerStates.Walking_RegisterNextLine;
                    }
                    else
                    {
                        takeComicTimer -= Time.deltaTime;
                    }
                }

                break;

            case CustomerStates.Waiting_RegisterLine:

                if (Player.Instance.IsOpenForSales && waitingRegisterLineIndex == 0)
                {
                    if (purchaseTimer <= 0f)
                    {
                        Purchase();
                    }
                    else
                    {
                        purchaseTimer -= Time.deltaTime;
                    }
                }

                break;

            case CustomerStates.Walking_RegisterNextLine:

                if (Vector3.Distance(transform.position, destinationPoint) < StopDistance)
                {
                    Agent.enabled = false;

                    Animator.SetBool("isRunning", false);

                    CurrentState = CustomerStates.Waiting_RegisterLine;

                    targetAngles.y = 90f;
                    transform.eulerAngles = targetAngles;
                }

                break;

            case CustomerStates.Walking_Exit:

                if (exitIndex == 0)
                {
                    if (Vector3.Distance(transform.position, destinationPoint) < StopDistance)
                    {
                        exitIndex = 1;

                        destinationPoint = GameManager.Instance.CustomerExitPoints[exitIndex].position;
                        Agent.SetDestination(destinationPoint);
                    }
                }
                else
                {
                    if (Vector3.Distance(transform.position, destinationPoint) < StopDistance)
                    {
                        Destroy(gameObject);
                    }
                }

                break;

            default:



                break;
        }
    }

    public void Initialize(int lineIndex)
    {
        waitingLineIndex = lineIndex;

        destinationPoint = GameManager.Instance.CustomerLinePoints[waitingLineIndex].position;
        Agent.SetDestination(destinationPoint);

        Animator.SetBool("isRunning", true);

        CurrentState = CustomerStates.Walking_NextLine;
    }

    public void Enable(Transform shelf, int id)
    {
        targetShelf = shelf;
        shelfID = id;
    }

    public void LineChanged()
    {
        if (CurrentState == CustomerStates.Waiting_Line || CurrentState == CustomerStates.Walking_NextLine)
        {
            waitingLineIndex = Mathf.Clamp(waitingLineIndex - 1, -1, 100);
            Agent.enabled = true;

            Animator.SetBool("isRunning", true);

            if (waitingLineIndex == -1)
            {
                takeComicTimer = TakeComicDuration;

                destinationPoint = targetShelf.transform.position;
                Agent.SetDestination(destinationPoint);

                CurrentState = CustomerStates.Walking_Comic;
            }
            else
            {
                destinationPoint = GameManager.Instance.CustomerLinePoints[waitingLineIndex].transform.position;
                Agent.SetDestination(destinationPoint);

                CurrentState = CustomerStates.Walking_NextLine;
            }
        }
    }

    public void RegisterLineChanged()
    {
        if (CurrentState == CustomerStates.Waiting_RegisterLine || CurrentState == CustomerStates.Walking_RegisterNextLine)
        {
            waitingRegisterLineIndex = Mathf.Clamp(waitingRegisterLineIndex - 1, 0, 100);
            Agent.enabled = true;

            Animator.SetBool("isRunning", true);

            destinationPoint = GameManager.Instance.CustomerRegisterLinePoints[waitingRegisterLineIndex].transform.position;
            Agent.SetDestination(destinationPoint);

            CurrentState = CustomerStates.Walking_RegisterNextLine;
        }
    }

    private void Purchase()
    {
        exitIndex = 0;

        GameManager.Instance.CustomerLeft(this, MoneyAmount);

        Agent.enabled = true;

        Animator.SetBool("isRunning", true);

        destinationPoint = GameManager.Instance.CustomerExitPoints[exitIndex].position;
        Agent.SetDestination(destinationPoint);

        CurrentState = CustomerStates.Walking_Exit;
    }
}
