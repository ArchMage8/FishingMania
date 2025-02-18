using UnityEngine;
using System.Collections;

public class StateManager : MonoBehaviour
{
    public NPC npcDataSO;
    public GameObject[] npcObjects;
    public GameObject npcFull;

    private bool canCoroutine = true;
    private NPCData tempData;

    private void Awake()
    {
        if (npcDataSO == null)
        {
            Debug.LogError("NPC SO reference is missing");
            return;
        }

        NPCData npcData = NPCManager.Instance?.npcTempList.Find(npc => npc.npcName == npcDataSO.npcName);

        tempData = npcData;

        if (npcData == null)
        {
            Debug.LogError($"NPC with name {npcDataSO.npcName} not found in list");
            return;
        }

        AdjustVersion(npcData);
        TransitionHandler(npcData);
    }

    private void Update()
    {
        if(tempData.isFull && canCoroutine)
        {
            StartCoroutine(transitionChange());
        }
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

    private void TransitionHandler(NPCData npcData)
    {
        if (npcData.isFull)
        {
            canCoroutine = false;
        }
        else
        {
            canCoroutine = true;
        }
    }

    private IEnumerator transitionChange()
    {
        canCoroutine = false;
        
        yield return new WaitForSeconds(1.5f);
        
        npcFull.SetActive(true);
        foreach (var obj in npcObjects)
        {
            obj.SetActive(false);
        }
    }
}
