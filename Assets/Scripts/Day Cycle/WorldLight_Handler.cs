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

    private float onPercentage = 75f;
    private float offPercentage = 21f;

    private float fadeOn_Percentage = 73f;
    private float fadeOff_Percentage = 83f;

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
        
        offEndTime = maxTime * (offPercentage / 100f); //Time all lights are off

        offStartTime = offEndTime * (fadeOff_Percentage/100f);

        //----------------------------------------------------------

        onEndTime = maxTime * (onPercentage / 100f); //Time  all lights are on

        onStartTime = onEndTime * (fadeOn_Percentage/100f);
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
