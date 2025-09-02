using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Weather_Handler : MonoBehaviour
{
    public static Weather_Handler Instance;

    public enum WeatherType { Sunny, Rainy }
    public WeatherType CurrentWeather { get; private set; }
    public bool IsSunny;
    public bool IsRainy;

    [Header("Weather Odds & Streaks")]
    [Range(0f, 1f)] public float sunnyChance = 0.6f;
    private int sunnyStreak = 0;
    private int rainyStreak = 0;

    public Weather_AudioHandler audioHandler;

    private List<ParticleSystem> sunnyParticles = new List<ParticleSystem>();
    private List<ParticleSystem> rainSystems = new List<ParticleSystem>();
    private Dictionary<ParticleSystem, float> rainMaxEmissions = new Dictionary<ParticleSystem, float>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            // Default weather on first load
            CurrentWeather = WeatherType.Sunny;
            IsSunny = true;
            IsRainy = false;
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

    public void ResetWeather()
    {
        WeatherType prevWeather = CurrentWeather;   // Track old state
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

        // --- Handle transitions ---
        if (prevWeather == WeatherType.Rainy && CurrentWeather == WeatherType.Sunny)
        {
            // Rain → Sunny
            StartCoroutine(RainFadeOutMultiple(rainSystems, 10f));
        }
        else if (prevWeather == WeatherType.Sunny && CurrentWeather == WeatherType.Rainy)
        {
            // Sunny → Rain
            if (Weather_ParticlesHolder.Instance != null)
            {
                foreach (var ps in rainSystems)
                {
                    var emission = ps.emission;
                    emission.rateOverTime = 0;
                    ps.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
                    ps.Play();
                }

                StartCoroutine(RainFadeInMultiple(rainSystems, 10f));
            }
            else
            {
                audioHandler.FadeToVolume(audioHandler.IndoorVolume, 10f);
            }
        }
        // else Sunny → Sunny OR Rainy → Rainy → do nothing
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

    public void RegisterSceneParticles(Weather_ParticlesHolder holder)
    {
        StartCoroutine(ApplyWeatherAfterDelay(holder));
    }

    private IEnumerator ApplyWeatherAfterDelay(Weather_ParticlesHolder holder)
    {
        yield return null;

        sunnyParticles.Clear();
        rainSystems.Clear();
        rainMaxEmissions.Clear();

        if (holder != null)
        {
            sunnyParticles.AddRange(holder.sunnyParticles);

            foreach (var ps in holder.rainSplashes)
            {
                if (ps != null)
                {
                    rainSystems.Add(ps);
                    var emission = ps.emission;
                    rainMaxEmissions[ps] = emission.rateOverTime.constant;
                }
            }

            // Ensure visuals match current weather
            if (CurrentWeather == WeatherType.Rainy)
            {
                foreach (var ps in rainSystems)
                {
                    var emission = ps.emission;
                    emission.rateOverTime = rainMaxEmissions[ps];
                    if (!ps.isPlaying) ps.Play();
                }
            }
            else
            {
                foreach (var ps in rainSystems)
                {
                    var emission = ps.emission;
                    emission.rateOverTime = 0f;
                    ps.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
                }
            }
        }
        else
        {
            Debug.Log("No Weather_ParticlesHolder found in this scene. Skipping weather visuals.");
        }
    }

    private IEnumerator RainFadeInMultiple(List<ParticleSystem> systems, float duration)
    {
        float elapsed = 0f;

        if (Weather_ParticlesHolder.Instance != null)
        {
            audioHandler.FadeToVolume(audioHandler.MaxVolume, duration);
        }
        else
        {
            audioHandler.FadeToVolume(audioHandler.IndoorVolume, duration);
        }

        while (elapsed < duration)
        {
            float t = elapsed / duration;

            foreach (var ps in systems)
            {
                if (rainMaxEmissions.TryGetValue(ps, out float maxEmission))
                {
                    var emission = ps.emission;
                    emission.rateOverTime = Mathf.Lerp(0f, maxEmission, t);
                }
            }

            elapsed += Time.deltaTime;
            yield return null;
        }

        foreach (var ps in systems)
        {
            if (rainMaxEmissions.TryGetValue(ps, out float maxEmission))
            {
                var emission = ps.emission;
                emission.rateOverTime = maxEmission;
            }
        }
    }

    private IEnumerator RainFadeOutMultiple(List<ParticleSystem> systems, float duration)
    {
        float elapsed = 0f;
        audioHandler.FadeToVolume(0, duration);

        while (elapsed < duration)
        {
            float t = elapsed / duration;

            foreach (var ps in systems)
            {
                if (rainMaxEmissions.TryGetValue(ps, out float maxEmission))
                {
                    var emission = ps.emission;
                    emission.rateOverTime = Mathf.Lerp(maxEmission, 0f, t);
                }
            }

            elapsed += Time.deltaTime;
            yield return null;
        }

        foreach (var ps in systems)
        {
            var emission = ps.emission;
            emission.rateOverTime = 0f;
            ps.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        }
    }
}
