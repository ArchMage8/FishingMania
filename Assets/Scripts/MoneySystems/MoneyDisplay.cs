using TMPro;
using UnityEngine;

public class MoneyDisplay : MonoBehaviour
{
    public TMP_Text Money_UI;

    private void Update()
    {
        if (Money_UI != null && MoneyManager.Instance != null)
        {
            Money_UI.text = $"Money: {MoneyManager.Instance.playerBalance}";
        }
    }
}
