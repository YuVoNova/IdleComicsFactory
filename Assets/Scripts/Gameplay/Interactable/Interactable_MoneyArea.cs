using System.Collections.Generic;
using UnityEngine;

public class Interactable_MoneyArea : Interactable
{
    [SerializeField]
    private Transform AreaMoneyParent;

    [SerializeField]
    private int MagnetizeStep;

    private List<AreaMoney> areaMoneyList;

    private int currentIndex;
    private int stepIndex;

    private bool isInteracting;


    protected override void Awake()
    {
        base.Awake();

        currentIndex = 0;

        areaMoneyList = new List<AreaMoney>();
        foreach (Transform child in AreaMoneyParent)
        {
            areaMoneyList.Add(child.GetComponent<AreaMoney>());
        }

        isInteracting = false;
    }

    protected override void Update()
    {
        base.Update();

        if (isInteracting)
        {
            for (stepIndex = 0; stepIndex < MagnetizeStep; stepIndex++)
            {
                if (currentIndex > 0 && currentIndex <= areaMoneyList.Count)
                {
                    currentIndex--;

                    areaMoneyList[currentIndex].Magnetize();
                }
                else
                {
                    break;
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

        isInteracting = false;
    }

    public void SpawnMoney(int amount)
    {
        int nextLimit = currentIndex + amount;

        while (currentIndex < nextLimit)
        {
            if (currentIndex < areaMoneyList.Count)
            {
                if (!areaMoneyList[currentIndex].gameObject.activeSelf)
                {
                    areaMoneyList[currentIndex].gameObject.SetActive(true);
                    currentIndex++;
                }
            }
            else
            {
                // TO DO -> Print "MAX" on top of the pile.

                break;
            }
        }
    }
}
