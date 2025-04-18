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
    public bool LoadWithButton = true;

    [Space(10)]
    public GameObject F_Indicator;

    [Space(20)]
    [Header("Force Direction On Start")]

    [Tooltip("When you want Iris to face a certain direction ")]
    public bool ForceDirectionOnLoad;
    public FaceDirection selectedDirection;

    private bool TeleportRunning = false;
    private bool PlayerInRange = false;

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

        if (!LoadWithButton)
        {
            if(Input.GetKeyDown(KeyCode.F) && !TeleportRunning && PlayerInRange)
            {
                MovePlayer();
            }
        }
    }

    public void MovePlayer()
    {
        TeleportRunning = true;
        StartCoroutine(LoadSceneAndMovePlayer());
    }

    private IEnumerator LoadSceneAndMovePlayer()
    {
        InventoryManager.Instance.SomeUIEnabled = true;
        Time.timeScale = 1f;

        sceneLoader.SetTrigger("CloseScene");

        yield return new WaitForSeconds(1f);

        InventoryManager.Instance.SomeUIEnabled = false;

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
        CinemachineFramingTransposer transposer = null;

        if(virtualCamera != null)
        {
            transposer = virtualCamera.GetCinemachineComponent<CinemachineFramingTransposer>();
        }

        if (player != null)
        {
            player.transform.position = TargetPosition;
            if (transposer != null)
            {
                transposer.m_SoftZoneHeight = 0;
                transposer.m_SoftZoneWidth = 0;
                transposer.m_XDamping = 0;
                transposer.m_YDamping = 0;
            }
        }

        if (ForceDirectionOnLoad)
        {
            PlayerManager.instance.SnapWithoutTarget(selectedDirection.ToString());
        }

        yield return new WaitForSeconds(0.5f);

        if (transposer != null)
        {
            transposer.m_SoftZoneHeight = 0.23f;
            transposer.m_SoftZoneWidth = 0.23f;

            transposer.m_XDamping = 0.8f;
            transposer.m_YDamping = 0.8f;
        }

       

        Destroy(gameObject); // Now safe to remove this object
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !LoadWithButton)
        {
            if (F_Indicator != null)
            {
                F_Indicator.SetActive(true);
            }
            PlayerInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !LoadWithButton)
        {
            if (F_Indicator != null)
            {
                F_Indicator.SetActive(false);
            }
            PlayerInRange = false;
        }
    }

    public enum FaceDirection
    {
        N,
        NE,
        E,
        SE,
        S,
        SW,
        W,
        NW
    }
}
