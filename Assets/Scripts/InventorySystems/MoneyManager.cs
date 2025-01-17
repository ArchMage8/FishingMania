using System;
using UnityEngine;
using TMPro;
using System.IO;
using System.Security.Cryptography;
using System.Text;

public class MoneyManager : MonoBehaviour
{
    [Header("Sales Money")]
    public int tempMoney;

    private static MoneyManager _instance;
    public static MoneyManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<MoneyManager>();

                if (_instance == null)
                {
                    GameObject singleton = new GameObject("MoneyManager");
                    _instance = singleton.AddComponent<MoneyManager>();
                    DontDestroyOnLoad(singleton);
                }
            }
            return _instance;
        }
    }

    [Header("Money Settings")]
    public int money;
    public int todayRevenue;
    public int todayExpense;

    [Header("UI References")]
    public TMP_Text moneyText;
    public TMP_Text revenueText;
    public TMP_Text expenseText;
    public GameObject revenueExpenseCanvas;

    private void Start()
    {
        UpdateMoneyUI();
        UpdateRevenueExpenseUI();
    }

    #region Core Money Management
    public void AddMoney(int amount)
    {
        if (amount <= 0) return;

        money += amount;
        todayRevenue += amount;
        UpdateMoneyUI();
        UpdateRevenueExpenseUI();
    }

    public bool CanDecreaseMoney(int amount)
    {
        return money >= amount;
    }

    public bool DecreaseMoney(int amount)
    {
        if (!CanDecreaseMoney(amount)) return false;

        money -= amount;
        todayExpense += amount;
        UpdateMoneyUI();
        UpdateRevenueExpenseUI();
        return true;
    }

    private void UpdateMoneyUI()
    {
        if (moneyText != null)
        {
            moneyText.text = $"Money: {money}";
        }
    }

    private void UpdateRevenueExpenseUI()
    {
        if (revenueText != null && expenseText != null)
        {
            revenueText.text = $"Today's Revenue: {todayRevenue}";
            expenseText.text = $"Today's Expense: {todayExpense}";

            if (revenueExpenseCanvas != null && !revenueExpenseCanvas.activeSelf)
            {
                revenueExpenseCanvas.SetActive(true);
            }
        }
    }
    #endregion

    #region Save/Load Money Data
    public void SaveMoney()
    {
        string json = JsonUtility.ToJson(new MoneyData(money));
        string encryptedJson = Encrypt(json);

        string filePath = Application.persistentDataPath + "/money.json";
        File.WriteAllText(filePath, encryptedJson);
        Debug.Log($"Money data saved to: {filePath}");
    }

    public void LoadMoney()
    {
        string path = Application.persistentDataPath + "/money.json";
        if (!File.Exists(path))
        {
            Debug.LogError("Money data file not found.");
            return;
        }

        string encryptedJson = File.ReadAllText(path);
        string decryptedJson = Decrypt(encryptedJson);
        MoneyData data = JsonUtility.FromJson<MoneyData>(decryptedJson);

        money = data.money;
        UpdateMoneyUI();
        Debug.Log("Money data loaded successfully.");
    }

    private string Encrypt(string plainText)
    {
        byte[] key = Encoding.UTF8.GetBytes("2203201822032018"); // Replace with a secure key
        byte[] iv = Encoding.UTF8.GetBytes("2203201822032018"); // Replace with a secure IV

        using (Aes aes = Aes.Create())
        {
            aes.Key = key;
            aes.IV = iv;

            ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

            using (var ms = new MemoryStream())
            {
                using (var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                {
                    using (var sw = new StreamWriter(cs))
                    {
                        sw.Write(plainText);
                    }
                }
                return Convert.ToBase64String(ms.ToArray());
            }
        }
    }

    private string Decrypt(string cipherText)
    {
        byte[] key = Encoding.UTF8.GetBytes("2203201822032018"); // Replace with a secure key
        byte[] iv = Encoding.UTF8.GetBytes("2203201822032018"); // Replace with a secure IV

        byte[] buffer = Convert.FromBase64String(cipherText);

        using (Aes aes = Aes.Create())
        {
            aes.Key = key;
            aes.IV = iv;

            ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

            using (var ms = new MemoryStream(buffer))
            {
                using (var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                {
                    using (var sr = new StreamReader(cs))
                    {
                        return sr.ReadToEnd();
                    }
                }
            }
        }
    }
    #endregion

    #region Helper Functions
    public void ClearRevenueExpense()
    {
        todayRevenue = 0;
        todayExpense = 0;
        UpdateRevenueExpenseUI();
    }

    public void ClearMoneySave()
    {
        string path = Application.persistentDataPath + "/money.json";

        if (File.Exists(path))
        {
            File.Delete(path);
            Debug.Log("Money save file cleared.");
        }
        else
        {
            Debug.LogWarning("No money save file found to clear.");
        }
    }
    #endregion

    public void AddToTempMoney(int amount)
    {
        if (amount > 0)
        {
            tempMoney += amount;
            Debug.Log($"Temp Money Increased: {tempMoney}");
        }
    }

    public void TransferTempMoneyToPlayer()
    {
        money += tempMoney;
        tempMoney = 0;
        UpdateMoneyUI();
        Debug.Log("Temp Money Transferred to Player");
    }


    [Serializable]
    public class MoneyData
    {
        public int money;

        public MoneyData(int money)
        {
            this.money = money;
        }
    }
}
