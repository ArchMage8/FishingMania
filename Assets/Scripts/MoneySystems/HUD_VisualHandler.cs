using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public class HUD_VisualHandler : MonoBehaviour
{
    [Header("References")]
    public Image[] hudImages;
    public TextMeshProUGUI[] hudTexts;

    [Header("Time Settings")]
    public TimeColorData[] timeColorSettings;

    [Header("Time Source")]
    public float currentTime; // Should be 0–23, updated externally

    [Header("Transition Settings")]
    public float colorLerpSpeed = 2f;

    [Header("Override")]
    public bool overrideWithWhite = false;

    private Color currentTargetColor;
    private bool isTransitioning = false;

    [System.Serializable]
    public class TimeColorData
    {
        public float time;
        public Color targetColor;
    }

    private void Update()
    {
        if (Daylight_Handler.Instance != null)
        {
            SetTime();

            // If there's a Daylight_Exception, set the color to white.
            if (Daylight_Exception.Instance != null)
            {
                SetTargetColor(Color.white);
            }
            else
            {
                // Otherwise, update the color based on time.
                UpdateTargetColorFromTime();
            }

            // If transitioning, adjust HUD colors.
            if (isTransitioning)
            {
                LerpHUDColorsToTarget();
            }
        }
        else
        {
            // If Daylight_Handler is null, check the Daylight_Exception status.
            if (Daylight_Exception.Instance != null)
            {
                SetTargetColor(Color.white);
                SetHUDColorsToTarget();
            }
            else
            {
                // Otherwise, set the color to a neutral gray tone.
                SetTargetColor(HexToColor("767676"));
                SetHUDColorsToTarget();
            }
        }

    }

    Color HexToColor(string hex)
    {
        if (ColorUtility.TryParseHtmlString("#" + hex, out var color))
        {
            Debug.Log(color);
            return color;
        }
        else
        {
            return Color.white; // Fallback color
        }
    }


    private void SetTime()
    {
        float TempcurrentTime = Daylight_Handler.Instance.GetCurrentTime();
        float normalizedTime = TempcurrentTime / Daylight_Handler.Instance.dayDuration;

        currentTime = normalizedTime * 24f;
    }

    private void UpdateTargetColorFromTime()
    {
        if (timeColorSettings == null || timeColorSettings.Length == 0)
        {
            isTransitioning = false;
            return;
        }

        var sortedSettings = timeColorSettings.OrderBy(t => t.time).ToArray();
        TimeColorData closest = sortedSettings.Last();

        foreach (var data in sortedSettings)
        {
            if (data.time <= currentTime)
                closest = data;
            else
                break;
        }

        SetTargetColor(closest.targetColor);
    }

    private void SetTargetColor(Color newColor)
    {
        if (currentTargetColor != newColor)
        {
            currentTargetColor = newColor;
            isTransitioning = true;
        }
    }

    private void LerpHUDColorsToTarget()
    {
        foreach (var img in hudImages)
        {
            img.color = Color.Lerp(img.color, currentTargetColor, Time.deltaTime * colorLerpSpeed);
        }

        foreach (var text in hudTexts)
        {
            text.color = Color.Lerp(text.color, currentTargetColor, Time.deltaTime * colorLerpSpeed);
        }
    }

    private void SetHUDColorsToTarget()
    {
        foreach (var img in hudImages)
        {
            img.color = currentTargetColor;
        }

        foreach (var text in hudTexts)
        {
            text.color = currentTargetColor;
        }
    }
}
