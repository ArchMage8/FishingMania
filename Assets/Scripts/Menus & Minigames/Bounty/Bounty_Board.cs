using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using System.Collections;

public class Bounty_Board : MonoBehaviour
{
    [System.Serializable]
    public struct BountyCardUI
    {
        public TextMeshProUGUI itemNameText;
        public Image icon;
        public TextMeshProUGUI rewardText;
        public Button submitButton;
        public GameObject completionStamp;
    }

    public int boardID; // Local ID used to match with the BountyManager
    public BountyCardUI[] bountyCards = new BountyCardUI[3];

    private List<Item> currentBounties;

    private void OnEnable()
    {
        RefreshBoard();
    }

    private void RefreshBoard()
    {
        currentBounties = Bounty_Manager.Instance.GetBounties(boardID.ToString());

        for (int i = 0; i < bountyCards.Length; i++)
        {
            if (i >= currentBounties.Count) continue;

            Item bountyItem = currentBounties[i];
            var ui = bountyCards[i];

            ui.icon.sprite = bountyItem.icon;
            ui.icon.color = Color.white;
            ui.itemNameText.text = bountyItem.itemName;

            int reward = bountyItem.price * 3;

            ui.rewardText.text = $"{reward}"; ; // Assuming price is the reward
            ui.submitButton.gameObject.SetActive(true);
            ui.submitButton.interactable = true;
            ui.completionStamp.SetActive(false);

            int bountyIndex = i;
            ui.submitButton.onClick.RemoveAllListeners();
            ui.submitButton.onClick.AddListener(() => SubmitBounty(bountyIndex));

            int owned = InventoryManager.Instance.GetTotalQuantity(bountyItem);
            if (owned < 1)
            {
                InsufficientItems(bountyIndex);
            }

            if (Bounty_Manager.Instance.IsBountyComplete(boardID.ToString(), bountyIndex))
            {
                SetCompletedUI(bountyIndex);
            }
        }
    }

    private void SetCompletedUI(int index)
    {
        var ui = bountyCards[index];
        ui.completionStamp.SetActive(true);
        ui.icon.color = new Color(0.6f, 0.6f, 0.6f); // Darkened icon
        ui.submitButton.gameObject.SetActive(false);
        ui.rewardText.text = "Completed!";
    }

    private void InsufficientItems(int index)
    {
        var ui = bountyCards[index];
        ui.icon.color = new Color(0.6f, 0.6f, 0.6f); // Darken the icon
        ui.submitButton.image.color = new Color32(195, 170, 135, 255); // #C3AA87
        ui.submitButton.interactable = false;
    }

    public void SubmitBounty(int index)
    {
        if (index < 0 || index >= currentBounties.Count)
        {
            return;
        }

        Item bountyItem = currentBounties[index];

        // Attempt removal
        bool removed = InventoryManager.Instance.RemoveItems(
            new List<ItemRemovalRequest> {
                new ItemRemovalRequest { item = bountyItem, quantity = 1 }
            });

        if (!removed)
        {
            InsufficientItems(index);
            return;
        }

        // Update bounty completion status
        Bounty_Manager.Instance.ModifyStatus(boardID.ToString(), index);

        // Reflect changes on UI
        SetCompletedUI(index);

        MoneyManager.Instance.playerBalance += bountyItem.price * 3;
    }

    public void DisableBoard()
    {
        StartCoroutine(CloseBoard());
    }

    private IEnumerator CloseBoard()
    {
        Animator temp = GetComponent<Animator>();

        temp.SetTrigger("Exit");
        yield return new WaitForSecondsRealtime(1f);
        this.gameObject.SetActive(false);



    }
}
