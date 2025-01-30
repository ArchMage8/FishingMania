using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class NPCDataManager : MonoBehaviour
{
    public static NPCDataManager Instance { get; private set; }

    private string saveFilePath;
    private List<NPCData> runtimeNPCData = new List<NPCData>();

    [SerializeField] private NPCMasterList npcMasterList; // Reference to the master list ScriptableObject

    private void Awake()
    {
        // Ensure Singleton pattern
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Persist this object across scenes
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        saveFilePath = Path.Combine(Application.persistentDataPath, "npc_data.json");
        //LoadNPCData();
    }

    public void LoadNPCData()
    {
        if (File.Exists(saveFilePath))
        {
            string json = File.ReadAllText(saveFilePath);
            runtimeNPCData = JsonUtility.FromJson<NPCDataListWrapper>(json).npcDataList;
            Debug.Log("NPC data loaded successfully.");
        }
        else
        {
            InitializeFromMasterList();
            SaveNPCData(); // Save immediately to establish the file for a new game
            Debug.Log("No save file found. Initialized from master list.");
        }
    }

    public void SaveNPCData()
    {
        NPCDataListWrapper wrapper = new NPCDataListWrapper { npcDataList = runtimeNPCData };
        string json = JsonUtility.ToJson(wrapper, true);

        File.WriteAllText(saveFilePath, json);
        Debug.Log($"NPC data saved to: {saveFilePath}");
    }

    private void InitializeFromMasterList()
    {
        runtimeNPCData.Clear();
        foreach (var npc in npcMasterList.npcList)
        {
            runtimeNPCData.Add(new NPCData
            {
                name = npc.name,
                friendshipLevel = npc.friendshipLevel,
                hasBeenInteracted = npc.hasBeenInteracted,
                isFull = npc.isFull
            });
        }
    }

    public List<NPCData> GetRuntimeNPCData()
    {
        return runtimeNPCData;
    }

    public NPCData GetNPCDataByName(string npcName)
    {
        return runtimeNPCData.Find(npc => npc.name == npcName);
    }

    private void ModifyFriendshipLevel(string npcName, int amount)
    {
        NPCData npc = GetNPCDataByName(npcName);
        if (npc != null)
        {
            npc.friendshipLevel += amount;
            Debug.Log($"{npcName}'s friendship level modified by {amount}. New level: {npc.friendshipLevel}");
        }
        else
        {
            Debug.LogWarning($"NPC with name {npcName} not found.");
        }
    }

    public void UpdateNPCData(string npcName, int newFriendshipLevel, bool newHasBeenInteracted, bool newIsFull)
    {
        NPCData npc = GetNPCDataByName(npcName);
        if (npc != null)
        {
            npc.friendshipLevel = newFriendshipLevel;
            npc.hasBeenInteracted = newHasBeenInteracted;
            npc.isFull = newIsFull;
        }
        else
        {
            Debug.LogWarning($"NPC {npcName} not found in runtime list.");
        }
    }

    public void DeleteSaveFile()
    {
        if (File.Exists(saveFilePath))
        {
            File.Delete(saveFilePath);
            Debug.Log("File Delete Completed");
        }

        else
        {
            Debug.Log("File doesn't exists");
        }
    }
}

[System.Serializable]
public class NPCData
{
    public string name;
    public int friendshipLevel;
    public bool hasBeenInteracted;
    public bool isFull;
}

[System.Serializable]
public class NPCDataListWrapper
{
    public List<NPCData> npcDataList;
}
