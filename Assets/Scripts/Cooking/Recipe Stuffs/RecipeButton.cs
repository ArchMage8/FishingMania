using UnityEngine;
using UnityEngine.UI;
using static CookingManager;

public class RecipeButton : MonoBehaviour
{
    public Recipe assignedRecipe;
    public CookingMethod cookingMethod;

    [Space(20)]

    [SerializeField] private Button buttonComponent;
    [SerializeField] private Image buttonImage;

    [Space(20)]
    [SerializeField] private GameObject cookComic;
    [SerializeField] private Color inactiveColor;

    private string methodName;

    private void Awake()
    {
        ButtonSetup();
        cookComic.SetActive(false);


    }

    private void ButtonSetup()
    {
        methodName = cookingMethod.ToString();

        if (assignedRecipe == null) return;

        // Set button icon to recipe's target dish
        else if (assignedRecipe.resultDish != null && assignedRecipe.resultDish.icon != null)
        {

            buttonImage.sprite = assignedRecipe.resultDish.icon;
        }

        SetState();
    }

    private void OnEnable()
    {
        //Debug.Log("banana1");
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
            CookingManager.Instance.RecieveRecipe(assignedRecipe, methodName);
        }
    }
}