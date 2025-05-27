using UnityEngine;
using UnityEngine.Rendering.Universal;
using System.Collections;

public class Daylight_Handler : MonoBehaviour
{
    public static Daylight_Handler Instance { get; private set; }

    public Light2D globalLight;
    public float dayDuration = 60f;

    [Header("Lighting Curves")]
    public Gradient colorOverDay;
    public AnimationCurve intensityOverDay;

    [Header("Clock State")]
    public float currentTime = 0f;
    public bool TimeRunning = true;

    private float endDayResumeDelay = 0.5f;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void Update()
    {
        if (!TimeRunning)
        {
            return;
        }

        if (globalLight == null || dayDuration <= 0f)
        {
            return;
        }

        currentTime += Time.deltaTime;
        if (currentTime > dayDuration)
        {
            currentTime -= dayDuration;
        }

        float timePercent = currentTime / dayDuration;

        globalLight.color = colorOverDay.Evaluate(timePercent);
        globalLight.intensity = intensityOverDay.Evaluate(timePercent);
    }

    public void SetTime(float newTime)
    {
        currentTime = Mathf.Clamp(newTime, 0f, dayDuration);
        UpdateLighting();
    }

    public void ResetDayCycle()
    {
        currentTime = 0f;
        UpdateLighting();
    }

    public float GetNormalizedTime()
    {
        return currentTime / dayDuration;
    }

    public float GetDayDuration()
    {
        return dayDuration;
    }

    public float GetCurrentTime()
    {
        return currentTime;
    }

    public void End_Day()
    {
        TimeRunning = false;
        SetTime(System_HourConvertor(7)); // Set to 07:00
        StartCoroutine(ResumeTimeAfterDelay());
    }

    private IEnumerator ResumeTimeAfterDelay()
    {
        yield return new WaitForSeconds(endDayResumeDelay);
        TimeRunning = true;
    }

    private void UpdateLighting()
    {
        float timePercent = currentTime / dayDuration;

        if (globalLight != null)
        {
            globalLight.color = colorOverDay.Evaluate(timePercent);
            globalLight.intensity = intensityOverDay.Evaluate(timePercent);
        }
    }

    private float System_HourConvertor(int hour)
    {
        return (dayDuration / 24f) * Mathf.Clamp(hour, 0, 23);
    }

    public float GetCurrentHour() 
    {
        return (currentTime / dayDuration) * 24f;

        //To be used by external scripts to get the current time in hours
        //Ex : if (Handler.GetCurrentHour == 5) do X
    }
}
