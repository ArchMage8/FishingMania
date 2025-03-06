using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using Cinemachine;

public class LoadWithDestination : MonoBehaviour
{

    [Header("Teleport Information")]
    public Vector3 TargetPosition;
    public int DestinationScene;

    [Header("Teleport Settings")]
    public Animator sceneLoader;
    public bool NoButton = false;

    private bool TeleportRunning;

    private void Awake()
    {
        Debug.Log("Awake is called");
    }

    private void Start()
    {
        TeleportRunning = false;

        Debug.Log("Start is called");

        DontDestroyOnLoad(this);
    }

    private void Update()
    {
        if (NoButton && Input.GetKeyDown(KeyCode.Escape) && !TeleportRunning)
        {
            TeleportRunning = true;
            MovePlayer();
        }
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
            TeleportRunning = true;
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
