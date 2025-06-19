using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIButton_TempDisable : MonoBehaviour
{
    //Documentation
    //The script's job is to basically turn of a UI button when some other UI is active

    private Button button;
    private Image image;

    private void Start()
    {
        button = GetComponent<Button>();
        image = GetComponent<Image>();
    }

    public void Update()
    {
        if (InventoryManager.Instance.SomeUIEnabled)
        {
            button.enabled = false;
            image.enabled = false;
        }

        else
        {
            button.enabled = true;
            image.enabled = true;
        }
    }


}
