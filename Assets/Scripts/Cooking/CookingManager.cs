using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class CookingManager : MonoBehaviour
{
    public static CookingManager Instance;

    [Space(20)]

    private InventoryManager inventoryManager; // Reference to inventory system

    [Header("Preview Components")]

    [SerializeField] private Image dishIcon; // UI display for dish icon
    [SerializeField] private TMP_Text cookQuantityText; // Display for selected cook quantity
    [SerializeField] private TMP_Text dishName; // Display for dish name
    [SerializeField] private TMP_Text dishDescription;

    [Space(10)]
    [Header("System Requirements")]

    public GameObject Manager_ContentHolder;
    public GameObject Minigame_ContentHolder;
    public Item FailureDish;
    public GameObject BG_Effect;


    private GameObject GameHUD;

    [Space(20)]

    [SerializeField] private IngredientDisplay[] ingredientDisplays; // UI elements for ingredients

    [HideInInspector] public Recipe currentRecipe;
    [HideInInspector] public int maxCookQuantity = 1;
    private int selectedCookQuantity = 1;
    private string Current_Cooking_Method = null;
    private bool RecipeUnlocked;


    private void Awake()
    {
       
        DisableIngredients();
        inventoryManager = InventoryManager.Instance;

        if (Instance == null)
        {
            Instance = this;
            StartCoroutine(GetHUD());
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        Minigame_ContentHolder.SetActive(false);

    }


    private void DisableIngredients()
    {
        foreach (var obj in ingredientDisplays)
        {
            obj.MainObject.SetActive(false);
        }

    }

    private void EnableIngredients()
    {
        foreach (var obj in ingredientDisplays)
        {
            obj.MainObject.SetActive(true);
        }
    }

    public void RecieveRecipe(Recipe recipe, string CookingMethod, bool recipeUnlocked)
    {

        RecipeUnlocked = recipeUnlocked;

        EnableIngredients();

        currentRecipe = recipe;

        Current_Cooking_Method = CookingMethod;

        if (recipeUnlocked)
        {
            maxCookQuantity = CalculateMaxQuantity(recipe);
            selectedCookQuantity = 1;
        }
        else
        {
            selectedCookQuantity = 0;
            maxCookQuantity = 0;
        }


        
        UpdateRecipeUI();
    }

    public int CalculateMaxQuantity(Recipe recipe)
    {
        int maxQty = int.MaxValue;

        foreach (var ingredient in recipe.ingredients)
        {
            int availableAmount = inventoryManager.GetTotalQuantity(ingredient.ingredientItem);
            int possibleQty = availableAmount / ingredient.quantityRequired;

            if (possibleQty < maxQty)
            {
                maxQty = possibleQty;
            }
        }

        return Mathf.Max(1, maxQty);
    }

    private void UpdateRecipeUI()
    {
        if (currentRecipe == null)
        {
            return;
        }

        if (RecipeUnlocked)
        {
            Unlocked_UI();
            dishIcon.gameObject.SetActive(true);
        }

        else if (!RecipeUnlocked)
        {
            Locked_UI();
            dishIcon.gameObject.SetActive(true);
        }
       
    }

    private void Unlocked_UI()
    {
        dishIcon.sprite = currentRecipe.resultDish.icon;
        dishName.text = currentRecipe.resultDish.itemName;
        dishDescription.text = currentRecipe.resultDish.description;

        for (int i = 0; i < ingredientDisplays.Length; i++)
        {
            if (i < currentRecipe.ingredients.Length)
            {
                var ingredient = currentRecipe.ingredients[i];
                ingredientDisplays[i].ingredientImage.sprite = ingredient.ingredientItem.icon;
                ingredientDisplays[i].ingredientName.text = ingredient.ingredientItem.itemName;
                ingredientDisplays[i].ingredientQTY.text = $"x{ingredient.quantityRequired}";
                ingredientDisplays[i].MainObject.SetActive(true);
            }
            else
            {
                ingredientDisplays[i].MainObject.SetActive(false);
            }
        }
    }


   

    private void Locked_UI()
    {
        dishIcon.sprite = currentRecipe.resultDish.icon;
        dishName.text = "Locked";
        dishDescription.text = "";

        for (int i = 0; i < ingredientDisplays.Length; i++)
        {
            ingredientDisplays[i].MainObject.SetActive(false);
        }
    }

    public void IncreaseCookQuantity()
    {
        if (RecipeUnlocked)
        {
            if (selectedCookQuantity < maxCookQuantity)
            {
                selectedCookQuantity++;
                cookQuantityText.text = selectedCookQuantity.ToString();

            }
        }
    }

    public void DecreaseCookQuantity()
    {
        if (RecipeUnlocked)
        {
            if (selectedCookQuantity > 1)
            {
                selectedCookQuantity--;
                cookQuantityText.text = selectedCookQuantity.ToString();

            }
        }
    }

    public void ExecuteCooking()
    {
        if (currentRecipe == null || maxCookQuantity == 0)
        {
            return;
        }

        if (!RecipeUnlocked)
        {
            return;
        }

        // Create a list of item removal requests
        List<ItemRemovalRequest> removalRequests = new List<ItemRemovalRequest>();
        foreach (var ingredient in currentRecipe.ingredients)
        {
            removalRequests.Add(new ItemRemovalRequest
            {
                item = ingredient.ingredientItem,
                quantity = ingredient.quantityRequired * selectedCookQuantity
            });
        }

        // Attempt to remove the required ingredients
        //Technically this is not required as the system has calculated max quantity
        if (!inventoryManager.RemoveItems(removalRequests))
        {
            Debug.Log("Not enough ingredients to cook!");
            return;
        }


        // Inventory Be full
        if (!CheckInventorySpace())
        {
            Debug.Log("Inventory is full!");
            return;
        }

        StartMinigameCycle();

    }

    private bool CheckInventorySpace()
    {
        bool canAddResult = inventoryManager.CanAddItem(currentRecipe.resultDish, selectedCookQuantity);
        bool canAddPerfect = inventoryManager.CanAddItem(currentRecipe.perfectDish, selectedCookQuantity);
        bool canAddFailure = inventoryManager.CanAddItem(FailureDish, selectedCookQuantity);

        if (!canAddResult) Debug.Log("Cannot add result dish to inventory");
        if (!canAddPerfect) Debug.Log("Cannot add perfect dish to inventory");
        if (!canAddFailure) Debug.Log("Cannot add failure dish to inventory");

        return canAddResult && canAddPerfect && canAddFailure;
    }


    private void StartMinigameCycle()
    {
        StartCoroutine(StartMinigameCoroutine());
    }

    private IEnumerator StartMinigameCoroutine()
    {

        Manager_ContentHolder.GetComponent<Animator>().SetTrigger("Exit");
        yield return new WaitForSeconds(0.5f);
        Manager_ContentHolder.SetActive(false);

        SetupMinigamePreview();

        Minigame_ContentHolder.SetActive(true);

        yield return new WaitForSeconds(0.5f);
        Cooking_Minigame_Manager.Instance.StartMinigameSequence(currentRecipe, Current_Cooking_Method, selectedCookQuantity);

    }

    private void SetupMinigamePreview()
    {
       Cooking_Minigame_Manager minigame_Manager = Cooking_Minigame_Manager.Instance;

        minigame_Manager.activeRecipe = currentRecipe;
        minigame_Manager.SetPreview();
    }

    public void TurnOnCookingUI(GameObject target)
    {
        if (inventoryManager.SomeUIEnabled == true)
        {
            return;
        }
        
        BG_Effect.SetActive(true);
        GameHUD.SetActive(false);
        target.SetActive(true);
        inventoryManager.SomeUIEnabled = true;
        
        //Time.timeScale = 0f;

        DefaultPreview();
    }

    public void TurnOffCookingUI(GameObject target)
    {
        //CloseButton.SetActive(false);
        BG_Effect.SetActive(false);
        target.SetActive(false);
        GameHUD.SetActive(false);
        InventoryManager.Instance.SomeUIEnabled = false;
        Time.timeScale = 1f;
    }

    private void DefaultPreview()
    {
        if (currentRecipe == null)
        {
            dishName.text = "Select a Recipe!";
            dishDescription.text = "";

            dishIcon.gameObject.SetActive(false);
        }
    }

    private IEnumerator GetHUD()
    {
        yield return new WaitForSecondsRealtime(0.5f);
        GameHUD = InventoryManager.Instance.HUD;
    }

   
}

[System.Serializable]
public class IngredientDisplay
{
    public GameObject MainObject;
    public Image ingredientImage;
    public TMP_Text ingredientName;
    public TMP_Text ingredientQTY;
}
