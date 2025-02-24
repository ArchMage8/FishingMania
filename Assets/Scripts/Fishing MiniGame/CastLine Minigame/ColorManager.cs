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
    public int stock = 10;
    public int deductValue;

    [Header("Score Values")]
    public int Score;
    public int target;

    private Slider goodSlider;
    private Slider normalSlider;
    private Slider badSlider;

    private int A, B, C;


    private int tempTopValue;
    private Dictionary<string, (int min, int max)> ranges = new Dictionary<string, (int, int)>();
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
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            //stock -= 1;
            AssignRandomColors();
            ResetValues();
        }

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
        else
        {
            RecalculateValues();
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
        if (goodSlider == bottomSlider)
        {
            goodSlider.value = 100;
        }
        else if (normalSlider == bottomSlider)
        {
            normalSlider.value = 100;
        }
        else if (badSlider == bottomSlider)
        {
            badSlider.value = 100;
        }
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
      if(target >= 0 && target <= topSlider.value)
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
        if(topSlider == goodSlider)
        {
            return 1;
        }

        else if(topSlider == normalSlider)
        {
            return 2;
        }

        else if(topSlider == badSlider)
        {
            return 3;
        }

        else
        {
            return 0;
        }
    }

    private int Set_Mid()
    {
        if (middleSlider == goodSlider)
        {
            return 1;
        }

        else if (middleSlider == normalSlider)
        {
            return 2;
        }

        else if (middleSlider == badSlider)
        {
            return 3;
        }

        else
        {
            return 0;
        }
    }

    private int Set_Bottom()
    {
        if (bottomSlider == goodSlider)
        {
            return 1;
        }

        else if (bottomSlider == normalSlider)
        {
            return 2;
        }

        else if (bottomSlider == badSlider)
        {
            return 3;
        }

        else
        {
            return 0;
        }
    }
}
