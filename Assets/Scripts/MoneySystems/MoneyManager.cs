using System.IO;
using TMPro;
using UnityEngine;

public class MoneyManager : MonoBehaviour
{
    public int playerBalance = 100 ; // Permanent player balance
    public int tempBalance;  // Temporary daily balance
    private int dailyEarnings; // Tracks daily earnings
    private int dailyExpenses; // Tracks daily expenses

    [Header("UI References")]
    public TMP_Text earningsText; // TMP object for earnings display
    public TMP_Text expensesText; // TMP object for expenses display

    private string saveFilePath;

    public static MoneyManager Instance;

    private void Awake()
    {
        Instance = this;

        // Initialize the save file path
        saveFilePath = Path.Combine(Application.persistentDataPath, "playerBalance.json");
       
    }

    /// <summary>
    /// Adds money to the temporary balance and tracks as daily earnings.
    /// </summary>
    public void AddToTempBalance(int amount)
    {
        if (amount <= 0) return;

        tempBalance += amount;
        dailyEarnings += amount;
        Debug.Log($"Added {amount} to temp balance. Current tempBalance: {tempBalance}");
    }

    /// <summary>
    /// Transfers the temporary balance to the permanent balance.
    /// Resets temp balance and daily tracking.
    /// </summary>
    public void TransferToPermanentBalance()
    {
        playerBalance += tempBalance;
        tempBalance = 0;
        ResetTempAccount();
        Debug.Log($"Transferred to permanent balance. Current playerBalance: {playerBalance}");
    }

    /// <summary>
    /// Deducts money directly from the permanent balance.
    /// Tracks the deduction as a daily expense.
    /// </summary>
    public bool ReduceMoney(int amount)
    {
        if (amount < 0 || amount > playerBalance)
        {
            Debug.LogWarning($"Cannot reduce {amount}. Not enough funds or invalid amount.");
            return false;
        }

        playerBalance -= amount;
        dailyExpenses += amount;
        Debug.Log($"Reduced {amount} from player balance. Remaining balance: {playerBalance}");
        return true;
    }

    /// <summary>
    /// Displays the current daily earnings and expenses.
    /// </summary>
    public void ShowSummary()
    {
        if (earningsText != null)
        {
            earningsText.text = $"Earnings: {dailyEarnings}";
        }

        if (expensesText != null)
        {
            expensesText.text = $"Expenses: {dailyExpenses}";
        }

        Debug.Log($"Summary - Earnings: {dailyEarnings}, Expenses: {dailyExpenses}");
    }

    /// <summary>
    /// Resets the temporary account and clears daily tracking data.
    /// </summary>
    public void ResetTempAccount()
    {
        tempBalance = 0;
        dailyEarnings = 0;
        dailyExpenses = 0;
        Debug.Log("Temporary account and daily tracking reset.");
    }

    /// <summary>
    /// Saves the player's permanent balance to a JSON file.
    /// </summary>
    public void SavePlayerBalance()
    {
        try
        {
            string json = JsonUtility.ToJson(new PlayerBalanceData { balance = playerBalance }, true);
            File.WriteAllText(saveFilePath, json);
            Debug.Log($"Balance saved successfully at {saveFilePath}");
        }
        catch
        {
            Debug.LogError("Failed to save player balance.");
        }
    }

    /// <summary>
    /// Loads the player's permanent balance from a JSON file.
    /// Initializes the balance if the file doesn't exist.
    /// </summary>
    public void LoadPlayerBalance()
    {
        if (!File.Exists(saveFilePath))
        {
            Debug.LogWarning("Save file not found. Initializing default balance.");
            playerBalance = 0;
            return;
        }

        try
        {
            string json = File.ReadAllText(saveFilePath);
            PlayerBalanceData data = JsonUtility.FromJson<PlayerBalanceData>(json);
            playerBalance = data.balance;
            Debug.Log("Balance loaded successfully.");
        }
        catch
        {
            Debug.LogError("Failed to load player balance. Initializing default balance.");
            playerBalance = 0;
        }
    }

    [System.Serializable]
    private class PlayerBalanceData
    {
        public int balance;
    }
}
