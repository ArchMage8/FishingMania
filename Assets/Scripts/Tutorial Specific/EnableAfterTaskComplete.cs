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
            TargetObject.SetActive(true);
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
}
