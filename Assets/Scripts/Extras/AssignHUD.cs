using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssignHUD : MonoBehaviour
{
    //Documentation:
    //The inventory manager needs to exists in all scenes
    //But the HUD does not

    //So if we are going to start the game from the tutorial
    //Where the HUD isn't there in the first 3 scenes

    //This script's job is to assign the HUD to the inventory manager
    //Which has existed from scene 1

    public GameObject HUD;

    private void Awake()
    {

        
        StartCoroutine(Setup());

    }

    private IEnumerator Setup()
    {
        yield return new WaitForSeconds(0.1f);
        Debug.Log("Call");
        InventoryManager.Instance.HUD = HUD;
    }
}
