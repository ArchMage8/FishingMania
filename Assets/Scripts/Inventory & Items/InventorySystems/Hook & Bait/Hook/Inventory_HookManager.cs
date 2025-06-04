using System.Collections.Generic;
using System.IO;
using UnityEngine;

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

    public int currentCombo; // Store the combo here!

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

    public bool GetHookStatus(int hookClass)
    {
        HookData hook = hooksInGame.Find(h => h.Class == hookClass);
        if (hook != null)
            return hook.Unlocked;

        Debug.LogWarning($"Hook class {hookClass} not found!");
        return false;
    }

    public void UnlockHook(int hookClass)
    {
        HookData hook = hooksInGame.Find(h => h.Class == hookClass);
        if (hook != null)
        {
            hook.Unlocked = true;
            SaveHooks();
        }
        else
        {
            Debug.LogWarning($"Hook class {hookClass} not found!");
        }
    }

    public void ResetHooks()
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
            string json = File.ReadAllText(savePath);
            SaveData data = JsonUtility.FromJson<SaveData>(json);

            hooksInGame = data.Hooks;
            currentCombo = data.CurrentCombo;
        }
        else //NewGame
        {
            ResetHooks();
            currentCombo = 11;
            SaveHooks();
        }
    }

    public void SaveHooks()
    {
        SaveData data = new SaveData
        {
            Hooks = hooksInGame,
            CurrentCombo = currentCombo
        };

        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(savePath, json);
    }
}
