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

    private void OnEnable()
    {
        
    }

    public void LoadCurrentCombo()
    {
        // Load current combo from HookManager
        currentCombo = Inventory_HookManager.Instance.SavedCombo;
        RestoreActiveEffects();

    }

    private void RestoreActiveEffects()
    {
        //Debug.Log("Call");
       
        if (currentCombo > 0)
        {
           
            int baitClass = (currentCombo / 10) - 1;
            int hookClass = (currentCombo % 10) - 1;

            if (baitClass >= 0)
            {        
                activeBaitTile = baitTiles[baitClass];
                activeBaitTile.EnableEffect();
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
