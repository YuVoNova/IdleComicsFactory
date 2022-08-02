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

    [SerializeField]
    private GameObject Level_1;

    [SerializeField]
    private Interactable_BuyShelf[] BuyShelves;
    public List<Interactable_Shelf> Shelves;

    //[HideInInspector]
    public List<int> EmptyShelves;
    //[HideInInspector]
    public List<int> AvailableShelves;

    [SerializeField]
    private GameObject CustomerPrefab;

    [HideInInspector]
    public List<Customer> InsideCostumers;
    [HideInInspector]
    public List<Customer> OutsideCustomers;

    public Transform[] CustomerLinePoints;
    public Transform[] CustomerRegisterLinePoints;
    public Transform[] CustomerExitPoints;
    public Transform CustomerSpawnPoint;

    [SerializeField]
    private Interactable_Sales Sales;


    // Values

    [Header("Values", order = 0)]

    [HideInInspector]
    public bool IsGameOn;
    [HideInInspector]
    public bool OnMenu;

    private int availableComicCount;
    private int registerLineCount;

    [SerializeField]
    private int InsideCustomerCapacity;
    [SerializeField]
    private int OutsideCustomerCapacity;

    [SerializeField]
    private float CustomerSpawnDuration;
    private float customerSpawnTimer;


    // Unity Functions

    private void Awake()
    {
        Instance = this;

        RebuildNavMesh();

        IsGameOn = true;
        OnMenu = false;

        InsideCostumers = new List<Customer>();
        OutsideCustomers = new List<Customer>();

        availableComicCount = 0;
        registerLineCount = 0;

        for (int i = 0; i < OutsideCustomerCapacity; i++)
        {
            SpawnCustomer(i, true);
        }

        for (int i = 0; i < BuyShelves.Length; i++)
        {
            BuyShelves[i].Initialize();
        }

        customerSpawnTimer = CustomerSpawnDuration;
    }

    private void Start()
    {
        RebuildNavMesh();
    }

    private void Update()
    {
        if (IsGameOn)
        {
            if (customerSpawnTimer <= 0f)
            {
                if (AvailableShelves.Count > 0 && availableComicCount > 0 && InsideCostumers.Count < InsideCustomerCapacity)
                {
                    EnableCustomer();
                }

                customerSpawnTimer = CustomerSpawnDuration;
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

        UIManager.Instance.UpdateMoneyText();

        //Player.Instance.AudioSource.volume = 0.5f;
        //Player.Instance.AudioSource.clip = Manager.Instance.Audios["Money"];
        //Player.Instance.AudioSource.Play();

        Manager.Instance.Save();
    }

    public void MoneySpent(int amount)
    {
        Manager.Instance.PlayerData.Money = Mathf.FloorToInt(Mathf.Clamp(Manager.Instance.PlayerData.Money - amount, 0f, float.MaxValue));

        UIManager.Instance.UpdateMoneyText();

        //Player.Instance.AudioSource.volume = 0.5f;
        //Player.Instance.AudioSource.clip = Manager.Instance.Audios["Money"];
        //Player.Instance.AudioSource.Play();

        Manager.Instance.Save();
    }

    public void EnableExpansion()
    {
        Level_1.SetActive(true);

        RebuildNavMesh();
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

    public void ComicAvailable(int id)
    {
        availableComicCount++;
    }

    private void SpawnCustomer(int lineIndex, bool isInitial)
    {
        Customer spawnedCustomer = Instantiate(CustomerPrefab, CustomerSpawnPoint.position, CustomerSpawnPoint.rotation).GetComponent<Customer>();
        spawnedCustomer.Initialize(lineIndex);

        if (isInitial)
        {
            spawnedCustomer.transform.position = CustomerLinePoints[lineIndex].position;
        }

        OutsideCustomers.Add(spawnedCustomer.GetComponent<Customer>());
    }

    private void EnableCustomer()
    {
        int index = -1;

        for (int i = 0; i < AvailableShelves.Count; i++)
        {
            if (Shelves[AvailableShelves[i]].GetAvailableComicCount() > 0)
            {
                index = AvailableShelves[i];
                break;
            }
        }

        if (index != -1)
        {
            availableComicCount = Mathf.Clamp(availableComicCount - 1, 0, InsideCustomerCapacity);
            Shelves[index].Reserved();

            OutsideCustomers[0].Enable(Shelves[index].ShelfCustomerPoint, index);

            for (int i = 0; i < OutsideCustomers.Count; i++)
            {
                OutsideCustomers[i].LineChanged();
            }

            InsideCostumers.Add(OutsideCustomers[0]);
            OutsideCustomers.RemoveAt(0);
        }

        SpawnCustomer(OutsideCustomerCapacity - 1, false);
    }

    public int GetRegisterLine()
    {
        int index = registerLineCount;
        registerLineCount = Mathf.Clamp(registerLineCount + 1, 0, InsideCustomerCapacity);

        return index;
    }

    public void CustomerLeft(Customer customer, int moneyAmount)
    {
        Sales.PurchaseComplete(moneyAmount);

        InsideCostumers.Remove(customer);

        registerLineCount = Mathf.Clamp(registerLineCount - 1, 0, InsideCustomerCapacity);

        for (int i = 0; i < InsideCostumers.Count; i++)
        {
            InsideCostumers[i].RegisterLineChanged();
        }
    }
}
