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
    private List<Interactable_Shelf> Shelves;

    [HideInInspector]
    public List<int> EmptyShelves;
    [HideInInspector]
    public List<int> AvailableShelves;


    // Values

    [Header("Values", order = 0)]

    [HideInInspector]
    public bool IsGameOn;
    [HideInInspector]
    public bool OnMenu;


    // Unity Functions

    private void Awake()
    {
        Instance = this;

        RebuildNavMesh();

        IsGameOn = true;
        OnMenu = false;
    }

    private void Start()
    {

    }

    private void Update()
    {

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
}
