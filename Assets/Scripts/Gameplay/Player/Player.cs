using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

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

    [SerializeField]
    private Rig Rig;


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

        Rig.weight = 0f;
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

        if (Rig.weight < 1f)
        {
            Rig.weight = 1f;
        }

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

            if (currentComicCount == 0)
            {
                Rig.weight = 0f;
            }

            return true;
        }
        else
        {
            return false;
        }
    }


}
