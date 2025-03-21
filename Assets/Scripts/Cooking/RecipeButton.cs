using UnityEngine;
using UnityEngine.UI;

public class RecipeButton : MonoBehaviour
{
    [SerializeField] private Recipe assignedRecipe;

    [Space(20)]

    [SerializeField] private Button buttonComponent;
    [SerializeField] private Image buttonImage;

    [Space(20)]
    [SerializeField] private GameObject cookComic;
    [SerializeField] private Color inactiveColor;

    private void OnEnable()
    {
        if (assignedRecipe == null) return;

        // Set button icon to recipe's target dish
        if (assignedRecipe.resultDish != null && assignedRecipe.resultDish.icon != null)
        {
            buttonImage.sprite = assignedRecipe.resultDish.icon;
        }

        SetState();
    }

    private void SetState()
    {
        // Check if recipe is unlocked
        bool isUnlocked = RecipeDataManager.Instance.CheckRecipeStatus(assignedRecipe.recipeName);
        int maxQuantity = CookingManager.Instance.CalculateMaxQuantity(assignedRecipe);

        if (isUnlocked && maxQuantity > 0)
        {
            buttonComponent.interactable = true;
            buttonImage.color = Color.white; // Active state
        }
        else
        {
            buttonComponent.interactable = false;
            buttonImage.color = inactiveColor; // Inactive state
        }
    }

    public void SendRecipe()
    {
        if (assignedRecipe != null)
        {
            CookingManager.Instance.RecieveRecipe(assignedRecipe, cookComic);
        }
    }
}
