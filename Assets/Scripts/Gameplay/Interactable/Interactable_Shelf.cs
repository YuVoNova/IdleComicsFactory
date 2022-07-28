using UnityEngine;

public class Interactable_Shelf : Interactable
{
    private int ID;

    [SerializeField]
    private Transform ShelfTargetCollider;

    [SerializeField]
    private GameObject[] ComicObjects;

    [HideInInspector]
    public int CurrentIndex;
    [HideInInspector]
    public int ReservedIndex;
    private int comicCapacity;

    [SerializeField]
    private float MagnetizeDuration;
    private float magnetizeTimer;

    private bool isInteracting;

    protected override void Awake()
    {
        base.Awake();

        CurrentIndex = 0;
        ReservedIndex = 0;
        comicCapacity = ComicObjects.Length;
        magnetizeTimer = MagnetizeDuration;

        isInteracting = false;
    }

    protected override void Update()
    {
        base.Update();

        if (isInteracting)
        {
            if (CurrentIndex < comicCapacity)
            {
                if (magnetizeTimer <= 0f)
                {
                    if (Player.Instance.GiveComic(ShelfTargetCollider.transform.position))
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
            else
            {
                ExitInteraction();
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

        isInteracting = false;
    }

    public void SetID(int id)
    {
        ID = id;
    }

    public void TakeComic()
    {
        ComicObjects[CurrentIndex].SetActive(true);

        CurrentIndex = Mathf.Clamp(CurrentIndex + 1, 0, comicCapacity);

        if (CurrentIndex > 0)
        {
            GameManager.Instance.ShelfAvailable(ID);
        }

        if (CurrentIndex == comicCapacity)
        {
            ExitInteraction();
        }
    }

    public bool GiveComic()
    {
        if (CurrentIndex > 0 && ReservedIndex > 0)
        {
            CurrentIndex--;
            ReservedIndex--;

            ComicObjects[CurrentIndex].SetActive(false);

            if (CurrentIndex == 0)
            {
                ReservedIndex = 0;
                GameManager.Instance.ShelfExpired(ID);
            }

            return true;
        }
        else
        {
            return false;
        }
    }

    public int GetAvailableComicCount()
    {
        return Mathf.Clamp(CurrentIndex - ReservedIndex, 0, comicCapacity);
    }
}
