using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class WorldLight_Handler : MonoBehaviour
{
    public Light2D LightSource;

    private Daylight_Handler daylight_handler;
    private float DayDuration;
    private float originalIntensity;

    private float offStartTime = 4.5f;
    private float offEndTime = 6f;
    private float onStartTime = 16.5f;
    private float onEndTime = 18f;

    private float currentTime;
    private float currentGameTime;
    private Animator animator;

    private void OnDestroy()
    {
        Daylight_Handler.OnNewDayStarted -= HandleNewDay;
    }

    private void Start()
    {
        daylight_handler = Daylight_Handler.Instance;

      

        animator = GetComponent<Animator>();

        if (LightSource != null)
        {
            originalIntensity = LightSource.intensity;
        }

        if (daylight_handler != null)
        {
            Daylight_Handler.OnNewDayStarted += HandleNewDay;
            DayDuration = daylight_handler.GetDayDuration();

            // Evaluate light state on scene load
            currentTime = daylight_handler.GetCurrentTime();
            GetGameTime();
            EvaluateInitialLightIntensity(currentGameTime);
        }
    }

    private void GetGameTime()
    {
        float normalizedTime = currentTime / DayDuration;
        currentGameTime = normalizedTime * 24f;
    }

    private void Update()
    {
        if (daylight_handler == null) return;

        if (daylight_handler.VisualsTest_Day == true)
        {
            LightSource.enabled = false;
            return;
        }

        currentTime = daylight_handler.GetCurrentTime();
        GetGameTime();

        HandleFadeOff(currentGameTime);
        HandleFadeOn(currentGameTime);
        PauseAnimator();
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
        if (time < offStartTime || (time > offEndTime && time < onStartTime))
        {
            LightSource.intensity = 0f;
        }
        else if (time > onEndTime)
        {
            LightSource.intensity = originalIntensity;
        }
    }

    private void EvaluateInitialLightIntensity(float gameTime)
    {
        if (gameTime >= offStartTime && gameTime <= offEndTime)
        {
            float t = Mathf.InverseLerp(offStartTime, offEndTime, gameTime);
            LightSource.intensity = Mathf.Lerp(originalIntensity, 0f, t);
        }
        else if (gameTime >= onStartTime && gameTime <= onEndTime)
        {
            float t = Mathf.InverseLerp(onStartTime, onEndTime, gameTime);
            LightSource.intensity = Mathf.Lerp(0f, originalIntensity, t);
        }
        else
        {
            ForceCorrectLightState(gameTime);
        }
    }

    private void PauseAnimator()
    {
        if (animator == null) return;

        if (Mathf.Abs(LightSource.intensity) < 0.001f)
        {
            animator.speed = 0f;
        }
        else
        {
            animator.speed = 1f;
        }
    }
}
