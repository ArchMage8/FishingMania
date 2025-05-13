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

   [HideInInspector] public bool IsTargetIdle  = false;

    private int currentSectionIndex = -1;
    private Coroutine bounceRoutine;

    void Start()
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
            Vector3 destination = sections[nextSectionIndex].position;

            IsTargetIdle = false;
            if (bounceRoutine != null)
            {
                StopCoroutine(bounceRoutine);
                bounceRoutine = null;
            }

            yield return StartCoroutine(MoveTarget(destination));
            currentSectionIndex = nextSectionIndex;

            IsTargetIdle = true;
            bounceRoutine = StartCoroutine(BounceWhileIdle(target.position));

            float waitTime = Random.Range(minWaitTime, maxWaitTime);
            yield return new WaitForSeconds(waitTime);

            // Stop bouncing before moving again
            if (bounceRoutine != null)
            {
                StopCoroutine(bounceRoutine);
                bounceRoutine = null;
                target.position = sections[currentSectionIndex].position;
            }
        }
    }

    private IEnumerator MoveTarget(Vector3 destination)
    {
        while (Vector3.Distance(target.position, destination) > 0.1f)
        {
            target.position = Vector3.MoveTowards(target.position, destination, moveSpeed * Time.deltaTime);
            yield return null;
        }

        target.position = destination;
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
            target.position = basePosition + new Vector3(offsetX, 0f, 0f);

            timer += Time.deltaTime;
            yield return null;
        }
    }
}
