using UnityEngine;
using System.Collections;

public class StateManager : MonoBehaviour
{
    public NPC npcDataSO;
    public GameObject[] npcObjects;
    public GameObject npcFull;

    private bool canCoroutine = true;

    private void Start()
    {
        if (npcDataSO == null)
        {
            Debug.LogError("NPC SO reference is missing");
            return;
        }

        // Register this NPC's StateManager to the refresher
        NPCStateRefresher.Instance?.Register(this);

        // Check that NPC exists in manager
        var exists = NPCManager.Instance?.npcTempList.Exists(npc => npc.npcName == npcDataSO.npcName);
        if (exists != true)
        {
            Debug.LogError($"NPC with name {npcDataSO.npcName} not found in NPCManager list");
            return;
        }

        TransitionBoolHandler();
    }

    private void Update()
    {
        if (NPCManager.Instance.GetIsFullState(npcDataSO.npcName) && canCoroutine)
        {
            StartCoroutine(transitionChange());
        }
    }

    private void OnEnable()
    {
        if (npcDataSO != null)
            StartCoroutine(DelayedAdjust());
    }

    private void OnDisable()
    {
        // Unregister from refresher when disabled
        NPCStateRefresher.Instance?.Unregister(this);
    }

    private IEnumerator DelayedAdjust()
    {
        yield return new WaitForSeconds(0f);
        AdjustVersion();
    }

    public void AdjustVersion()
    {
        if (npcDataSO == null) return;

        bool isFull = NPCManager.Instance.GetIsFullState(npcDataSO.npcName);
        int level = NPCManager.Instance.GetFriendshipLevel(npcDataSO.npcName);

        if (isFull)
        {
            npcFull.SetActive(true);
            foreach (var obj in npcObjects)
            {
                obj.SetActive(false);
            }
        }
        else
        {
            if (npcFull != null)
                npcFull.SetActive(false);

            for (int i = 0; i < npcObjects.Length; i++)
            {
                npcObjects[i].SetActive(i == level);
            }
        }
    }

    public void NewDayAdjust()
    {
        if (npcDataSO == null) return;

        if (npcFull.gameObject != null)
        {
            npcFull.SetActive(false);
        }

        NPCManager.Instance.ModifyIsFullState(npcDataSO.npcName, false);

        int level = NPCManager.Instance.GetFriendshipLevel(npcDataSO.npcName);

        for (int i = 0; i < npcObjects.Length; i++)
        {
            npcObjects[i].SetActive(i == level);
        }
    }

    private void TransitionBoolHandler()
    {
        bool isFull = NPCManager.Instance.GetIsFullState(npcDataSO.npcName);
        canCoroutine = !isFull;
    }

    private IEnumerator transitionChange()
    {
        canCoroutine = false;
        InventoryManager.Instance.SomeUIEnabled = true;
        yield return new WaitForSeconds(0.5f);

        npcFull.SetActive(true);
        InventoryManager.Instance.SomeUIEnabled = false;

        foreach (var obj in npcObjects)
        {
            obj.SetActive(false);
        }
    }
}
