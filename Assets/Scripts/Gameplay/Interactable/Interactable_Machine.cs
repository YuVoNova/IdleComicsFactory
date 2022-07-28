using System.Collections.Generic;
using UnityEngine;

public class Interactable_Machine : Interactable
{
    [SerializeField]
    private Transform ComicsParent;

    private List<Comic> comicsList;

    private int currentIndex;

    [SerializeField]
    private float MagnetizeDuration;
    private float magnetizeTimer;

    [SerializeField]
    private float ComicSpawnDuration;
    private float comicSpawnTimer;

    [SerializeField]
    private bool isInteracting;

    protected override void Awake()
    {
        base.Awake();

        currentIndex = 0;

        comicSpawnTimer = ComicSpawnDuration;
        magnetizeTimer = MagnetizeDuration;

        comicsList = new List<Comic>();
        foreach (Transform child in ComicsParent)
        {
            comicsList.Add(child.GetComponent<Comic>());
        }

        isInteracting = false;
    }

    protected override void Update()
    {
        base.Update();

        if (comicSpawnTimer <= 0f)
        {
            SpawnComic();

            comicSpawnTimer = ComicSpawnDuration;
        }
        else
        {
            comicSpawnTimer -= Time.deltaTime;
        }
        if (isInteracting)
        {
            if (currentIndex > 0 && currentIndex <= comicsList.Count && Player.Instance.AvailableForComics)
            {
                if (magnetizeTimer <= 0f)
                {
                    currentIndex--;

                    comicsList[currentIndex].Magnetize();

                    magnetizeTimer = MagnetizeDuration;
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

        isInteracting = true;
    }

    public override void ExitInteraction()
    {
        base.ExitInteraction();

        comicSpawnTimer = ComicSpawnDuration;
        magnetizeTimer = MagnetizeDuration;

        isInteracting = false;
    }

    private void SpawnComic()
    {
        if (currentIndex != comicsList.Count)
        {
            if (!comicsList[currentIndex].gameObject.activeSelf)
            {
                comicsList[currentIndex].gameObject.SetActive(true);
                currentIndex++;
            }
        }
    }


}
