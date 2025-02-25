using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "HookMasterList", menuName = "Fishing/Hook Master List", order = 2)]
public class HookMasterListSO : ScriptableObject
{
    public List<HookSO> hooks; // Master list of all hook types
}
