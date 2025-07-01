using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableAfterTask_Cooking : MonoBehaviour
{
    //This is meant to handle the condition of the scr

    public GameObject TargetObject;
    private bool CriteriaMet = false;

    [Header("Required Items (any one)")]
    public Item[] CriteriaItems;
    public int MinimumQTY = 1;

    [Space(20)]

    [Header("Needs Delay")]
    public bool DelayBeforeEnable;
    public float DelayDuration;

    private void Start()
    {
        TargetObject.SetActive(false);
    }

    private void Update()
    {
        if (!CriteriaMet)
        {
            CheckCriteria();
        }
        else
        {
            if (!TargetObject.activeSelf) // Prevent re-trigger
            {
                if (DelayBeforeEnable)
                    StartCoroutine(EnableAfterDelay());
                else
                    TargetObject.SetActive(true);
            }
        }
    }

    private void CheckCriteria()
    {
        foreach (var item in CriteriaItems)
        {
            int qty = InventoryManager.Instance.GetTotalQuantity(item);
            if (qty >= MinimumQTY)
            {
                CriteriaMet = true;
                break;
            }
        }
    }

    private IEnumerator EnableAfterDelay()
    {
        yield return new WaitForSeconds(DelayDuration);
        TargetObject.SetActive(true);
    }
}
