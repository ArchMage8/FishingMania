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
    public int currentCombo;

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
        LoadCurrentCombo();
        RestoreActiveEffects();
    }

    private void LoadCurrentCombo()
    {
        // Load current combo from HookManager
        currentCombo = Inventory_HookManager.Instance.currentCombo;
    }

    private void RestoreActiveEffects()
    {
        if (currentCombo > 0)
        {
            int baitClass = currentCombo / 10;
            int hookClass = currentCombo % 10;

            if (baitClass >= 0 && baitClass < baitTiles.Length)
            {
                activeBaitTile = baitTiles[baitClass];
                activeBaitTile.EnableEffect();
            }

            if (hookClass >= 0 && hookClass < hookTiles.Length)
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

            // Update HookManager's current combo to keep save data in sync
            Inventory_HookManager.Instance.currentCombo = currentCombo;
            Inventory_HookManager.Instance.SaveHooks();
        }
    }
}
