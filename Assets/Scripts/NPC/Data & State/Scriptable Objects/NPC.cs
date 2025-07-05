using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewNPC", menuName = "NPC System/NPC Data")]
public class NPC : ScriptableObject
{
    public string npcName;
    public int friendshipLevel;
    public bool isFull;
}
