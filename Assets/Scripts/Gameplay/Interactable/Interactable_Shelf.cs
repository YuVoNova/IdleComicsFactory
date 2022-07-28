using UnityEngine;

public class Interactable_Shelf : Interactable
{
    [SerializeField]
    private int ID;

    [SerializeField]
    private Transform ShelfTargetCollider;

    [SerializeField]
    private GameObject[] ComicObjects;

    private int currentIndex;
    private int comicCapacity;

    [SerializeField]
    private float MagnetizeDuration;
    private float magnetizeTimer;

    private bool isInteracting;

    protected override void Awake()
    {
        base.Awake();

        currentIndex = 0;
        comicCapacity = ComicObjects.Length;
        magnetizeTimer = MagnetizeDuration;

        isInteracting = false;
    }

    protected override void Update()
    {
        base.Update();

        if (isInteracting)
        {
            if (currentIndex < comicCapacity)
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

    public void TakeComic()
    {
        ComicObjects[currentIndex].SetActive(true);

        currentIndex = Mathf.Clamp(currentIndex + 1, 0, comicCapacity);

        if (currentIndex == comicCapacity)
        {
            ExitInteraction();
        }
    }

    public bool GiveComic()
    {
        if (currentIndex > 0)
        {
            currentIndex--;

            ComicObjects[currentIndex].SetActive(false);

            return true;
        }
        else
        {
            return false;
        }
    }


}
