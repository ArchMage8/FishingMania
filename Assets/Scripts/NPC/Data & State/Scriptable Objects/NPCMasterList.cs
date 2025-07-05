using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NPCMasterList", menuName = "NPC System/NPC Master List")]
public class NPCMasterList : ScriptableObject
{
    public List<NPC> npcList;
}