using UnityEngine;

public class Particle_FollowPlayer : MonoBehaviour
{
    public Transform player;
    public Vector3 offset = Vector3.zero; // Offset set to zero by default

    void LateUpdate()
    {
        if (player != null)
        {
            transform.position = player.position + offset;
        }
    }
}
