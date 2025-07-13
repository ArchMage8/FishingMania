using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Bounty_Manager : MonoBehaviour
{
    public static Bounty_Manager Instance;

    [System.Serializable]
    public class BountyBoardData
    {
        public string boardID;
        public Bounty_Pool pool;
        public List<Bounty> bounties = new List<Bounty>();
    }

    [System.Serializable]
    public class Bounty
    {
        public Item item;
        public bool isCompleted;
    }

    [SerializeField] private List<BountyBoardData> bountyBoards = new List<BountyBoardData>();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        InitializeBoards();
    }

    private void InitializeBoards()
    {
        foreach (var board in bountyBoards)
        {
            if (board.pool == null || board.pool.availableItems.Count < 3) continue;

            List<Item> chosenItems = board.pool.availableItems
                .OrderByDescending(item => item.price)
                .Distinct()
                .OrderBy(_ => Random.value)
                .Take(3)
                .ToList();

            chosenItems = chosenItems.OrderByDescending(i => i.price).ToList(); // Sort after selection

            board.bounties.Clear();
            foreach (var item in chosenItems)
            {
                board.bounties.Add(new Bounty { item = item, isCompleted = false });
            }
        }
    }

    public void ResetBounties()
    {
        InitializeBoards();
    }

    public void ModifyStatus(string boardID, int bountyIndex)
    {
        var board = bountyBoards.Find(b => b.boardID == boardID);
        if (board != null && bountyIndex >= 0 && bountyIndex < board.bounties.Count)
        {
            board.bounties[bountyIndex].isCompleted = true;
        }
        else
        {
            Debug.LogWarning($"Invalid bounty index or board ID: {boardID} / {bountyIndex}");
        }
    }

    public List<Item> GetBounties(string boardID)
    {
        var board = bountyBoards.Find(b => b.boardID == boardID);
        if (board == null) return new List<Item>();

        return board.bounties
            .OrderByDescending(b => b.item.price)
            .Select(b => b.item)
            .ToList();
    }

    public bool IsBountyComplete(string boardID, int bountyIndex)
    {
        var board = bountyBoards.Find(b => b.boardID == boardID);
        if (board != null && bountyIndex >= 0 && bountyIndex < board.bounties.Count)
        {
            return board.bounties[bountyIndex].isCompleted;
        }
        return false;
    }
}
