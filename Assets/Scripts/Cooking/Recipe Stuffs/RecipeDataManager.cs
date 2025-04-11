using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class RecipeDataManager : MonoBehaviour
{
    public static RecipeDataManager Instance;

    [SerializeField] private RecipeDefaultList defaultRecipeList; // Reference to the default recipe list SO

    private Dictionary<string, bool> recipeUnlockData = new Dictionary<string, bool>(); // Stores recipes and their unlock status
    private string saveFilePath;

    private void Awake()
    {
        // Singleton pattern
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        saveFilePath = Path.Combine(Application.persistentDataPath, "recipe_data.json");
        LoadRecipeData(); // Load data at game start
    }

    private void InitializeDefaultRecipeData()
    {
        recipeUnlockData.Clear();

        foreach (var recipeEntry in defaultRecipeList.recipes)
        {
            recipeUnlockData[recipeEntry.recipe.recipeName] = recipeEntry.unlocked; // Store recipe name and its unlock status
        }
    }

    public void SaveRecipeData()
    {
        RecipeSaveData saveData = new RecipeSaveData();
        saveData.recipes = new List<RecipeSaveEntry>();

        foreach (var entry in recipeUnlockData)
        {
            saveData.recipes.Add(new RecipeSaveEntry { recipeName = entry.Key, unlocked = entry.Value });
        }

        string json = JsonUtility.ToJson(saveData, true);
        File.WriteAllText(saveFilePath, json);
        Debug.Log($"Recipe data saved to {saveFilePath}");
    }

    public void LoadRecipeData()
    {
        if (File.Exists(saveFilePath))
        {
            string json = File.ReadAllText(saveFilePath);
            RecipeSaveData loadedData = JsonUtility.FromJson<RecipeSaveData>(json);

            recipeUnlockData.Clear();

            foreach (var entry in loadedData.recipes)
            {
                recipeUnlockData[entry.recipeName] = entry.unlocked;
            }

            Debug.Log("Recipe data loaded successfully.");
        }
        else
        {
            Debug.LogWarning("No recipe save data found. Initializing default list.");
            InitializeDefaultRecipeData();
        }
    }

    public void DeleteRecipeData()
    {
        if (File.Exists(saveFilePath))
        {
            File.Delete(saveFilePath);
            Debug.Log("Recipe data deleted.");
        }
        else
        {
            Debug.LogWarning("No recipe save data to delete.");
        }
    }

    public bool CheckRecipeStatus(string recipeName)
    {
        if (recipeUnlockData.TryGetValue(recipeName, out bool isUnlocked))
        {
            return isUnlocked;
        }

        Debug.LogWarning($"Recipe {recipeName} not found in data.");
        return false;
    }

    public void UnlockRecipe(string recipeName)
    {
        if (recipeUnlockData.ContainsKey(recipeName))
        {
            recipeUnlockData[recipeName] = true;
            Debug.Log($"Recipe {recipeName} unlocked!");
        }
        else
        {
            Debug.LogWarning($"Recipe {recipeName} not found in data.");
        }
    }
}

// Class for JSON serialization
[System.Serializable]
public class RecipeSaveEntry
{
    public string recipeName;
    public bool unlocked;
}

[System.Serializable]
public class RecipeSaveData
{
    public List<RecipeSaveEntry> recipes;
}
