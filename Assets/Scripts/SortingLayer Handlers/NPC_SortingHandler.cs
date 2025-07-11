using UnityEngine;
using System.Collections.Generic;

public class NPC_SortingHandler : MonoBehaviour
{
    private Collider2D areaCollider;
    private readonly List<GameObject> npcsInRange = new List<GameObject>();

    private void Awake()
    {
        areaCollider = GetComponent<Collider2D>();
        if (areaCollider == null || !areaCollider.isTrigger)
            Debug.LogWarning("NPC_SortingHandler requires a trigger collider on the same GameObject.");
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("MovingNPC") && !npcsInRange.Contains(other.gameObject))
        {
            npcsInRange.Add(other.gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("MovingNPC"))
        {
            npcsInRange.Remove(other.gameObject);
        }
    }

    public void AdjustNPCSpriteLayers(int amount)
    {
        foreach (GameObject npc in npcsInRange)
        {
            SpriteRenderer targetRenderer = FindMainSpriteRenderer(npc);

            if (targetRenderer != null)
            {
                targetRenderer.sortingOrder += amount;
            }
        }
    }

    private SpriteRenderer FindMainSpriteRenderer(GameObject npc)
    {
        foreach (var sr in npc.GetComponentsInChildren<SpriteRenderer>())
        {
            PolygonCollider2D polyCol = sr.GetComponent<PolygonCollider2D>();
            if (polyCol != null && !polyCol.isTrigger)
            {
                return sr;
            }

            // Check parent
            polyCol = sr.transform.parent?.GetComponent<PolygonCollider2D>();
            if (polyCol != null && !polyCol.isTrigger)
            {
                return sr;
            }
        }

        return null;
    }
}
