using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Cooking_ExecuteButton : MonoBehaviour
{
    private void Update()
    {
        CheckRecipe();
    }

    private void CheckRecipe()
    {
        if (CookingManager.Instance.currentRecipe != null)
        {
            if (CookingManager.Instance.maxCookQuantity == 0)
            {
                DimEffect();
            }
        }
        else
        {
            DimEffect();
        }
    }

    private void DimEffect()
    {
        Color darkColor = new Color(0.3f, 0.3f, 0.3f, 1f);
        GetComponent<Image>().color = darkColor;
       

    }
}
