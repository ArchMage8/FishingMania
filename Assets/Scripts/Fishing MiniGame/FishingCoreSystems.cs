using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class FishingCoreSystems : MonoBehaviour
{
    //A new script called FishingSpotManager has been introduced 09/04/25 due to a system oversight
    //Open the script mentioned for full details


    //The new system now works like this:
    //The fishcoresystem and fishgenerator have become local scripts that are unique to the fishing spot scene
    //The script kept persistent is the FishingSpotManager

    //The fishing spot manager is the one that holds the stocks of all the fishing spots


    public static FishingCoreSystems instance;

    [Header("UI Elements")]
    public GameObject MinigameIndicator; //Reference to the visual representation to what button to press to start minigame
    public GameObject CastMinigame;

    [Space(10)]

    public GameObject BiteIndicator;
    public GameObject InventoryError;
    public GameObject BaitError;
    public Slider ReelSlider;

    [Header("Fishing Spot Data")]
    public Animator FishingPlayer;
    public FishGenerator fishGenerator;
    public ColorManager colorManager;

    private bool error = false;
   [HideInInspector] public int reelQTY;
   [HideInInspector] public int increaseRate = 10;
   [HideInInspector] public int decreaseRate = 5;
    private float decreaseInterval = 1f;
    private bool isReeling;

    private GameObject ActiveError;
    private Animator ErrorAnimator;

    private void Awake()
    {
        if (instance == null) 
        {
            instance = this;
        }

        else
        {
            Destroy(gameObject);
            return;
        }
    }


    private void Start()
    {

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

        int BaitQTY = InventoryManager.Instance.GetTotalQuantity(Inventory_EquipmentManager.Instance.ActiveBaitType);
        reelQTY = 50;

        if (!InventoryManager.Instance.IsInventoryFull() && BaitQTY > 0)
        {
            MinigameIndicator.SetActive(false);
            StartCoroutine(StartMiniGame());
        }
        else
        {
            error = true;
            if (InventoryManager.Instance.IsInventoryFull())
            {
                Debug.LogError("Ur Inventory Be full");
                InventoryError.SetActive(true);
                ActiveError = InventoryError;
                StartCoroutine(RemoveErrorBoard());
            }
            else if (BaitQTY == 0)
            {
                Debug.LogError("Ur bait has been exhausted");
                BaitError.SetActive(true);
                ActiveError = BaitError;
                StartCoroutine(RemoveErrorBoard());
            }
        }
    }

    private IEnumerator RemoveErrorBoard()
    {
        ErrorAnimator = ActiveError.GetComponent<Animator>();
        yield return new WaitForSecondsRealtime(1.5f);

        ErrorAnimator.SetTrigger("Exit");
        yield return new WaitForSecondsRealtime(0.5f);

        ActiveError.SetActive(false);
        ActiveError = null;
  
        
    }

    private IEnumerator StartMiniGame()
    {

        Debug.Log("Call 2");
        FishingPlayer.SetTrigger("Next");

        InventoryManager.Instance.SomeUIEnabled = true;

        yield return new WaitForSeconds(0.5f);

        CastMinigame.SetActive(true);
        colorManager.PrepareColors();
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
            3 => 40,
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
            Debug.Log("Enter Reel in phase 1");

            if (Input.GetKeyDown(KeyCode.F))
            {
                ReelSlider.gameObject.SetActive(true);
                Debug.Log("Enter Reel in phase 2");
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
        InventoryManager.Instance.SomeUIEnabled = false;
        StartCoroutine(WaitForInput());
    }

    private void DeductBait()
    {
        Item activeBait = Inventory_EquipmentManager.Instance.activeBaitTile.BaitItem;

        List<ItemRemovalRequest> removalRequests = new List<ItemRemovalRequest>();
        removalRequests.Add(new ItemRemovalRequest { item = activeBait, quantity = 1 });

        InventoryManager.Instance.RemoveItems(removalRequests);
    }

    public void DeductStock(string spotName)
    {
        FishingSpotManager.instance.CheckFishStock(spotName);
    }
}
