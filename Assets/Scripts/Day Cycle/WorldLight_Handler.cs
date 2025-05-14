using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class WorldLight_Handler : MonoBehaviour
{
    public Light2D LightSource;

    private float On_Percentage = 75f;
    private float Off_Percentage = 12f;

    private float fadeDuration = 5f;

    private Daylight_Handler daylight_handler;
    private float maxTime;
    private float originalIntensity;

    private float offStartTime;
    private float offEndTime;
    private float onStartTime;
    private float onEndTime;

    private void Start()
    {
        daylight_handler = Daylight_Handler.Instance;
        maxTime = daylight_handler.GetDayDuration();

        originalIntensity = LightSource.intensity;

        CalculateFadeWindows();
    }

    private void CalculateFadeWindows()
    {
        offEndTime = maxTime * (Off_Percentage / 100f);
        offStartTime = offEndTime - fadeDuration;

        onEndTime = maxTime * (On_Percentage / 100f);
        onStartTime = onEndTime - fadeDuration;
    }

    private void Update()
    {
        if (daylight_handler == null) return;

        float currentTime = daylight_handler.GetCurrentTime();

        HandleFadeOff(currentTime);
        HandleFadeOn(currentTime);
    }

    private void HandleFadeOff(float currentTime)
    {
        if (currentTime >= offStartTime && currentTime <= offEndTime)
        {
            float t = Mathf.InverseLerp(offStartTime, offEndTime, currentTime);
            LightSource.intensity = Mathf.Lerp(originalIntensity, 0f, t);
        }
    }

    private void HandleFadeOn(float currentTime)
    {
        if (currentTime >= onStartTime && currentTime <= onEndTime)
        {
            float t = Mathf.InverseLerp(onStartTime, onEndTime, currentTime);
            LightSource.intensity = Mathf.Lerp(0f, originalIntensity, t);
        }
    }
}
