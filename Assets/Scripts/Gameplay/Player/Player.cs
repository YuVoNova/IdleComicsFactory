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

    [SerializeField]
    private PlayerComic[] Comics;

    public Animator Animator;

    public MoneyFlow MoneyFlow;
    public Transform MoneyFlowPoint;

    public AudioSource AudioSource;


    // Values

    [Header("Values", order = 0)]

    public int ComicCapacity;
    private int currentComicCount;

    [HideInInspector]
    public bool AvailableForComics;
    [HideInInspector]
    public bool IsOpenForSales;


    // Unity Functions

    private void Awake()
    {
        Instance = this;

        currentComicCount = 0;

        AvailableForComics = true;
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

    public void TakeComic()
    {
        Comics[currentComicCount].gameObject.SetActive(true);

        currentComicCount++;

        /*
        AudioSource.volume = 0.4f;
        AudioSource.clip = Manager.Instance.Audios["EnergyPickup"];
        AudioSource.Play();
        */

        if (currentComicCount >= ComicCapacity)
        {
            AvailableForComics = false;

            // TO DO -> Print "MAX" here.
        }
    }

    public bool GiveComic(Vector3 position)
    {
        if (currentComicCount > 0)
        {
            currentComicCount--;

            Comics[currentComicCount].Magnetize(position);

            /*
            AudioSource.volume = 0.4f;
            AudioSource.clip = Manager.Instance.Audios["EnergyAcquired"];
            AudioSource.Play();
            */

            if (currentComicCount < ComicCapacity)
            {
                AvailableForComics = true;
            }

            return true;
        }
        else
        {
            return false;
        }
    }


}
