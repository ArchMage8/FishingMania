using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class WorldLight_Handler : MonoBehaviour
{
    public Light2D LightSource;

    private float onPercentage = 80f;
    private float offPercentage = 12f;

    private float fadeOnDuration = 7f;
    private float fadeOffDuration = 3f;

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
        offEndTime = maxTime * (offPercentage / 100f);
        offStartTime = offEndTime - fadeOffDuration;

        onEndTime = maxTime * (onPercentage / 100f);
        onStartTime = onEndTime - fadeOnDuration;
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
