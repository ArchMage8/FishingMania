using UnityEngine;
using System.Collections;

public class Bake_TargetMover : MonoBehaviour
{
    [Header("UI Target Settings")]
    public RectTransform targetRectTransform; // The UI element that rotates

    [Header("Rotation Settings")]
    public float rotationSpeed = 90f; // degrees per second
    public float waitTime = 2f;       // seconds to wait before moving again
    public int padding = 5;           // degrees trimmed from section ends

    [Header("State")]
    public int currentAngle;         // int angle (0 - 360)
    public bool isMoving = false;

    private int targetAngle;
    private Section currentSection;

    private enum Section { A, B, C }

    private void Start()
    {
        if (targetRectTransform == null)
        {
            Debug.LogError("Bake_TargetMover: targetRectTransform is not assigned.");
            enabled = false;
            return;
        }

        currentAngle = Mathf.RoundToInt(targetRectTransform.eulerAngles.z);
        currentSection = GetSectionFromAngle(currentAngle);
        StartCoroutine(MoveTargetRoutine());
    }

    private void Update()
    {
        if (!isMoving || targetRectTransform == null)
            return;

        float step = rotationSpeed * Time.deltaTime;
        float newAngle = (currentAngle + step) % 360;

        if (AngleReachedOrPassed(currentAngle, targetAngle, step))
        {
            newAngle = targetAngle;
            isMoving = false;
            currentSection = GetSectionFromAngle(newAngle);
            StartCoroutine(MoveTargetRoutine());
        }

        targetRectTransform.rotation = Quaternion.Euler(0f, 0f, newAngle);
        currentAngle = Mathf.RoundToInt(targetRectTransform.eulerAngles.z);
    }

    private IEnumerator MoveTargetRoutine()
    {
        yield return new WaitForSeconds(waitTime);

        Section newSection;
        do
        {
            newSection = (Section)Random.Range(0, 3);
        } while (newSection == currentSection);

        targetAngle = GetRandomAngleInSection(newSection, padding);
        isMoving = true;
    }

    private Section GetSectionFromAngle(float angle)
    {
        if (angle >= 0 && angle < 120) return Section.A;
        else if (angle >= 120 && angle < 240) return Section.B;
        else return Section.C;
    }

    private int GetRandomAngleInSection(Section section, int padding)
    {
        int start = 0, end = 0;
        switch (section)
        {
            case Section.A:
                start = 0 + padding;
                end = 120 - padding;
                break;
            case Section.B:
                start = 120 + padding;
                end = 240 - padding;
                break;
            case Section.C:
                start = 240 + padding;
                end = 360 - padding;
                break;
        }

        return Random.Range(start, end);
    }

    private bool AngleReachedOrPassed(float current, float target, float step)
    {
        float normalizedCurrent = current % 360;
        float normalizedTarget = target % 360;

        if (normalizedCurrent <= normalizedTarget)
            return normalizedCurrent + step >= normalizedTarget;
        else // wraparound case
            return (normalizedCurrent + step) % 360 >= normalizedTarget;
    }
}
