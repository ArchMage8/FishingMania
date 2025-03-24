using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine;

public class TransitionOnDisable : MonoBehaviour
{
    public Animator sceneLoader;
    public int DestinationScene;

    private void OnDisable()
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
