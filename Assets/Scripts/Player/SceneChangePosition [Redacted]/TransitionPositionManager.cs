using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionPositionManager : MonoBehaviour
{
    public static TransitionPositionManager Instance { get; private set; }

    private Vector3 lastExitPosition;
    private bool hasSavedPosition = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Keep across scenes
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetExitPosition(Vector3 position)
    {
        lastExitPosition = position;
        hasSavedPosition = true;
    }

    public Vector3 GetExitPosition()
    {
        return lastExitPosition;
    }

    public bool HasSavedPosition()
    {
        return hasSavedPosition;
    }
}
