using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NPCMasterList", menuName = "ScriptableObjects/NPCMasterList", order = 1)]
public class NPCMasterList : ScriptableObject
{
    public List<NPCData> npcList = new List<NPCData>();
}

