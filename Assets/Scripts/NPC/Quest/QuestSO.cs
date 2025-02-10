using UnityEngine;

[CreateAssetMenu(fileName = "NewQuest", menuName = "Quests/Quest")]
public class QuestSO : ScriptableObject
{
    [Header("Quest Info")]
    public string npcName; // Unique identifier for the NPC
    public int questID;
    public string questDescription;

    [Header("Quest Requirements")]
    public Item desiredItem; // Reference to the item ScriptableObject
    public int requiredQuantity;

    [Header("Quest Rewards")]
    public int moneyReward;
}
