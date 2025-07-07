using UnityEngine;
using System.Collections;

public class NPC_Schedule : MonoBehaviour
{
    [Header("NPC Schedule Time Range (24hr format)")]
    [Range(0f, 24f)] public float startHour = 13.5f; // e.g., 13:30
    [Range(0f, 24f)] public float endHour = 17f;

    [Header("NPC Components")]
    public GameObject npcVisual;
    public GameObject npcShadow;
    public GameObject dialogueTrigger;
    public Collider2D npcCollider;

    [Header("Fade Settings")]
    public float fadeDuration = 1f;

    private bool isPresent = false;
    private Coroutine fadeCoroutine;

    private SpriteRenderer visualRenderer;
    private SpriteRenderer shadowRenderer;

    private void Start()
    {
        visualRenderer = npcVisual.GetComponent<SpriteRenderer>();
        shadowRenderer = npcShadow.GetComponent<SpriteRenderer>();

        UpdatePresenceImmediate();
    }
    private void OnEnable()
    {
        UpdatePresenceImmediate();
    }
    private void Update()
    {
        float currentHour = Daylight_Handler.Instance.GetCurrentHour();
        bool shouldBePresent = IsWithinSchedule(currentHour);

        if (shouldBePresent != isPresent)
        {
            isPresent = shouldBePresent;

            if (fadeCoroutine != null)
                StopCoroutine(fadeCoroutine);

            fadeCoroutine = StartCoroutine(isPresent ? EnterNPC() : ExitNPC());
        }
    }

    private bool IsWithinSchedule(float hour)
    {
        // Handles both standard and looping (overnight) time ranges
        if (startHour <= endHour)
        {
            return hour >= startHour && hour <= endHour;
        }
        else
        {
            return hour >= startHour || hour <= endHour;
        }
    }

    private IEnumerator ExitNPC()
    {
        dialogueTrigger.SetActive(false);

        yield return StartCoroutine(FadeNPC(1f, 0f));

        if (npcCollider != null)
            npcCollider.enabled = false;
    }

    private IEnumerator EnterNPC()
    {
        if (npcCollider != null)
            npcCollider.enabled = true;

        yield return StartCoroutine(FadeNPC(0f, 1f));

        dialogueTrigger.SetActive(true);
    }

    private IEnumerator FadeNPC(float fromAlpha, float toAlpha)
    {
        float timer = 0f;

        while (timer < fadeDuration)
        {
            float t = timer / fadeDuration;
            float currentAlpha = Mathf.Lerp(fromAlpha, toAlpha, t);

            SetAlpha(currentAlpha);

            timer += Time.deltaTime;
            yield return null;
        }

        SetAlpha(toAlpha);
    }

    private void SetAlpha(float alpha)
    {
        if (visualRenderer != null)
        {
            Color c = visualRenderer.color;
            c.a = alpha;
            visualRenderer.color = c;
        }

        if (shadowRenderer != null)
        {
            Color c = shadowRenderer.color;

            // Shadow alpha fades from 0 when invisible to 60/255 when visible
            float shadowAlpha = Mathf.Lerp(0f, 60f / 255f, alpha);
            c.a = shadowAlpha;
            shadowRenderer.color = c;
        }
    }


    private void UpdatePresenceImmediate()
    {
        float currentHour = Daylight_Handler.Instance.GetCurrentHour();
        isPresent = IsWithinSchedule(currentHour);

        SetAlpha(isPresent ? 1f : 0f);

        if (npcCollider != null)
            npcCollider.enabled = isPresent;

        dialogueTrigger.SetActive(isPresent);
    }
}
