using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Boil_Minigame : MonoBehaviour
{
    [Header("References")]
    public RectTransform marker;
    public RectTransform bar;

    [Space(10)]
    public RectTransform target;
    public Boil_TargetHandler targetHandler;

    [Header("Movement Settings")]
    public float markerSpeed = 400f;

    [Header("Progress Settings")]
    public float dwellTimeRequired = 1f;
    public float currentProgress = 0f;
    public float maxProgress = 100f;

    [Header("Progress Fill Rate")]
    public float Fill_Value = 10f;
    public float Time_Value = 1f;

    [Header("Failure Settings")]
    public float failureTimeLimit = 30f; // Gameplay timer
    private float remainingTime;
    private Coroutine failureTimerRoutine;

    [Header("Pregame Settings")]
    public float pregameMoveDistance = 100f;
    public float pregameMoveDuration = 0.5f;
    public float pregameCountdownDuration = 3f;

    [Header("UI")]
    public Slider progressBar;
    public TextMeshProUGUI timerText;

    private int lastDisplayedTime = -1;
    private bool isInTarget = false;
    private Coroutine dwellRoutine;
    private Coroutine progressRoutine;
    private bool hasCompleted = false;
    private bool isPregame = true;

    void OnEnable()
    {
        StartCoroutine(Start_WithAnimation());
    }

    void OnDisable()
    {
        ResetVisuals();
    }

    private IEnumerator Start_WithAnimation()
    {
        yield return new WaitForSeconds(0.5f);
        StartCoroutine(PregameSequence());
    }

    void Update()
    {

        if (hasCompleted || isPregame)
        {
            
            return;

        }

        HandleMarkerMovement();
        CheckIfInsideTarget();
    }

    private void HandleMarkerMovement()
    {
        float input = Input.GetAxisRaw("Horizontal");
        if (Mathf.Approximately(input, 0f)) return;

        Vector2 newAnchoredPos = marker.anchoredPosition + new Vector2(input * markerSpeed * Time.deltaTime, 0f);

        float halfBarWidth = bar.rect.width * 0.5f;
        float leftBound = bar.anchoredPosition.x - halfBarWidth;
        float rightBound = bar.anchoredPosition.x + halfBarWidth;
        newAnchoredPos.x = Mathf.Clamp(newAnchoredPos.x, leftBound, rightBound);

        marker.anchoredPosition = newAnchoredPos;

    }

    private void CheckIfInsideTarget()
    {
        bool currentlyInside = IsMarkerInsideTarget();
        Debug.Log($"CurrentlyInside: {currentlyInside}, IsTargetIdle: {targetHandler.IsTargetIdle}");

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
        float targetLeft = target.position.x - (target.rect.width * target.lossyScale.x * 0.5f);
        float targetRight = target.position.x + (target.rect.width * target.lossyScale.x * 0.5f);


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

        progressRoutine = StartCoroutine(MakeProgress());
        dwellRoutine = null;
    }

    private IEnumerator MakeProgress()
    {
        while (true)
        {
            currentProgress += Fill_Value;

            if (progressBar != null)
                StartCoroutine(UpdateProgressBarSmooth(progressBar.value, currentProgress));

            if (currentProgress >= maxProgress)
            {
                CompleteMinigame(true);
                yield break;
            }

            yield return new WaitForSeconds(Time_Value);
        }
    }

    private IEnumerator UpdateProgressBarSmooth(float from, float to)
    {
        float duration = 0.3f;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            progressBar.value = Mathf.Lerp(from, to, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        progressBar.value = to;
    }

    private IEnumerator FailureTimer()
    {
        while (remainingTime > 0f)
        {
            if ((int)remainingTime != lastDisplayedTime)
            {
                lastDisplayedTime = (int)remainingTime;
                if (timerText != null)
                {
                    timerText.text = lastDisplayedTime.ToString();
                }
            }

            yield return new WaitForSeconds(1f);
            remainingTime--;

            if (remainingTime <= 0f && currentProgress < maxProgress)
            {
                if (timerText != null)
                {
                    timerText.text = "0";
                }

                CompleteMinigame(false);
            }
        }
    }

    private IEnumerator PregameSequence()
    {
        

        isPregame = true;
        hasCompleted = false;

        Start_Minigame();

        Vector2 origin = target.anchoredPosition;
        Vector2 right = origin + Vector2.right * pregameMoveDistance;
        Vector2 left = origin - Vector2.right * pregameMoveDistance;

        yield return MoveTargetTo(right);
        yield return MoveTargetTo(left);
        yield return MoveTargetTo(origin);

        float countdown = pregameCountdownDuration;
        while (countdown > 0)
        {
            timerText.text = Mathf.CeilToInt(countdown).ToString();
            yield return new WaitForSeconds(1f);
            countdown -= 1f;
        }

        timerText.text = ((int)failureTimeLimit).ToString();
        isPregame = false;

        targetHandler.enabled = true;

        if (targetHandler != null)
        {
            
            targetHandler.BeginTargetMovement();
        }

        failureTimerRoutine = StartCoroutine(FailureTimer());
    }

    private IEnumerator MoveTargetTo(Vector2 targetPos)
    {
        Vector2 start = target.anchoredPosition;
        float elapsed = 0f;

        while (elapsed < pregameMoveDuration)
        {
            target.anchoredPosition = Vector2.Lerp(start, targetPos, elapsed / pregameMoveDuration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        target.anchoredPosition = targetPos;
    }

    public void Start_Minigame()
    {
        currentProgress = 0f;

        if (progressBar != null)
        {
            progressBar.minValue = 0;
            progressBar.maxValue = maxProgress;
            progressBar.value = 0f;
        }

        remainingTime = failureTimeLimit;
        lastDisplayedTime = -1;

  

        if (failureTimerRoutine != null)
            StopCoroutine(failureTimerRoutine);

        // Center marker and target relative to bar
        Vector2 barCenter = bar.anchoredPosition;
        marker.anchoredPosition = new Vector2(barCenter.x, marker.anchoredPosition.y);
        target.anchoredPosition = new Vector2(barCenter.x, target.anchoredPosition.y);

        if (targetHandler != null)
        {
            targetHandler.ResetTarget();
            targetHandler.enabled = false;
        }
    }

    private void CompleteMinigame(bool success)
    {

        StopMinigame();
        hasCompleted = true;

        targetHandler.StopAllTargetMovement();

        if (success)
        {
            Debug.Log("Success");
            Cooking_Minigame_Manager.Instance.CookingMinigameComplete();
        }

        else
        {
            Debug.Log("Fail");
            Cooking_Minigame_Manager.Instance.LoseHealth();
            Cooking_Minigame_Manager.Instance.CookingMinigameComplete();
        }
    }

    public void StopMinigame()
    {
        if (hasCompleted)
            return;

        hasCompleted = true;

        // Disable player input by marking minigame as done
        isPregame = false;

        // Stop all ongoing coroutines
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

        if (failureTimerRoutine != null)
        {
            StopCoroutine(failureTimerRoutine);
            failureTimerRoutine = null;
        }

        // Stop the target from moving
        if (targetHandler != null)
            targetHandler.enabled = false;
    }

    private void ResetVisuals()
    {
        if (progressBar != null)
        {
            progressBar.value = 0f;
        }

        if (timerText != null)
        {
            timerText.text = "3";
        }
    }

}
