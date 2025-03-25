using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine;

public class TransitionOnDisable : MonoBehaviour
{
    public TransitionCaller caller;

    private void OnDisable()
    {
        caller.LoadNextScene();
    }

}
