using UnityEngine;

[CreateAssetMenu(fileName = "RecipeDefaultData", menuName = "Cooking/RecipeDefaultData", order = 1)]
public class RecipeDefaultDataSO : ScriptableObject
{
    [System.Serializable]
    public struct RecipeEntry
    {
        public RecipeSO recipe;
        public bool isUnlocked;
    }

    public RecipeEntry[] recipes;
}
