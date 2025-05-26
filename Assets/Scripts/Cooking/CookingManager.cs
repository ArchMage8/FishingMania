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

    public GameObject CloseButton;
    public GameObject CookingUI;
    public Item FailureDish;

    [Space(20)]

    [SerializeField] private IngredientDisplay[] ingredientDisplays; // UI elements for ingredients

    private Recipe currentRecipe;
    private int maxCookQuantity = 1;
    private int selectedCookQuantity = 1;
    private string Current_Cooking_Method = null;

    public enum CookingMethod
    {
        Fry,
        Boil,
        Bake
    }


    private void Awake()
    {
        CloseButton.SetActive(true);

        DisableIngredients();
        inventoryManager = InventoryManager.Instance;

        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
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

    public void RecieveRecipe(Recipe recipe, string CookingMethod)
    {

        EnableIngredients();

        currentRecipe = recipe;

        Current_Cooking_Method = CookingMethod;
    
        // Determine max cook quantity
        maxCookQuantity = CalculateMaxQuantity(recipe);
        selectedCookQuantity = 1;

        // Update UI
        UpdateRecipeUI();
    }

    public int CalculateMaxQuantity(Recipe recipe)
    {
        int maxQty = int.MaxValue;
        Debug.Log(recipe.recipeName);

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
        if (currentRecipe == null) return;

        // Update Dish Icon
        dishIcon.sprite = currentRecipe.resultDish.icon;

        //Update Active Recipe Name
        dishName.text = currentRecipe.resultDish.itemName;

        //Update Dish description
        dishDescription.text = currentRecipe.resultDish.description;

        // Update Ingredients UI
        for (int i = 0; i < ingredientDisplays.Length; i++)
        {
            if (i < currentRecipe.ingredients.Length)
            {
                ingredientDisplays[i].ingredientImage.sprite = currentRecipe.ingredients[i].ingredientItem.icon;
                ingredientDisplays[i].ingredientQTY.text = $"{currentRecipe.ingredients[i].quantityRequired}";
                ingredientDisplays[i].MainObject.SetActive(true);
            }
            else
            {
                ingredientDisplays[i].MainObject.SetActive(false);
            }
        }
    }

    public void IncreaseCookQuantity()
    {
        if (selectedCookQuantity < maxCookQuantity)
        {
            selectedCookQuantity++;
            cookQuantityText.text = selectedCookQuantity.ToString();

        }
    }

    public void DecreaseCookQuantity()
    {
        if (selectedCookQuantity > 1)
        {
            selectedCookQuantity--;
            cookQuantityText.text = selectedCookQuantity.ToString();

        }
    }

    public void ExecuteCooking()
    {
        if (currentRecipe == null) //No recipe
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
        if (inventoryManager.CanAddItem(currentRecipe.resultDish, selectedCookQuantity))
        {
            if(inventoryManager.CanAddItem(currentRecipe.perfectDish, selectedCookQuantity))
            {
                if(inventoryManager.CanAddItem(FailureDish, selectedCookQuantity))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }
    }


    private void StartMinigameCycle()
    {
        Cooking_Minigame_Manager.Instance.StartMinigameSequence(currentRecipe, Current_Cooking_Method, selectedCookQuantity);
    }

    //Very important this ya
    public void TurnOffCookingUI(GameObject target)
    {
        CloseButton.SetActive(false);
        target.SetActive(false);

        InventoryManager.Instance.SomeUIEnabled = false;
        Time.timeScale = 1f;
    }
}

[System.Serializable]
public class IngredientDisplay
{
    public GameObject MainObject;
    public Image ingredientImage;
    public TMP_Text ingredientQTY;
}
