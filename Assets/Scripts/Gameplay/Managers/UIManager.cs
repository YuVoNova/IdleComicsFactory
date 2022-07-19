using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    // Singleton

    public static UIManager Instance;


    // Money Panel

    [SerializeField]
    private TMP_Text MoneyText;


    // Unity Functions

    private void Awake()
    {
        Instance = this;

        //UpdateMoneyText();
    }

    // Methods

    public void UpdateMoneyText()
    {
        MoneyText.text = "" + Manager.Instance.PlayerData.Money;
    }
}
