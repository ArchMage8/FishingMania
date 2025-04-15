using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableAfterTaskComplete : MonoBehaviour
{
    public GameObject TargetObject;
    private bool CriteriaMet = false;
    
    [Header("Required Items")]
    public Item CriteriaItem;
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
            if (DelayBeforeEnable == false)
            {
                TargetObject.SetActive(true);
            }

            else
            {
                StartCoroutine(EnableAfterDelay());
            }
        }
    }

    private void CheckCriteria()
    {
        int objectQuantity = InventoryManager.Instance.GetTotalQuantity(CriteriaItem);
        if(objectQuantity >= MinimumQTY)
        {
            CriteriaMet = true;
        }
    }

    private IEnumerator EnableAfterDelay()
    {
        yield return new WaitForSeconds(DelayDuration);
        TargetObject.SetActive(true);

    }
}
