using UnityEngine;
using UnityEngine.Rendering.Universal;

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

        currentTime += Time.deltaTime;
        if (currentTime > dayDuration)
            currentTime -= dayDuration;

        float timePercent = currentTime / dayDuration;

        globalLight.color = colorOverDay.Evaluate(timePercent);
        globalLight.intensity = intensityOverDay.Evaluate(timePercent);
    }

    public void ResetDayCycle()
    {
        currentTime = 0f;
    }

    public void SetTime(float newTime)
    {
        currentTime = Mathf.Clamp(newTime, 0f, dayDuration);
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
}
