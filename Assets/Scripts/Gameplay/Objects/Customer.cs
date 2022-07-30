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

    private Vector3 destinationPoint;

    [SerializeField]
    private float TakeComicDuration;
    private float takeComicTimer;

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
                        CurrentState = CustomerStates.Waiting_Line;

                        //Animator.SetBool("isRunning", false);

                        Agent.enabled = false;
                    }
                }
                else
                {
                    destinationPoint = targetShelf.transform.position;
                    Agent.SetDestination(destinationPoint);

                    CurrentState = CustomerStates.Walking_Comic;
                }

                break;

            case CustomerStates.Walking_Comic:

                if (Vector3.Distance(transform.position, destinationPoint) < StopDistance)
                {
                    takeComicTimer = TakeComicDuration;

                    //Animator.SetBool("isRunning", false);

                    Agent.enabled = false;
                }
                else
                {
                    if (takeComicTimer <= 0f)
                    {
                        GameManager.Instance.Shelves[shelfID].GiveComic();
                        ComicObject.SetActive(true);

                        // TO DO -> Set waitingRegisterLineIndex here.

                        Agent.enabled = true;

                        //Animator.SetBool("isRunning", true);

                        destinationPoint = GameManager.Instance.CustomerRegisterLinePoints[waitingRegisterLineIndex].transform.position;
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

                if (Player.Instance.IsOpenForSales)
                {
                    Purchase();
                }

                break;

            case CustomerStates.Walking_RegisterNextLine:

                if (Vector3.Distance(transform.position, destinationPoint) < StopDistance)
                {
                    CurrentState = CustomerStates.Waiting_RegisterLine;

                    //Animator.SetBool("isRunning", false);

                    Agent.enabled = false;
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
                    
                }

                break;

            default:



                break;
        }
    }

    public void Initialize(int shelfId, Transform shelfTransform, int lineIndex)
    {
        shelfID = shelfId;
        targetShelf = shelfTransform;
        waitingLineIndex = lineIndex;

        CurrentState = CustomerStates.Walking_NextLine;
    }

    public void LineChanged()
    {
        if (CurrentState == CustomerStates.Waiting_Line || CurrentState == CustomerStates.Walking_NextLine)
        {
            waitingLineIndex = Mathf.Clamp(waitingLineIndex - 1, -1, 100);
            Agent.enabled = true;

            //Animator.SetBool("isRunning", true);

            destinationPoint = GameManager.Instance.CustomerLinePoints[waitingLineIndex].transform.position;
            Agent.SetDestination(destinationPoint);

            CurrentState = CustomerStates.Walking_NextLine;
        }
        else if (CurrentState == CustomerStates.Waiting_RegisterLine || CurrentState == CustomerStates.Walking_RegisterNextLine)
        {
            if (waitingRegisterLineIndex > 0)
            {
                waitingRegisterLineIndex = Mathf.Clamp(waitingLineIndex - 1, 0, 100);

                destinationPoint = GameManager.Instance.CustomerRegisterLinePoints[waitingRegisterLineIndex].transform.position;
                Agent.SetDestination(destinationPoint);

                CurrentState = CustomerStates.Walking_RegisterNextLine;
            }
        }
    }

    private void Purchase()
    {
        exitIndex = 0;

        GameManager.Instance.CustomerLeft(MoneyAmount);

        Agent.enabled = true;

        //Animator.SetBool("isRunning", true);

        destinationPoint = GameManager.Instance.CustomerExitPoints[exitIndex].position;
        Agent.SetDestination(destinationPoint);

        CurrentState = CustomerStates.Walking_Exit;
    }
}
