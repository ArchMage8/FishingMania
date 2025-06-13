using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class WorldLight_Handler : MonoBehaviour
{
    //Documentation:
    //To prevent frustration and repetetive tasks, the values are hardcoded into the script
    //This is so we don't have to modify each and every prefab that holds this script


    public Light2D LightSource;

    private Daylight_Handler daylight_handler;
    private float DayDuration;
    private float originalIntensity;

    private float offStartTime = 4.5f;
    private float offEndTime = 6;
    private float onStartTime = 16.5f;
    private float onEndTime = 18f;

    private float currentTime;
    private float currentGameTime;

    private void Start()
    {
        daylight_handler = Daylight_Handler.Instance;
        DayDuration = daylight_handler.GetDayDuration();

        originalIntensity = LightSource.intensity;
    }

    private void GetGameTime()
    {

        float normalizedTime = currentTime / DayDuration;
        currentGameTime = normalizedTime * 24f;
        
    }

    private void Update()
    {
        if (daylight_handler == null) return;

        currentTime = daylight_handler.GetCurrentTime();

        GetGameTime();

        HandleFadeOff(currentGameTime);
        HandleFadeOn(currentGameTime);
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
