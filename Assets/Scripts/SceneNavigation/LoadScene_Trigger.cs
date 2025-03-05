using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScene_Trigger : MonoBehaviour
{
    public GameObject F_Indicator;
    public int DestinationScene;
    public Animator sceneLoader;
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
        sceneLoader.SetTrigger("CloseScene");

        yield return new WaitForSeconds(1f);
        
        SceneManager.LoadScene(DestinationScene);
    }
}
