using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitForTransition : MonoBehaviour
{
   public void TransitionStart()
   {
        InventoryManager.Instance.SomeUIEnabled = true;
   }

    public void TransitionCompleted()
    {
        InventoryManager.Instance.SomeUIEnabled = false;
    }
}
