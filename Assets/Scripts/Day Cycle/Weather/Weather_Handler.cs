using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Weather_Handler : MonoBehaviour
{
    public static Weather_Handler Instance;

    public enum WeatherType { Sunny, Rainy }
    public WeatherType CurrentWeather { get; private set;}
    public bool IsSunny;
    public bool IsRainy;



    [Header("Weather Odds & Streaks")]
    [Range(0f, 1f)] public float sunnyChance = 0.6f;
    private int sunnyStreak = 0;
    private int rainyStreak = 0;

    // Scene-specific particle system references
    private List<ParticleSystem> sunnyParticles = new List<ParticleSystem>();
    private List<ParticleSystem> rainParticles = new List<ParticleSystem>();

    private ParticleSystem MainRain;

    private void Awake()
    {   
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Persist through scene changes
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        HandleSunnyParticles();
    }

    public void ResetWeather() //Called on day reset
    {
        WeatherType nextWeather = DetermineNextWeather();

        if (nextWeather == WeatherType.Sunny)
        {
            sunnyStreak++;
            rainyStreak = 0;
        }
        else
        {
            rainyStreak++;
            sunnyStreak = 0;
        }

        CurrentWeather = nextWeather;

        IsSunny = (CurrentWeather == WeatherType.Sunny);
        IsRainy = (CurrentWeather == WeatherType.Rainy);

        

        ApplyWeatherVisuals(CurrentWeather);
        var emission = MainRain.emission;
        emission.rateOverTime = 0;

        if (CurrentWeather == WeatherType.Rainy)
        {
            MainRain.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
            MainRain.Play();
            StartCoroutine(RainFadeIn(MainRain, 10f, 45f));
        }
    }

    private WeatherType DetermineNextWeather()
    {
        if (sunnyStreak >= 6)
            return WeatherType.Rainy;

        if (rainyStreak >= 2)
            return WeatherType.Sunny;

        float roll = Random.value;
        return (roll <= sunnyChance) ? WeatherType.Sunny : WeatherType.Rainy;
    }

    private void HandleSunnyParticles()
    {
        if (Daylight_Handler.Instance == null) return;

        float hour = Daylight_Handler.Instance.GetCurrentHour();
        bool shouldPlay = CurrentWeather == WeatherType.Sunny && hour >= 10f && hour < 18f;

        foreach (var ps in sunnyParticles)
        {
            if (ps == null) continue;

            if (shouldPlay && !ps.isPlaying)
                ps.Play();
            else if (!shouldPlay && ps.isPlaying)
                ps.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        }
    }

    private void PlayRainParticles()
    {
        foreach (var ps in rainParticles)
        {
            if (ps != null && !ps.isPlaying)
                ps.Play();
        }
    }

    private void StopRainParticles()
    {
        foreach (var ps in rainParticles)
        {
            if (ps != null && ps.isPlaying)
                ps.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        }
    }

    private void StopSunnyParticles()
    {
        foreach (var ps in sunnyParticles)
        {
            if (ps != null && ps.isPlaying)
                ps.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        }
    }

    public void RegisterSceneParticles(Weather_ParticlesHolder holder)
    {
        StartCoroutine(ApplyWeatherAfterDelay(holder));
    }

    private IEnumerator ApplyWeatherAfterDelay(Weather_ParticlesHolder holder)
    {
        yield return null; // Wait one frame to ensure async scene is fully loaded

        sunnyParticles.Clear();
        rainParticles.Clear();

        if (holder != null)
        {
            sunnyParticles.AddRange(holder.sunnyParticles);
            rainParticles.AddRange(holder.rainSplashes);

            MainRain = holder.MainRain;

            ApplyWeatherVisuals(CurrentWeather);
        }
        else
        {
            Debug.Log("No Weather_ParticlesHolder found in this scene. Skipping weather visuals.");
        }
    }

    private void ApplyWeatherVisuals(WeatherType type)
    {
        if (type == WeatherType.Rainy)
        {
            PlayRainParticles();
            StopSunnyParticles();
        }
        else
        {
            StopRainParticles();
          
        }
    }

    private IEnumerator RainFadeIn(ParticleSystem system, float duration, float maxEmission)
    {
        var emission = system.emission;
          
        emission.rateOverTime = 0;

        float elapsed = 0f;

        while (elapsed < duration)
        {
            float t = elapsed / duration;
            float currentRate = Mathf.Lerp(0, maxEmission, t);
            emission.rateOverTime = currentRate;

            elapsed += Time.deltaTime;
            yield return null;
        }

        emission.rateOverTime = maxEmission;
    }
}
