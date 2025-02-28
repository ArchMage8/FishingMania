using UnityEngine;

[CreateAssetMenu(fileName = "NewRecipe", menuName = "Cooking/Recipe", order = 1)]
public class RecipeSO : ScriptableObject
{
    [System.Serializable]
    public struct Ingredient
    {
        public Item item;
        public int quantity;
    }

    public string RecipeName;
    public Ingredient[] ingredients;
    public bool isUnlocked;
    public Item result;
}
