using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class HookVisuals : MonoBehaviour
{
    [System.Serializable]
    public struct HookButton
    {
        public Button button;
        public HookSO hookData;
    }

    public HookButton[] hookButtons = new HookButton[5]; // Array of 5 hooks
    public TMP_Text hookNameText;
    public TMP_Text hookDescText;

    private void OnEnable()
    {
        UpdateHookButtons();
    }

    /// <summary>
    /// Updates the buttons with available hooks.
    /// </summary>
    private void UpdateHookButtons()
    {
        foreach (var hookButton in hookButtons)
        {
            if (hookButton.hookData == null) continue;

            if (!HookManager.Instance.IsHookUnlocked(hookButton.hookData.hookName))
            {
                DisableButton(hookButton.button);
                continue;
            }

            hookButton.button.image.sprite = hookButton.hookData.icon;
            hookButton.button.onClick.AddListener(() => DisplayHookInfo(hookButton.hookData));
            hookButton.button.interactable = true;
        }
    }

    /// <summary>
    /// Disables a button (can be expanded later).
    /// </summary>
    private void DisableButton(Button btn)
    {
        btn.interactable = false;
    }

    /// <summary>
    /// Displays selected hook's name & description.
    /// </summary>
    private void DisplayHookInfo(HookSO hook)
    {
        hookNameText.text = hook.hookName;
        hookDescText.text = hook.description;
    }
}
