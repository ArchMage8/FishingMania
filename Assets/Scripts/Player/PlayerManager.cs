using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance;

    //public Transform target; // The target object
    public PlayerMovement playerMovement;

    private float AnimatorX;
    private float AnimatorY;

    private void Awake()
    {
        instance = this;
    }

    public void SnapDirection(Transform target)
    {
        if (target == null)
            return;
      
        Vector2 direction = ((Vector2)target.position - (Vector2)transform.position).normalized;

        string directionName = GetIsometricDirection(direction);

        if (directionName == "N") 
        {
            AnimatorX = 0;
            AnimatorY = 1;
        }
        else if (directionName == "NE") 
        {
            AnimatorX = 1;
            AnimatorY = 1;
        }
        else if (directionName == "E")
        {
            AnimatorX = 1;
            AnimatorY = 0;
        }
        else if (directionName == "SE") 
        {
            AnimatorX = 1;
            AnimatorY = -1;
        }
        else if (directionName == "S")
        {
            AnimatorX = 0;
            AnimatorY = -1;
        }
        else if (directionName == "SW") 
        {
            AnimatorX = -1;
            AnimatorY = -1;
        }
        else if (directionName == "W") 
        {
            AnimatorX = -1;
            AnimatorY = 0;
        }
        else if (directionName == "NW")
        {
            AnimatorX = -1;
            AnimatorY = 1;
        }
        else
        {
            Debug.Log("Invalid Direction");
        }

        SetAnimation();
    }

    public void SnapWithoutTarget(string directionName)
    {
        if (directionName == "N")
        {
            AnimatorX = 0;
            AnimatorY = 1;
        }
        else if (directionName == "NE")
        {
            AnimatorX = 1;
            AnimatorY = 1;
        }
        else if (directionName == "E")
        {
            AnimatorX = 1;
            AnimatorY = 0;
        }
        else if (directionName == "SE")
        {
            AnimatorX = 1;
            AnimatorY = -1;
        }
        else if (directionName == "S")
        {
            AnimatorX = 0;
            AnimatorY = -1;
        }
        else if (directionName == "SW")
        {
            AnimatorX = -1;
            AnimatorY = -1;
        }
        else if (directionName == "W")
        {
            AnimatorX = -1;
            AnimatorY = 0;
        }
        else if (directionName == "NW")
        {
            AnimatorX = -1;
            AnimatorY = 1;
        }
        else
        {
            Debug.Log("Invalid Direction");
        }

        SetAnimation();
    }

    private void SetAnimation()
    {
        //playerMovement.canMove = false;
        playerMovement.animX = AnimatorX;
        playerMovement.animY = AnimatorY;
    }

    string GetIsometricDirection(Vector2 direction)
    {
        // Convert the direction vector to an angle in degrees
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // Normalize the angle to a 0-360 degree range
        angle = (angle + 360) % 360;

        if (angle >= 350 || angle < 10)
            return "E";
        if (angle >= 10 && angle < 80)
            return "NE";
        if (angle >= 80 && angle < 100)
            return "N";
        if (angle >= 100 && angle < 170)
            return "NW";
        if (angle >= 170 && angle < 190)
            return "W";
        if (angle >= 190 && angle < 260)
            return "SW";
        if (angle >= 260 && angle < 280)
            return "S";
        if (angle >= 280 && angle < 350)
            return "SE";

        return "Unknown";
    }
}
