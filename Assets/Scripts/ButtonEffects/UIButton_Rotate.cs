using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


//Documentation:
//The job of this script is to handle if we want to make a button move (visually) when hovered on

//Requirements:
//- Script is simple so if the button has multiple states (like inventory delete), dont use this
//- Adapt the logic into the script that handles the states

//- A separate "visual object" is the one being rotated not the button or its hitbox
//- The script however is attached to the button
//- Remember to ensure all positions (before/after rotate) are still within the button hitbox


public class UIButton_Rotate : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject Visual;
    public float target_Rotation = -10;
    public GameObject HoverEffect;

    private float initial_Rotation;
    private RectTransform RT;


    private void Start()
    {
        RT = Visual.GetComponent<RectTransform>();
        initial_Rotation = this.GetComponent<RectTransform>().localEulerAngles.z;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!FlickerProtect)
        {
            if(HoverEffect != null)
            {
                HoverEffect.SetActive(true);
            }

            RT.rotation = Quaternion.Euler(0, 0, target_Rotation);   
            StartCoroutine(FlickerProtect_Toggle());
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (HoverEffect != null)
        {
            HoverEffect.SetActive(false);
        }

        if (!FlickerProtect)
        {
            RT.rotation = Quaternion.Euler(0, 0, initial_Rotation);
            StartCoroutine(FlickerProtect_Toggle());
        }
    }

    private bool FlickerProtect = false;
    private IEnumerator FlickerProtect_Toggle()
    {
        //Debug.Log("Call");

        FlickerProtect = true;
        yield return new WaitForSeconds(0.001f);
        FlickerProtect = false;

    }
}
