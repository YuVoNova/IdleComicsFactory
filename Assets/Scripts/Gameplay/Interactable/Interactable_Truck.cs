using System.Collections;
using UnityEngine;

public class Interactable_Truck : Interactable
{
    [SerializeField]
    private Interactable_MoneyArea MoneyArea;

    [SerializeField]
    private Animator Animator;

    [SerializeField]
    private Transform TruckTargetCollider;

    [SerializeField]
    private Transform TruckTransform;
    [SerializeField]
    private Transform TruckArrivalPoint;

    private Vector3 originPosition;

    [SerializeField]
    private GameObject[] PackageObjects;

    [SerializeField]
    private int AmountPerSale;

    private int truckCapacity;
    private int currentIndex;

    [SerializeField]
    private float MagnetizeDuration;
    private float magnetizeTimer;

    [SerializeField]
    private float MoveTruckDuration;
    private float moveTruckTimer;

    [SerializeField]
    private float MinSpeed;
    [SerializeField]
    private float MaxSpeed;
    [SerializeField]
    private float Acceleration;

    private float truckSpeed;

    private bool isInteracting;
    private bool isAvailable;

    protected override void Awake()
    {
        base.Awake();

        currentIndex = 0;
        magnetizeTimer = MagnetizeDuration;

        originPosition = TruckTransform.localPosition;

        isInteracting = false;
        isAvailable = true;
    }

    private void Start()
    {
        truckCapacity = Player.Instance.ComicCapacity;

        Animator.SetBool("isOpen", true);
    }

    protected override void Update()
    {
        base.Update();

        if (isAvailable)
        {
            if (isInteracting)
            {
                if (magnetizeTimer <= 0f)
                {
                    if (Player.Instance.GiveComic(TruckTargetCollider.transform.position))
                    {
                        magnetizeTimer = MagnetizeDuration;
                    }
                    else
                    {
                        ExitInteraction();
                    }
                }
                else
                {
                    magnetizeTimer -= Time.deltaTime;
                }
            }
        }
    }

    public override void ExitPreInteraction()
    {
        base.ExitPreInteraction();

        magnetizeTimer = MagnetizeDuration;

        isInteracting = true;
    }

    public override void ExitInteraction()
    {
        base.ExitInteraction();

        if (currentIndex > 0 && isAvailable)
        {
            isAvailable = false;

            StartCoroutine(MoveTruck());
        }

        isInteracting = false;
    }

    public void TakeComic()
    {
        PackageObjects[currentIndex].SetActive(true);

        currentIndex = Mathf.Clamp(currentIndex + 1, 0, truckCapacity);

        if (currentIndex == truckCapacity)
        {
            ExitInteraction();
        }
    }

    private IEnumerator MoveTruck()
    {
        yield return new WaitForSeconds(MoveTruckDuration);

        Animator.SetBool("isOpen", false);

        MoneyArea.SpawnMoney(currentIndex);

        yield return new WaitForSeconds(0.25f);

        truckSpeed = MinSpeed;
        while (Vector3.Distance(TruckTransform.localPosition, TruckArrivalPoint.localPosition) > 0.2f)
        {
            TruckTransform.localPosition = Vector3.MoveTowards(TruckTransform.localPosition, TruckArrivalPoint.localPosition, truckSpeed * Time.deltaTime);
            truckSpeed = Mathf.Clamp(truckSpeed + Acceleration, MinSpeed, MaxSpeed);

            yield return null;
        }

        for (int i = 0; i < truckCapacity; i++)
        {
            PackageObjects[i].SetActive(false);
        }

        truckSpeed = MaxSpeed;
        while (Vector3.Distance(TruckTransform.localPosition, originPosition) > 0.2f)
        {
            TruckTransform.localPosition = Vector3.MoveTowards(TruckTransform.localPosition, originPosition, truckSpeed * Time.deltaTime);
            truckSpeed = Mathf.Clamp(truckSpeed - Acceleration, MinSpeed, MaxSpeed);

            yield return null;
        }

        Animator.SetBool("isOpen", true);

        currentIndex = 0;

        isAvailable = true;
    }


}
