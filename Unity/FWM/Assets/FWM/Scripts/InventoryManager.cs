using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FWM
{
    public class InventoryManager : MonoBehaviour
	{
		private List<FishInfo> inventoryFish = new List<FishInfo>();
		public GameObject inventoryItemPrefab;
		private GameObject currentItem;
		
		public void AddToInventory(List<FishInfo> newFish)
		{
			for(int i = 0; i < newFish.Count; i++)
			{
				inventoryFish.Add(newFish[i]);
			}
		}
		
		public void PopulateInventoryUI(GameObject itemParent)
		{
			for(int i = 0; i < inventoryFish.Count; i++)
			{
				currentItem = Instantiate(inventoryItemPrefab, itemParent.transform);
				
			}
		}
	}
}
