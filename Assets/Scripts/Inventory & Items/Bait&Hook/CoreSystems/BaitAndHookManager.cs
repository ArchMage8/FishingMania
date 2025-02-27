using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.IO;

[System.Serializable]
public struct HookStatusData
{
    public string hookName;
    public bool isUnlocked;
}

[CreateAssetMenu(fileName = "HookStatusSO", menuName = "Fishing/HookStatusSO")]
public class HookStatusSO : ScriptableObject
{
    public HookStatusData[] defaultHookStatuses;
}

public class BaitAndHookManager : MonoBehaviour
{
    public static BaitAndHookManager Instance;

    public BaitSO activeBait;
    public HookSO activeHook;

    public Image baitPreview;
    public Image hookPreview;
    public GameObject UI_Holder;

    public int BaitClass;
    public int HookClass;

    public TMP_Text SelectedDescription;
    public TMP_Text BaitName;
    public TMP_Text HookName;

    public HookStatusSO defaultHookStatusSO;
    private string baitHookSaveFile;
    private string hookStatusSaveFile;
    private HookStatusData[] hookStatuses;

    private HookSO TempHook;
    private BaitSO TempBait;

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

    public void UpdatePreviewUI()
    {
        if (activeBait != null)
        {
            baitPreview.sprite = activeBait.icon;
            BaitName.text = activeBait.name;
        }

        if (activeHook != null)
        {
            hookPreview.sprite = activeHook.icon;
            HookName.text = activeHook.name;
        }
    }

    public void SetClass()
    {
        if (activeBait != null)
            BaitClass = activeBait.baitClass;

        if (activeHook != null)
            HookClass = activeHook.hookClass;
    }

    public void SetDescription(string description)
    {
        if (SelectedDescription != null)
            SelectedDescription.text = description;
    }

    public void ResetTMP()
    {
        if (SelectedDescription != null)
            SelectedDescription.text = "-";
    }

    public bool IsHookUnlocked(string HookName)
    {
        foreach (var hook in BaitAndHookManager.Instance.hookStatuses)
        {
            if (hook.hookName == HookName)
            {
                if (!hook.isUnlocked)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }
        Debug.LogError("Your Hook be missing");
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
        if (!File.Exists(baitHookSaveFile)) return;

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

        SetClass(); // Update class values when changing active bait and hook
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
