using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class CookingManager : MonoBehaviour
{
    public static CookingManager Instance;

    public RecipeDefaultDataSO defaultRecipeData; // Assign in Inspector
    public List<RecipeDataEntry> recipeDataList = new List<RecipeDataEntry>();

    private string saveFilePath;

    private void Awake()
    {
        Instance = this;
        saveFilePath = Path.Combine(Application.persistentDataPath, "recipeData.json");

        LoadRecipeData();
    }

    [System.Serializable]
    public class RecipeDataEntry
    {
        public string recipeName;
        public bool isUnlocked;

        public RecipeDataEntry(string name, bool unlocked)
        {
            recipeName = name;
            isUnlocked = unlocked;
        }
    }

    public void SaveRecipeData()
    {
        string json = JsonUtility.ToJson(new RecipeSaveData { recipes = recipeDataList }, true);
        File.WriteAllText(saveFilePath, json);
        Debug.Log("Recipe data saved.");
    }

    public void LoadRecipeData()
    {
        if (File.Exists(saveFilePath))
        {
            string json = File.ReadAllText(saveFilePath);
            RecipeSaveData saveData = JsonUtility.FromJson<RecipeSaveData>(json);
            recipeDataList = saveData.recipes;
            Debug.Log("Recipe data loaded.");
        }
        else
        {
            InitializeDefaultData();
        }
    }

    private void InitializeDefaultData()
    {
        recipeDataList.Clear();
        foreach (var entry in defaultRecipeData.recipes)
        {
            recipeDataList.Add(new RecipeDataEntry(entry.recipe.RecipeName, entry.isUnlocked));
        }
        SaveRecipeData();
    }

    public void DeleteRecipeData()
    {
        if (File.Exists(saveFilePath))
        {
            File.Delete(saveFilePath);
            Debug.Log("Recipe data deleted.");
        }
    }

    public void UnlockRecipe(RecipeSO recipe)
    {
        foreach (var entry in recipeDataList)
        {
            if (entry.recipeName == recipe.RecipeName)
            {
                entry.isUnlocked = true;
                SaveRecipeData();
                Debug.Log($"Recipe {recipe.RecipeName} unlocked.");
                return;
            }
        }
        Debug.LogWarning($"Recipe {recipe.RecipeName} not found in data list.");
    }

    [System.Serializable]
    private class RecipeSaveData
    {
        public List<RecipeDataEntry> recipes;
    }
}
