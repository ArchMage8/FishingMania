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

    private List<ParticleSystem> sunnyParticles = new List<ParticleSystem>();
    private List<ParticleSystem> rainParticles = new List<ParticleSystem>();
    private List<ParticleSystem> rainSystems = new List<ParticleSystem>();
    private Dictionary<ParticleSystem, float> rainMaxEmissions = new Dictionary<ParticleSystem, float>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
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

        if (CurrentWeather == WeatherType.Rainy)
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
        else // transitioning to sunny
        {
            StartCoroutine(RainFadeOutMultiple(rainSystems, 10f));
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
        yield return null;

        sunnyParticles.Clear();
        rainParticles.Clear();
        rainSystems.Clear();
        rainMaxEmissions.Clear();

        if (holder != null)
        {
            sunnyParticles.AddRange(holder.sunnyParticles);

            foreach (var ps in holder.rainSplashes)
            {
                if (ps != null)
                {
                    rainParticles.Add(ps);
                    rainSystems.Add(ps);

                    var emission = ps.emission;
                    rainMaxEmissions[ps] = emission.rateOverTime.constant;
                }
            }

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

    private IEnumerator RainFadeInMultiple(List<ParticleSystem> systems, float duration)
    {
        float elapsed = 0f;

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