using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScene_Trigger : MonoBehaviour
{
    
    public int DestinationScene;

    [Space(20)]

    public Animator sceneLoader;
    public GameObject F_Indicator;
    private bool playerInRange = false;

    private void Start()
    {
        if (F_Indicator != null)
        {
            F_Indicator.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            if (F_Indicator != null)
            {
                F_Indicator.SetActive(true);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            if (F_Indicator != null)
            {
                F_Indicator.SetActive(false);
            }
        }
    }

    private void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.F))
        {
            StartCoroutine(LoadScene());
        }
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
