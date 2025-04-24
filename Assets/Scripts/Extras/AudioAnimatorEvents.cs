using UnityEngine;
using System.Collections;

public class AudioAnimatorEvents : MonoBehaviour
{
    [Header("Audio Clip Settings")]
    public AudioSource audioSourceToPlay;
    public AudioClip clipToPlay;

   
    public void PlayAudioClip()
    {
        
        if (audioSourceToPlay != null && clipToPlay != null)
        {
            audioSourceToPlay.PlayOneShot(clipToPlay);
        }
        else
        {
            Debug.LogWarning("Missing audio source or clip.");
        }
    }

    
    
    public void FadeOutAudio()
    {
        StartCoroutine(FadeOutCoroutine(audioSourceToPlay, 2f));
    }

    private IEnumerator FadeOutCoroutine(AudioSource source, float duration)
    {
        if (source == null) yield break;

        float startVolume = source.volume;

        float time = 0f;
        while (time < duration)
        {
            time += Time.deltaTime;
            source.volume = Mathf.Lerp(startVolume, 0f, time / duration);
            yield return null;
        }

        source.volume = 0f;
        source.Stop();
    }
}
