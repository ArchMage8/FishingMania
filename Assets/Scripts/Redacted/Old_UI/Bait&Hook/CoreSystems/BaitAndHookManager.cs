using UnityEngine;
using System.IO;

public class BaitAndHookManager : MonoBehaviour
{
    public static BaitAndHookManager Instance;

    [Header("Active Bait and Hook")]
    [HideInInspector] public BaitSO activeBait;
    [HideInInspector] public HookSO activeHook;

    [Header("Class Values")]
    public int BaitClass;
    public int HookClass;

    [Header("Hook Status Management")]
    public HookStatusSO defaultHookStatusSO;
    [HideInInspector] public HookStatusData[] hookStatuses;

    [Header("Temporary Selections")]
    private HookSO TempHook;
    private BaitSO TempBait;

    [Header("Default Combo")]
    public DefaultCombo defaultCombo;

    [Header("Save File Paths")]
    private string baitHookSaveFile;
    private string hookStatusSaveFile;

    private void Awake()
    {
        Instance = this;
        baitHookSaveFile = Path.Combine(Application.persistentDataPath, "baitHookSave.json");
        hookStatusSaveFile = Path.Combine(Application.persistentDataPath, "hookStatusSave.json");
        LoadData();
        SetClass();
    }

    public void SetActiveBaitAndHook(BaitSO newBait, HookSO newHook)
    {
        activeBait = newBait;
        activeHook = newHook;
        SetClass();
    }

    public void SetClass()
    {
        if (activeBait != null)
            BaitClass = activeBait.baitClass;

        if (activeHook != null)
            HookClass = activeHook.hookClass;
    }

    public bool IsHookUnlocked(string HookName)
    {
        foreach (var hook in hookStatuses)
        {
            if (hook.hookName == HookName)
                return hook.isUnlocked;
        }

        return false;
    }

    public void SaveData()
    {
        SaveBaitAndHook();
        SaveHookStatuses();
    }

    public void LoadData()
    {
        LoadBaitAndHook();
        LoadHookStatuses();
    }

    private void SaveBaitAndHook()
    {
        BaitHookSaveData saveData = new BaitHookSaveData
        {
            baitName = activeBait != null ? activeBait.name : "",
            hookName = activeHook != null ? activeHook.name : ""
        };

        string json = JsonUtility.ToJson(saveData, true);
        File.WriteAllText(baitHookSaveFile, json);
    }

    private void LoadBaitAndHook()
    {
        if (!File.Exists(baitHookSaveFile))
        {
            activeBait = defaultCombo.defaultBait;
            activeHook = defaultCombo.defaultHook;
            return;
        }

        string json = File.ReadAllText(baitHookSaveFile);
        BaitHookSaveData saveData = JsonUtility.FromJson<BaitHookSaveData>(json);

        activeBait = Resources.Load<BaitSO>("Baits/" + saveData.baitName);
        activeHook = Resources.Load<HookSO>("Hooks/" + saveData.hookName);

        SetClass();
    }

    private void SaveHookStatuses()
    {
        string json = JsonUtility.ToJson(new HookStatusWrapper { hookStatuses = hookStatuses }, true);
        File.WriteAllText(hookStatusSaveFile, json);
    }

    private void LoadHookStatuses()
    {
        if (File.Exists(hookStatusSaveFile))
        {
            string json = File.ReadAllText(hookStatusSaveFile);
            HookStatusWrapper loadedData = JsonUtility.FromJson<HookStatusWrapper>(json);
            hookStatuses = loadedData.hookStatuses;
        }
        else
        {
            hookStatuses = defaultHookStatusSO.defaultHookStatuses;
            SaveHookStatuses();
        }
    }

    public void SetTempHook(HookSO hook)
    {
        TempHook = hook;
    }

    public void SetTempBait(BaitSO bait)
    {
        TempBait = bait;
    }

    public void ConfirmTemps()
    {
        if (TempHook != null)
            activeHook = TempHook;

        if (TempBait != null)
            activeBait = TempBait;

        SetClass();
    }

    public void ResetTemps()
    {
        TempHook = null;
        TempBait = null;
    }
}

[System.Serializable]
public class BaitHookSaveData
{
    public string baitName;
    public string hookName;
}

[System.Serializable]
public class HookStatusWrapper
{
    public HookStatusData[] hookStatuses;
}
