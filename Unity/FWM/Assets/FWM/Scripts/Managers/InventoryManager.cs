using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FWM
{
    public class InventoryManager : MonoBehaviour
	{
		public static List<FishInfo> inventoryFish = new List<FishInfo>();
		//public GameObject inventoryItemPrefab;
		//private GameObject currentItem;
		//private InventoryFishInstantiator currentScript;
		
		public void AddToInventory(List<FishInfo> newFish)
		{
			for(int i = 0; i < newFish.Count; i++)
			{
				inventoryFish.Add(newFish[i]);
			}
		}
		/*
		public void PopulateInventoryUI(GameObject itemParent)
		{
			for(int i = 0; i < inventoryFish.Count; i++)
			{
				currentItem = Instantiate(inventoryItemPrefab, itemParent.transform);
				currentScript = currentItem.GetComponent<InventoryFishInstantiator>();
				
				currentScript.fishName = inventoryFish[i].fishName;
				currentScript.fishLength = inventoryFish[i].fishLength;
				currentItem.SetActive(true);
			}
		}*/

	}
}
