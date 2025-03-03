using System.Collections;
using UnityEngine;
using System.Collections.Generic;


public class FishingCoreSystems : MonoBehaviour
{
    public static FishingCoreSystems instance;

    [Header("UI Elements")]
    public GameObject MinigameIndicator;
    public GameObject CastMinigame;
    public GameObject BiteIndicator;
    public GameObject InventoryError;
    public GameObject BaitError;

    [Header("Fishing Spot Data")]
    public FishingSpotStock[] fishingSpots;

    private bool error = false;

    [System.Serializable]
    public struct FishingSpotStock
    {
        public string SpotName;
        public int FishStock;
    }

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }

        MinigameIndicator.SetActive(true);
        StartCoroutine(WaitForInput());
    }

    private IEnumerator WaitForInput()
    {
        yield return new WaitForSeconds(2f); // Adjust time as needed

        while (!Input.GetKeyDown(KeyCode.F))
        {
            yield return null;
        }

        PreCheck();
    }

    private void PreCheck()
    {
        if (!InventoryManager.Instance.IsInventoryFull() && InventoryManager.Instance.GetTotalQuantity(BaitAndHookManager.Instance.activeBait) > 0)
        {
            StartMiniGame();
        }
        else
        {
            error = true;
            if (InventoryManager.Instance.IsInventoryFull())
                InventoryError.SetActive(true);
            else
                BaitError.SetActive(true);
        }
    }

    private void StartMiniGame()
    {
        CastMinigame.SetActive(true);
    }

    public void StartWaitPhase(int score)
    {
        int waitTime = score switch
        {
            1 => 30,
            2 => 120,
            3 => 180,
            _ => 30
        };

        StartCoroutine(WaitPhase(waitTime));
    }

    private IEnumerator WaitPhase(int waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        StartBitePhase();
    }

    private void StartBitePhase()
    {
        BiteIndicator.SetActive(true);
        StartCoroutine(BiteWindow());
    }

    private IEnumerator BiteWindow()
    {
        float biteTime = 2f; // Adjust as needed
        float elapsedTime = 0f;

        while (elapsedTime < biteTime)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                CatchFish();
                yield break;
            }
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        FailCatch();
    }

    private void CatchFish()
    {
        DeductBait();
        GoToIdle();
    }

    private void FailCatch()
    {
        DeductBait();
        GoToIdle();
    }

    private void GoToIdle()
    {
        MinigameIndicator.SetActive(true);
        StartCoroutine(WaitForInput());
    }

    private void DeductBait()
    {
        Item activeBait = BaitAndHookManager.Instance.activeBait;

        List<ItemRemovalRequest> removalRequests = new List<ItemRemovalRequest>();
        removalRequests.Add(new ItemRemovalRequest { item = activeBait, quantity = 1 });

        InventoryManager.Instance.RemoveItems(removalRequests);
    }

    public void DeductStock(string spotName)
    {
        for (int i = 0; i < fishingSpots.Length; i++)
        {
            if (fishingSpots[i].SpotName == spotName)
            {
                fishingSpots[i].FishStock = Mathf.Max(0, fishingSpots[i].FishStock - 1);
                return;
            }
        }
    }

    public void ResetStock()
    {
        for (int i = 0; i < fishingSpots.Length; i++)
        {
            fishingSpots[i].FishStock = 10;
        }
    }
}
