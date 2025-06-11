using UnityEngine;
using UnityEngine.UI;
using TMPro;
using static Recipe;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Button))]
public class Cooking_RecipeButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Recipe Data")]
    [SerializeField] private Recipe recipe;

    [Header("UI Elements")]
    [SerializeField] private Image dishIcon;
    [SerializeField] private TMP_Text dishName;

    [Space(10)]

    public Sprite Active_Sprite;

    private Button button;
    private bool unlocked;
    private Sprite OriginalSprite;

    private void Awake()
    {
        button = GetComponent<Button>();
        OriginalSprite = GetComponent<Image>().sprite;
    }

    private void OnEnable()
    {
        if (recipe == null)
            return;

       
        SetupRecipeButton();
        //Debug.Log(unlocked);
    }

    public void SendRecipe()
    {
        string Method = recipe.method.ToString();
        CookingManager.Instance.RecieveRecipe(recipe, Method, unlocked);
    }

    private void SetupRecipeButton()
    {
        int cookQTY = CookingManager.Instance.CalculateMaxQuantity(recipe);

        dishIcon.sprite = recipe.resultDish.icon;
        dishName.text = recipe.resultDish.itemName;

        bool isUnlocked = Cooking_RecipeDataManager.Instance.GetStatus(recipe.recipeName);
        unlocked = isUnlocked;

        if (!isUnlocked)
        {
            DimEffect();
            dishName.gameObject.SetActive(false);
            
        }
        else if(isUnlocked && cookQTY == 0)
        {
            DimEffect();
            dishName.gameObject.SetActive(false);
        }
        else if (isUnlocked && cookQTY > 0)
        {
            NormalEffect();
            dishName.gameObject.SetActive(true);
        }
    }

    private void DimEffect()
    {
        Color darkColor = new Color(0.3f, 0.3f, 0.3f, 1f);
        GetComponent<Image>().color = darkColor;
        dishIcon.color = darkColor;
        
    }

    private void NormalEffect()
    {
        GetComponent<Image>().color = Color.white;
        dishIcon.color = Color.white;
        
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
       this.GetComponent<Image>().sprite = Active_Sprite;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        this.GetComponent<Image>().sprite = OriginalSprite;
    }
}
