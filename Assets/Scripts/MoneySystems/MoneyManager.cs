using System.IO;
using UnityEngine;

public class MoneyManager : MonoBehaviour
{
    public int playerBalance = 100; // Permanent player balance
    public int tempBalance;  // Temporary daily balance
    public int Today_Earnings { get; private set; }
    public int Today_Expenses { get; private set; }

    private string saveFilePath;

    public static MoneyManager Instance;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
          
            Destroy(gameObject);
            return;
        }

        Instance = this;
        saveFilePath = Path.Combine(Application.persistentDataPath, "playerBalance.json");


    }

    public void AddToTempBalance(int amount)
    {
        if (amount <= 0) return;

        tempBalance += amount;
        Today_Earnings += amount;
        Debug.Log($"Added {amount} to temp balance. Current tempBalance: {tempBalance}");
    }

    public void TransferToPermanentBalance()
    {
        playerBalance = Mathf.Min(playerBalance + tempBalance, 99999999);

        tempBalance = 0;
        ResetTempAccount();
        Debug.Log($"Transferred to permanent balance. Current playerBalance: {playerBalance}");
    }

    public bool ReduceMoney(int amount)
    {
        if (amount < 0 || amount > playerBalance)
        {
            Debug.LogWarning($"Cannot reduce {amount}. Not enough funds or invalid amount.");
            return false;
        }

        playerBalance -= amount;
        Today_Expenses += amount;
        Debug.Log($"Reduced {amount} from player balance. Remaining balance: {playerBalance}");
        return true;
    }

    public void ResetTempAccount()
    {
        tempBalance = 0;
        Today_Earnings = 0;
        Today_Expenses = 0;
        Debug.Log("Temporary account and daily tracking reset.");
    }

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
