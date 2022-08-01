using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable_Sales : Interactable
{
    [SerializeField]
    private Interactable_MoneyArea MoneyArea;

    public override void ExitPreInteraction()
    {
        base.ExitPreInteraction();

        Player.Instance.IsOpenForSales = true;
    }

    public override void ExitInteraction()
    {
        base.ExitInteraction();

        Player.Instance.IsOpenForSales = false;
    }

    public void PurchaseComplete(int amount)
    {
        MoneyArea.SpawnMoney(amount);
    }
}
