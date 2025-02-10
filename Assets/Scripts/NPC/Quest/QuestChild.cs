using UnityEngine;

public class QuestChild : MonoBehaviour
{
    public QuestSO assignedQuest;
    public StateManager stateManager; //This is the manager local to each npc

    //1. Pass this NPC's quest info to the quest manager
    public void SendInfo()
    {
        if (!QuestManager.Instance.activeQuestPresent)
        {
            QuestManager.Instance.SetActiveQuest(assignedQuest);
        }
    }

    //2. Pass this NPC's quest requirements to the quest manager (when we are about to submit stuff for the quest)

    public void SumbitQuest()
    {
        bool completed = QuestManager.Instance.SubmitQuest(assignedQuest.desiredItem, assignedQuest.requiredQuantity);

        if (QuestManager.Instance.activeQuestID == assignedQuest.questID)
        {

            if (completed)
            {
                //Function to increase corresponding NPC's friendship level + update to is full state
                QuestCompleteUpdate();
                
                //Trigger Complete Quest Dialogue

            }

            else
            {
                //Logic if quest has not been completed
            }
        }
    }

    private void QuestCompleteUpdate()
    {
        NPCData npcData = NPCManager.Instance?.npcTempList.Find(npc => npc.npcName == assignedQuest.npcName);
        if (npcData != null)
        {
            npcData.friendshipLevel += 1;
            npcData.isFull = true;
        }
        else
        {
            Debug.LogError("This NPC's data is not found :v");
        }
    }

    public bool IsOurQuestActive()
    {
        if (QuestManager.Instance.activeQuestID == assignedQuest.questID)
        {
            return true;
        }

        else
        {
            return false;
        }
    }
    
    //*Note that handling of collisions + dialogue is handled by the dialogue trigger
    //*Dialogue trigger also handles the checking of "ActiveQuestPresent"
}