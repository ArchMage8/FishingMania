using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class FishingCoreSystems : MonoBehaviour
{
    public static FishingCoreSystems instance;

    [Header("UI Elements")]
    public GameObject MinigameIndicator; //Reference to the visual representation to what button to press to start minigame
    public GameObject CastMinigame;
    public GameObject BiteIndicator;
    public GameObject InventoryError;
    public GameObject BaitError;
    public Slider ReelSlider;

    [Header("Fishing Spot Data")]
    public FishingSpotStock[] fishingSpots;
    public Animator FishingPlayer;

    public FishGenerator fishGenerator;

    private bool error = false;
    public int reelQTY;
    public int increaseRate = 10;
    public int decreaseRate = 5;
    private float decreaseInterval = 1f;
    private bool isReeling;

    [System.Serializable]
    public struct FishingSpotStock
    {
        public string SpotName;
        public int FishStock;
    }

    private void Start()
    {
        if (instance == null)
            instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }

        MinigameIndicator.SetActive(false);

        CastMinigame.SetActive(false);
        StartCoroutine(WaitForInput());

        ReelSlider.value = reelQTY;

        ReelSlider.gameObject.SetActive(false);
    }

    private void Update()
    {
        if(isReeling == true)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                reelQTY += increaseRate;
                ReelSlider.value = reelQTY;
            }
        }
    }

    private IEnumerator WaitForInput()
    {
        yield return new WaitForSeconds(1f);

        while (InventoryManager.Instance.SomeUIEnabled)
        {
            yield return null;
        }

        yield return new WaitForSeconds(1f);


        MinigameIndicator.SetActive(true);
        while (!Input.GetKeyDown(KeyCode.F))
        {
            yield return null;
        }

        PreCheck();

        reelQTY = 50;
    }

    private void PreCheck()
    {
        Debug.Log("Call 1");

        reelQTY = 50;

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
        FishingPlayer.SetTrigger("Next");

        yield return new WaitForSeconds(0.5f);

        CastMinigame.SetActive(true);
        CastMinigame.GetComponentInChildren<Orbit>().isOrbiting = true;
    }

    public void StartWaitPhase(int score)
    {
        FishingPlayer.SetTrigger("Next");
        Debug.Log("Call 3");

        int waitTime = score switch
        {
            1 => 10,
            2 => 20,
            3 => 50,
            _ => 30
        };

        StartCoroutine(WaitPhase(waitTime));
    }

    private IEnumerator WaitPhase(int waitTime)
    {
        FishingPlayer.SetTrigger("Next");
        Debug.Log("Call 4");

        yield return new WaitForSeconds(1f);
        FishingPlayer.SetTrigger("Next");
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
        FishingPlayer.SetTrigger("Next");
        yield return new WaitForSeconds(0.5f);
        BiteIndicator.SetActive(true);

        float biteTime = 2f;
        float elapsedTime = 0f;

        while (elapsedTime < biteTime)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                StartCoroutine(ReelingPhase());
                yield break;
            }
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        MinigameIndicator.SetActive(false);
        FailCatch();
    }

    private IEnumerator ReelingPhase()
    {
        Debug.Log("Reeling Phase Started");
        reelQTY = 50;
        ReelSlider.gameObject.SetActive(true);
        ReelSlider.value = reelQTY;
        ReelSlider.maxValue = 100;

        while (reelQTY > 0 && reelQTY < 100)
        {
            isReeling = true;

            yield return new WaitForSeconds(decreaseInterval);
            reelQTY -= decreaseRate;
            reelQTY = Mathf.Clamp(reelQTY, 0, 100);
            ReelSlider.value = reelQTY;

            if (reelQTY == 0)
            {
                isReeling = false;
                FailCatch();
                ReelSlider.gameObject.SetActive(false);
                yield break;
            }
            else if (reelQTY == 100)
            {
                isReeling = false;
                CatchFish();
                ReelSlider.gameObject.SetActive(false);
                yield break;
            }
        }
    }

    private void CatchFish()
    {
        Debug.Log("Call 6a");
        MinigameIndicator.SetActive(false);
        FishingPlayer.SetTrigger("Success");
        fishGenerator.SelectFish();
        StartCoroutine(ReturnToIdle());
    }

    private void FailCatch()
    {
        Debug.Log("Call 6b");
        MinigameIndicator.SetActive(false);
        FishingPlayer.SetTrigger("Fail");
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
