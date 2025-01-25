using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class NPCDataManager : MonoBehaviour
{
    public NPCMasterList masterList; // Reference to the default NPC data ScriptableObject
    private List<NPCData> runtimeNPCList = new List<NPCData>();

    private string saveFilePath;

    private void Awake()
    {
        saveFilePath = Path.Combine(Application.persistentDataPath, "npcData.json");

        if (File.Exists(saveFilePath))
        {
            LoadNPCData();
        }
        else
        {
            InitializeFromMasterList();
        }
    }

    private void InitializeFromMasterList()
    {
        runtimeNPCList.Clear();
        foreach (var npc in masterList.npcList)
        {
            runtimeNPCList.Add(new NPCData
            {
                npcName = npc.npcName,
                friendshipLevel = npc.friendshipLevel,
                hasBeenInteracted = npc.hasBeenInteracted,
                isFull = npc.isFull
            });
        }
    }

    public void ModifyFriendshipLevel(string npcName, int amount)
    {
        foreach (var npc in runtimeNPCList)
        {
            if (npc.npcName == npcName)
            {
                npc.friendshipLevel += amount;
                return;
            }
        }
        Debug.LogWarning($"NPC with name {npcName} not found.");
    }

    public void SetHasBeenInteracted(string npcName, bool value)
    {
        foreach (var npc in runtimeNPCList)
        {
            if (npc.npcName == npcName)
            {
                npc.hasBeenInteracted = value;
                return;
            }
        }
        Debug.LogWarning($"NPC with name {npcName} not found.");
    }

    public void SetIsFull(string npcName, bool value)
    {
        foreach (var npc in runtimeNPCList)
        {
            if (npc.npcName == npcName)
            {
                npc.isFull = value;
                return;
            }
        }
        Debug.LogWarning($"NPC with name {npcName} not found.");
    }

    public void SaveGame()
    {
        string json = JsonUtility.ToJson(new NPCSaveData { npcList = runtimeNPCList }, true);
        File.WriteAllText(saveFilePath, json);
        Debug.Log($"NPC data saved. File path: {saveFilePath}");
    }

    private void LoadNPCData()
    {
        string json = File.ReadAllText(saveFilePath);
        NPCSaveData loadedData = JsonUtility.FromJson<NPCSaveData>(json);

        runtimeNPCList.Clear();
        foreach (var npc in loadedData.npcList)
        {
            runtimeNPCList.Add(new NPCData
            {
                npcName = npc.npcName,
                friendshipLevel = npc.friendshipLevel,
                hasBeenInteracted = npc.hasBeenInteracted,
                isFull = npc.isFull
            });
        }

        Debug.Log("NPC data loaded.");
    }

    public int GetFriendshipLevel(string npcName)
    {
        foreach (var npc in runtimeNPCList)
        {
            if (npc.npcName == npcName)
            {
                return npc.friendshipLevel;
            }
        }
        Debug.LogWarning($"NPC with name {npcName} not found.");
        return -1;
    }

    public bool GetHasBeenInteracted(string npcName)
    {
        foreach (var npc in runtimeNPCList)
        {
            if (npc.npcName == npcName)
            {
                return npc.hasBeenInteracted;
            }
        }
        Debug.LogWarning($"NPC with name {npcName} not found.");
        return false;
    }

    public bool GetIsFull(string npcName)
    {
        foreach (var npc in runtimeNPCList)
        {
            if (npc.npcName == npcName)
            {
                return npc.isFull;
            }
        }
        Debug.LogWarning($"NPC with name {npcName} not found.");
        return false;
    }
}

[System.Serializable]
public class NPCSaveData
{
    public List<NPCData> npcList = new List<NPCData>();
}

[System.Serializable]
public class NPCData
{
    public string npcName;
    public int friendshipLevel;
    public bool hasBeenInteracted;
    public bool isFull;
}
