using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using Cinemachine;

public class LoadWithDestination : MonoBehaviour
{
    public Vector3 TargetPosition;
    public int DestinationScene;

    public Animator sceneLoader;

    private void Start()
    {
        GameObject parent = this.transform.parent.gameObject;

        DontDestroyOnLoad(parent); // Ensures this object persists during scene load
        DontDestroyOnLoad(this);
    }

    public void MovePlayer()
    {
        StartCoroutine(LoadSceneAndMovePlayer());
    }

    private IEnumerator LoadSceneAndMovePlayer()
    {

        sceneLoader.SetTrigger("CloseScene");

        yield return new WaitForSeconds(1f);

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(DestinationScene);

        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        yield return new WaitForSeconds(0.1f); // Ensures scene fully initializes before accessing objects

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        CinemachineVirtualCamera virtualCamera;

        virtualCamera = FindObjectOfType<CinemachineVirtualCamera>();
        CinemachineFramingTransposer transposer = virtualCamera.GetCinemachineComponent<CinemachineFramingTransposer>();
        if (player != null)
        {
            player.transform.position = TargetPosition;
            transposer.m_SoftZoneHeight = 0;
            transposer.m_SoftZoneWidth = 0;
            transposer.m_XDamping = 0;
            transposer.m_YDamping = 0;
        }

        yield return new WaitForSeconds(0.5f);

        transposer.m_SoftZoneHeight = 0.23f;
        transposer.m_SoftZoneWidth = 0.23f;

        transposer.m_XDamping = 0.8f;
        transposer.m_YDamping = 0.8f;
        Destroy(gameObject); // Now safe to remove this object
    }
}
