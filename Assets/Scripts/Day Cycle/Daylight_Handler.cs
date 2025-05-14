using UnityEngine;
using UnityEngine.Rendering.Universal; // Required for Light2D

public class Daylight_Handler : MonoBehaviour
{

    public static Daylight_Handler Instance { get; private set; }


    [SerializeField] private Light2D globalLight;
    public float dayDuration = 60f;

    [Space(20)]

    [SerializeField] private Gradient colorOverDay;

    [Space(10)]

    [SerializeField] private AnimationCurve intensityOverDay;

    public float currentTime = 0f;

    void Awake()
    {
        // Singleton pattern setup
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }


    void Update()
    {
        if (globalLight == null || dayDuration <= 0f) return;

        // Progress time
        currentTime += Time.deltaTime;
        if (currentTime > dayDuration)
            currentTime -= dayDuration; // Loop the day cycle

        float timePercent = currentTime / dayDuration;

        // Update light color from gradient
        globalLight.color = colorOverDay.Evaluate(timePercent);

        // Update intensity from curve
        globalLight.intensity = intensityOverDay.Evaluate(timePercent);
    }

    // Optional: Call this to manually reset the time (e.g., for scene reloads)
    public void ResetDayCycle()
    {
        currentTime = 0f;
    }

    // Optional: Set time manually for cutscenes/testing
    public void SetTime(float newTime)
    {
        currentTime = Mathf.Clamp(newTime, 0f, dayDuration);
    }

    // Optional: Get normalized time of day (0 - 1)
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
}
