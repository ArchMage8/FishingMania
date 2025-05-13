using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI; // Required for Slider

public class Boil_Minigame : MonoBehaviour
{
    [Header("References")]
    public RectTransform marker;               // Player-controlled marker
    public RectTransform bar;                  // Horizontal bar defining bounds

    [Space(10)]
    public RectTransform target;               // Moving target from Boil_TargetHandler
    public Boil_TargetHandler targetHandler;   // Reference to target handler

    [Header("Movement Settings")]
    public float markerSpeed = 400f;

    [Header("Progress Settings")]
    public float dwellTimeRequired = 1f;       // Time to stay in target before progress starts

    [Space(10)]

    public float currentProgress = 0f;
    public float maxProgress = 100f;

    [Space(10)]

    [Header("Progress Fill Rate")]
    
    public float Fill_Value = 10f;
    public float Time_Value = 1f;

    [Space(10)]

    [Header("UI")]
    public Slider progressBar;                // UI progress bar

    private RectTransform barRect;
    private bool isInTarget = false;
    private Coroutine dwellRoutine;
    private Coroutine progressRoutine;
    private bool hasCompleted = false;

    void Start()
    {
        barRect = bar.GetComponent<RectTransform>();
        InitializeProgressBar();  // Separate method for slider initialization
    }

    void Update()
    {
        if (hasCompleted) return;

        HandleMarkerMovement();
        CheckIfInsideTarget();
    }

    private void InitializeProgressBar()
    {
        if (progressBar != null)
        {
            progressBar.minValue = 0;
            progressBar.maxValue = maxProgress;
            progressBar.value = currentProgress;
        }
    }

    private void HandleMarkerMovement()
    {
        float input = Input.GetAxisRaw("Horizontal"); // A/D or Arrow Keys
        if (Mathf.Approximately(input, 0f)) return;

        Vector3 newPosition = marker.position + new Vector3(input * markerSpeed * Time.deltaTime, 0f, 0f);

        // Clamp marker position within bar
        float leftBound = bar.position.x - (bar.rect.width * 0.5f);
        float rightBound = bar.position.x + (bar.rect.width * 0.5f);
        newPosition.x = Mathf.Clamp(newPosition.x, leftBound, rightBound);

        marker.position = newPosition;
    }

    private void CheckIfInsideTarget()
    {
        bool currentlyInside = IsMarkerInsideTarget();

        if (currentlyInside && targetHandler.IsTargetIdle)
        {
            if (!isInTarget)
            {
                isInTarget = true;

                if (dwellRoutine == null)
                    dwellRoutine = StartCoroutine(ProgressWhenInsideTarget());
            }
        }
        else
        {
            isInTarget = false;

            if (dwellRoutine != null)
            {
                StopCoroutine(dwellRoutine);
                dwellRoutine = null;
            }

            if (progressRoutine != null)
            {
                StopCoroutine(progressRoutine);
                progressRoutine = null;
            }
        }
    }

    private bool IsMarkerInsideTarget()
    {
        float markerX = marker.position.x;

        float targetLeft = target.position.x - (target.rect.width * 0.5f);
        float targetRight = target.position.x + (target.rect.width * 0.5f);

        return markerX >= targetLeft && markerX <= targetRight;
    }

    private IEnumerator ProgressWhenInsideTarget()
    {
        float timer = 0f;

        while (timer < dwellTimeRequired)
        {
            if (!IsMarkerInsideTarget() || !targetHandler.IsTargetIdle)
                yield break;

            timer += Time.deltaTime;
            yield return null;
        }

        // Dwell complete — start making progress
        progressRoutine = StartCoroutine(MakeProgress());
        dwellRoutine = null;
    }

    private IEnumerator MakeProgress()
    {
        while (true)
        {
            currentProgress += Fill_Value;
            Debug.Log("Progress: " + currentProgress);

            progressBar.value = currentProgress;
            
            if (currentProgress >= maxProgress)
            {
                CompleteMinigame();
                yield break;
            }

            yield return new WaitForSeconds(Time_Value);
        }
    }

    private void CompleteMinigame()
    {
        hasCompleted = true;

        // Stop target movement
        if (targetHandler != null)
        {
            targetHandler.enabled = false;
        }

        // Optionally stop marker visually or disable completely
        Debug.Log("Minigame Complete!");

        // You can trigger UI, sound, or next step here
    }
}
