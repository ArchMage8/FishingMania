using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TransitionCaller : MonoBehaviour
{
    public Animator sceneLoader;
    public int DestinationScene;

    public void LoadNextScene()
    {
        StartCoroutine(LoadScene());
    }

    private IEnumerator LoadScene()
    {
        InventoryManager.Instance.SomeUIEnabled = true;
        sceneLoader.SetTrigger("CloseScene");

        yield return new WaitForSeconds(1f);

        InventoryManager.Instance.SomeUIEnabled = false;
        SceneManager.LoadScene(DestinationScene);
    }
}
