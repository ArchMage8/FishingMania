using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Inventory_HookManager : MonoBehaviour
{
    public static Inventory_HookManager Instance { get; private set; }

    [System.Serializable]
    public class HookData
    {
        public int Class;
        public bool Unlocked;
    }

    [System.Serializable]
    private class HookDataListWrapper
    {
        public List<HookData> Hooks;
    }

    private List<HookData> hooksInGame = new List<HookData>();
    private string savePath;

    private void Awake()
    {
        Instance = this;
        savePath = Path.Combine(Application.persistentDataPath, "hooks.json");
    }

    public void Hook_NewGame()
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
        SaveHooks();
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
            HookDataListWrapper wrapper = JsonUtility.FromJson<HookDataListWrapper>(json);
            hooksInGame = wrapper.Hooks;
        }
        else
        {
            ResetHooks();
            SaveHooks();
        }
    }

    public void SaveHooks()
    {
        HookDataListWrapper wrapper = new HookDataListWrapper { Hooks = hooksInGame };
        string json = JsonUtility.ToJson(wrapper, true);
        File.WriteAllText(savePath, json);
    }
}
