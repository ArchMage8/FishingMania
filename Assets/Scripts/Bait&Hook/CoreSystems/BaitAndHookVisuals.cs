using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BaitAndHookVisuals : MonoBehaviour
{
    public static BaitAndHookVisuals Instance;

    [Header("UI Elements")]
    public Image baitPreview;
    public Image hookPreview;

    [Space(15)]

    public TMP_Text SelectedDescription;

    [Space(10)]
    public TMP_Text BaitName;
    public TMP_Text HookName;

    private BaitAndHookManager manager;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        manager = BaitAndHookManager.Instance;
        UpdateVisuals(); // Optional: update at start
    }

    public void UpdateVisuals()
    {
        if (manager.activeBait != null)
        {
            baitPreview.sprite = manager.activeBait.icon;
            BaitName.text = manager.activeBait.name;
        }

        if (manager.activeHook != null)
        {
            hookPreview.sprite = manager.activeHook.icon;
            HookName.text = manager.activeHook.name;
        }
    }

    public void SetDescription(string description)
    {
        if (SelectedDescription != null)
            SelectedDescription.text = description;
    }

    public void ResetDescription()
    {
        if (SelectedDescription != null)
            SelectedDescription.text = "-";
    }
}
