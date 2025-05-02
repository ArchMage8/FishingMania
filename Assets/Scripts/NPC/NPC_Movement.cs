using UnityEngine;

public class NPC_Movement : MonoBehaviour
{
    public float distance = 1f;
    [Range(0f, 90f)]
    public float angleDegrees = 22.7f;
    public float moveSpeed = 1f;

    public Direction direction = Direction.NE;

    private Vector2 destination;
    private bool isMoving = false;

    public enum Direction
    {
        NE,
        SE,
        SW,
        NW
    }

    void Awake()
    {
        Vector2 origin = transform.position;
        destination = CalculateDestination(origin, distance, angleDegrees, direction);
        isMoving = true;
    }

    void Update()
    {
        if (isMoving)
        {
            MoveTowardsDestination();
        }
    }

    void MoveTowardsDestination()
    {
        Vector2 currentPosition = transform.position;
        Vector2 newPosition = Vector2.MoveTowards(currentPosition, destination, moveSpeed * Time.deltaTime);
        transform.position = newPosition;

        if (Vector2.Distance(newPosition, destination) < 0.01f)
        {
            transform.position = destination; // Snap to final position
            isMoving = false;
            Debug.Log("NPC reached destination: " + destination);
        }
    }

    Vector2 CalculateDestination(Vector2 origin, float distance, float angleDegrees, Direction direction)
    {
        float angleRad = angleDegrees * Mathf.Deg2Rad;

        float offsetX = Mathf.Cos(angleRad) * distance;
        float offsetY = Mathf.Sin(angleRad) * distance;

        switch (direction)
        {
            case Direction.NE: break;
            case Direction.SE: offsetY *= -1; break;
            case Direction.SW: offsetX *= -1; offsetY *= -1; break;
            case Direction.NW: offsetX *= -1; break;
        }

        return origin + new Vector2(offsetX, offsetY);
    }
}
