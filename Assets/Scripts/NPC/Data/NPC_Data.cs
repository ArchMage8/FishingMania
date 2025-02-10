using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_Data : MonoBehaviour
{
    //Just a temp script to hold the 2 NPC Data assets
}

[CreateAssetMenu(fileName = "NewNPC", menuName = "NPC System/NPC Data")]
public class NPC : ScriptableObject
{
    public string npcName;
    public int friendshipLevel;
    public bool isFull;
}

[CreateAssetMenu(fileName = "NPCMasterList", menuName = "NPC System/NPC Master List")]
public class NPCMasterList : ScriptableObject
{
    public List<NPC> npcList;
}
