using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HookButton : MonoBehaviour
{
    public HookSO HookReference;
    public Image ContentIcon;
    private Button button;

    private void Awake()
    {
        button = GetComponent<Button>();
    }

    private void OnEnable()
    {
        CheckStatus();
    }

    public void CheckStatus()
    {
        ContentIcon.sprite = HookReference.icon;

        if (!BaitAndHookManager.Instance.IsHookUnlocked(HookReference.name))
        {
            HandleLockedHook();
        }
    }

    private void HandleLockedHook()
    {
        button.interactable = false; // Disable the button
    }

    public void TransferToPreview()
    {
        BaitAndHookManager manager = BaitAndHookManager.Instance;

        if (manager != null)
        {
            manager.hookPreview.sprite = HookReference.icon;
            manager.HookName.text = HookReference.name;
            manager.SelectedDescription.text = HookReference.description;

            manager.SetTempHook(HookReference); // Assign HookReference to TempHook
        }
    }
}
