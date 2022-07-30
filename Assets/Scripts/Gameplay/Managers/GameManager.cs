using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GameManager : MonoBehaviour
{
    // Singleton

    public static GameManager Instance;


    // Objects & Components

    [Header("Objects & Components", order = 0)]

    [SerializeField]
    private NavMeshSurface NavMeshSurface;

    public CameraMovement CameraMovement;

    public Interactable_Truck Truck;

    public List<Interactable_Shelf> Shelves;

    [HideInInspector]
    public List<int> EmptyShelves;
    [HideInInspector]
    public List<int> AvailableShelves;

    [HideInInspector]
    public List<Customer> Customers;

    public Transform[] CustomerLinePoints;
    public Transform[] CustomerRegisterLinePoints;
    public Transform[] CustomerExitPoints;
    public Transform CustomerSpawnPoints;

    [SerializeField]
    private Interactable_Sales Sales;


    // Values

    [Header("Values", order = 0)]

    [HideInInspector]
    public bool IsGameOn;
    [HideInInspector]
    public bool OnMenu;

    private int availableComicCount;

    [SerializeField]
    private int TotalCustomerCapacity;
    [SerializeField]
    private int InnerCustomerCapacity;

    private int lineIndex;
    private int registerLineIndex;

    private float customerSpawnDuration;
    private float customerSpawnTimer;


    // Unity Functions

    private void Awake()
    {
        Instance = this;

        RebuildNavMesh();

        IsGameOn = true;
        OnMenu = false;

        Customers = new List<Customer>();

        availableComicCount = 0;

        lineIndex = 0;
        registerLineIndex = 0;
    }

    private void Start()
    {

    }

    private void Update()
    {
        if (IsGameOn)
        {
            if (customerSpawnTimer <= 0f)
            {
                if (AvailableShelves.Count > 0 && Customers.Count < availableComicCount && Customers.Count < TotalCustomerCapacity)
                {
                    SpawnCustomer();

                    customerSpawnTimer = customerSpawnDuration;
                }
            }
            else
            {
                customerSpawnTimer -= Time.deltaTime;
            }
        }
    }

    private void FixedUpdate()
    {

    }


    // Methods

    public void RebuildNavMesh()
    {
        NavMeshSurface.BuildNavMesh();
    }

    public void MoneyEarned(int amount)
    {
        Manager.Instance.PlayerData.Money += Mathf.FloorToInt(amount);

        //UIManager.Instance.UpdateMoneyText();

        //Player.Instance.AudioSource.volume = 0.5f;
        //Player.Instance.AudioSource.clip = Manager.Instance.Audios["Money"];
        //Player.Instance.AudioSource.Play();

        Manager.Instance.Save();
    }

    public void MoneySpent(int amount)
    {
        Manager.Instance.PlayerData.Money = Mathf.FloorToInt(Mathf.Clamp(Manager.Instance.PlayerData.Money - amount, 0f, float.MaxValue));

        //UIManager.Instance.UpdateMoneyText();

        //Player.Instance.AudioSource.volume = 0.5f;
        //Player.Instance.AudioSource.clip = Manager.Instance.Audios["Money"];
        //Player.Instance.AudioSource.Play();

        Manager.Instance.Save();
    }

    public void EnableExpansion()
    {
        // TO DO -> Enable expansion here.
    }

    public void AddShelf(int id)
    {
        if (!EmptyShelves.Contains(id))
        {
            EmptyShelves.Add(id);
            Shelves[id].SetID(id);
        }
    }

    public void ShelfAvailable(int id)
    {
        if (EmptyShelves.Contains(id) && !AvailableShelves.Contains(id))
        {
            EmptyShelves.Remove(id);
            AvailableShelves.Add(id);
        }
    }

    public void ShelfExpired(int id)
    {
        if (AvailableShelves.Contains(id) && !EmptyShelves.Contains(id))
        {
            AvailableShelves.Remove(id);
            EmptyShelves.Add(id);
        }
    }

    private void SpawnCustomer()
    {

    }

    public int CustomerLined()
    {
        lineIndex = Mathf.Clamp(lineIndex + 1, 0, InnerCustomerCapacity - 1);
        return lineIndex;
    }

    public void CustomerDelined()
    {
        lineIndex = Mathf.Clamp(lineIndex - 1, 0, InnerCustomerCapacity - 1);
    }

    public void CustomerLeft(int moneyAmount)
    {
        Sales.PurchaseComplete(moneyAmount);

        
    }
}
