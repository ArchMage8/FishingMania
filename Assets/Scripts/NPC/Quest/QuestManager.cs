using UnityEngine;
using System.IO;
using System.Collections.Generic;

public class QuestManager : MonoBehaviour
{
    public static QuestManager Instance;
    private void Awake() 
    {
        Instance = this;
        LoadActiveQuest();
    }

    public int activeQuestID;

    public string NameOfQuestGiver;
    private string activeQuestDescription;
    [HideInInspector] public string activeQuestNPC;
    [HideInInspector] public bool activeQuestPresent;

    private string savePath => Path.Combine(Application.persistentDataPath, "ActiveQuest.json");


    public void SetActiveQuest(QuestSO quest)
    {
        activeQuestID = quest.questID;
        activeQuestDescription = quest.questDescription;
        activeQuestNPC = quest.npcName;
        activeQuestPresent = true;

        NameOfQuestGiver = quest.npcName;
    }



    public bool SubmitQuest(Item item, int quantity)
    {

        if (InventoryManager.Instance.RemoveForQuest(item, quantity))
        {
            ResetActiveQuest();
            return true;
        }
        else
        {
            return false;
        }
    }

    public void ResetActiveQuest()
    {
        activeQuestID = -1;
        activeQuestPresent = false;
        activeQuestNPC = null;
        activeQuestDescription = "No active quests at the moment";

    }
    public int GetActiveQuestID()
    {
        return activeQuestPresent ? activeQuestID : -1;
    }

    public void SaveActiveQuest()
    {
        ActiveQuestData data = new ActiveQuestData(activeQuestID, activeQuestDescription, activeQuestNPC, activeQuestPresent);
        File.WriteAllText(savePath, JsonUtility.ToJson(data));
        Debug.Log("Active quest data saved to: " + savePath);
    }

    public void LoadActiveQuest()
    {
        if (File.Exists(savePath))
        {
            ActiveQuestData data = JsonUtility.FromJson<ActiveQuestData>(File.ReadAllText(savePath));
            activeQuestID = data.questID;
            activeQuestDescription = data.questDescription;
            activeQuestNPC = data.npcName;
            activeQuestPresent = data.isActive;
        }
    }
}

[System.Serializable]
public class ActiveQuestData
{
    public int questID;
    public string questDescription;
    public string npcName;
    public bool isActive;

    public ActiveQuestData(int id, string description, string npc, bool active)
    {
        questID = id;
        questDescription = description;
        npcName = npc;
        isActive = active;
    }

    public class ItemRemovalRequest
    {
        public Item item;
        public int quantity;
    }
}
