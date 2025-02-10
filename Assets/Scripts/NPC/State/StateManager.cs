using UnityEngine;

public class StateManager : MonoBehaviour
{
    public NPC npcDataSO;
    public GameObject[] npcObjects;
    public GameObject npcFull;

    private void Awake()
    {
        if (npcDataSO == null)
        {
            Debug.LogError("NPC SO reference is missing");
            return;
        }

        NPCData npcData = NPCManager.Instance?.npcTempList.Find(npc => npc.npcName == npcDataSO.npcName);

        if (npcData == null)
        {
            Debug.LogError($"NPC with name {npcDataSO.npcName} not found in list");
            return;
        }

        AdjustVersion(npcData);
    }

    private void AdjustVersion(NPCData npcData)
    {
        if (npcData.isFull)
        {
            npcFull.SetActive(true);
            foreach (var obj in npcObjects)
            {
                obj.SetActive(false);
            }
        }
        else
        {
            npcFull.SetActive(false);
            for (int i = 0; i < npcObjects.Length; i++)
            {
                npcObjects[i].SetActive(i == npcData.friendshipLevel);
            }
        }
    }
}
