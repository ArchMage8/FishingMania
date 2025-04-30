using UnityEngine;

public class NPC_Movement : MonoBehaviour
{
    public enum DiagonalDirection
    {
        NorthEast,
        SouthEast,
        SouthWest,
        NorthWest
    }

    public DiagonalDirection direction = DiagonalDirection.NorthEast;
    public float distance = 5f;
    public float moveSpeed = 5f;
    [Range(0, 90)] public float diagonalAngle = 45f;

    private Rigidbody2D rb;
    private Vector2 startPoint;
    private Vector2 targetPoint;
    private bool isMoving = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        startPoint = rb.position;

        // Calculate movement direction based on selected diagonal angle
        float angleRad = diagonalAngle * Mathf.Deg2Rad;
        float x = Mathf.Cos(angleRad);
        float y = Mathf.Sin(angleRad);

        Vector2 moveDirection = Vector2.zero;

        switch (direction)
        {
            case DiagonalDirection.NorthEast:
                moveDirection = new Vector2(x, y);
                break;
            case DiagonalDirection.SouthEast:
                moveDirection = new Vector2(x, -y);
                break;
            case DiagonalDirection.SouthWest:
                moveDirection = new Vector2(-x, -y);
                break;
            case DiagonalDirection.NorthWest:
                moveDirection = new Vector2(-x, y);
                break;
        }

        moveDirection.Normalize();
        targetPoint = startPoint + moveDirection * distance;
        isMoving = true;
    }

    void FixedUpdate()
    {
        if (!isMoving) return;

        Vector2 currentPosition = rb.position;
        Vector2 toTarget = (targetPoint - currentPosition).normalized;

        // Apply the same Y adjustment as in your player script
        Vector2 adjustedMove = new Vector2(toTarget.x, toTarget.y * 0.75f);
        Vector2 newPosition = currentPosition + adjustedMove * moveSpeed * Time.fixedDeltaTime;

        // If we're close enough to the target, snap and stop
        if (Vector2.Distance(newPosition, targetPoint) <= 0.05f)
        {
            rb.MovePosition(targetPoint);
            isMoving = false;
        }
        else
        {
            rb.MovePosition(newPosition);
        }
    }
}
