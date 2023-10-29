using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace FWM
{
    public class InventoryScript : MonoBehaviour
    {
		/*
		public GameObject contents;
		public Button backButton;
		private Button initButton;
		
		public ScrollRect scrollRect;
		public RectTransform contentPanel;
		
		public void OnEnable()
		{
			if(contents.transform.childCount > 0)
			{
				initButton = contents.transform.GetChild(0).GetComponent<Button>();
			}
			else
			{
				initButton = backButton;
			}
			
			initButton.Select();
		}*/
		
		private bool instantiated = false;
		
		private List<FishInfo> inv = InventoryManager.inventoryFish;
		
		public GameObject content;
		public GameObject inventoryItem;
		private GameObject currentItem;
		private InventoryFishInstantiator currentScript;
		
		private void OnEnable()
		{
			if(!instantiated)
			{
				AddContent();
			}
			
			instantiated = true;
		}
		
		public void AddContent()
		{
			for(int i = 0; i < inv.Count; i++)
			{
				currentItem = Instantiate(inventoryItem, content.transform);
				currentScript = currentItem.GetComponent<InventoryFishInstantiator>();
				
				currentScript.fishName = inv[i].fishName;
				currentScript.fishLength = inv[i].fishLength;
				currentItem.SetActive(true);
			}
		}
    }
}
