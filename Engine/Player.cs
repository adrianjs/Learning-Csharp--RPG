using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine
{
    public class Player : LivingCreature
    {
        public int Gold { get; set; }
        public int ExperiencePoints { get; set; }
        public int Level { get; set; }
        public Location CurrentLocation { get; set; }
        public List<InventoryItem> Inventory { get; set; }
        public List<PlayerQuest> Quests { get; set; }

        public Player(int currentHitPoints, int maximumHitPoints, int gold, int experiencePoints, int level) : base(
            currentHitPoints, maximumHitPoints)
        {
            Gold = gold;
            ExperiencePoints = experiencePoints;
            Level = level;

            Inventory = new List<InventoryItem>();
            Quests = new List<PlayerQuest>();
        }

        public bool HasRequiredItemToEnterThisLocation(Location location)
        {
            if (location.ItemRequiredToEnter == null)
            {
                //There is no required item to enter, so return "true"
                return true;
            }

            //See if the player has the required item in their inventory
            foreach (InventoryItem ii in Inventory)
            {
                if (ii.Details.ID == location.ItemRequiredToEnter.ID)
                {
                    //Found required item, so return "true"
                    return true;
                }
            }

            //we didn't find the required item, so return "false"
            return false;
        }

        public bool HasThisQuest(Quest quest)
        {
            foreach (PlayerQuest pq in Quests)
            {
                if (pq.QuestDetails.ID == quest.ID)
                {
                    return true;
                }
            }

            return false;
        }

        public bool CompletedThisQuest(Quest quest)
        {
            foreach (PlayerQuest pq in Quests)
            {
                if (pq.QuestDetails.ID == quest.ID)
                {
                    return pq.IsCompleted;
                }
            }

            return false;
        }

        public bool HasAllQuestCompletionItems(Quest quest)
        {
            //See if the player has all the items required to complete the quest
            foreach (QuestCompletionItem qci in quest.QuestCompletionItems)
            {
                bool foundItemInPlayerInventory = false;

                //Check each item in the player's inventory, to see if they have the item and enough qty
                foreach (InventoryItem ii in Inventory)
                {
                    if (ii.Details.ID == qci.Details.ID)
                    {
                        foundItemInPlayerInventory = true;

                        if (ii.Quantity < qci.Quantity) //player does not have enough qty
                        {
                            return false;
                        }
                    }
                }

                //Player does not have any of this qci in their inventory
                if (!foundItemInPlayerInventory)
                {
                    return false;
                }
            }

            //If we got here, then the player has all he needs
            return true;
        }

        public void RemoveQuestCompletionItems(Quest quest)
        {
            foreach (QuestCompletionItem qci in quest.QuestCompletionItems)
            {
                foreach (InventoryItem ii in Inventory)
                {
                    if (ii.Details.ID == qci.Details.ID)
                    {
                        //Subtract the quantity from player's inventory that was required for quest completion
                        ii.Quantity -= qci.Quantity;
                        break;
                    }
                }
            }
        }

        public void AddItemToInventory(Item itemToAdd)
        {
            foreach (InventoryItem ii in Inventory)
            {
                if (ii.Details.ID == itemToAdd.ID)
                {
                    //Player has item, add qty
                    ii.Quantity++;

                    return; //Have added item, and are done, so get out of function
                }
            }

            //Didn't have item, so add to inventory with qty of 1
            Inventory.Add(new InventoryItem(itemToAdd, 1));
        }

        public void MarkQuestCompleted(Quest quest)
        {
            //Find quesst in player's quest list
            foreach (PlayerQuest pq in Quests)
            {
                if (pq.QuestDetails.ID == quest.ID)
                {
                    //Mark it as completed
                    pq.IsCompleted = true;

                    return; //found the quest and marked complete, so get out of function
                }
            }
        }
    }
}
