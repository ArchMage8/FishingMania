using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct FishingSpotStock
{
    public string SpotName;
    public int FishStock;
}

public class FishingSpotManager : MonoBehaviour
{
    //Updated 09/04/2025 -> Darryl

    //Due to system oversight, this script has been introduced
    //This script's main job is to store a list of ints representing the stock of fish of a fishing spot

    //The reason why the script is introduced is to allow the stock values to remain persistent
    //Whereas the old system relied on FishCoreSystem script to be persistent
    //Which doesn't work as the it references several UI elements unique to a fishing spot

    

    public FishingSpotStock[] fishingSpots;
    public static FishingSpotManager instance;

    private void Awake()
    {
        instance = this;
    }

    public int CheckFishStock(string fishingSpotName)
    {
        foreach (FishingSpotStock spot in fishingSpots)
        {
            if (spot.SpotName == fishingSpotName)
            {
                return spot.FishStock;
            }
        }

        Debug.LogWarning($"Fishing spot '{fishingSpotName}' not found.");
        return -1; // or another sentinel value to indicate not found
    }

    public void ResetFishStocks()
    {
        for (int i = 0; i < fishingSpots.Length; i++)
        {
            fishingSpots[i].FishStock = 10;
        }
    }

}
