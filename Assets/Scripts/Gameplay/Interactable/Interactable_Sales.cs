using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable_Sales : Interactable
{
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
}
