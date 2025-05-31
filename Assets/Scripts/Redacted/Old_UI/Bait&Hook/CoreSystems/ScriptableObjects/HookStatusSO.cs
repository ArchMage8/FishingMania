using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
