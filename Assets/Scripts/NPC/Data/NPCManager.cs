using UnityEngine;
using System.Collections.Generic;
using System.IO;

public class NPCManager : MonoBehaviour
{
    public static NPCManager Instance;
    private void Awake() { Instance = this; }

    public NPCMasterList npcMasterList;
    public List<NPCData> npcTempList = new List<NPCData>();
    private string savePath => Path.Combine(Application.persistentDataPath, "NPCData.json");

    public void NewGame()
    {
        npcTempList.Clear();
        foreach (var npc in npcMasterList.npcList)
        {
            npcTempList.Add(new NPCData(npc.npcName, npc.friendshipLevel, npc.isFull));
        }
        SaveNPCData();
    }

    public void LoadGame()
    {
        if (File.Exists(savePath))
        {
            string json = File.ReadAllText(savePath);
            npcTempList = JsonUtility.FromJson<NPCDataList>(json).npcList;
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

    public void ModifyFriendshipLevel(string npcName, int newFriendshipLevel)
    {
        foreach (var npc in npcTempList)
        {
            if (npc.npcName == npcName)
            {
                npc.friendshipLevel = newFriendshipLevel;
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
