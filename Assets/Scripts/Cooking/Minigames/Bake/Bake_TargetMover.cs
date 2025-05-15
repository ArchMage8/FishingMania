using UnityEngine;
using System.Collections;

public class Bake_TargetMover : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform targetTransform; // Object that rotates

    [Header("Rotation Settings")]
    [SerializeField] private float rotationSpeed = 60f; // Degrees per second
    [SerializeField] private float waitTimeBetweenMoves = 2f;
    [SerializeField] private int sectionPadding = 5; // Degrees trimmed from each end of a section

    [Header("Runtime Debug")]
    public int currentAngle = 0;              // Current Z rotation (0–359)
    public bool isMoving = false;             // Whether target is rotating
    public Section currentSection = Section.A; // Current section target is in

    public enum Section { A, B, C }

    private void Start()
    {
        SetInitialRotation();
        StartCoroutine(MoveTargetRoutine());
    }

    private void SetInitialRotation()
    {
        currentAngle = GetRandomAngleInSection(currentSection);
        ApplyRotation(currentAngle);
    }

    private IEnumerator MoveTargetRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(waitTimeBetweenMoves);
            Section nextSection = GetNextSection(currentSection);
            int nextAngle = GetRandomAngleInSection(nextSection);
            isMoving = true;
            yield return StartCoroutine(RotateToAngleClockwise(currentAngle, nextAngle));
            currentAngle = nextAngle;
            currentSection = nextSection;
            isMoving = false;
        }
    }

    private IEnumerator RotateToAngleClockwise(int fromAngle, int toAngle)
    {
        float current = fromAngle;
        float target = toAngle;

        // Always move clockwise, wrap if needed
        if (target <= current)
            target += 360f;

        while (current < target)
        {
            current += rotationSpeed * Time.deltaTime;
            float displayAngle = current % 360f;
            currentAngle = Mathf.RoundToInt(displayAngle);
            targetTransform.rotation = Quaternion.Euler(0f, 180f, displayAngle);
            yield return null;
        }

        // Final snap
        currentAngle = toAngle % 360;
        targetTransform.rotation = Quaternion.Euler(0f, 180f, currentAngle);
    }

    private int GetRandomAngleInSection(Section section)
    {
        int min = 0, max = 0;
        switch (section)
        {
            case Section.A:
                min = 0 + sectionPadding;
                max = 120 - sectionPadding;
                break;
            case Section.B:
                min = 120 + sectionPadding;
                max = 240 - sectionPadding;
                break;
            case Section.C:
                min = 240 + sectionPadding;
                max = 360 - sectionPadding;
                break;
        }
        return Random.Range(min, max);
    }

    private Section GetNextSection(Section current)
    {
        Section[] options = current switch
        {
            Section.A => new[] { Section.B, Section.C },
            Section.B => new[] { Section.A, Section.C },
            Section.C => new[] { Section.A, Section.B },
            _ => new[] { Section.A }
        };
        return options[Random.Range(0, options.Length)];
    }

    private void ApplyRotation(int angle)
    {
        targetTransform.rotation = Quaternion.Euler(0f, 180f, angle);
    }
}
