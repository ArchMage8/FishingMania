using UnityEngine;
using System.Collections.Generic;
using System.IO;

public class NPCManager : MonoBehaviour
{
    public static NPCManager Instance;


    public NPCMasterList npcMasterList;
    public List<NPCData> npcTempList = new List<NPCData>();
    private string savePath => Path.Combine(Application.persistentDataPath, "NPCData.json");

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            LoadGame(); 
        }
        else
        {
            Destroy(gameObject); // Prevent duplicate
        }
    }

    public void NewGame()
    {
        npcTempList.Clear();
        foreach (var npc in npcMasterList.npcList)
        {
            npcTempList.Add(new NPCData(npc.npcName, npc.friendshipLevel, npc.isFull));
        }
        //SaveNPCData();
    }

    public void LoadGame()
    {
        if (File.Exists(savePath))
        {
            string json = File.ReadAllText(savePath);
            npcTempList = JsonUtility.FromJson<NPCDataList>(json).npcList;
        }

        else
        {
            foreach (var npc in npcMasterList.npcList)
            {
                npcTempList.Add(new NPCData(npc.npcName, npc.friendshipLevel, npc.isFull));
            }
        }
    }

    public void SaveNPCData()
    {
        NPCDataList data = new NPCDataList(npcTempList);
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(savePath, json);
        ResetNPCs();
    }

    public void ResetNPCs()
    {
        foreach (var npc in npcTempList)
        {
            npc.isFull = false;
        }
    }

    public void AddFriendshipLevel(string npcName)
    {
        Debug.Log("Bananas");
        
        foreach (var npc in npcTempList)
        {
            if (npc.npcName == npcName)
            {
                npc.friendshipLevel++;
                break;
            }
        }
    }

    public void ModifyIsFullState(string npcName, bool isFull)
    {
     

        foreach (var npc in npcTempList)
        {
            if (npc.npcName == npcName)
            {
                npc.isFull = isFull;
                break;
            }
        }
    }

    public int GetFriendshipLevel(string npcName)
    {
        foreach (var npc in npcTempList)
        {
            if (npc.npcName == npcName)
                return npc.friendshipLevel;
        }
        return -1; // Return -1 if NPC not found
    }

    public bool GetIsFullState(string npcName)
    {
        foreach (var npc in npcTempList)
        {
            if (npc.npcName == npcName)
                return npc.isFull;
        }
        return false; // Return false if NPC not found
    }

}

[System.Serializable]
public class NPCData
{
    public string npcName;
    public int friendshipLevel;
    public bool isFull;

    public NPCData(string name, int friendship, bool full)
    {
        npcName = name;
        friendshipLevel = friendship;
        isFull = full;
    }
}

[System.Serializable]
public class NPCDataList
{
    public List<NPCData> npcList;
    public NPCDataList(List<NPCData> list) { npcList = list; }
}
