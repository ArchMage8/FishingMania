using UnityEngine;
using TMPro;

public class Daylight_UI : MonoBehaviour
{
    [Header("Clock Components")]
    public TextMeshProUGUI hoursText;
    public TextMeshProUGUI minutesText;

    private Daylight_Handler daylight;

    private void Start()
    {
        daylight = Daylight_Handler.Instance;
    }

    private void Update()
    {
        if (daylight == null || daylight.GetDayDuration() <= 0f)
        {
            return;
        }

        UpdateClockDisplay();
    }

    private void UpdateClockDisplay()
    {
        float dayDuration = daylight.GetDayDuration();
        float currentTime = daylight.GetCurrentTime();

        float secondsPerHour = dayDuration / 24f;
        float totalHours = currentTime / secondsPerHour;

        int hour = Mathf.FloorToInt(totalHours);
        int rawMinute = Mathf.FloorToInt((totalHours - hour) * 60f);
        int roundedMinute = (rawMinute / 5) * 5;

        if (hoursText != null)
        {
            hoursText.text = hour.ToString("D2");
        }

        if (minutesText != null)
        {
            minutesText.text = roundedMinute.ToString("D2");
        }
    }
}
