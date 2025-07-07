using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Cooking_Minigame_Manager : MonoBehaviour
{
    public static Cooking_Minigame_Manager Instance;

    [Header("Minigame State")]
    public int health = 3;

    [Space(10)]

    [Header("Minigame GameObjects")]
    public GameObject Chop_Minigame;
    public GameObject Fry_Minigame;
    public GameObject Boil_Minigame;
    public GameObject Bake_Minigame;

    [Space(10)]

    [Header("Health Visuals")]
     public Image[] heartImages; 
     public Sprite EmptyHeartSprite; 
     public Sprite FullHeartSprite;

    [Header("Preview System")]
    public Image Dish_Preview_Image;
    public TMPro.TextMeshProUGUI Method_Preview_Text;
    public GameObject FailDisplay;

    [HideInInspector] public Recipe activeRecipe;
    private string currentMethod;

    private int Dish_QTY;
    private Item ResultDish;

    private bool isInProgress = false;

    [HideInInspector] public bool isTranstitioning;

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

    private void OnEnable()
    {
        health = 3;
        isInProgress = false;
        
    }

    // Called by CookingManager to begin the minigame process
    public void StartMinigameSequence(Recipe recipe, string method, int qty)
    {
        if (isInProgress)
        {
           
            return; //Safeguard
        }

  

        isInProgress = true;
        activeRecipe = recipe;
        currentMethod = method;

       
        

        //Resulting dish is extracted from the activeRecipe
        //And will be set in the finalize minigame

        Dish_QTY = qty;
        
        //Fail Safe
        health = 3;
        UpdateHearts();

        // Start with chop phase
        StartCoroutine(StartChop());
        
    }

    private IEnumerator StartChop()
    {
        yield return new WaitForSeconds(0.4f);
        Chop_Minigame.SetActive(true);
    }

    // Called by Chop Minigame when it's complete
    public IEnumerator OnChopMinigameComplete()
    {
        yield return new WaitForSeconds(2f);
        
        Chop_Minigame.GetComponent<Animator>().SetTrigger("Exit");
        StartCoroutine(BeginCookingPhase());
    }


    // Handles transition after delay to cooking method phase
    private IEnumerator BeginCookingPhase()
    {
       
        yield return new WaitForSeconds(0.5f);
        Chop_Minigame.SetActive(false);

        switch (currentMethod)
        {
            case "Fry":
                Fry_Minigame.SetActive(true);
                break;

            case "Boil":
                Boil_Minigame.SetActive(true);
                break;

            case "Bake":
                Bake_Minigame.SetActive(true);
                break;

            default: //Failsafe
                Debug.LogWarning($"Unknown cooking method: {currentMethod}");
                break;
        }
    }

    // Called by the cooking method minigame (Fry, Boil, or Bake) when it's complete
    public void CookingMinigameComplete()
    {
        if (health > 0)
        {
            FinalizeMinigame();
        }
         
        else if (health == 0)
        {
            HandleFailure();
        }
    }

    
    // Call this from minigames to reduce health
    public void LoseHealth()
    {
        //Debug.Log("Health");
        
       
        health--;
        UpdateHearts();
    }

    // Determine reward and finalize minigame cycle
    private void FinalizeMinigame()
    {
        if (health == 3)
        {
            // Perfect
            ResultDish = activeRecipe.perfectDish;
            HandleSuccess();
        }
        else if (health > 0 && health < 3)
        {
            // Standard
            ResultDish = activeRecipe.resultDish;
            HandleSuccess();
        }

        //Failure Dish Logic is handeld by the Fail Function

        ResetMinigameCycle();
    }

    private void HandleSuccess()
    {
        //Need logic to show preview

        if (ResultDish == null)
        {
            Debug.LogError("ResultDish is NULL before AddItem");
        }
        else
        {
            Debug.Log($"Adding {Dish_QTY} x {ResultDish.itemName}");
        }


        ResetQTYUI();
        InventoryManager.Instance.AddItem(ResultDish, Dish_QTY);
        
    }

    private void ResetQTYUI()
    {
        CookingManager.Instance.selectedCookQuantity = 0;
        CookingManager.Instance.cookQuantityText.text = "0";
    }

    private void HandleFailure() //Ran out of health
    {
        //Needs logic to show preview

        Debug.Log(CookingManager.Instance.FailureDish.name + " of qty " + Dish_QTY);

        InventoryManager.Instance.AddItem(CookingManager.Instance.FailureDish, Dish_QTY);
        StartCoroutine(ShowFail());

        ResetQTYUI();
    }

    private IEnumerator ShowFail()
    {
        FailDisplay.SetActive(true);
        yield return new WaitForSecondsRealtime(1.5f); //Enable
        
        Animator animator = FailDisplay.GetComponent<Animator>();
        

        animator.SetTrigger("Exit");
        yield return new WaitForSecondsRealtime(0.5f); //Close

        FailDisplay.SetActive(false);

        ResetMinigameCycle();
    }

    public void SetPreview()
    {
        string method = activeRecipe.method.ToString();

        Dish_Preview_Image.sprite = activeRecipe.resultDish.icon;
        Method_Preview_Text.text = $"2. {method} the fish";
    }

    private void ResetMinigameCycle()
    {
        health = 3;
        isInProgress = false;

        StartCoroutine(Minigame_Exit());
    }

    private IEnumerator Minigame_Exit()
    {
        isTranstitioning = true;

        GameObject ActiveMinigame = GetActiveMinigame();

        yield return new WaitForSeconds(2f);

        //ActiveMinigame.GetComponent<Animator>().SetTrigger("Exit");
        yield return new WaitForSeconds(0.5f);

        ActiveMinigame.SetActive(false);
        CookingManager.Instance.Minigame_ContentHolder.GetComponent<Animator>().SetTrigger("Exit");
        yield return new WaitForSeconds(0.5f);

        CookingManager.Instance.Manager_ContentHolder.SetActive(true);
        CookingManager.Instance.Minigame_ContentHolder.SetActive(false);

        isTranstitioning = false;
    }


    //CloseButton Logic Required

    private IEnumerator CloseCookingMinigame() //This should be internally called
                                               //Player cannot close on-going minigame
    {
        GameObject ActiveMinigame = GetActiveMinigame();
        Animator MinigameAnimator = ActiveMinigame.GetComponent<Animator>();
        yield return new WaitForSecondsRealtime(0.5f);
        ResetMinigameCycle();

        //Incomplete
    }

    public GameObject GetActiveMinigame()
    {
        if (Chop_Minigame.activeSelf)
        {
            return Chop_Minigame;
        }

        if (Fry_Minigame.activeSelf)
        {
            return Fry_Minigame;
        }

        if (Boil_Minigame.activeSelf)
        {
            return Boil_Minigame;
        }

        if (Bake_Minigame.activeSelf)
        {
            return Bake_Minigame;
        }

        return null; // No minigame is currently active
    }

    public void UpdateHearts()
    {
        //Debug.Log("Beans");

        for (int i = 0; i < heartImages.Length; i++)
        {
            // Determine if this heart should be full or empty
            if (i < health)
            {
                heartImages[i].sprite = FullHeartSprite;
            }
            else
            {
                heartImages[i].sprite = EmptyHeartSprite;
            }
        }
    }

}
