using UnityEngine;

public class Inventory_EquipmentManager : MonoBehaviour
{
    [Header("Tile Arrays")]
    public Inventory_HookTile[] hookTiles;
    public Inventory_BaitTile[] baitTiles;

    [Header("Active Selections")]
    private Inventory_HookTile activeHookTile;
    private Inventory_BaitTile activeBaitTile;

    [Header("Current Combo")]
    public int currentCombo;

    private void OnEnable()
    {
        RestoreActiveEffects();
    }

    private void RestoreActiveEffects()
    {
        if (currentCombo > 0)
        {
            int baitClass = currentCombo / 10;
            int hookClass = currentCombo % 10;

            // Since arrays are ordered by class, index = class
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
        }
    }
}
