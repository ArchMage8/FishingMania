using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NPC_Movement : MonoBehaviour
{
    public enum Direction { NE, SE, SW, NW }

    [System.Serializable]
    public class RouteCheckpoint
    {
        public float distance;
        public Direction direction;
        public float waitTime;
    }

    public List<RouteCheckpoint> route = new List<RouteCheckpoint>();

    [Range(0f, 90f)]
    public float angleDegrees = 22.7f;

    public float moveSpeed = 1f;

    private Vector2 origin;
    private Vector2 currentDestination;
    private int routeIndex = 0;
    private bool isReturning = false;
    private List<Vector2> visitedPositions = new List<Vector2>();

    private Animator animator;

    void Awake()
    {
        origin = transform.position;
        animator = GetComponent<Animator>();
        StartCoroutine(FollowRoute());
    }

    IEnumerator FollowRoute()
    {
        visitedPositions.Clear();
        routeIndex = 0;
        isReturning = false;

        while (routeIndex < route.Count)
        {
            Vector2 startPosition = transform.position;
            RouteCheckpoint checkpoint = route[routeIndex];
            currentDestination = CalculateDestination(startPosition, checkpoint);
            visitedPositions.Add(startPosition);

            yield return StartCoroutine(MoveTo(currentDestination, checkpoint));
            yield return new WaitForSeconds(checkpoint.waitTime);

            routeIndex++;
        }

        Vector2 finalPosition = transform.position;
        if (Vector2.Distance(finalPosition, origin) < 0.1f)
        {
            yield return new WaitForSeconds(route[route.Count - 1].waitTime);
            StartCoroutine(FollowRoute()); // Restart route
        }
        else
        {
            StartCoroutine(ReturnToOrigin());
        }
    }

    IEnumerator ReturnToOrigin()
    {
        isReturning = true;

        for (int i = visitedPositions.Count - 1; i >= 0; i--)
        {
            if (i < route.Count)
            {
                yield return StartCoroutine(MoveTo(visitedPositions[i], route[i]));
                yield return new WaitForSeconds(route[i].waitTime);
            }
            else
            {
                yield return StartCoroutine(MoveTo(visitedPositions[i], new RouteCheckpoint
                {
                    direction = Direction.SW,
                    distance = 0f,
                    waitTime = 0f
                }));
            }
        }

        Debug.Log("Returned to origin.");
    }

    IEnumerator MoveTo(Vector2 target, RouteCheckpoint checkpoint)
    {
        int directionCode = GetDirectionCode(checkpoint.direction);
        ProcessDirectionCode(directionCode);

        while (Vector2.Distance(transform.position, target) > 0.01f)
        {
            transform.position = Vector2.MoveTowards(transform.position, target, moveSpeed * Time.deltaTime);
            yield return null;
        }

        transform.position = target;
    }

    Vector2 CalculateDestination(Vector2 origin, RouteCheckpoint checkpoint)
    {
        float angleRad = angleDegrees * Mathf.Deg2Rad;

        float offsetX = Mathf.Cos(angleRad) * checkpoint.distance;
        float offsetY = Mathf.Sin(angleRad) * checkpoint.distance;

        switch (checkpoint.direction)
        {
            case Direction.NE: break;
            case Direction.SE: offsetY *= -1; break;
            case Direction.SW: offsetX *= -1; offsetY *= -1; break;
            case Direction.NW: offsetX *= -1; break;
        }

        return origin + new Vector2(offsetX, offsetY);
    }

    int GetDirectionCode(Direction dir)
    {
        switch (dir)
        {
            case Direction.NE: return 1;
            case Direction.SE: return 2;
            case Direction.NW: return 3;
            case Direction.SW: return 4;
            default: return 0;
        }
    }

    void ProcessDirectionCode(int code)
    {
        Debug.Log($"Direction Code: {code}");

        if (code == 1) // NE
        {
            animator.SetFloat("Horizontal", 1);
            animator.SetFloat("Vertical", 1);
        }
        else if (code == 2) // SE
        {
            animator.SetFloat("Horizontal", 1);
            animator.SetFloat("Vertical", -1);
        }
        else if (code == 3) // NW
        {
            animator.SetFloat("Horizontal", -1);
            animator.SetFloat("Vertical", 1);
        }
        else if (code == 4) // SW
        {
            animator.SetFloat("Horizontal", -1);
            animator.SetFloat("Vertical", -1);
        }
    }
}
