using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static HUD_VisualHandler;

public class VisibleButton_Handler : MonoBehaviour
{
    private Button button;
    private Image image;

    private float currentTime;
    private float colorLerpSpeed = 2f;

    private Color currentTargetColor;
    private bool isTransitioning = false;

    private Color grayColor = new Color32(0x76, 0x76, 0x76, 0xFF); // #767676
    private Color whiteColor = Color.white;

    private void Awake()
    {
        button = GetComponent<Button>();
        image = GetComponent<Image>();
    }

    private void Update()
    {
       SetStatus();

        if (Daylight_Exception.Instance != null)
        {
            SetTargetColor(Color.white);
        }
        else
        {
            UpdateTargetColorFromTime();
        }

        if (isTransitioning)
        {
            LerpImageColorToTarget();
        }
    }

    private void SetStatus()
    {
        if (InventoryManager.Instance.SomeUIEnabled)
        {
            image.enabled = false;
            button.enabled = false;
        }

        else
        {
            button.enabled = true;
            image.enabled = true;
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
        // Time from 18 to 6 is considered "night" -> gray, else white
        if (currentTime >= 18 || currentTime <= 6)
            SetTargetColor(grayColor);
        else
            SetTargetColor(whiteColor);
    }

    private void SetTargetColor(Color newColor)
    {
        if (currentTargetColor != newColor)
        {
            currentTargetColor = newColor;
            isTransitioning = true;
        }
    }

    private void LerpImageColorToTarget()
    {
        if (image != null)
        {
            image.color = Color.Lerp(image.color, currentTargetColor, Time.deltaTime * colorLerpSpeed);
        }
    }
}
