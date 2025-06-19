using UnityEngine;

public class Inventory_EquipmentManager : MonoBehaviour
{
    public static Inventory_EquipmentManager Instance { get; private set; }

    [Header("Tile Arrays")]
    public Inventory_HookTile[] hookTiles;
    public Inventory_BaitTile[] baitTiles;

    [Header("Active Selections")]
    public Inventory_HookTile activeHookTile;
    public Inventory_BaitTile activeBaitTile;

    [Header("Current Combo")]
    public int currentCombo; //Is the In-Game list
                             //Look at documentation for explanation


    public string ActiveBaitName;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void LoadCurrentCombo()
    {
        // Load current combo from HookManager
        currentCombo = Inventory_HookManager.Instance.SavedCombo;
        //RestoreActiveEffects();

    }

    public void RestoreActiveEffects()
    {
        
        if (currentCombo > 0)
        {
           
            int baitClass = (currentCombo / 10) - 1;
            int hookClass = (currentCombo % 10) - 1;

            if (baitClass >= 0)
            {
               Debug.Log("Bait Class: " + baitClass);

                activeBaitTile = baitTiles[baitClass];
                activeBaitTile.EnableEffect();

                ActiveBaitType = activeBaitTile.BaitItem;
               

            }

            if (hookClass >= 0)
            {
              
                activeHookTile = hookTiles[hookClass];
                activeHookTile.EnableEffect();
            }
        }
    }

    public void OnHookTilePressed(Inventory_HookTile pressedTile)
    {
        if (activeHookTile != null)
        {
            activeHookTile.DisableEffect();
        }

        activeHookTile = pressedTile;
        activeHookTile.EnableEffect();

        UpdateCombo();
    }

    public void OnBaitTilePressed(Inventory_BaitTile pressedTile)
    {
        if (activeBaitTile != null)
        {
            activeBaitTile.DisableEffect();
        }

        activeBaitTile = pressedTile;
        activeBaitTile.EnableEffect();

        UpdateCombo();
    }

    private void UpdateCombo()
    {
        if (activeHookTile != null && activeBaitTile != null)
        {
            int hookClass = activeHookTile.HookClass;
            int baitClass = activeBaitTile.BaitClass;

            currentCombo = int.Parse($"{baitClass}{hookClass}");

            //Look at documentation to understand why we are not updating the Hook Manager's Combo
        }
    }
}
