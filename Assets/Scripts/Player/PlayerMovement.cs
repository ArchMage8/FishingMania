using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    [Range(0, 90)] public float diagonalAngle = 45f; // Adjustable diagonal movement angle
    public Animator animator;
    public bool canMove = true;
    private bool isMoving;
    [HideInInspector] public float animX, animY;
    private int directionInt;

    private Rigidbody2D rb;
    private Vector2 moveDirection;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        animX = 1;
        animY = -1;
            
    }

    void Update()
    {
        ProcessInput();
        UpdateAnimator();
        UpdateDirection();
    }

    void FixedUpdate()
    {
        MovePlayer();
    }

    void ProcessInput()
    {
        if (!canMove)
        {
            isMoving = false;
            return;
        }

        float horizontal = 0f;
        float vertical = 0f;

        if (Input.GetKey(KeyCode.D)) horizontal = 1f;
        else if (Input.GetKey(KeyCode.A)) horizontal = -1f;

        if (Input.GetKey(KeyCode.W)) vertical = 1f;
        else if (Input.GetKey(KeyCode.S)) vertical = -1f;

        moveDirection = new Vector2(horizontal, vertical).normalized;

        if (horizontal != 0 && vertical != 0)
        {
            float angleRad = diagonalAngle * Mathf.Deg2Rad;
            float adjustedX = Mathf.Cos(angleRad) * horizontal;
            float adjustedY = Mathf.Sin(angleRad) * vertical;
            moveDirection = new Vector2(adjustedX, adjustedY).normalized;
        }

        isMoving = moveDirection != Vector2.zero;

        if (isMoving)
        {
            animX = horizontal;
            animY = vertical;
        }
    }

    void MovePlayer()
    {
        rb.MovePosition(rb.position + moveDirection * moveSpeed * Time.fixedDeltaTime);
    }

    void UpdateAnimator()
    {
        animator.SetFloat("X", animX);
        animator.SetFloat("Y", animY);
        animator.SetBool("IsMoving", isMoving);
    }

    void UpdateDirection()
    {
        if (animX == 0 && animY == 1) directionInt = 1; // North
        else if (animX == 1 && animY == 1) directionInt = 2; // NE
        else if (animX == 1 && animY == 0) directionInt = 3; // East
        else if (animX == 1 && animY == -1) directionInt = 4; // SE
        else if (animX == 0 && animY == -1) directionInt = 5; // South
        else if (animX == -1 && animY == -1) directionInt = 6; // SW
        else if (animX == -1 && animY == 0) directionInt = 7; // West
        else if (animX == -1 && animY == 1) directionInt = 8; // NW
    }
}
