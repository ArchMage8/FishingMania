using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstantCloseUI : MonoBehaviour
{
    public GameObject ObjectToDisable;

    public void DisableObject()
    {
        ObjectToDisable.SetActive(false);
    }
}
