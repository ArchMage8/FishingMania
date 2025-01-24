using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCTest : MonoBehaviour
{
    public NPCDataManager manager;
    public string TargetNPC;

   public void AddLevel()
    {
        manager.ModifyFriendshipLevel(TargetNPC, 1);
    }
}
