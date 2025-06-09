using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Cooking_RecipeDataManager : MonoBehaviour
{
    public static Cooking_RecipeDataManager Instance;

    
    [SerializeField] private RecipeDefaultList defaultListSO;

    private string saveFilePath;
    private List<RecipeEntryData> recipeDataList = new List<RecipeEntryData>();

    [System.Serializable]
    public class RecipeEntryData
    {
        public string recipeName;
        public bool unlocked;
    }

    private void Awake()
    {
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

        saveFilePath = Path.Combine(Application.persistentDataPath, "recipeData.json");
        LoadData();

        
    }

 
    public void SaveData()
    {
        string json = JsonUtility.ToJson(new RecipeDataListWrapper { recipes = recipeDataList }, true);
        File.WriteAllText(saveFilePath, json);
     
    }

   
    public void LoadData()
    {
        if (File.Exists(saveFilePath))
        {
            string json = File.ReadAllText(saveFilePath);
            RecipeDataListWrapper loadedData = JsonUtility.FromJson<RecipeDataListWrapper>(json);
            recipeDataList = loadedData.recipes;
            Debug.Log("Load Successful");

        }
        else
        {
            InitializeDefaultList();
            Debug.Log("Load Successful");
        }
    }


    private void InitializeDefaultList()
    {
        recipeDataList.Clear();

        foreach (var entry in defaultListSO.recipes)
        {
            recipeDataList.Add(new RecipeEntryData
            {
                recipeName = entry.recipe.recipeName,
                unlocked = entry.unlocked
            });
        }
    }

  
    public void DeleteData()
    {
        if (File.Exists(saveFilePath))
        {
            File.Delete(saveFilePath);
            Debug.Log("Recipe data file deleted.");
        }
    }

    public bool GetStatus(string recipeName)
    {
        RecipeEntryData entry = recipeDataList.Find(r => r.recipeName == recipeName);
        if (entry != null)
        {
            return entry.unlocked;
        }

        Debug.LogWarning($"Recipe {recipeName} not found in data list.");
        return false;
    }

    public void UnlockRecipe(string recipeName)
    {
        RecipeEntryData entry = recipeDataList.Find(r => r.recipeName == recipeName);
        if (entry != null)
        {
            entry.unlocked = true;
            Debug.Log($"Recipe {recipeName} unlocked.");
        }
        else
        {
            Debug.LogWarning($"Recipe {recipeName} not found in data list.");
        }
    }

    [System.Serializable]
    private class RecipeDataListWrapper
    {
        public List<RecipeEntryData> recipes;
    }
}
