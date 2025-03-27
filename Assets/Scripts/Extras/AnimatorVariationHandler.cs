using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorVariationHandler : MonoBehaviour
{
    private Animator TargetAnimator;
    public bool isVariation;

    private void Start()
    {
        TargetAnimator = GetComponent<Animator>();

        if (TargetAnimator == null)
        {
            Debug.LogError(this.gameObject.name + "'s animator is missing");
        }

        HandleVariation();
    }

    private void HandleVariation()
    {
        if (isVariation == true)
        {
            TargetAnimator.SetTrigger("Variation");
        }

        else if (isVariation == false) 
        {
            TargetAnimator.SetTrigger("Default");
        }
    }

}
