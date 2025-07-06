using UnityEngine;

public class NPCStateRefresher : MonoBehaviour
{
    public static NPCStateRefresher Instance { get; private set; }

    [SerializeField] private StateManager[] npcStateManagers;

    private NPCData tempData;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void RefreshAllNPCStates()
    {

        foreach (var manager in npcStateManagers)
        {
            if (manager != null)
            {
                NPCManager.Instance.ModifyIsFullState(manager.npcDataSO.npcName, false);
                manager.NewDayAdjust();
            }
        }
    }
}
