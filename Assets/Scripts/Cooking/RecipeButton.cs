using UnityEngine;
using UnityEngine.UI;

public class RecipeButton : MonoBehaviour
{
    public RecipeSO targetRecipe; // The recipe associated with this button
    public Image recipeImage; // UI image for the recipe
  
    public Color lockedColor = Color.gray; // Color for locked recipes

    private void OnEnable()
    {
        SetupButton();
        if (IsRecipeUnlocked())
        {
            CheckInventory();
        }
        else
        {
            recipeImage.color = lockedColor; // Darken the image if locked
            GetComponent<Button>().interactable = false; // Disable button interaction
        }
    }

    private void SetupButton()
    {
        if (IsRecipeUnlocked())
        {
            recipeImage.sprite = targetRecipe.result.icon; // Set recipe image
        }
    }

    private bool IsRecipeUnlocked()
    {
        foreach (var recipeEntry in CookingManager.Instance.recipeDataList)
        {
            if (recipeEntry.recipeName == targetRecipe.name)
            {
                return recipeEntry.isUnlocked;
            }
        }
        return false; // If not found, assume locked
    }

    private void CheckInventory()
    {
        bool hasIngredients = true;

        foreach (var ingredient in targetRecipe.ingredients)
        {
            int inventoryQty = InventoryManager.Instance.GetTotalQuantity(ingredient.item);
            if (inventoryQty < ingredient.quantity)
            {
                hasIngredients = false;
                break;
            }
        }

        recipeImage.color = hasIngredients ? Color.white : lockedColor; // Darken if missing ingredients
    }

    public void SelectRecipe()
    {
        if (IsRecipeUnlocked())
        {
            CookingUIManager.Instance.SetPreviews(targetRecipe); // Send recipe to UI
        }
    }
}
