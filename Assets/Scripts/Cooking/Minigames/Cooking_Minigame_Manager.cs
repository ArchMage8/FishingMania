using System.Collections;
using UnityEngine;

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

    [Header("Minigame Complete Preview")]
    public GameObject Preview_Canvas;


    private Recipe activeRecipe;
    private string currentMethod;

    private int Dish_QTY;
    private Item ResultDish;

    private bool isInProgress = false;

    private void Awake()
    {
        // Singleton pattern
        if (Instance == null)
        {
            Instance = this;
            //DontDestroyOnLoad(gameObject); // Optional if persisting between scenes
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Called by CookingManager to begin the minigame process
    public void StartMinigameSequence(Recipe recipe, string method, int qty)
    {
        if (isInProgress) return;

        isInProgress = true;
        activeRecipe = recipe;
        currentMethod = method;

        Dish_QTY = qty;

        health = 3;

        // Start with chop phase
        Chop_Minigame.SetActive(true);
    }

    // Called by Chop Minigame when it's complete
    public void OnChopMinigameComplete()
    {
       
        Chop_Minigame.SetActive(false);
        StartCoroutine(BeginCookingPhase(1.5f));
    }

    // Handles transition after delay to cooking method phase
    private IEnumerator BeginCookingPhase(float delay)
    {
        yield return new WaitForSeconds(delay);

        

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
        health--;
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
        InventoryManager.Instance.AddItem(ResultDish, Dish_QTY);
        
    }

    private void HandleFailure() //Ran out of health
    {
        //Needs logic to show preview

        InventoryManager.Instance.AddItem(CookingManager.Instance.FailureDish, Dish_QTY);
        
    }

    private void ResetMinigameCycle()
    {
        health = 3;
        isInProgress = false;
    }

    //CloseButton Logic Required

    public void CloseCookingButton() //Button cannot call coroutine directly, so separate script is needed
    {
        StartCoroutine(CloseCookingMinigame());
    }

    private IEnumerator CloseCookingMinigame()
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

}
