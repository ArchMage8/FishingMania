using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoNotDestroy : MonoBehaviour
{
    private void Awake()
    {
        // Check if another instance of this object already exists
        foreach (var obj in FindObjectsOfType<DoNotDestroy>())
        {
            if (obj != this && obj.gameObject.name == gameObject.name)
            {
                Destroy(gameObject);
                return;
            }
        }

        // Persist this object across scenes
        DontDestroyOnLoad(gameObject);
    }
}
