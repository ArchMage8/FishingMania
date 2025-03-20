using UnityEngine;

[CreateAssetMenu(fileName = "NewRecipeList", menuName = "Cooking/Recipe List", order = 2)]
public class RecipeDefaultList : ScriptableObject
{
    [System.Serializable]
    public struct RecipeEntry
    {
        public Recipe recipe; // The recipe itself
        public bool unlocked; // Whether the player has unlocked this recipe
    }

    public RecipeEntry[] recipes; // List of all recipes and their unlocked states
}
