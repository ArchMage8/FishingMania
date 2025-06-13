using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System.Collections;

public class Inventory_HookManager : MonoBehaviour
{
    [System.Serializable]
    public class HookData
    {
        public int Class;
        public bool Unlocked;
    }

    [System.Serializable]
    private class SaveData
    {
        public List<HookData> Hooks;
        public int CurrentCombo;
    }

    public static Inventory_HookManager Instance { get; private set; }

    private List<HookData> hooksInGame = new List<HookData>();
    private string savePath;

    public int SavedCombo; // Store the combo here!

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
           
        }
        else
        {
            Destroy(gameObject);
        }

        
        savePath = Path.Combine(Application.persistentDataPath, "hooks.json");
    }

    private void Start()
    {
        LoadHooks();
        Inventory_EquipmentManager.Instance.LoadCurrentCombo();
        Inventory_EquipmentManager.Instance.RestoreActiveEffects();
    }


    public bool GetHookStatus(int hookClass)
    {
        HookData hook = hooksInGame.Find(h => h.Class == hookClass);
        if (hook != null)
        {
            return hook.Unlocked;
        }

        else
        {
            Debug.LogWarning($"Hook class {hookClass} not found!");
            return false;
        }

       
    }

    public void UnlockHook(int hookClass)
    {
        HookData hook = hooksInGame.Find(h => h.Class == hookClass);
        if (hook != null)
        {
            hook.Unlocked = true;
        }
        else
        {
            Debug.LogWarning($"Hook class {hookClass} not found!");
        }
    }

    public void ResetHooks() //This Resets the In-Game list to inital settings
    {
        hooksInGame.Clear();
        for (int i = 1; i <= 5; i++)
        {
            hooksInGame.Add(new HookData
            {
                Class = i,
                Unlocked = (i == 1)
            });
        }

        
    }

    public void LoadHooks()
    {
        if (File.Exists(savePath))
        {
            //Debug.Log(savePath);

            //Debug.Log("Call A");
            
            string json = File.ReadAllText(savePath);
            SaveData data = JsonUtility.FromJson<SaveData>(json);

            hooksInGame = data.Hooks;
            SavedCombo = data.CurrentCombo;
        }
        else //NewGame
        {
           

            ResetHooks();
            Inventory_EquipmentManager.Instance.currentCombo = 11;
            SaveEquipmentData();
        }
    }

    public void SaveEquipmentData()
    {
        SaveData data = new SaveData
        {
            Hooks = hooksInGame,
            CurrentCombo = Inventory_EquipmentManager.Instance.currentCombo

            //This "CurrentCombo" is a variable within the JSON Data
        };

        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(savePath, json);
    }

    public void DeleteHookData()
    {
        if (File.Exists(savePath))
        {
            File.Delete(savePath);
        }
    }
}
