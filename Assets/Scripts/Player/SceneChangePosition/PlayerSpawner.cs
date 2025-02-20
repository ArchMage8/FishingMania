using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    public Transform defaultSpawnPoint; // Assign a default spawn point in the Main Scene

    private void Start()
    {
        if (TransitionPositionManager.Instance.HasSavedPosition())
        {
            transform.position = TransitionPositionManager.Instance.GetExitPosition();
        }
        else
        {
            transform.position = defaultSpawnPoint.position; // Default spawn if no saved position
        }
    }
}
