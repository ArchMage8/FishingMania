using UnityEngine;

public class Daylight_Exception : MonoBehaviour
{
    public static Daylight_Exception Instance { get; private set; }

    public bool implementDaylight = true;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }
}
