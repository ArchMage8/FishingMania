using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;
using System.Collections;

public class Daylight_Handler : MonoBehaviour
{
    public static event System.Action OnNewDayStarted;

    public static Daylight_Handler Instance { get; private set; }

    public Light2D globalLight;
    public float dayDuration = 60f;

    [Header("Lighting Curves")]
    public Gradient colorOverDay;
    public AnimationCurve intensityOverDay;

    [Header("Clock State")]
    public float currentTime = 0f;
    public bool TimeRunning = true;

    [Header("Test")]
    public bool VisualsTest_Day = false;
    private float lastCheckedHour = -1f;

    private bool exceptionExists;

    private bool NewDayHandled = false;
    private bool TimeLoopBack = false;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        var exception = FindObjectOfType<Daylight_Exception>();
        if (exception == null)
        {
            exceptionExists = false;
        }
        else
        {
            exceptionExists = true;
        }

        UpdateLighting();
    }

    private void Update()
    {
        if (!TimeRunning)
            return;

        if (globalLight == null || dayDuration <= 0f)
            return;

        currentTime += Time.deltaTime;
        currentTime = Mathf.Repeat(currentTime, dayDuration);

        float currentHour = GetCurrentHour();

        if (currentHour >= 5f && TimeLoopBack && !NewDayHandled)
        {
            HandleNewDay_Visuals();
            HandleNewDay_System();
        }

        if (currentHour > 5f)
        {
            TimeLoopBack = false;
            NewDayHandled = false; // ✅ Reset here for next cycle
        }
        else if (currentHour < 5f && currentHour > 0f)
        {
            TimeLoopBack = true;
        }

        float timePercent = currentTime / dayDuration;

        if (!exceptionExists)
        {
            globalLight.gameObject.SetActive(true);
            globalLight.color = colorOverDay.Evaluate(timePercent);

            if (Weather_Handler.Instance != null && Weather_Handler.Instance.CurrentWeather == Weather_Handler.WeatherType.Rainy)
            {
                globalLight.intensity = 0.55f;
            }
            else
            {
                globalLight.intensity = intensityOverDay.Evaluate(timePercent);
            }
        }
        else
        {
            globalLight.gameObject.SetActive(false);
        }
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

    private void UpdateLighting()
    {
        float timePercent = currentTime / dayDuration;

        if (globalLight != null)
        {
            globalLight.color = colorOverDay.Evaluate(timePercent);
            globalLight.intensity = intensityOverDay.Evaluate(timePercent);
        }
    }

    private float System_HourConvertor(float hour)
    {
        return (dayDuration / 24f) * Mathf.Clamp(hour, 0, 23);
    }

    public float GetCurrentHour()
    {
        return (currentTime / dayDuration) * 24f;
    }

    public void CallNewDay()
    {
        TimeRunning = false;
        currentTime = 122f;
        UpdateLighting();

        HandleNewDay_Visuals();
        StartCoroutine(ResumeTimeAfterDelay());
        OnNewDayStarted?.Invoke();
    }

    private IEnumerator ResumeTimeAfterDelay()
    {
        yield return new WaitForSeconds(0f);
        TimeRunning = true;
    }

    private void HandleNewDay_System()
    {
        NPCManager.Instance.ResetNPCs();
        NPCStateRefresher.Instance.RefreshAllNPCStates();
        NPC_CriteriaChecker.Instance.RunChecksOnAll();
    }

    private void HandleNewDay_Visuals()
    {
        if (!NewDayHandled)
        {
            Debug.Log("Bananas");
            NewDayHandled = true;
            Weather_Handler.Instance.ResetWeather();
        }
    }
}
