using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System.Collections.Generic;

public class ColorManager : MonoBehaviour
{
    [Header("Sliders")]
    public Slider topSlider;
    public Slider middleSlider;
    public Slider bottomSlider;

    [Header("Colors")]
    public Color goodColor;
    public Color normalColor;
    public Color badColor;

    [Header("Default Values")]
    public int defaultGood;
    public int defaultNormal;
    public int defaultBad;

    private int goodValue;
    private int normalValue;
    private int badValue;

    [Header("Slider Values")]
    public float GoodSliderValue;
    public float NormalSliderValue;
    public float BadSliderValue;

    [Header("Extra Values")]
    public int TempStock = 10;
    private int stock = 10;
    public int deductValue;

    [Header("Score Values")]
    public int Score;
    public int target;

    private Slider goodSlider;
    private Slider normalSlider;
    private Slider badSlider;

    private int tempTopValue;
    private Dictionary<string, int> returnValues = new Dictionary<string, int>
    {
        { "A", 1 },
        { "B", 2 },
        { "C", 3 }
    };

    void Start()
    {
        AssignRandomColors();
        ResetValues();
        stock = 10;
        SimulateStockDecrease();
    }

    private void SimulateStockDecrease()
    {
        stock = 10;

        int decreaseCount = 10 - TempStock;
        for (int i = 0; i < decreaseCount; i++)
        {
            stock -= 1;
            RecalculateValues();
        }
        ResetValues();
    }

    private void AssignRandomColors()
    {
        Slider[] sliders = { topSlider, middleSlider, bottomSlider };
        sliders = sliders.OrderBy(x => Random.value).ToArray();

        goodSlider = sliders[0];
        normalSlider = sliders[1];
        badSlider = sliders[2];

        goodSlider.fillRect.GetComponent<Image>().color = goodColor;
        normalSlider.fillRect.GetComponent<Image>().color = normalColor;
        badSlider.fillRect.GetComponent<Image>().color = badColor;
    }

    public void ResetValues()
    {
        if (stock == 10)
        {
            goodValue = defaultGood;
            normalValue = defaultNormal;
            badValue = defaultBad;
        }
        ApplySliderValues();
    }

    private void RecalculateValues()
    {
        if (stock >= 6)
        {
            goodValue = Mathf.Max(0, goodValue - deductValue);
            normalValue += deductValue;
        }
        else if (stock >= 3)
        {
            int halfDeduct = deductValue / 2;
            goodValue = Mathf.Max(0, goodValue - halfDeduct);
            normalValue = Mathf.Max(0, normalValue - halfDeduct);
            badValue += deductValue;
        }
        else
        {
            normalValue = Mathf.Max(0, normalValue - deductValue);
            badValue += deductValue;
        }
    }

    private void ApplySliderValues()
    {
        IdentifyBottom();
        IdentifyTop();
        IdentifyMid();
        PrepareSliderValues();
    }

    private void IdentifyBottom()
    {
        bottomSlider.value = 100;
    }

    private void IdentifyTop()
    {
        if (goodSlider == topSlider)
        {
            tempTopValue = goodValue;
            goodSlider.value = goodValue;
        }
        else if (normalSlider == topSlider)
        {
            tempTopValue = normalValue;
            normalSlider.value = normalValue;
        }
        else if (badSlider == topSlider)
        {
            tempTopValue = badValue;
            badSlider.value = badValue;
        }
    }

    private void IdentifyMid()
    {
        int tempMidValue;

        if (goodSlider == middleSlider)
        {
            tempMidValue = goodValue + tempTopValue;
            goodSlider.value = tempMidValue;
        }
        else if (normalSlider == middleSlider)
        {
            tempMidValue = normalValue + tempTopValue;
            normalSlider.value = tempMidValue;
        }
        else if (badSlider == middleSlider)
        {
            tempMidValue = badValue + tempTopValue;
            badSlider.value = tempMidValue;
        }
    }

    private void PrepareSliderValues()
    {
        GoodSliderValue = goodSlider.value;
        NormalSliderValue = normalSlider.value;
        BadSliderValue = badSlider.value;
    }

    public void SetScore()
    {
        if (target >= 0 && target <= topSlider.value)
        {
            Score = Set_Top();
        }
        else if (target > topSlider.value && target <= middleSlider.value)
        {
            Score = Set_Mid();
        }
        else if (target > middleSlider.value && target <= bottomSlider.value)
        {
            Score = Set_Bottom();
        }
    }

    private int Set_Top()
    {
        return returnValues[topSlider == goodSlider ? "A" : topSlider == normalSlider ? "B" : "C"];
    }

    private int Set_Mid()
    {
        return returnValues[middleSlider == goodSlider ? "A" : middleSlider == normalSlider ? "B" : "C"];
    }

    private int Set_Bottom()
    {
        return returnValues[bottomSlider == goodSlider ? "A" : bottomSlider == normalSlider ? "B" : "C"];
    }
}
