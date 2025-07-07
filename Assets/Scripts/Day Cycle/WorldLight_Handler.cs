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
    private Animator animator;


    //There is an exception built in to this script to handle the absense of the daylight handler
    //This is for the tutorial

    private void OnDestroy()
    {
        Daylight_Handler.OnNewDayStarted -= HandleNewDay;
    }

    private void Start()
    {
        daylight_handler = Daylight_Handler.Instance;

        animator = GetComponent<Animator>();

        if (daylight_handler != null)
        {
            Daylight_Handler.OnNewDayStarted += HandleNewDay;
            DayDuration = daylight_handler.GetDayDuration();
        }

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

        if(daylight_handler.VisualsTest_Day == true)
        {
            LightSource.enabled = false;
            return;
        }

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

    private void HandleNewDay()
    {
        ForceCorrectLightState(5f); // new day always resets to 5am
    }

    private void ForceCorrectLightState(float time)
    {
        // If time is before OffStart or after OffEnd but also before OnStart
        if (time < offStartTime || (time > offEndTime && time < onStartTime))
        {
            LightSource.intensity = 0f;
        }
        // If time is after OnEnd
        else if (time > onEndTime)
        {
            LightSource.intensity = originalIntensity;
        }
    }

    private void PauseAnimator()
    {
        if (Mathf.Abs(LightSource.intensity) < 0.001)
        {
            animator.speed = 0f;
        }

        else
        {
            animator.speed = 1f;
        }
    }
}
