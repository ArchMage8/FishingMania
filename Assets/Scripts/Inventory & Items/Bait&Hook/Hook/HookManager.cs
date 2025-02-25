using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class HookManager : MonoBehaviour
{
    public static HookManager Instance; // Singleton instance

    [SerializeField] private HookMasterListSO hookMasterList; // Reference to the master list
    private List<HookData> hookDataList = new List<HookData>(); // In-game hook data

    private string saveFilePath;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        saveFilePath = Path.Combine(Application.persistentDataPath, "hooks.json");

        if (File.Exists(saveFilePath)) LoadHookData();
        else InitializeDefaultHooks();
    }

    // Load default hooks from the master list (only on a new game)
    private void InitializeDefaultHooks()
    {
        hookDataList.Clear();
        foreach (HookSO hook in hookMasterList.hooks)
        {
            hookDataList.Add(new HookData(hook.hookName, hook.description, hook.Class, false)); // Default: Locked
        }
        SaveHookData(); // Save initial state
    }

    // Load hooks from JSON
    public void LoadHookData()
    {
        if (!File.Exists(saveFilePath)) return;

        string json = File.ReadAllText(saveFilePath);
        HookSaveData saveData = JsonUtility.FromJson<HookSaveData>(json);

        hookDataList = saveData.hooks;
        Debug.Log("Hook data loaded.");
    }

    // Save hooks to JSON
    public void SaveHookData()
    {
        HookSaveData saveData = new HookSaveData { hooks = hookDataList };
        string json = JsonUtility.ToJson(saveData, true);
        File.WriteAllText(saveFilePath, json);
        Debug.Log("Hook data saved.");
    }

    // Unlocks a hook by name
    public void UnlockHook(string hookName)
    {
        foreach (HookData hook in hookDataList)
        {
            if (hook.hookName == hookName)
            {
                hook.isUnlocked = true;
                //Debug.Log($"Hook '{hookName}' unlocked!");
                return;
            }
        }
        //Debug.LogWarning($"Hook '{hookName}' not found.");
    }

    // Returns a hook's current status
    public bool IsHookUnlocked(string hookName)
    {
        foreach (HookData hook in hookDataList)
        {
            if (hook.hookName == hookName) return hook.isUnlocked;
        }
        return false;
    }

    // Get all hooks
    public List<HookData> GetAllHooks() => hookDataList;
}

// Serializable Hook Data (for JSON saving)
[System.Serializable]
public class HookData
{
    public string hookName;
    public string description;
    public int hookClass;
    public bool isUnlocked;

    public HookData(string name, string desc, int hClass, bool unlocked)
    {
        hookName = name;
        description = desc;
        hookClass = hClass;
        isUnlocked = unlocked;
    }
}

// Wrapper class for saving multiple hooks
[System.Serializable]
public class HookSaveData
{
    public List<HookData> hooks;
}
  