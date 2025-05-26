using System.Collections;
using UnityEngine;

public class Bake_TargetMover : MonoBehaviour
{
    [Header("Target Rotation Settings")]
    [SerializeField] private float rotationSpeed = 90f; // Degrees per second
    [SerializeField] private float pauseDuration = 1f;  // Time between rotations
    [SerializeField] private int sectionPadding = 5;

    [Header("Debug Info")]
    [SerializeField] private string targetSection; // A, B, or C
    [SerializeField] private int targetAngle;

    public int currentAngle { get; private set; } = 0;
    public bool isMoving { get; private set; } = false;

    private Coroutine movementCoroutine;

    private void OnEnable()
    {
        ResetRotation();
    }

    public void ResetRotation()
    {
        transform.localEulerAngles = new Vector3(0f, -180f, 0f);
        currentAngle = 0;
        targetAngle = 0;
        isMoving = false;

        if (movementCoroutine != null)
        {
            StopCoroutine(movementCoroutine);
        }
    }

    public void StartTargetMovement()
    {
        movementCoroutine = StartCoroutine(InitialMoveThenLoop());
    }

    private IEnumerator InitialMoveThenLoop()
    {
        // Immediate first move
        ChooseNewTarget();
        yield return RotateToTarget();

        // Then start looping normally
        while (true)
        {
            yield return new WaitForSeconds(pauseDuration);
            ChooseNewTarget();
            yield return RotateToTarget();
        }
    }

    private void ChooseNewTarget()
    {
        int currentSection = GetSection(currentAngle);
        int newSection;

        // Ensure a different section is chosen
        do
        {
            newSection = Random.Range(0, 3);
        } while (newSection == currentSection);

        // Save debug info
        targetSection = SectionName(newSection);

        int min = 0, max = 0;

        switch (newSection)
        {
            case 0: min = 0 + sectionPadding; max = 120 - sectionPadding; break;   // A
            case 1: min = 120 + sectionPadding; max = 240 - sectionPadding; break; // B
            case 2: min = 240 + sectionPadding; max = 360 - sectionPadding; break; // C
        }

        targetAngle = Random.Range(min, max + 1); // Inclusive of max
    }

    private IEnumerator RotateToTarget()
    {
        isMoving = true;
        float zRotation = currentAngle;

        while (Mathf.Abs(Mathf.DeltaAngle(zRotation, targetAngle)) > 0.1f)
        {
            float step = rotationSpeed * Time.deltaTime;

            if (targetAngle > zRotation)
                zRotation += step;
            else
                zRotation -= step;

            zRotation = Mathf.Clamp(zRotation, Mathf.Min(currentAngle, targetAngle), Mathf.Max(currentAngle, targetAngle));
            transform.localEulerAngles = new Vector3(0f, -180f, zRotation);
            yield return null;
        }

        zRotation = targetAngle;
        transform.localEulerAngles = new Vector3(0f, -180f, zRotation);
        currentAngle = Mathf.RoundToInt(zRotation);
        isMoving = false;
    }

    private int GetSection(int angle)
    {
        if (angle >= 0 + sectionPadding && angle < 120 - sectionPadding) return 0;   // A
        if (angle >= 120 + sectionPadding && angle < 240 - sectionPadding) return 1; // B
        return 2; // C
    }

    private string SectionName(int section)
    {
        switch (section)
        {
            case 0: return "A";
            case 1: return "B";
            case 2: return "C";
            default: return "?";
        }
    }

    public void SetImmediateAngle(int angle)
    {
        angle = Mathf.Clamp(angle, 0, 360);
        currentAngle = angle;
        targetAngle = angle;
        transform.localEulerAngles = new Vector3(0f, -180f, angle);
    }

    public IEnumerator RotateToAngle(int angle)
    {
        isMoving = true;
        float zRotation = currentAngle;

        while (Mathf.Abs(Mathf.DeltaAngle(zRotation, angle)) > 0.1f)
        {
            float step = rotationSpeed * Time.deltaTime;

            if (angle > zRotation)
                zRotation += step;
            else
                zRotation -= step;

            zRotation = Mathf.Clamp(zRotation, Mathf.Min(currentAngle, angle), Mathf.Max(currentAngle, angle));
            transform.localEulerAngles = new Vector3(0f, -180f, zRotation);
            yield return null;
        }

        zRotation = angle;
        transform.localEulerAngles = new Vector3(0f, -180f, zRotation);
        currentAngle = angle;
        targetAngle = angle;
        isMoving = false;
    }

    public void ResetTarget()
    {
        isMoving = false;
        targetAngle = 0;
        transform.rotation = Quaternion.Euler(0f, -180f, 0f); // Reset rotation visually
    }

}
