using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConditionalNPC : MonoBehaviour
{
    [System.Serializable]
    public class FriendshipCriteria
    {
        public NPC npcReference;
        public int minFriendshipLevel;
    }

    
    public List<FriendshipCriteria> conditions = new List<FriendshipCriteria>();

    public GameObject targetObject; // explicitly assign target object

    private void Start()
    {
        RunCheck();
    }

    private void OnEnable()
    {
       StartCoroutine(RegisterCriteria());  
    }

    private void OnDisable()
    {
        if (NPC_CriteriaChecker.Instance != null)
        {
            NPC_CriteriaChecker.Instance.Unregister(this);
        }
    }

    public void RunCheck()
    {
        bool allMet = true;

        foreach (var condition in conditions)
        {
            if (condition.npcReference == null)
            {
                Debug.LogWarning($"ConditionalNPC: Missing NPC reference on {gameObject.name}");
                allMet = false;
                break;
            }

            string npcName = condition.npcReference.npcName;
            int currentLevel = NPCManager.Instance.GetFriendshipLevel(npcName);

            if (currentLevel < condition.minFriendshipLevel)
            {
                allMet = false;
                break;
            }
        }

        if (targetObject != null)
        {
            targetObject.SetActive(allMet);
        }
    }

    private IEnumerator RegisterCriteria()
    {
        yield return new WaitForSeconds(0.1f);
        
        if (NPC_CriteriaChecker.Instance != null)
        {
            NPC_CriteriaChecker.Instance.Register(this);
        }

        RunCheck(); // initial check on enable
    }
}
