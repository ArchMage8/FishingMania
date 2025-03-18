 using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class CookingUIManager : MonoBehaviour
{
    public static CookingUIManager Instance;

    [Header("Dish Display")]
    public Image TargetDish; // Displays the dish being cooked
    public TMP_Text NamePreview; // Displays the dish name
    public TMP_Text QtyPreview; // Displays the quantity of dishes
    public TMP_Text DescriptionPreview; // Displays dish description

    [Header("Cooking System")]
    public int CookQty; // Number of dishes to be cooked
    private int MaxDishes = 0; // Maximum dishes that can be cooked with available ingredients
    private RecipeSO SelectedRecipeSO; // Currently selected recipe

    [System.Serializable]
    public struct IngredientSlot
    {
        public Image ingredientImage; // UI representation of ingredient
        public string ingredientQty; // Required quantity of ingredient
    }

    [Header("Ingredient Slots")]
    public IngredientSlot[] SelectedIngredients = new IngredientSlot[5]; // Fixed size for UI slots

    private void Awake()
    {
        Instance = this;
    }

    // Resets all ingredient UI slots
    public void ResetIngredientDisplay()
    {
        SelectedRecipeSO = null; // Reset selected recipe
        foreach (var slot in SelectedIngredients)
        {
            slot.ingredientImage.gameObject.SetActive(false);
            slot.ingredientImage.sprite = null; // Clear icon image
        }
        NamePreview.text = ""; // Clear dish name
        QtyPreview.text = ""; // Clear cook quantity
        DescriptionPreview.text = ""; // Clear description
    }

    // Sets preview data based on the selected recipe
    public void SetPreviews(RecipeSO recipe)
    {
        if (recipe == null) return;

        SelectedRecipeSO = recipe;
        DisplayIngredients(recipe);

        TargetDish.sprite = recipe.result.icon; // Set dish image
        NamePreview.text = recipe.result.name; // Set dish name
        DescriptionPreview.text = recipe.result.description; // Set description

        // Set initial quantity preview
        CookQty = MaxDishes > 0 ? 1 : 0;
        QtyPreview.text = CookQty.ToString();
    }

    // Displays required ingredients for the selected recipe
    public void DisplayIngredients(RecipeSO recipe)
    {
        if (SelectedRecipeSO == null) return;

        ResetIngredientDisplay(); // Ensure UI starts fresh

        for (int i = 0; i < recipe.ingredients.Length; i++)
        {
            SelectedIngredients[i].ingredientImage.sprite = recipe.ingredients[i].item.icon; // Set icon
            SelectedIngredients[i].ingredientQty = recipe.ingredients[i].quantity.ToString(); // Set quantity
            SelectedIngredients[i].ingredientImage.gameObject.SetActive(true); // Enable slot
        }

        CheckInventoryForIngredients(recipe); // Check inventory availability
    }

    // Checks if inventory has required items
    private void CheckInventoryForIngredients(RecipeSO recipe)
    {
        if (SelectedRecipeSO == null) return;

        MaxDishes = int.MaxValue;

        for (int i = 0; i < recipe.ingredients.Length; i++)
        {
            int inventoryQty = InventoryManager.Instance.GetTotalQuantity(recipe.ingredients[i].item);
            int requiredQty = recipe.ingredients[i].quantity;

            if (inventoryQty < requiredQty)
            {
                SelectedIngredients[i].ingredientImage.color = Color.gray; // Darken missing items
                MaxDishes = 0;
            }
            else
            {
                SelectedIngredients[i].ingredientImage.color = Color.white;
                MaxDishes = Mathf.Min(MaxDishes, inventoryQty / requiredQty); // Calculate max cookable dishes
            }
        }
    }

    public void AddCookQty()
    {
        if (MaxDishes > 0 && CookQty < MaxDishes)
        {
            CookQty++;
            QtyPreview.text = CookQty.ToString(); // Update UI quantity preview
        }
    }

    public void DecreaseCookQty()
    {
        if (MaxDishes > 0 && CookQty > 1)
        {
            CookQty--;
            QtyPreview.text = CookQty.ToString(); // Update UI quantity preview
        }
    }

    public void SetQtyZero()
    {
        CookQty = 0;
        QtyPreview.text = "0"; // Update UI quantity preview
    }

    public void CookConfirm()
    {
        if (SelectedRecipeSO == null) return;

        if (InventoryManager.Instance.AddItem(SelectedRecipeSO.result, CookQty))
        {
            SuccessCook();
        }
        else
        {
            FailCook();
        }
    }

    private void FailCook()
    {
        Debug.Log("Cook Fail: Inventory is Full");
    }

    private void SuccessCook()
    {
        if (SelectedRecipeSO == null) return;

        List<ItemRemovalRequest> removalRequests = new List<ItemRemovalRequest>();

        foreach (var ingredient in SelectedRecipeSO.ingredients)
        {
            removalRequests.Add(new ItemRemovalRequest { item = ingredient.item, quantity = ingredient.quantity * CookQty });
        }

        InventoryManager.Instance.RemoveItems(removalRequests); // Remove used ingredients
    }
}
