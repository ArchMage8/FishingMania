#### **TLDR:**



**What it does:**

**----------------**

* **System is tasked in handling NPC data**
* **The NPCs with data are the ones that can be leveled up via quest completion**





**How is data stored and accessed:**

**----------------------------------**

* **Data is stored through an in game list, which is then uploaded to the JSON file when saved**



* **Each NPC is assigned a StateManager**
* **While the scene is given an NPCManager** 



***NPCManager:***

* **The script holds 2 key functions:**
* 
**&nbsp;	1. To change the state of the NPC**

	**2. To change the Level of the NPC**



***StateManager:***

* **Holds the function AdjustVersion, which updates the NPC based on the data held by the NPC Manager**
* **<i>
\*Call Refresh All NPC States (NPC State Refresher) when a new day arrives\*</i>**





**Relationship with Quest System:**

**--------------------------------**

* **When a quest is complete the NPC enters a "Full State"**



* **This NPC is separate from the main NPCs:**

  **1. It cannot give quests**

  **2. It also has unique dialogue**

  

**When the day resets, all NPCs in the full state are leveled up**



***Important:***

* **The dialogue manager calls the NPC manager when a quest is complete**
* **It updates the full state**
* **As well as the current friendship level**



