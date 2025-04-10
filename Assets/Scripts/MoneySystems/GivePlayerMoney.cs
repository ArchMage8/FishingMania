using System.Collections;
using TMPro;
using UnityEngine;

public class GivePlayerMoney : MonoBehaviour
{
    [Header("UI References")]
    public TMP_Text Earnings_Preview;
    public TMP_Text Expenses_Preview;
    public TMP_Text Income_Preview;

    [Space(20)]

    [Header("Preview Card")]
    public GameObject PreviewCard;

    private int Today_Income;
    private Animator previewAnimator;

    private void Start()
    {
        if (PreviewCard != null)
        {
            PreviewCard.SetActive(false);
            previewAnimator = PreviewCard.GetComponent<Animator>();
        }
        else
        {
            Debug.LogWarning("PreviewCard reference not set.");
        }
    }

    public void ShowSummary()
    {
        if (PreviewCard == null || previewAnimator == null) return;

        PreviewCard.SetActive(true);

        int earnings = MoneyManager.Instance.Today_Earnings;
        int expenses = MoneyManager.Instance.Today_Expenses;
        Today_Income = earnings - expenses;

        if (Earnings_Preview != null)
            Earnings_Preview.text = $"Earnings: {earnings}";

        if (Expenses_Preview != null)
            Expenses_Preview.text = $"Expenses: {expenses}";

        if (Income_Preview != null)
            Income_Preview.text = $"Net Income: {Today_Income}";
    }

    public void CloseSummary()
    {
        if (previewAnimator != null)
        {
            previewAnimator.SetTrigger("Exit");
            StartCoroutine(HideAfterDelay(1.0f)); // Adjust delay time to match exit animation
        }
    }

    private IEnumerator HideAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        PreviewCard.SetActive(false);
    }
}
