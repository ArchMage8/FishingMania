using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boil_TargetHandler : MonoBehaviour
{

    public List<RectTransform> sections;

    [Space(10)]

    public RectTransform target;

    [Space(10)]

    [Header("Movement Settings")]
    public float moveSpeed = 500f;
    public float minWaitTime = 1f;
    public float maxWaitTime = 3f;

    [Space(10)]

    [Header("Idle Bounce Settings")]
    public float bounceAmplitude = 5f; // How far left/right it bounces
    public float bounceSpeed = 4f;      // How fast it bounces

    [HideInInspector] public bool IsTargetIdle = false;

    private int currentSectionIndex = -1;
    private Coroutine bounceRoutine;

    public void BeginTargetMovement()
    {
        if (sections.Count < 2)
        {
            Debug.LogError("You need at least 2 sections for the target to move between.");
            return;
        }

        StartCoroutine(TargetMovementRoutine());
    }

    private IEnumerator TargetMovementRoutine()
    {
        while (true)
        {
            int nextSectionIndex = GetNextSectionIndex();
            Vector2 destination = sections[nextSectionIndex].anchoredPosition;

            // Disable idle state and stop bouncing
            IsTargetIdle = false;
            if (bounceRoutine != null)
            {
                StopCoroutine(bounceRoutine);
                bounceRoutine = null;
            }

            // Move to the new target section
            yield return StartCoroutine(MoveTarget(destination));
            currentSectionIndex = nextSectionIndex;

            // Enter idle state and start bouncing
            IsTargetIdle = true;
            bounceRoutine = StartCoroutine(BounceWhileIdle(target.anchoredPosition));

            // Get wait time between moves
            float waitTime = Random.Range(minWaitTime, maxWaitTime);

            // If wait time is zero, still yield once so IsTargetIdle is detectable for at least 1 frame
            if (waitTime <= 0f)
            {
                yield return null;
            }
            else
            {
                yield return new WaitForSeconds(waitTime);
            }

            // Stop bouncing before moving again
            if (bounceRoutine != null)
            {
                StopCoroutine(bounceRoutine);
                bounceRoutine = null;
                target.anchoredPosition = sections[currentSectionIndex].anchoredPosition;
            }
        }
    }



    private IEnumerator MoveTarget(Vector3 destination)
    {
        while (Vector3.Distance(target.anchoredPosition, destination) > 0.1f)
        {
            target.anchoredPosition = Vector3.MoveTowards(target.anchoredPosition, destination, moveSpeed * Time.deltaTime);
            yield return null;
        }

        target.anchoredPosition = destination;
    }

    private int GetNextSectionIndex()
    {
        int nextIndex;
        do
        {
            nextIndex = Random.Range(0, sections.Count);
        }
        while (nextIndex == currentSectionIndex);

        return nextIndex;
    }

    private IEnumerator BounceWhileIdle(Vector3 basePosition)
    {
        float timer = 0f;

        while (true)
        {
            float offsetX = Mathf.Sin(timer * bounceSpeed) * bounceAmplitude;
            target.anchoredPosition = basePosition + new Vector3(offsetX, 0f, 0f);

            timer += Time.deltaTime;
            yield return null;
        }
    }

    public void ResetTarget()
    {
        StopAllCoroutines();
        IsTargetIdle = false;

        // You can also re-center the target or reinitialize internal logic here if needed
    }

    public void StopAllTargetMovement()
    {
        StopAllCoroutines(); // stop move + bounce

        if (bounceRoutine != null)
        {
            StopCoroutine(bounceRoutine);
            bounceRoutine = null;
        }

        IsTargetIdle = false;

        // Optionally reset the target to its current section position
        //if (currentSectionIndex >= 0 && currentSectionIndex < sections.Count)
        //{
        //    target.anchoredPosition = sections[currentSectionIndex].anchoredPosition;
        //}

    }
}
