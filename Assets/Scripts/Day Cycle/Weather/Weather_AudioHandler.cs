using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class Weather_AudioHandler : MonoBehaviour
{
    public int MaxVolume;
    public int IndoorVolume = 2;
    public AudioSource weatherAudioSource;

    private Coroutine volumeCoroutine;

    private void Awake()
    { 

        if (weatherAudioSource != null)
        {
            MaxVolume = Mathf.RoundToInt(weatherAudioSource.volume * 10f);
        }

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        InterruptVolume();
    }

    private void InterruptVolume()
    {
        
        StopAllCoroutines();

        Debug.Log("aa");

        if (Weather_Handler.Instance.IsRainy)
        {
            if (Weather_ParticlesHolder.Instance != null) //Outdoors
            {
                FadeToVolume(MaxVolume, 2f);
            }

            else if (Weather_ParticlesHolder.Instance == null) //Indoors
            {
                FadeToVolume(IndoorVolume, 2f);
            }
        }

        else
        {
            FadeToVolume(0, 0f);
            return;
        }
    }

    public void FadeToVolume(int targetVolume, float duration)
    {
        if (weatherAudioSource == null) return;

        if (volumeCoroutine != null)
            StopCoroutine(volumeCoroutine);

        volumeCoroutine = StartCoroutine(FadeVolumeCoroutine(targetVolume, duration));
    }

    private IEnumerator FadeVolumeCoroutine(int targetVolume, float duration)
    {
        float startVolume = weatherAudioSource.volume;
        float endVolume = targetVolume / 10f;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            float t = elapsed / duration;
            weatherAudioSource.volume = Mathf.Lerp(startVolume, endVolume, t);
            elapsed += Time.deltaTime;
            yield return null;
        }

        weatherAudioSource.volume = endVolume;
    }
}
