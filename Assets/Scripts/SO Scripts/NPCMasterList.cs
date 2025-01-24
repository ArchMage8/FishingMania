using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NPCMasterList", menuName = "ScriptableObjects/NPCMasterList", order = 1)]
public class NPCMasterList : ScriptableObject
{
    public List<NPCData> npcList = new List<NPCData>();
}

[System.Serializable]
public class NPCData
{
    public string npcName;
    public int friendshipLevel;
    public bool hasBeenInteracted;
}
