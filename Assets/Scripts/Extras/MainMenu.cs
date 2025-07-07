using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public Animator sceneFade;

    public void LoadSceneByIndex(int sceneIndex)
    {
        sceneFade.SetTrigger("CloseScene");
        StartCoroutine(LoadAfterEffect(sceneIndex));
    }

    private IEnumerator LoadAfterEffect(int sceneIndex)
    {
        yield return new WaitForSeconds(0.8f);
        SceneManager.LoadScene(sceneIndex);
    }
}
