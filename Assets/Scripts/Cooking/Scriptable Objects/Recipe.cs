using UnityEngine;

[CreateAssetMenu(fileName = "NewRecipe", menuName = "Cooking/Recipe", order = 1)]
public class Recipe : ScriptableObject
{
    [System.Serializable]
    public struct RecipeIngredient
    {
        public Item ingredientItem; // The ingredient itself (ScriptableObject)
        public int quantityRequired; // How much of it is needed
    }

    public string recipeName; // Name of the recipe
    public RecipeIngredient[] ingredients; // List of required ingredients

    [Space(10)]

    public Item resultDish; // The dish created after cooking
    public Item perfectDish; // The perfect version of the created dish
}
