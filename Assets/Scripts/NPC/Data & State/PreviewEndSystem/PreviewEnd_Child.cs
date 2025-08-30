using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class PreviewEnd_Child : MonoBehaviour
{

    public Animator sceneLoader;
    public int DestinationScene;


    private void Start()
    {
        // If manager exists and preview not complete, run CompletePreview
        if (PreviewEnd_Manager.Instance != null && PreviewEnd_Manager.Instance.isPreviewComplete == false)
        {
            PreviewEnd_Manager.Instance.isPreviewComplete = true;
            CompletePreview();
        }
    }

    private void CompletePreview()
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
