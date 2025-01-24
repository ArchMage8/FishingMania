using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class NPCDataManager : MonoBehaviour
{
    public NPCMasterList masterList; // Reference to the default NPC data ScriptableObject
    private List<NPCData> runtimeNPCList = new List<NPCData>();

    private string saveFilePath;
    public static NPCDataManager Instance;

    private void Awake()
    {
        saveFilePath = Path.Combine(Application.persistentDataPath, "npcData.json");

        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        else
        {
            Destroy(gameObject);
        }

        if (File.Exists(saveFilePath))
        {
           LoadNPCData();
        }
        else
        {
            InitializeFromMasterList();
        }
    }

    public void InitializeFromMasterList()
    {
        // Populate runtime list from master list
        runtimeNPCList.Clear();
        foreach (var npc in masterList.npcList)
        {
            runtimeNPCList.Add(new NPCData
            {
                npcName = npc.npcName,
                friendshipLevel = npc.friendshipLevel,
                hasBeenInteracted = npc.hasBeenInteracted
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

    public void SetInteractionStatus(string npcName, bool status)
    {
        foreach (var npc in runtimeNPCList)
        {
            if (npc.npcName == npcName)
            {
                npc.hasBeenInteracted = status;
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
                hasBeenInteracted = npc.hasBeenInteracted
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

    public bool GetInteractionStatus(string npcName)
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

    public NPCData GetNPCData(string npcName)
    {
        foreach (var npcData in runtimeNPCList)
        {
            if (npcData.npcName == npcName)
            {
                return npcData;
            }
        }
        Debug.LogError($"NPC data not found for {npcName}.");
        return null;
    }
}

[System.Serializable]
public class NPCSaveData
{
    public List<NPCData> npcList = new List<NPCData>();
}
