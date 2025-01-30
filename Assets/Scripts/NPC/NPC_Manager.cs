using UnityEngine;

public class NPC_Manager : MonoBehaviour
{
    [Header("NPC Child References")]
    public NPC_Child[] npcVersions; // Array of NPC_Child scripts representing friendship levels (0 to 4)

    [Header("NPC Data Variables")]
    public string npcName;          // NPC's name
    public int friendshipLevel;     // NPC's friendship level (0 to 4)
    public bool hasBeenInteracted;  // Whether the NPC has been interacted with
    public bool isFull;             // Whether the NPC is in the "full" state

    [Space(20)]

    public string prerequisiteNPC;

    public void CheckPrerequisite()
    {
        // If no prerequisite is set, this NPC remains active
        if (string.IsNullOrEmpty(prerequisiteNPC))
        {
            gameObject.SetActive(true);
            return;
        }

        // Get the prerequisite NPC's data from the NPCDataManager
        var prerequisiteData = NPCDataManager.Instance.GetNPCDataByName(prerequisiteNPC);

        // If prerequisite NPC doesn't exist in the data, disable this NPC
        if (prerequisiteData == null)
        {
            Debug.LogWarning($"Prerequisite NPC '{prerequisiteNPC}' not found in NPCDataManager.");
            gameObject.SetActive(false);
            return;
        }

        // If the prerequisite NPC hasn't been interacted with, disable this NPC
        if (!prerequisiteData.hasBeenInteracted)
        {
            gameObject.SetActive(false);
            return;
        }

        // If the prerequisite NPC has been interacted with and this NPC is not in "full" mode, enable it
        if (!isFull)
        {
            gameObject.SetActive(true);
        }
    }

        public void LoadNPCData()
    {
        var npcData = NPCDataManager.Instance.GetNPCDataByName(npcName);
        if (npcData != null)
        {
            friendshipLevel = npcData.friendshipLevel;
            hasBeenInteracted = npcData.hasBeenInteracted;
            isFull = npcData.isFull;
        }
        else
        {
            Debug.LogWarning($"NPC data for {npcName} not found in NPCDataManager.");
        }
    }

    public void SaveNPCData()
    {
        NPCDataManager.Instance.UpdateNPCData(npcName, friendshipLevel, hasBeenInteracted, isFull);
    }

  
    public void IncreaseFriendshipLevel()
    {
        LoadNPCData();
        
        if (friendshipLevel < npcVersions.Length - 1)
        {
            friendshipLevel++;
           
            AdjustVersion(); // Update NPC's active version
            SaveNPCData(); // Save updated friendship level
        }
    }

   
    public void AdjustVersion()
    {
        CheckPrerequisite();
        
        Debug.Log("Call Test");

        // Disable all versions initially
        foreach (var version in npcVersions)
        {
            version.DisableAll();
        }

        if (isFull)
        {
            // If "isFull" is true, use the full mode of the previous friendship level
            if (friendshipLevel > 0)
            {
                npcVersions[friendshipLevel - 1].EnableFull();
            }
        }
        else
        {   
            // If "isFull" is false, use the default mode of the current friendship level
            npcVersions[friendshipLevel].EnableDefault();
        }

        if(friendshipLevel >= 4)
        {
            hasBeenInteracted = true;
        }
    }

    private void OnDestroy()
    {
        // Save NPC data when the NPC is destroyed or the scene changes
        SaveNPCData();
    }
}
