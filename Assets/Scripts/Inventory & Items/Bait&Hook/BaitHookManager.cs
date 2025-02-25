using UnityEngine;

public class BaitHookManager : MonoBehaviour
{
    public static BaitHookManager Instance { get; private set; }

    private BaitSO activeBait;
    private HookSO activeHook;

    public int BaitClass;
    public int HookClass;

    //Programmer Notes (Because i got confused by my own logic):
    //This Script only has 2 jobs
    
    //1. Ensure we have enough bait to fish
    //2. What is the current bait + hook combination
    
    //*Note there is no way a "Not Unlocked" hook has been selected
    //Handling UI of unavailable resources is to be handled by external scripts
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetActiveCombo(BaitSO bait, HookSO hook)
    {
        if (bait == null || hook == null)
        {
            Debug.LogWarning("Bait or Hook is missing! Cannot set active combo.");
            return;
        }

        HookClass = activeHook.Class;
        BaitClass = activeBait.Class;
    }

    public bool HasRequiredBait()
    {
        if (activeBait == null)
        {
            Debug.LogWarning("No active bait selected!");
            return false;
        }

        int totalBaitAvailable = InventoryManager.Instance.GetTotalQuantity(activeBait);
        return totalBaitAvailable > 0;
    }
}
