using UnityEngine;
using UnityEngine.UI;

public class RecipeButton : MonoBehaviour
{
    public RecipeSO targetRecipe; // The assigned recipe for this button
    public Image recipeImage; // The UI image displaying the recipe icon
    public Color unavailableColor = Color.gray; // Color when ingredients are insufficient

    private void OnEnable()
    {
        SetupButton();
        CheckInventory();
    }

    // Sets up the button image based on the recipe result
    public void SetupButton()
    {
        if (targetRecipe != null && targetRecipe.result != null)
        {
            recipeImage.sprite = targetRecipe.result.icon;
        }
    }

    // Checks if there are enough ingredients in the inventory
    public void CheckInventory()
    {
        int maxDishes = int.MaxValue;

        foreach (var ingredient in targetRecipe.ingredients)
        {
            int inventoryQty = InventoryManager.Instance.GetTotalQuantity(ingredient.item);
            int requiredQty = ingredient.quantity;

            if (inventoryQty < requiredQty)
            {
                recipeImage.color = unavailableColor; // Darken if missing ingredients
                return;
            }
            else
            {
                maxDishes = Mathf.Min(maxDishes, inventoryQty / requiredQty);
            }
        }

        recipeImage.color = Color.white; // Reset to normal if enough ingredients
    }

    // Selects the recipe and sends it to the Cooking UI Manager
    public void SelectRecipe()
    {
        if (CookingUIManager.Instance != null && targetRecipe != null)
        {
            CookingUIManager.Instance.SetPreviews(targetRecipe);
        }
    }
}
