using System.Collections;
using UnityEngine;

public class Orbit : MonoBehaviour
{
    [Header("Position Values")]
    public Transform pivot;
    public GameObject startPoint;

    [Header("Rotation Settings")]
    public float speed = 90f;
    public bool clockwork = true;
    public bool isOrbiting = true; // Controls whether the object is moving

    [Space(20)]
    public ColorManager colorManager;

    private float currentAngle;
    public float radius;
    private float startAngle;
    private int posPercentage;
    private int Score;

    void Start()
    {
        

        Vector3 direction = startPoint.transform.position - pivot.position;
        startAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        currentAngle = startAngle;
        UpdatePosition();
    }

    private void OnEnable()
    {
        isOrbiting = true;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (isOrbiting)
            {
                StopOrbit();
                colorManager.target = GetPositionPercentage();
                colorManager.SetScore();
                Score = colorManager.Score;

                FishingCoreSystems.instance.StartWaitPhase(Score);
                StartCoroutine(DisableDelay());

                isOrbiting = false;
            }
        }
        
        if (isOrbiting)
        {
          OrbitMovement();
        }

    }

    private IEnumerator DisableDelay()
    {
        yield return new WaitForSeconds(1f);
        FishingCoreSystems.instance.CastMinigame.SetActive(false);

    }

    void OrbitMovement()
    {
        float directionMultiplier = clockwork ? -1f : 1f;
        currentAngle += directionMultiplier * speed * Time.deltaTime;
        currentAngle = Mathf.Repeat(currentAngle, 360f);

        UpdatePosition();
        RotateObject();

        posPercentage = GetPositionPercentage();
        //Debug.Log("Position Percentage: " + posPercentage);
    }

    void UpdatePosition()
    {
        float rad = currentAngle * Mathf.Deg2Rad;
        Vector3 offset = new Vector3(Mathf.Cos(rad), Mathf.Sin(rad), 0) * radius;
        transform.position = pivot.position + offset;
    }

    void RotateObject()
    {
        Vector2 direction = (transform.position - pivot.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    int GetPositionPercentage()
    {
        float angleFromStart = (currentAngle - startAngle + 360f) % 360f;
        float percentage = (angleFromStart / 360f) * 100f;

        return clockwork ? Mathf.RoundToInt(100 - percentage) : Mathf.RoundToInt(percentage);
    }

    public void StopOrbit()
    {
        isOrbiting = false;
        colorManager.target = posPercentage;
        colorManager.SetScore();
    }

    public void StartOrbit()
    {
        isOrbiting = true;
    }
}
