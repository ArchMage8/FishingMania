using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewRecipe", menuName = "Cooking/Recipe", order = 1)]
public class Recipe : ScriptableObject
{
    [System.Serializable]
    public struct Ingredient
    {
        public Item item;
        public int qty;
    }

    public Ingredient[] ingredients; // Array of ingredients
    public Item result; // The resulting item

    public MethodType methodType; // Cooking method (user-defined enum)
}

public enum MethodType
{
   Default, Stove
}

