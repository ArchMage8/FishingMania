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
    public Animator FishingPlayer;

    public FishGenerator fishGenerator;

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

        CastMinigame.SetActive(false);
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
        Debug.Log("Call 1");

        if (!InventoryManager.Instance.IsInventoryFull() && InventoryManager.Instance.GetTotalQuantity(BaitAndHookManager.Instance.activeBait) > 0)
        {
          
            MinigameIndicator.SetActive(false);
            StartCoroutine(StartMiniGame());
            
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

    private IEnumerator StartMiniGame()
    {
        Debug.Log("Call 2");
        FishingPlayer.SetTrigger("Next"); //Player Moves to cast the line

        yield return new WaitForSeconds(0.5f);
        
        CastMinigame.SetActive(true);
        CastMinigame.GetComponentInChildren<Orbit>().isOrbiting = true;


    }

    public void StartWaitPhase(int score) //Called by the Arrow (Orbit.cs)
    {
        FishingPlayer.SetTrigger("Next"); //Casts the line
        Debug.Log("Call 3");

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
        FishingPlayer.SetTrigger("Next"); 
        Debug.Log("Call 4");

        yield return new WaitForSeconds(1f);

        FishingPlayer.SetTrigger("Next"); //Enter Wait Animation

        yield return new WaitForSeconds(waitTime);
        
        StartBitePhase();
    }

    private void StartBitePhase()
    {
        Debug.Log("Call 5");
        StartCoroutine(BiteWindow());
    }

    private IEnumerator BiteWindow()
    {
        FishingPlayer.SetTrigger("Next"); //Enters bite animation
        yield return new WaitForSeconds(0.5f);
        BiteIndicator.SetActive(true);

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

        MinigameIndicator.SetActive(false);
        FailCatch();
    }

    private void CatchFish()
    {
        Debug.Log("Call 6a");

        FishingPlayer.SetTrigger("Success"); //Success Animation
        fishGenerator.SelectFish();
        StartCoroutine(ReturnToIdle());
    }

    private void FailCatch()
    {
        Debug.Log("Call 6b");

        FishingPlayer.SetTrigger("Fail"); //Fail Animation

        StartCoroutine(ReturnToIdle());
    }

    private IEnumerator ReturnToIdle()
    {

        yield return new WaitForSeconds(3f);
        FishingPlayer.SetTrigger("Next");
        DeductBait();
        GoToIdle();
    }

    private void GoToIdle()
    {
        Debug.Log("Call 7");

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
