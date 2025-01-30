using UnityEngine;

public class NPC_Child : MonoBehaviour
{
    [Header("Mode References")]
    public GameObject defaultMode; // Reference to the default mode child
    public GameObject fullMode;    // Reference to the full mode child

    /// <summary>
    /// Enables the default mode and disables the full mode.
    /// </summary>
    public void EnableDefault()
    {
        defaultMode.SetActive(true);
        fullMode.SetActive(false);
    }

    /// <summary>
    /// Enables the full mode and disables the default mode.
    /// </summary>
    public void EnableFull()
    {
        defaultMode.SetActive(false);
        fullMode.SetActive(true);
    }

    /// <summary>
    /// Disables both modes.
    /// </summary>
    public void DisableAll()
    {
        defaultMode.SetActive(false);
        fullMode.SetActive(false);
    }
}
