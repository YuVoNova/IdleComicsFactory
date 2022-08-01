using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Interactable_BuyShelf : Interactable
{
    [SerializeField]
    private int ID;

    [SerializeField]
    private TMP_Text PriceText;
    [SerializeField]
    private Image FillerCircle;

    [SerializeField]
    private GameObject Shelf;

    [SerializeField]
    private int BasePrice;
    private int currentPrice;
    private int payValue;
    private int step;

    [SerializeField]
    private bool isFirst;

    protected override void Awake()
    {
        base.Awake();
    }

    public override void ExitPreInteraction()
    {
        base.ExitPreInteraction();

        if (currentPrice != 0)
        {
            Player.Instance.MoneyFlow.StartFlow(Player.Instance.transform, transform);
        }
    }

    public override void ExitInteraction()
    {
        base.ExitInteraction();

        Player.Instance.MoneyFlow.EndFlow();
    }

    protected override void ProgressInteraction()
    {
        base.ProgressInteraction();

        if (Manager.Instance.PlayerData.Money > 0 && GameManager.Instance.IsGameOn)
        {
            if (currentPrice != 0)
            {
                if (Manager.Instance.PlayerData.Money >= step)
                {
                    payValue = Mathf.FloorToInt(Mathf.Clamp(step, 0f, currentPrice));
                }
                else
                {
                    if (Manager.Instance.PlayerData.Money >= currentPrice)
                    {
                        payValue = currentPrice;
                    }
                    else
                    {
                        payValue = Manager.Instance.PlayerData.Money;
                    }
                }

                currentPrice = Mathf.FloorToInt(Mathf.Clamp(currentPrice - payValue, 0f, float.MaxValue));
                // TO DO LATER -> Save currentPrice to PlayerData here.

                GameManager.Instance.MoneySpent(payValue);

                UpdatePriceText();

                if (currentPrice == 0)
                {
                    Player.Instance.MoneyFlow.EndFlow();

                    EnableShelf();
                }
            }
        }
        else
        {
            Player.Instance.MoneyFlow.EndFlow();
        }
    }

    private void UpdatePriceText()
    {
        PriceText.text = currentPrice + "";

        FillerCircle.fillAmount = 1f - ((float)currentPrice / BasePrice);
    }

    private void EnableShelf()
    {
        Shelf.SetActive(true);

        gameObject.SetActive(false);

        GameManager.Instance.AddShelf(ID);
    }

    public void Initialize()
    {
        currentPrice = BasePrice;
        step = Mathf.FloorToInt(Mathf.Clamp(currentPrice / 50f, 1f, float.MaxValue));

        if (isFirst)
        {
            EnableShelf();
        }
        else
        {
            UpdatePriceText();
            Shelf.SetActive(false);
        }
    }
}
