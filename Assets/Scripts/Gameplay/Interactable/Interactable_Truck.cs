using UnityEngine;

public class Interactable_Truck : Interactable
{
    [SerializeField]
    private Interactable_MoneyArea MoneyArea;

    [SerializeField]
    private Transform TruckTargetCollider;

    [SerializeField]
    private GameObject[] PackageObjects;

    [SerializeField]
    private int AmountPerSale;

    private int truckCapacity;

    private bool isInteracting;

    protected override void Awake()
    {
        base.Awake();

        isInteracting = false;

        truckCapacity = Player.Instance.ComicCapacity;

        //SpawnMoney(100);
    }

    protected override void Update()
    {
        base.Update();

        if (isInteracting)
        {
            
        }
    }

    public override void ExitPreInteraction()
    {
        base.ExitPreInteraction();

        isInteracting = true;
    }

    public override void ExitInteraction()
    {
        base.ExitInteraction();

        isInteracting = false;
    }


}
