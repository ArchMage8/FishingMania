using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class LoadWithDestination : MonoBehaviour
{
    public Vector3 TargetPosition;
    public int DestinationScene;

    private void Start()
    {
        DontDestroyOnLoad(gameObject); // Ensures this object persists during scene load
    }

    public void MovePlayer()
    {
        StartCoroutine(LoadSceneAndMovePlayer());
    }

    private IEnumerator LoadSceneAndMovePlayer()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(DestinationScene);
        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        yield return new WaitForSeconds(0.1f); // Ensures scene fully initializes before accessing objects

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            player.transform.position = TargetPosition;
        }

        Destroy(gameObject); // Now safe to remove this object
    }
}
