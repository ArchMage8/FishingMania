using UnityEngine;
using System.Collections.Generic;

public class Weather_Handler : MonoBehaviour
{
    public static Weather_Handler Instance;

    public enum WeatherType { Sunny, Rainy }
    public WeatherType CurrentWeather { get; private set; }

    [Header("Streak Tracking")]
    private int sunnyStreak = 0;
    private int rainyStreak = 0;

    [Header("Weather Odds")]
    [Range(0f, 1f)] public float sunnyChance = 0.6f;

    //[Header("Sunny Particle Systems")]
    [SerializeField] private List<ParticleSystem> sunnyParticles = new List<ParticleSystem>();

    //[Header("Rain Particle Systems")]
    [SerializeField] private List<ParticleSystem> rainParticles = new List<ParticleSystem>();

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
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

        if (CurrentWeather == WeatherType.Rainy)
        {
            PlayRainParticles();
            StopSunnyParticles(); // Stop in case it was active
        }
        else
        {
            StopRainParticles();
            // Sunny particles handled by time in Update()
        }

        //Debug.Log($"New Day Weather: {CurrentWeather} (Sunny: {sunnyStreak}, Rainy: {rainyStreak})");
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
}
