using UnityEngine;

[CreateAssetMenu(fileName = "DefaultCombo", menuName = "Fishing/Default Combo", order = 3)]
public class DefaultCombo : ScriptableObject
{
    public BaitSO defaultBait;
    public HookSO defaultHook;
}
