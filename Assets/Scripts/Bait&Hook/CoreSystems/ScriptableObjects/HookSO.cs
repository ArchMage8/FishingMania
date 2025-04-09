using UnityEngine;

[CreateAssetMenu(fileName = "NewHook", menuName = "Fishing/Hook", order = 1)]
public class HookSO : ScriptableObject
{
    public string hookName;
    public string description;
    public Sprite icon;
    public int hookClass;
    public bool isUnlocked;
}
