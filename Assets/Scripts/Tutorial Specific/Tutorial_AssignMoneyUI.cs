using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Tutorial_AssignMoneyUI : MonoBehaviour
{
    public TMP_Text TMP_UIObject;

    private void Start()
    {
        AssignMoneyUI();
    }

    private void AssignMoneyUI()
    {
        
        MoneyDisplay moneyDisplay = FindObjectOfType<MoneyDisplay>();

        if (moneyDisplay != null && TMP_UIObject != null)
        {
            // Assign the TMP_Text object to MoneyDisplay's Money_UI
            moneyDisplay.Money_UI = TMP_UIObject;
           
        }
        else
        {
            Debug.LogWarning("MoneyDisplay or TMP_UIObject is not assigned correctly.");
        }
    }
}
