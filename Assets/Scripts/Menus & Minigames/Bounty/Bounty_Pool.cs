using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewBountyPool", menuName = "Bounty System/Bounty Pool")]
public class Bounty_Pool : ScriptableObject
{
    public List<Item> availableItems;
}
